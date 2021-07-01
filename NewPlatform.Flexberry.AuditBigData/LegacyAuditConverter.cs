namespace NewPlatform.Flexberry.AuditBigData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Audit.Exceptions;
    using ICSSoft.STORMNET.Business.Audit.HelpStructures;
    using ICSSoft.STORMNET.Business.Audit.Objects;
    using ICSSoft.STORMNET.Exceptions;

    using Microsoft.Spatial;

    /// <summary>
    /// <see cref="ILegacyAuditConverter"/> implementation.
    /// </summary>
    /// <typeparam name="T">Тип прокси-объекта <see cref="IFieldAuditData"/>.</typeparam>
    public class LegacyAuditConverter<T> : ILegacyAuditConverter
        where T : IFieldAuditData, new()
    {
        /// <summary>
        /// Сообщение, которое возникнет, если на формирование полей выдана ошибочная операция.
        /// </summary>
        private const string ErrorMessageOnFields = "Exception";

        /// <summary>
        /// Преобразовывает пространственные данные в формат хранения в БД.
        /// </summary>
        /// <returns> Преобразованные пространственные данные.</returns>
        private static readonly WellKnownTextSqlFormatter Formatter = SpatialImplementation.CurrentImplementation.CreateWellKnownTextSqlFormatter(false);

        /// <inheritdoc/>
        public IEnumerable<IFieldAuditData> Convert(AuditAdditionalInfo auditAdditionalInfo)
        {
            if (auditAdditionalInfo == null)
            {
                throw new ArgumentNullException(nameof(auditAdditionalInfo));
            }

            var fieldAuditDataList = new List<IFieldAuditData>();
            Type objectType = Type.GetType(auditAdditionalInfo.AssemblyQualifiedObjectType, true);

            foreach (var fieldChange in auditAdditionalInfo.KeptFieldsValues)
            {
                // Сначала проверяем, что у соответствующего поля есть необходимый атрибут.
                if (!AuditAdditionalInfo.HasPropertyDisableInsertPropertyAttribute(
                    fieldChange.Key, objectType))
                {
                    throw new WrongAdditionalInfoAuditException();
                }

                // Заполняем список данными аудита изменённых полей.
                AuditAdditionalInfo.FieldValues fieldValues = fieldChange.Value;
                if (fieldValues.OldValue != fieldValues.NewValue)
                {
                    IFieldAuditData fieldAuditData = new T
                    {
                        Field = fieldChange.Key,
                        NewValue = fieldValues.NewValue,
                        OldValue = fieldValues.OldValue,
                    };
                    fieldAuditDataList.Add(fieldAuditData);
                }
            }

            return Convert2FieldAuditDataItems(fieldAuditDataList, null);
        }

        /// <inheritdoc/>
        public IEnumerable<IFieldAuditData> Convert(IEnumerable<CustomAuditField> items)
        {
            // Преобразуем в последовательность данных аудита полей.
            List<IFieldAuditData> fieldAuditDataList = items?.Select(
                x =>
                {
                    IFieldAuditData fieldAuditData = new T
                    {
                        Field = x.FieldName,
                        NewValue = x.NewFieldValue,
                        OldValue = x.OldFieldValue,
                    };
                    return fieldAuditData;
                })
                .ToList();

            return Convert2FieldAuditDataItems(fieldAuditDataList, null);
        }

        /// <inheritdoc/>
        public IEnumerable<IFieldAuditData> Convert(CommonAuditParameters commonAuditParameters)
        {
            if (commonAuditParameters == null)
            {
                throw new ArgumentNullException(nameof(commonAuditParameters));
            }

            DataObject oldDataObject = commonAuditParameters.OldVersionOperatedObject;
            DataObject operatedObject = commonAuditParameters.OperatedObject;
            Type operatedObjectType = operatedObject.GetType();
            View auditView = Information.GetView(commonAuditParameters.AuditView, operatedObjectType);
            if (auditView == null)
            {
                throw new CantFindViewException(operatedObjectType, commonAuditParameters.AuditView);
            }

            var fieldAuditDataList = new List<IFieldAuditData>();

            if (oldDataObject != null)
            {
                GenerateFields(
                    oldDataObject,
                    operatedObject,
                    auditView,
                    fieldAuditDataList,
                    commonAuditParameters.LoadedProperties);
            }
            else
            {
                GenerateFields(
                    operatedObject,
                    auditView,
                    fieldAuditDataList,
                    commonAuditParameters.TypeOfAuditOperation);
            }

            return Convert2FieldAuditDataItems(fieldAuditDataList, null);
        }

        /// <summary>
        /// Преобразовать предподготовленные данные аудита в окончательную форму.
        /// </summary>
        /// <param name="items">Перечень предподготовленных данных аудита.</param>
        /// <param name="mainChange">Родительское изменение.</param>
        /// <param name="convertedMainChange">Преобразованное родительское изменение.</param>
        /// <returns>Окончательные данные аудита.</returns>
        private static List<IFieldAuditData> Convert2FieldAuditDataItems(ICollection<IFieldAuditData> items, IFieldAuditData mainChange, IFieldAuditData convertedMainChange = null)
        {
            var accumulator = new List<IFieldAuditData>();

            IEnumerable<IFieldAuditData> parents = items
                .Where(x => x.MainChange == mainChange);

            foreach (IFieldAuditData parent in parents)
            {
                IFieldAuditData convertedItem = new T
                {
                    MainChange = convertedMainChange,
                    Field = parent.Field,
                    NewValue = parent.NewValue,
                    OldValue = parent.OldValue,
                };

                accumulator.Add(convertedItem);

                foreach (IFieldAuditData child in items.Where(x => x.MainChange == parent))
                {
                    accumulator.AddRange(Convert2FieldAuditDataItems(items, parent, convertedItem));
                }
            }

            return accumulator;
        }

        /// <summary>
        /// Генерация экземпляра <see cref="IFieldAuditData"/> для операции добавления или удаления набора детейлов.
        /// </summary>
        /// <param name="objectDetail">Описание детейла из представления агрегатора.</param>
        /// <param name="oldPropertyValue">Старое значение массива детейлов.</param>
        /// <param name="newPropertyValue">Новое значение массива детейлов.</param>
        /// <param name="fieldAuditDataList">Создаваемая запись аудита, куда добавляются <see cref="IFieldAuditData"/>.</param>
        private static void GenerateDetailFields(
            DetailInView objectDetail,
            object oldPropertyValue,
            object newPropertyValue,
            List<IFieldAuditData> fieldAuditDataList)
        {
            var oldDetailArray = oldPropertyValue as DetailArray;
            var newDetailArray = newPropertyValue as DetailArray;
            if ((oldDetailArray == null || oldDetailArray.Count == 0) && (newDetailArray == null || newDetailArray.Count == 0))
            {
                // Как ничего в детейлах не было, так ничего и не стало, обработка не требуется.
                return;
            }

            if (oldDetailArray == null || oldDetailArray.Count == 0)
            {
                // Детейлов не было, их создали.
                DataObject[] detailObjects = newDetailArray.GetAllObjects();
                GenerateDetailFields(detailObjects, objectDetail, fieldAuditDataList, tTypeOfAuditOperation.INSERT, 0);
                return;
            }

            if (newDetailArray == null || newDetailArray.Count == 0)
            {
                // Удалили все детейлы.
                DataObject[] detailObjects = oldDetailArray.GetAllObjects();
                GenerateDetailFields(detailObjects, objectDetail, fieldAuditDataList, tTypeOfAuditOperation.DELETE, 0);
                return;
            }

            // Набор детейлов возможно как-то изменили.
            DataObject[] detailObjectOldArray = oldDetailArray.GetAllObjects();
            List<DataObject> detailObjectNewList = newDetailArray.GetAllObjects().ToList();
            for (int i = 0; i < detailObjectOldArray.Length; i++)
            {
                DataObject detailObjectOld = detailObjectOldArray[i];
                DataObject detailObjectNew = newDetailArray.GetByKey(detailObjectOld.__PrimaryKey);

                if (detailObjectNew == null || detailObjectNew.GetStatus() == ObjectStatus.Deleted)
                {
                    ProcessMasterOrDetail(string.Format("{0} ({1})", objectDetail.Name, i), detailObjectOld, fieldAuditDataList, tTypeOfAuditOperation.DELETE, objectDetail.View);
                }
                else if (detailObjectNew.GetStatus() == ObjectStatus.Altered)
                {
                    ProcessMasterOrDetail(string.Format("{0} ({1})", objectDetail.Name, i), detailObjectOld, detailObjectNew, fieldAuditDataList, objectDetail.View);
                }

                if (detailObjectNew != null)
                {
                    // TODO: пока сверка идёт по старой схеме.
                    detailObjectNewList.Remove(detailObjectNew);
                }
            }

            if (detailObjectNewList.Count > 0)
            {
                GenerateDetailFields(detailObjectNewList.ToArray(), objectDetail, fieldAuditDataList, tTypeOfAuditOperation.INSERT, detailObjectOldArray.Length);
            }
        }

        /// <summary>
        /// Генерация набора экземпляров <see cref="IFieldAuditData"/> для операции добавления или удаления набора детейлов.
        /// </summary>
        /// <param name="detailObjects">Набор детейлов.</param>
        /// <param name="objectDetail">Настройка детейла.</param>
        /// <param name="fieldAuditDataList">Создаваемая запись аудита, куда добавляются <see cref="IFieldAuditData"/>.</param>
        /// <param name="typeOfAuditOperation">Тип операции аудита (добавление или удаление).</param>
        /// <param name="initialCounter">
        /// Начальное значение для счётчика детейлов
        /// (будет использовано в имени поля для каждого детейла).
        /// </param>
        private static void GenerateDetailFields(
            IEnumerable<DataObject> detailObjects,
            DetailInView objectDetail,
            List<IFieldAuditData> fieldAuditDataList,
            tTypeOfAuditOperation typeOfAuditOperation,
            int initialCounter)
        {
            int counter = initialCounter;
            foreach (var detailObject in detailObjects)
            {
                ProcessMasterOrDetail(
                    string.Format("{0} ({1})", objectDetail.Name, counter),
                    detailObject,
                    fieldAuditDataList,
                    typeOfAuditOperation,
                    objectDetail.View);
                counter++;
            }
        }

        /// <summary>
        /// Генерация экземпляров <see cref="IFieldAuditData"/> для объекта.
        /// </summary>
        /// <param name="operationedObject">Удаляемый объект.</param>
        /// <param name="auditView">Представление.</param>
        /// <param name="fieldAuditDataList">Список, куда добавляются сгенерированные экземпляры <see cref="IFieldAuditData"/>.</param>
        /// <param name="typeOfAuditOperation">Тип операции аудита.</param>
        private static void GenerateFields(
            DataObject operationedObject,
            View auditView,
            List<IFieldAuditData> fieldAuditDataList,
            tTypeOfAuditOperation typeOfAuditOperation)
        {
            var curViewProperties = auditView.Properties.Where(x => x.Visible).ToList(); // Здесь вместе мастера и собственные поля. Пишем только видимые поля.
            var fieldNameList = new List<string>();
            foreach (var curViewProperty in curViewProperties)
            {
                // Сначала мы обрабатываем собственные свойства и мастера.
                string propertyName = curViewProperty.Name;
                if (propertyName.Contains("."))
                {
                    propertyName = propertyName.Split('.')[0]; // Получаем имя мастера.
                }

                // В объекте нет одинаковых полей, если оно есть в списке то это поле мастера.
                if (!fieldNameList.Contains(propertyName))
                {
                    fieldNameList.Add(propertyName);
                    object curPropertyValue = Information.GetPropValueByName(operationedObject, propertyName);
                    if (curPropertyValue is DataObject dataObject)
                    {
                        // Вероятнее всего это мастер.
                        ProcessMasterOrDetail(
                            propertyName,
                            dataObject,
                            fieldAuditDataList,
                            typeOfAuditOperation,
                            auditView.GetViewForMaster(propertyName));
                    }
                    else
                    {
                        // Вероятнее всего это собственное свойство.
                        string curPropertyValueStr = ConvertPropertyValueToString(curPropertyValue);

                        IFieldAuditData fieldAuditData = new T
                        {
                            Field = propertyName,
                        };

                        switch (typeOfAuditOperation)
                        {
                            case tTypeOfAuditOperation.DELETE:
                                fieldAuditData.OldValue = curPropertyValue != null ? curPropertyValueStr : null;
                                fieldAuditData.NewValue = AuditConstants.FieldValueDeletedConst;
                                break;
                            case tTypeOfAuditOperation.INSERT:
                                fieldAuditData.OldValue = AuditConstants.FieldValueDeletedConst;
                                fieldAuditData.NewValue = curPropertyValue != null ? curPropertyValueStr : null;
                                break;
                            default:
                                fieldAuditData.OldValue = ErrorMessageOnFields;
                                fieldAuditData.NewValue = ErrorMessageOnFields;
                                break;
                        }

                        fieldAuditDataList.Add(fieldAuditData);
                    }
                }
            }

            var objectDetails = auditView.Details.Where(x => x.Visible).ToList(); // Берём только видимые.
            foreach (var objectDetail in objectDetails)
            {
                // Теперь нужно обработать детейлы.
                object curPropertyValue = Information.GetPropValueByName(operationedObject, objectDetail.Name);
                if (curPropertyValue is DetailArray curDetailArray && curDetailArray.Count > 0)
                {
                    var detailObjects = curDetailArray.GetAllObjects();
                    GenerateDetailFields(detailObjects, objectDetail, fieldAuditDataList, typeOfAuditOperation, 0);
                }
            }
        }

        /// <summary>
        /// Генерация экземпляров <see cref="IFieldAuditData"/> для изменённого объекта.
        /// </summary>
        /// <param name="operationedObjectOld">Старый вариант объекта.</param>
        /// <param name="operationedObjectNew">Новый вариант объекта.</param>
        /// <param name="auditView">Представление, по которому ведётся аудит.</param>
        /// <param name="fieldAuditDataList">Список, куда добавляются сгенерированные экземпляры <see cref="IFieldAuditData"/>.</param>
        /// <param name="loadedProperties">Загруженные свойства.</param>
        private static void GenerateFields(
            DataObject operationedObjectOld,
            DataObject operationedObjectNew,
            View auditView,
            List<IFieldAuditData> fieldAuditDataList,
            string[] loadedProperties)
        {
            if (operationedObjectOld == null)
            {
                throw new ArgumentNullException(nameof(operationedObjectOld)); // Такая ситуация должна была быть отрезана ранее
            }

            if (operationedObjectNew == null)
            {
                throw new ArgumentNullException(nameof(operationedObjectNew)); // Такая ситуация должна была быть отрезана ранее
            }

            if (operationedObjectNew.GetType() != operationedObjectOld.GetType())
            {
                throw new DifferentDataObjectTypesException();
            }

            var curViewProperties = auditView.Properties.Where(x => x.Visible).ToList(); // Здесь вместе мастера и собственные поля. Пишем только видимые поля.
            var fieldNameList = new List<string>();

            foreach (var curViewProperty in curViewProperties)
            {
                // сначала мы обрабатываем собственные свойства и мастера
                string propertyName = curViewProperty.Name;
                if (propertyName.Contains("."))
                {
                    propertyName = propertyName.Split('.')[0]; // получаем имя мастера
                }

                if (!loadedProperties.Contains(propertyName))
                {
                    continue;
                }

                object oldPropertyValue = Information.GetPropValueByName(operationedObjectOld, propertyName);
                object newPropertyValue = Information.GetPropValueByName(operationedObjectNew, propertyName);

                if (oldPropertyValue == null && newPropertyValue == null)
                {
                    continue;
                }

                if (oldPropertyValue is DataObject || newPropertyValue is DataObject)
                {
                    // вероятнее всего это мастер (возможно в каком-то варианте это null)
                    if (!fieldNameList.Contains(propertyName))
                    {
                        // и мастера этого мы ещё не рассматривали
                        GenerateMasterField(
                            oldPropertyValue,
                            newPropertyValue,
                            propertyName,
                            fieldAuditDataList,
                            auditView.GetViewForMaster(propertyName));

                        fieldNameList.Add(propertyName);
                    }
                }
                else
                {
                    // это собственное свойство
                    IFieldAuditData fieldAuditData = GenerateOwnField(oldPropertyValue, newPropertyValue, propertyName);
                    if (fieldAuditData != null)
                    {
                        fieldAuditDataList.Add(fieldAuditData);
                    }
                }
            }

            var objectDetails = auditView.Details.Where(x => x.Visible).ToList(); // Берём только видимые.
            foreach (var objectDetail in objectDetails)
            {
                // Теперь нужно обработать детейлы
                if (!loadedProperties.Contains(objectDetail.Name))
                {
                    continue;
                }

                object oldPropertyValue = Information.GetPropValueByName(operationedObjectOld, objectDetail.Name);
                object newPropertyValue = Information.GetPropValueByName(operationedObjectNew, objectDetail.Name);
                GenerateDetailFields(objectDetail, oldPropertyValue, newPropertyValue, fieldAuditDataList);
            }
        }

        /// <summary>
        /// Генерация для мастера объекта структуры типа <see cref="IFieldAuditData"/>.
        /// </summary>
        /// <param name="oldPropertyValue">Старое значение поля.</param>
        /// <param name="newPropertyValue">Новое значение поля.</param>
        /// <param name="propertyName">Имя поля.</param>
        /// <param name="fieldAuditDataList">Список, куда добавляются сгенерированные экземпляры <see cref="IFieldAuditData"/>.</param>
        /// <param name="masterView">Представление мастера, по которому ведётся аудит.</param>
        private static void GenerateMasterField(
            object oldPropertyValue,
            object newPropertyValue,
            string propertyName,
            List<IFieldAuditData> fieldAuditDataList,
            View masterView)
        {
            if (oldPropertyValue == null ||
                    newPropertyValue == null ||
                    oldPropertyValue.GetType() != newPropertyValue.GetType() ||
                    !((DataObject)oldPropertyValue).__PrimaryKey.Equals(((DataObject)newPropertyValue).__PrimaryKey))
            {
                // то есть мастера всё-таки разные
                ProcessMasterOrDetail(
                    propertyName,
                    (DataObject)oldPropertyValue,
                    (DataObject)newPropertyValue,
                    fieldAuditDataList,
                    masterView);
            }
        }

        /// <summary>
        /// Провести генерацию конструкции типа <see cref="IFieldAuditData"/> для собственного поля.
        /// </summary>
        /// <param name="oldPropertyValue">Старое значение поля.</param>
        /// <param name="newPropertyValue">Новое значение поля.</param>
        /// <param name="propertyName">Имя поля.</param>
        /// <returns> Сгенерированная конструкция типа <see cref="IFieldAuditData"/> для собственного поля.</returns>
        private static IFieldAuditData GenerateOwnField(
            object oldPropertyValue,
            object newPropertyValue,
            string propertyName)
        {
            string newPropertyValueStr = ConvertPropertyValueToString(newPropertyValue);

            string oldPropertyValueStr = ConvertPropertyValueToString(oldPropertyValue);

            if (oldPropertyValue == null)
            {
                IFieldAuditData fieldAuditData = new T
                {
                    Field = propertyName,
                    NewValue = newPropertyValueStr,
                };
                return fieldAuditData;
            }

            if (newPropertyValue == null)
            {
                IFieldAuditData fieldAuditData = new T
                {
                    Field = propertyName,
                    OldValue = oldPropertyValueStr,
                };
                return fieldAuditData;
            }

            bool unaltered = oldPropertyValue is IComparableType comparableType
                ? comparableType.Compare(newPropertyValue) == 0
                : oldPropertyValueStr == newPropertyValueStr;
            if (!unaltered)
            {
                IFieldAuditData fieldAuditData = new T
                {
                    Field = propertyName,
                    NewValue = newPropertyValueStr,
                    OldValue = oldPropertyValueStr,
                };
                return fieldAuditData;
            }

            return null;
        }

        /// <summary>
        /// Обработка добавления/удаления мастера или детейла объекта.
        /// </summary>
        /// <param name="fieldName">Имя мастера или детейла.</param>
        /// <param name="curDataObject">Объект мастера или детейла.</param>
        /// <param name="fieldAuditDataList">Список, куда добавляются сгенерированные экземпляры <see cref="IFieldAuditData"/>.</param>
        /// <param name="typeOfAuditOperation">Тип операции аудита (добавление или удаление).</param>
        /// <param name="curSubView">Представление для детейла или мастера, по которому и происходит запись данных.</param>
        private static void ProcessMasterOrDetail(
            string fieldName,
            DataObject curDataObject,
            List<IFieldAuditData> fieldAuditDataList,
            tTypeOfAuditOperation typeOfAuditOperation,
            View curSubView)
        {
            IFieldAuditData auditField = new T
            {
                Field = fieldName,
            };
            fieldAuditDataList.Add(auditField);
            IFieldAuditData auditFieldWithPriKey = new T
            {
                Field = $"{fieldName}({AuditConstants.FieldNamePrimaryKey})",
                MainChange = auditField,
            };
            fieldAuditDataList.Add(auditFieldWithPriKey);

            switch (typeOfAuditOperation)
            {
                case tTypeOfAuditOperation.DELETE:
                    auditField.OldValue =
                        curDataObject.ToStringForAudit(curSubView);
                    auditField.NewValue = AuditConstants.FieldValueDeletedConst;
                    auditFieldWithPriKey.OldValue = curDataObject.__PrimaryKey.ToString();
                    auditFieldWithPriKey.NewValue = AuditConstants.FieldValueDeletedConst;
                    break;
                case tTypeOfAuditOperation.INSERT:
                    auditField.OldValue = AuditConstants.FieldValueDeletedConst;
                    auditField.NewValue =
                        curDataObject.ToStringForAudit(curSubView);
                    auditFieldWithPriKey.OldValue = AuditConstants.FieldValueDeletedConst;
                    auditFieldWithPriKey.NewValue = curDataObject.__PrimaryKey.ToString();
                    break;
                default:
                    auditField.OldValue = ErrorMessageOnFields;
                    auditField.NewValue = ErrorMessageOnFields;
                    auditFieldWithPriKey.OldValue = ErrorMessageOnFields;
                    auditFieldWithPriKey.NewValue = ErrorMessageOnFields;
                    break;
            }
        }

        /// <summary>
        /// Обработка мастера/детейла объекта.
        /// </summary>
        /// <param name="fieldName">Имя мастера или детейла.</param>
        /// <param name="operationedObjectOld">Старый объект.</param>
        /// <param name="operationedObjectNew">Новый объект.</param>
        /// <param name="fieldAuditDataList">Список, куда добавляются сгенерированные экземпляры <see cref="IFieldAuditData"/>.</param>
        /// <param name="curSubView">Представление для детейла или мастера, по которому и происходит запись данных.</param>
        private static void ProcessMasterOrDetail(
            string fieldName,
            DataObject operationedObjectOld,
            DataObject operationedObjectNew,
            List<IFieldAuditData> fieldAuditDataList,
            View curSubView)
        {
            if (operationedObjectOld == null && operationedObjectNew == null)
            {
                return;
            }

            if (operationedObjectOld == null || operationedObjectNew == null)
            {
                if (operationedObjectOld == null)
                {
                    ProcessMasterOrDetail(fieldName, operationedObjectNew, fieldAuditDataList, tTypeOfAuditOperation.INSERT, curSubView);
                }
                else
                {
                    ProcessMasterOrDetail(fieldName, operationedObjectOld, fieldAuditDataList, tTypeOfAuditOperation.DELETE, curSubView);
                }
            }
            else
            {
                IFieldAuditData auditField = new T
                {
                    Field = fieldName,
                    OldValue = operationedObjectOld.ToStringForAudit(curSubView),
                    NewValue = operationedObjectNew.ToStringForAudit(curSubView),
                };
                fieldAuditDataList.Add(auditField);

                IFieldAuditData auditFieldWithPriKey = new T
                {
                    Field = $"{fieldName}({AuditConstants.FieldNamePrimaryKey})",
                    MainChange = auditField,
                    OldValue = operationedObjectOld.__PrimaryKey.ToString(),
                    NewValue = operationedObjectNew.__PrimaryKey.ToString(),
                };
                fieldAuditDataList.Add(auditFieldWithPriKey);
            }
        }

        /// <summary>
        /// Преобразование значение свойства к строке.
        /// </summary>
        /// <param name="propValue">Значение свойства.</param>
        /// <returns>Преобразованное значение.</returns>
        private static string ConvertPropertyValueToString(object propValue)
        {
            string propValueStr;
            if (propValue is Geometry geometry)
            {
                propValueStr = Formatter.Write(geometry);
            }
            else
            {
                propValueStr = propValue?.ToString();
            }

            return propValueStr;
        }
    }
}
