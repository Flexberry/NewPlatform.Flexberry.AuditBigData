namespace ICSSoft.STORMNET.Business.Audit.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET.Business.Audit.Objects;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;

    using NewPlatform.Flexberry.Audit.Tests;
    using NewPlatform.Flexberry.AuditBigData;
    using NewPlatform.Flexberry.AuditBigData.Serialization;

    using Xunit;

    /// <summary>
    /// Проверка аудита полей мастера при создании объекта с местером.
    /// </summary>
    public class AuditUnitTest : BaseAuditServiceTest
    {
        /// <summary>
        /// Проверка аудита полей мастера при создании .
        /// </summary>
        [Fact]
        public void CreateObjectWithMasterAuditTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                Class1 class1 = new Class1
                {
                    Field11 = "Field11",
                    Field12 = "Field12",
                };

                Class2 class2 = new Class2
                {
                    Field21 = "Field21",
                    Field22 = "Field22",
                    Class1 = class1,
                };

                string class2AuditUpdateText = "Class1(Class1.Field11=Field11, Class1.Field12=Field12)";

                // Act.
                DataObject[] dataObjects = new DataObject[] { class1, class2};
                dataService.UpdateObjects(ref dataObjects);

                ExternalLangDef langDef = ExternalLangDef.LanguageDef;

                // Вычитка записей аудита.
                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(AuditRecord), AuditRecord.Views.AllFields);
                lcs.LimitFunction = langDef.GetFunction(langDef.funcEQ, new VariableDef(langDef.GuidType, Information.ExtractPropertyPath<AuditRecord>(x => x.ObjectPrimaryKey)), class2.__PrimaryKey);
                string sortProperty = Information.ExtractPropertyPath<AuditRecord>(x => x.OperationTime);
                lcs.ColumnsSort = new[] { new ColumnsSortDef(sortProperty, SortOrder.Desc) };
                DataObject[] primaryAuditObjects = dataService.LoadObjects(lcs);
                DataObject[] ratifyingAuditObjects = GetRatifyingAuditObjects(dataService, primaryAuditObjects);
                string operationType = AuditOperationType.Insert.ToString();
                AuditRecord primaryAuditRecord = primaryAuditObjects.Cast<AuditRecord>().First(x => x.OperationType == operationType);

                // Assert.
                Assert.Single(primaryAuditObjects);
                Assert.Single(primaryAuditObjects
                    .Cast<AuditRecord>()
                    .Where(x => x.OperationType == operationType));
                Assert.Single(ratifyingAuditObjects);
                Assert.Single(ratifyingAuditObjects
                    .Cast<AuditRecord>()
                    .Where(x => x.OperationType == AuditOperationType.Ratify.ToString())
                    .Where(x => x.HeadAuditEntity.__PrimaryKey.Equals(primaryAuditRecord.__PrimaryKey)));

                // Вычитка полей аудита.
                IEnumerable<IFieldAuditData> fieldAuditDataItems = new JsonLegacyAuditSerializer().Deserialize(primaryAuditRecord.SerializedFields);
                Assert.Equal(9, fieldAuditDataItems.Count());

                int countOfClass1Fields = 0;
                foreach (IFieldAuditData item in fieldAuditDataItems)
                {
                    if (item.Field == "Class1")
                    {
                        Assert.Equal(AuditConstants.FieldValueDeletedConst, item.OldValue);
                        Assert.Equal(class2AuditUpdateText, item.NewValue);
                        countOfClass1Fields++;
                    }
                }

                Assert.Equal(1, countOfClass1Fields);
            }
        }

        /// <summary>
        /// Проверка аудита полей мастера при изменении.
        /// </summary>
        [Fact]
        public void UpdateObjectWithMasterAuditTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                Class1 class1 = new Class1
                {
                    Field11 = "Field11",
                    Field12 = "Field12",
                };

                Class2 class2 = new Class2
                {
                    Field21 = "Field21",
                    Field22 = "Field22",
                    Class1 = class1,
                };

                Class1 class12 = new Class1
                {
                    Field11 = "Field13",
                    Field12 = "Field14",
                };

                string class2AuditUpdateText1 = "Class1(Class1.Field11=Field11, Class1.Field12=Field12)";
                string class2AuditUpdateText2 = "Class1(Class1.Field11=Field13, Class1.Field12=Field14)";

                // Act.
                DataObject[] dataObjects = new DataObject[] { class1, class2, class12 };
                dataService.UpdateObjects(ref dataObjects);

                class2.Class1 = class12;

                dataService.UpdateObject(class2);

                ExternalLangDef langDef = ExternalLangDef.LanguageDef;

                // Вычитка записей аудита.
                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(AuditRecord), AuditRecord.Views.AllFields);
                lcs.LimitFunction = langDef.GetFunction(langDef.funcEQ, new VariableDef(langDef.GuidType, Information.ExtractPropertyPath<AuditRecord>(x => x.ObjectPrimaryKey)), class2.__PrimaryKey);
                string sortProperty = Information.ExtractPropertyPath<AuditRecord>(x => x.OperationTime);
                lcs.ColumnsSort = new[] { new ColumnsSortDef(sortProperty, SortOrder.Desc) };
                DataObject[] primaryAuditObjects = dataService.LoadObjects(lcs);
                DataObject[] ratifyingAuditObjects = GetRatifyingAuditObjects(dataService, primaryAuditObjects);
                string operationType = AuditOperationType.Update.ToString();
                AuditRecord primaryAuditRecord = primaryAuditObjects.Cast<AuditRecord>().First(x => x.OperationType == operationType);

                // Assert.
                Assert.Equal(2, primaryAuditObjects.Length);
                Assert.Single(primaryAuditObjects
                    .Cast<AuditRecord>()
                    .Where(x => x.OperationType == operationType));
                Assert.Equal(2, ratifyingAuditObjects.Length);
                Assert.Single(ratifyingAuditObjects
                    .Cast<AuditRecord>()
                    .Where(x => x.OperationType == AuditOperationType.Ratify.ToString())
                    .Where(x => x.HeadAuditEntity.__PrimaryKey.Equals(primaryAuditRecord.__PrimaryKey)));

                // Вычитка полей аудита.
                IEnumerable<IFieldAuditData> fieldAuditDataItems = new JsonLegacyAuditSerializer().Deserialize(primaryAuditRecord.SerializedFields);
                Assert.Equal(4, fieldAuditDataItems.Count());

                int countOfClass1Fields = 0;
                foreach (IFieldAuditData item in fieldAuditDataItems)
                {
                    if (item.Field == "Class1")
                    {
                        Assert.Equal(class2AuditUpdateText1, item.OldValue);
                        Assert.Equal(class2AuditUpdateText2, item.NewValue);
                        countOfClass1Fields++;
                    }
                }

                Assert.Equal(1, countOfClass1Fields);
            }
        }

        /// <summary>
        /// Проверка аудита полей мастера при удалении.
        /// </summary>
        [Fact]
        public void DeleteObjectWithMasterAuditTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                Class1 class1 = new Class1
                {
                    Field11 = "Field11",
                    Field12 = "Field12",
                };

                Class2 class2 = new Class2
                {
                    Field21 = "Field11",
                    Field22 = "Field12",
                    Class1 = class1,
                };

                string class2AuditUpdateText = "Class1(Class1.Field11=Field11, Class1.Field12=Field12)";

                // Act.
                DataObject[] dataObjects = new DataObject[] { class1, class2 };
                dataService.UpdateObjects(ref dataObjects);

                class2.Class1 = null;

                dataService.UpdateObject(class2);

                ExternalLangDef langDef = ExternalLangDef.LanguageDef;

                // Вычитка записей аудита.
                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(AuditRecord), AuditRecord.Views.AllFields);
                lcs.LimitFunction = langDef.GetFunction(langDef.funcEQ, new VariableDef(langDef.GuidType, Information.ExtractPropertyPath<AuditRecord>(x => x.ObjectPrimaryKey)), class2.__PrimaryKey);
                string sortProperty = Information.ExtractPropertyPath<AuditRecord>(x => x.OperationTime);
                lcs.ColumnsSort = new[] { new ColumnsSortDef(sortProperty, SortOrder.Desc) };
                DataObject[] primaryAuditObjects = dataService.LoadObjects(lcs);
                DataObject[] ratifyingAuditObjects = GetRatifyingAuditObjects(dataService, primaryAuditObjects);
                string operationType = AuditOperationType.Update.ToString();
                AuditRecord primaryAuditRecord = primaryAuditObjects.Cast<AuditRecord>().First(x => x.OperationType == operationType);

                // Assert.
                Assert.Equal(2, primaryAuditObjects.Length);
                Assert.Single(primaryAuditObjects
                    .Cast<AuditRecord>()
                    .Where(x => x.OperationType == operationType));
                Assert.Equal(2, ratifyingAuditObjects.Length);
                Assert.Single(ratifyingAuditObjects
                    .Cast<AuditRecord>()
                    .Where(x => x.OperationType == AuditOperationType.Ratify.ToString())
                    .Where(x => x.HeadAuditEntity.__PrimaryKey.Equals(primaryAuditRecord.__PrimaryKey)));

                // Вычитка полей аудита.
                IEnumerable<IFieldAuditData> fieldAuditDataItems = new JsonLegacyAuditSerializer().Deserialize(primaryAuditRecord.SerializedFields);
                Assert.Equal(4, fieldAuditDataItems.Count());

                int countOfClass1Fields = 0;
                foreach (IFieldAuditData field in fieldAuditDataItems)
                {
                    if (field.Field == "Class1")
                    {
                        Assert.Equal(class2AuditUpdateText, field.OldValue);
                        Assert.Equal(AuditConstants.FieldValueDeletedConst, field.NewValue);
                        countOfClass1Fields++;
                    }
                }

                Assert.Equal(1, countOfClass1Fields);
            }
        }

        /// <summary>
        /// Проверка имени поля мастера при его добавлении.
        /// </summary>
        [Fact]
        public void CreateObjectAuditFieldNameTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                Class1 class1 = new Class1
                {
                    Field11 = "Field11",
                    Field12 = "Field12",
                };

                Class2 class2 = new Class2
                {
                    Field21 = "Field21",
                    Field22 = "Field22",
                    Class1 = class1,
                };

                // Act.
                DataObject[] dataObjects = new DataObject[] { class1, class2 };
                dataService.UpdateObjects(ref dataObjects);

                ExternalLangDef langDef = ExternalLangDef.LanguageDef;

                // Вычитка записей аудита.
                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(AuditRecord), AuditRecord.Views.AllFields);
                lcs.LimitFunction = langDef.GetFunction(langDef.funcEQ, new VariableDef(langDef.GuidType, Information.ExtractPropertyPath<AuditRecord>(x => x.ObjectPrimaryKey)), class2.__PrimaryKey);
                DataObject[] primaryAuditObjects = dataService.LoadObjects(lcs);
                DataObject[] ratifyingAuditObjects = GetRatifyingAuditObjects(dataService, primaryAuditObjects);
                string operationType = AuditOperationType.Insert.ToString();
                AuditRecord primaryAuditRecord = primaryAuditObjects.Cast<AuditRecord>().First(x => x.OperationType == operationType);

                // Assert.
                Assert.Single(primaryAuditObjects);
                Assert.Single(primaryAuditObjects
                    .Cast<AuditRecord>()
                    .Where(x => x.OperationType == operationType));
                Assert.Single(ratifyingAuditObjects);
                Assert.Single(ratifyingAuditObjects
                    .Cast<AuditRecord>()
                    .Where(x => x.OperationType == AuditOperationType.Ratify.ToString())
                    .Where(x => x.HeadAuditEntity.__PrimaryKey.Equals(primaryAuditRecord.__PrimaryKey)));

                // Вычитка полей аудита.
                IEnumerable<IFieldAuditData> fieldAuditDataItems = new JsonLegacyAuditSerializer().Deserialize(primaryAuditRecord.SerializedFields);
                Assert.Equal(9, fieldAuditDataItems.Count());

                string text = $"Class1({AuditConstants.FieldNamePrimaryKey})";
                int countOfLinkedPrimaryKeyFields = fieldAuditDataItems.Count(x => x.Field == text);

                Assert.Equal(1, countOfLinkedPrimaryKeyFields);
            }
        }

        /// <summary>
        /// Проверка аудита полей мастера при создании объекта с пустым местером.
        /// </summary>
        [Fact]
        public void CreateObjectWithEmptyMasterAuditTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                Class2 class2 = new Class2
                {
                    Field21 = "Field11",
                    Field22 = "Field12",
                };

                // Act.
                dataService.UpdateObject(class2);

                ExternalLangDef langDef = ExternalLangDef.LanguageDef;

                // Вычитка записей аудита.
                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(AuditRecord), AuditRecord.Views.AllFields);
                lcs.LimitFunction = langDef.GetFunction(langDef.funcEQ, new VariableDef(langDef.GuidType, Information.ExtractPropertyPath<AuditRecord>(x => x.ObjectPrimaryKey)), class2.__PrimaryKey);
                DataObject[] primaryAuditObjects = dataService.LoadObjects(lcs);
                DataObject[] ratifyingAuditObjects = GetRatifyingAuditObjects(dataService, primaryAuditObjects);
                string operationType = AuditOperationType.Insert.ToString();
                AuditRecord primaryAuditRecord = primaryAuditObjects.Cast<AuditRecord>().First(x => x.OperationType == operationType);

                // Assert.
                Assert.Single(primaryAuditObjects);
                Assert.Single(primaryAuditObjects
                    .Cast<AuditRecord>()
                    .Where(x => x.OperationType == operationType));
                Assert.Single(ratifyingAuditObjects);
                Assert.Single(ratifyingAuditObjects
                    .Cast<AuditRecord>()
                    .Where(x => x.OperationType == AuditOperationType.Ratify.ToString())
                    .Where(x => x.HeadAuditEntity.__PrimaryKey.Equals(primaryAuditRecord.__PrimaryKey)));

                // Вычитка полей аудита.
                IEnumerable<IFieldAuditData> fieldAuditDataItems = new JsonLegacyAuditSerializer().Deserialize(primaryAuditRecord.SerializedFields);
                Assert.Equal(8, fieldAuditDataItems.Count());

                int countOfClass1AtAuditFields = fieldAuditDataItems.Count(x => x.Field.IndexOf("Class1") != -1);
                Assert.Equal(1, countOfClass1AtAuditFields);
            }
        }

        /// <summary>
        /// Gets the enabled audit service for the test.
        /// </summary>
        /// <returns>Returns instance of the <see cref="AuditService" /> class that will be used for the test.</returns>
        protected override AuditService GetAuditServiceForTest()
        {
            return new AuditService
            {
                AppSetting = new AuditAppSetting { AuditEnabled = true },
                ApplicationMode = AppMode.Win,
                Audit = new EmptyAudit(),
            };
        }
    }
}
