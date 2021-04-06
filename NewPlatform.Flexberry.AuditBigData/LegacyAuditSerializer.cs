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
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// <see cref="ILegacyAuditSerializer"/> implementation.
    /// </summary>
    public class LegacyAuditSerializer : ILegacyAuditSerializer
    {
        /// <summary>
        /// Сообщение, которое возникнет, если на формирование полей выдана ошибочная операция.
        /// </summary>
        private const string ErrorMessageOnFields = "Exception";

        /// <inheritdoc/>
        public IEnumerable<IFieldAuditData> Deserialize(string value)
        {
            return JsonConvert.DeserializeObject<FieldAuditData[]>(value ?? string.Empty) ?? new FieldAuditData[] { }.Cast<IFieldAuditData>();
        }

        /// <inheritdoc/>
        public string Serialize(AuditAdditionalInfo auditAdditionalInfo)
        {
            if (auditAdditionalInfo == null)
            {
                throw new ArgumentNullException(nameof(auditAdditionalInfo));
            }

            var fieldAuditDataList = new List<FieldAuditData>();
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
                    fieldAuditDataList.Add(new FieldAuditData() { Field = fieldChange.Key, NewValue = fieldValues.NewValue, OldValue = fieldValues.OldValue });
                }
            }

            return Serialize(fieldAuditDataList);
        }

        /// <inheritdoc/>
        public string Serialize(IEnumerable<CustomAuditField> customAuditFields)
        {
            // Преобразуем в последовательность данных аудита полей.
            IEnumerable<FieldAuditData> fieldAuditDataItems = customAuditFields?
                .Select(x => new FieldAuditData() { Field = x.FieldName, NewValue = x.NewFieldValue, OldValue = x.OldFieldValue });

            return Serialize(fieldAuditDataItems);
        }

        /// <inheritdoc/>
        public string Serialize(CommonAuditParameters commonAuditParameters)
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

            var fieldAuditDataList = new List<FieldAuditData>();

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

            return Serialize(fieldAuditDataList);
        }

        /// <inheritdoc/>
        public string Serialize(IEnumerable<AuditField> auditFields)
        {
            IEnumerable<FieldAuditData> fieldAuditDataItems = null;

            if (auditFields != null)
            {
                var accumulator = new List<FieldAuditData>();
                ConvertAuditFields(accumulator, auditFields, null);
                fieldAuditDataItems = accumulator.ToArray();
            }

            return Serialize(fieldAuditDataItems);
        }

        private static void ConvertAuditFields(List<FieldAuditData> accumulator, IEnumerable<AuditField> items, object mainChangePrimaryKey, FieldAuditData convertedMainChange = null)
        {
            IEnumerable<AuditField> parents = mainChangePrimaryKey == null ?
                items.Where(x => x.MainChange?.__PrimaryKey == null) :
                items.Where(x => x.MainChange?.__PrimaryKey != null && x.MainChange.__PrimaryKey.Equals(mainChangePrimaryKey));

            foreach (AuditField parent in parents)
            {
                var convertedItem = new FieldAuditData()
                {
                    MainChange = convertedMainChange,
                    Field = parent.Field,
                    NewValue = parent.NewValue,
                    OldValue = parent.OldValue,
                };

                accumulator.Add(convertedItem);

                foreach (AuditField child in items.Where(x => x.MainChange?.__PrimaryKey != null && x.MainChange.__PrimaryKey.Equals(parent.__PrimaryKey)))
                {
                    ConvertAuditFields(accumulator, items, parent.__PrimaryKey, convertedItem);
                }
            }
        }

        /// <summary>
        /// Генерация экземпляра <see cref="FieldAuditData"/> для операции добавления или удаления набора детейлов.
        /// </summary>
        /// <param name="objectDetail">Описание детейла из представления агрегатора.</param>
        /// <param name="oldPropertyValue">Старое значение массива детейлов.</param>
        /// <param name="newPropertyValue">Новое значение массива детейлов.</param>
        /// <param name="fieldAuditDataList">Создаваемая запись аудита, куда добавляются <see cref="FieldAuditData"/>.</param>
        private static void GenerateDetailFields(
            DetailInView objectDetail,
            object oldPropertyValue,
            object newPropertyValue,
            List<FieldAuditData> fieldAuditDataList)
        {
            var oldDetailArray = oldPropertyValue as DetailArray;
            var newDetailArray = newPropertyValue as DetailArray;
            if ((oldDetailArray == null || oldDetailArray.Count == 0) && (newDetailArray == null || newDetailArray.Count == 0))
            { // Как ничего в детейлах не было, так ничего и не стало, обработка не требуется.
                return;
            }

            if (oldDetailArray == null || oldDetailArray.Count == 0)
            { // Детейлов не было, их создали.
                DataObject[] detailObjects = newDetailArray.GetAllObjects();
                GenerateDetailFields(detailObjects, objectDetail, fieldAuditDataList, tTypeOfAuditOperation.INSERT, 0);
                return;
            }

            if (newDetailArray == null || newDetailArray.Count == 0)
            { // Удалили все детейлы.
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
                { // TODO: пока сверка идёт по старой схеме.
                    detailObjectNewList.Remove(detailObjectNew);
                }
            }

            if (detailObjectNewList.Count > 0)
            {
                GenerateDetailFields(detailObjectNewList.ToArray(), objectDetail, fieldAuditDataList, tTypeOfAuditOperation.INSERT, detailObjectOldArray.Length);
            }
        }

        /// <summary>
        /// Генерация набора экземпляров <see cref="FieldAuditData"/> для операции добавления или удаления набора детейлов.
        /// </summary>
        /// <param name="detailObjects">Набор детейлов.</param>
        /// <param name="objectDetail">Настройка детейла.</param>
        /// <param name="fieldAuditDataList">Создаваемая запись аудита, куда добавляются <see cref="FieldAuditData"/>.</param>
        /// <param name="typeOfAuditOperation">Тип операции аудита (добавление или удаление).</param>
        /// <param name="initialCounter">
        /// Начальное значение для счётчика детейлов
        /// (будет использовано в имени поля для каждого детейла).
        /// </param>
        private static void GenerateDetailFields(
            IEnumerable<DataObject> detailObjects,
            DetailInView objectDetail,
            List<FieldAuditData> fieldAuditDataList,
            tTypeOfAuditOperation typeOfAuditOperation,
            int initialCounter)
        {
            var counter = initialCounter;
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
        /// Генерация экземпляров <see cref="FieldAuditData"/> для объекта.
        /// </summary>
        /// <param name="operationedObject">Удаляемый объект.</param>
        /// <param name="auditView">Представление.</param>
        /// <param name="fieldAuditDataList">Список, куда добавляются сгенерированные экземпляры <see cref="FieldAuditData"/>.</param>
        /// <param name="typeOfAuditOperation">Тип операции аудита.</param>
        private static void GenerateFields(
            DataObject operationedObject,
            View auditView,
            List<FieldAuditData> fieldAuditDataList,
            tTypeOfAuditOperation typeOfAuditOperation)
        {
            var type = operationedObject.GetType();
            var curViewProperties = auditView.Properties.Where(x => x.Visible).ToList(); // Здесь вместе мастера и собственные поля. Пишем только видимые поля.
            var fieldNameList = new List<string>();
            foreach (var curViewProperty in curViewProperties)
            { // Сначала мы обрабатываем собственные свойства и мастера.
                string propertyName = curViewProperty.Name;
                if (propertyName.Contains("."))
                {
                    propertyName = propertyName.Split('.')[0]; // Получаем имя мастера.
                }

                // В объекте нет одинаковых полей, если оно есть в списке то это поле мастера.
                if (!fieldNameList.Contains(propertyName))
                {
                    fieldNameList.Add(propertyName);
                    var curPropertyValue = Information.GetPropValueByName(operationedObject, propertyName);
                    if (curPropertyValue is DataObject)
                    {
                        // Вероятнее всего это мастер.
                        ProcessMasterOrDetail(
                            propertyName,
                            (DataObject)curPropertyValue,
                            fieldAuditDataList,
                            typeOfAuditOperation,
                            auditView.GetViewForMaster(propertyName));
                    }
                    else
                    { // Вероятнее всего это собственное свойство.
                        var fieldAuditData = new FieldAuditData { Field = propertyName };
                        switch (typeOfAuditOperation)
                        {
                            case tTypeOfAuditOperation.DELETE:
                                fieldAuditData.OldValue = curPropertyValue != null ? curPropertyValue.ToString() : null;
                                fieldAuditData.NewValue = AuditConstants.FieldValueDeletedConst;
                                break;
                            case tTypeOfAuditOperation.INSERT:
                                fieldAuditData.OldValue = AuditConstants.FieldValueDeletedConst;
                                fieldAuditData.NewValue = curPropertyValue != null ? curPropertyValue.ToString() : null;
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
            { // Теперь нужно обработать детейлы.
                var curPropertyValue = Information.GetPropValueByName(operationedObject, objectDetail.Name);
                var curDetailArray = curPropertyValue as DetailArray;
                if (curDetailArray != null && curDetailArray.Count > 0)
                {
                    var detailObjects = curDetailArray.GetAllObjects();
                    GenerateDetailFields(detailObjects, objectDetail, fieldAuditDataList, typeOfAuditOperation, 0);
                }
            }
        }

        /// <summary>
        /// Генерация экземпляров <see cref="FieldAuditData"/> для изменённого объекта.
        /// </summary>
        /// <param name="operationedObjectOld">Старый вариант объекта.</param>
        /// <param name="operationedObjectNew">Новый вариант объекта.</param>
        /// <param name="auditView">Представление, по которому ведётся аудит.</param>
        /// <param name="fieldAuditDataList">Список, куда добавляются сгенерированные экземпляры <see cref="FieldAuditData"/>.</param>
        /// <param name="loadedProperties">Загруженные свойства.</param>
        private static void GenerateFields(
            DataObject operationedObjectOld,
            DataObject operationedObjectNew,
            View auditView,
            List<FieldAuditData> fieldAuditDataList,
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

            var type = operationedObjectNew.GetType();
            if (operationedObjectOld.GetType() != type)
            {
                throw new DifferentDataObjectTypesException();
            }

            var curViewProperties = auditView.Properties.Where(x => x.Visible).ToList(); // Здесь вместе мастера и собственные поля. Пишем только видимые поля.
            var fieldNameList = new List<string>();

            foreach (var curViewProperty in curViewProperties)
            { // сначала мы обрабатываем собственные свойства и мастера
                var propertyName = curViewProperty.Name;
                if (propertyName.Contains("."))
                {
                    propertyName = propertyName.Split('.')[0]; // получаем имя мастера
                }

                if (!loadedProperties.Contains(propertyName))
                {
                    continue;
                }

                var oldPropertyValue = Information.GetPropValueByName(operationedObjectOld, propertyName);
                var newPropertyValue = Information.GetPropValueByName(operationedObjectNew, propertyName);

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
                    FieldAuditData fieldAuditData = GenerateOwnField(oldPropertyValue, newPropertyValue, propertyName);
                    if (fieldAuditData != null)
                    {
                        fieldAuditDataList.Add(fieldAuditData);
                    }
                }
            }

            var objectDetails = auditView.Details.Where(x => x.Visible).ToList(); // Берём только видимые.
            foreach (var objectDetail in objectDetails)
            { // Теперь нужно обработать детейлы
                if (!loadedProperties.Contains(objectDetail.Name))
                {
                    continue;
                }

                var oldPropertyValue = Information.GetPropValueByName(operationedObjectOld, objectDetail.Name);
                var newPropertyValue = Information.GetPropValueByName(operationedObjectNew, objectDetail.Name);
                GenerateDetailFields(objectDetail, oldPropertyValue, newPropertyValue, fieldAuditDataList);
            }
        }

        /// <summary>
        /// Генерация для мастера объекта структуры типа <see cref="FieldAuditData"/>.
        /// </summary>
        /// <param name="oldPropertyValue">Старое значение поля.</param>
        /// <param name="newPropertyValue">Новое значение поля.</param>
        /// <param name="propertyName">Имя поля.</param>
        /// <param name="fieldAuditDataList">Список, куда добавляются сгенерированные экземпляры <see cref="FieldAuditData"/>.</param>
        /// <param name="masterView">Представление мастера, по которому ведётся аудит.</param>
        private static void GenerateMasterField(
            object oldPropertyValue,
            object newPropertyValue,
            string propertyName,
            List<FieldAuditData> fieldAuditDataList,
            View masterView)
        {
            if (oldPropertyValue == null ||
                    newPropertyValue == null ||
                    oldPropertyValue.GetType() != newPropertyValue.GetType() ||
                    !((DataObject)oldPropertyValue).__PrimaryKey.Equals(((DataObject)newPropertyValue).__PrimaryKey))
            { // то есть мастера всё-таки разные
                ProcessMasterOrDetail(
                    propertyName,
                    (DataObject)oldPropertyValue,
                    (DataObject)newPropertyValue,
                    fieldAuditDataList,
                    masterView);
            }
        }

        /// <summary>
        /// Провести генерацию конструкции типа <see cref="FieldAuditData"/> для собственного поля.
        /// </summary>
        /// <param name="oldPropertyValue">Старое значение поля.</param>
        /// <param name="newPropertyValue">Новое значение поля.</param>
        /// <param name="propertyName">Имя поля.</param>
        /// <returns> Сгенерированная конструкция типа <see cref="FieldAuditData"/> для собственного поля.</returns>
        private static FieldAuditData GenerateOwnField(
            object oldPropertyValue,
            object newPropertyValue,
            string propertyName)
        {
            if (oldPropertyValue == null)
            {
                return new FieldAuditData { Field = propertyName, OldValue = null, NewValue = newPropertyValue.ToString() };
            }

            if (newPropertyValue == null)
            {
                return new FieldAuditData { Field = propertyName, OldValue = oldPropertyValue.ToString(), NewValue = null };
            }

            bool unaltered = oldPropertyValue is IComparableType comparableType
                ? comparableType.Compare(newPropertyValue) == 0
                : oldPropertyValue.ToString() == newPropertyValue.ToString();
            if (!unaltered)
            {
                return new FieldAuditData { Field = propertyName, OldValue = oldPropertyValue.ToString(), NewValue = newPropertyValue.ToString() };
            }

            return null;
        }

        /// <summary>
        /// Обработка добавления/удаления мастера или детейла объекта.
        /// </summary>
        /// <param name="fieldName">Имя мастера или детейла.</param>
        /// <param name="curDataObject">Объект мастера или детейла.</param>
        /// <param name="fieldAuditDataList">Список, куда добавляются сгенерированные экземпляры <see cref="FieldAuditData"/>.</param>
        /// <param name="typeOfAuditOperation">Тип операции аудита (добавление или удаление).</param>
        /// <param name="curSubView">Представление для детейла или мастера, по которому и происходит запись данных.</param>
        private static void ProcessMasterOrDetail(
            string fieldName,
            DataObject curDataObject,
            List<FieldAuditData> fieldAuditDataList,
            tTypeOfAuditOperation typeOfAuditOperation,
            View curSubView)
        {
            var auditField = new FieldAuditData { Field = fieldName };
            fieldAuditDataList.Add(auditField);
            var auditFieldWithPriKey = new FieldAuditData
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
        /// <param name="fieldAuditDataList">Список, куда добавляются сгенерированные экземпляры <see cref="FieldAuditData"/>.</param>
        /// <param name="curSubView">Представление для детейла или мастера, по которому и происходит запись данных.</param>
        private static void ProcessMasterOrDetail(
            string fieldName,
            DataObject operationedObjectOld,
            DataObject operationedObjectNew,
            List<FieldAuditData> fieldAuditDataList,
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
                var auditField = new FieldAuditData
                {
                    Field = fieldName,
                    OldValue = operationedObjectOld.ToStringForAudit(curSubView),
                    NewValue = operationedObjectNew.ToStringForAudit(curSubView),
                };
                fieldAuditDataList.Add(auditField);

                var auditFieldWithPriKey = new FieldAuditData
                {
                    Field = $"{fieldName}({AuditConstants.FieldNamePrimaryKey})",
                    MainChange = auditField,
                    OldValue = operationedObjectOld.__PrimaryKey.ToString(),
                    NewValue = operationedObjectNew.__PrimaryKey.ToString(),
                };
                fieldAuditDataList.Add(auditFieldWithPriKey);
            }
        }

        private static string Serialize(IEnumerable<FieldAuditData> fieldAuditDataItems)
        {
            return fieldAuditDataItems != null && fieldAuditDataItems.Any() ? JsonConvert.SerializeObject(fieldAuditDataItems.ToArray()) : null;
        }

        [JsonObject(IsReference = true)]
        private class FieldAuditData : IFieldAuditData
        {
            /// <summary>
            /// Field value setter.
            /// </summary>
            [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
            public string Field { private get; set; }

            /// <summary>
            /// MainChange value setter.
            /// </summary>
            [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
            public FieldAuditData MainChange { private get; set; }

            /// <summary>
            /// NewValue value setter.
            /// </summary>
            [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
            public string NewValue { private get; set; }

            /// <summary>
            /// OldValue value setter.
            /// </summary>
            [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
            public string OldValue { private get; set; }

            /// <inheritdoc/>
            string IFieldAuditData.Field => Field;

            /// <inheritdoc/>
            IFieldAuditData IFieldAuditData.MainChange => MainChange;

            /// <inheritdoc/>
            string IFieldAuditData.NewValue => NewValue;

            /// <inheritdoc/>
            string IFieldAuditData.OldValue => OldValue;
        }
    }
}
