namespace ICSSoft.STORMNET.Business.Audit.Tests
{
    using System.Linq;
    using ICSSoft.STORMNET.Business.Audit.Objects;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;
    using NewPlatform.Flexberry.Audit.Tests;
    using NewPlatform.Flexberry.AuditBigData;
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
                DataObject[] auditEntities = dataService.LoadObjects(lcs);
                AuditRecord auditRecord = auditEntities.Cast<AuditRecord>().FirstOrDefault();

                // Assert.
                Assert.Equal("Создание", auditRecord.OperationType);
                Assert.Single(auditEntities);

                int countOfClass1Fields = 0;

                Assert.Equal(1, countOfClass1Fields);

                // TODO: проверка полей аудита.

            }
        }

        /// <summary>
        /// Проверка аудита полей мастера при изменении.
        /// </summary>
        [Fact]
        public void UpdapteObjectWithMasterAuditTest()
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
                DataObject[] auditEntities = dataService.LoadObjects(lcs);
                AuditRecord auditRecord = auditEntities.Cast<AuditRecord>().FirstOrDefault();

                // TODO: проверка полей аудита.

                // Assert.
                Assert.Equal("Изменение", auditRecord.OperationType);
                Assert.Equal(2, auditEntities.Length);

                int countOfClass1Fields = 0;

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
                DataObject[] auditEntities = dataService.LoadObjects(lcs);
                AuditRecord auditRecord = auditEntities.Cast<AuditRecord>().FirstOrDefault();

                // TODO: проверка полей аудита.

                // Assert.
                Assert.Equal("Изменение", auditRecord.OperationType);
                Assert.Equal(2, auditEntities.Length);

                int countOfClass1Fields = 0;

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
                DataObject[] auditEntities = dataService.LoadObjects(lcs);
                AuditRecord auditRecord = auditEntities.Cast<AuditRecord>().FirstOrDefault();

                // TODO: проверка полей аудита.

                // Assert.
                Assert.Equal("Создание", auditRecord.OperationType);
                Assert.Single(auditEntities);

                int countOfLinkedPrimaryKeyFields = 0;
                string text = $"Class1({AuditConstants.FieldNamePrimaryKey})";

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
                DataObject[] auditEntities = dataService.LoadObjects(lcs);
                AuditRecord auditRecord = auditEntities.Cast<AuditRecord>().FirstOrDefault();

                // TODO: проверка полей аудита.

                // Assert.
                Assert.Equal("Создание", auditRecord.OperationType);

                int countOfClass1AtAuditFields = 0;

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
