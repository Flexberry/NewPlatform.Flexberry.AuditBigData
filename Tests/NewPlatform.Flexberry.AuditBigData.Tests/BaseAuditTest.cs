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
    /// Базовые тесты аудита.
    /// </summary>
    public class BaseAuditTest : BaseAuditServiceTest
    {
        /// <summary>
        /// Базовый тест аудита создания объекта.
        /// </summary>
        [Fact]
        public void AuditCreateObjectAuditTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                string firstFild = "Field11";
                string secondFild = "Field12";

                Class1 class1 = new Class1
                {
                    Field11 = firstFild,
                    Field12 = secondFild,
                };

                // Act.
                dataService.UpdateObject(class1);

                ExternalLangDef langDef = ExternalLangDef.LanguageDef;

                // Вычитка записей аудита.
                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(AuditRecord), AuditRecord.Views.AllFields);
                lcs.LimitFunction = langDef.GetFunction(langDef.funcEQ, new VariableDef(langDef.GuidType, Information.ExtractPropertyPath<AuditRecord>(x => x.ObjectPrimaryKey)), class1.__PrimaryKey);
                DataObject[] auditEntities = dataService.LoadObjects(lcs);
                AuditRecord auditRecord = auditEntities.Cast<AuditRecord>().FirstOrDefault();

                // TODO: проверка полей аудита.

                // Assert.
                Assert.Equal("Создание", auditRecord.OperationType);
                Assert.Single(auditEntities);
            }
        }

        /// <summary>
        /// Базовый тест аудита обновления объекта.
        /// </summary>
        [Fact]
        public void AuditUpdateObjectAuditTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                string firstFild1 = "Field11";
                string secondFild1 = "Field12";

                string firstFild2 = "Field11AfterChange";
                string secondFild2 = "Field12fterChange";

                Class1 class1 = new Class1
                {
                    Field11 = firstFild1,
                    Field12 = secondFild1,
                };

                // Act.
                dataService.UpdateObject(class1);

                class1.Field11 = firstFild2;
                class1.Field12 = secondFild2;

                dataService.UpdateObject(class1);

                ExternalLangDef langDef = ExternalLangDef.LanguageDef;

                // Вычитка записей аудита.
                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(AuditRecord), AuditRecord.Views.AllFields);
                lcs.LimitFunction = langDef.GetFunction(langDef.funcEQ, new VariableDef(langDef.GuidType, Information.ExtractPropertyPath<AuditRecord>(x => x.ObjectPrimaryKey)), class1.__PrimaryKey);
                string sortProperty = Information.ExtractPropertyPath<AuditRecord>(x => x.OperationTime);
                lcs.ColumnsSort = new[] { new ColumnsSortDef(sortProperty, SortOrder.Desc) };
                DataObject[] auditEntities = dataService.LoadObjects(lcs);
                AuditRecord auditRecord = auditEntities.Cast<AuditRecord>().FirstOrDefault();

                // TODO: проверка полей аудита.

                // Assert.
                Assert.Equal("Изменение", auditRecord.OperationType);
                Assert.Equal(2, auditEntities.Length);
            }
        }

        /// <summary>
        /// Базовый тест аудита удаления объекта.
        /// </summary>
        [Fact]
        public void AuditDeleteObjectAuditTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                string firstFild = "Field11";
                string secondFild = "Field12";

                Class1 class1 = new Class1
                {
                    Field11 = firstFild,
                    Field12 = secondFild,
                };

                // Act.
                dataService.UpdateObject(class1);

                class1.SetStatus(ObjectStatus.Deleted);

                dataService.UpdateObject(class1);

                ExternalLangDef langDef = ExternalLangDef.LanguageDef;

                // Вычитка записей аудита.
                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(AuditRecord), AuditRecord.Views.AllFields);
                lcs.LimitFunction = langDef.GetFunction(langDef.funcEQ, new VariableDef(langDef.GuidType, Information.ExtractPropertyPath<AuditRecord>(x => x.ObjectPrimaryKey)), class1.__PrimaryKey);
                string sortProperty = Information.ExtractPropertyPath<AuditRecord>(x => x.OperationTime);
                lcs.ColumnsSort = new[] { new ColumnsSortDef(sortProperty, SortOrder.Desc) };
                DataObject[] auditEntities = dataService.LoadObjects(lcs);
                AuditRecord auditRecord = auditEntities.Cast<AuditRecord>().FirstOrDefault();

                // TODO: проверка полей аудита.

                // Assert.
                Assert.Equal("Удаление", auditRecord.OperationType);
                Assert.Equal(2, auditEntities.Length);
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
