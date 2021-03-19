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
                lcs.LimitFunction = langDef.GetFunction(
                    langDef.funcEQ,
                    new VariableDef(langDef.GuidType, Information.ExtractPropertyPath<AuditRecord>(x => x.ObjectPrimaryKey)),
                    class1.__PrimaryKey);
                DataObject[] primaryAuditObjects = dataService.LoadObjects(lcs);
                DataObject[] ratifyingAuditObjects = GetRatifyingAuditObjects(dataService, primaryAuditObjects);
                string operationType = AuditOperationType.Insert.ToString();
                AuditRecord primaryAuditRecord = primaryAuditObjects.Cast<AuditRecord>().FirstOrDefault(x => x.OperationType == operationType);

                // TODO: проверка полей аудита.

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
                lcs.LimitFunction = langDef.GetFunction(
                    langDef.funcEQ,
                    new VariableDef(langDef.GuidType, Information.ExtractPropertyPath<AuditRecord>(x => x.ObjectPrimaryKey)),
                    class1.__PrimaryKey);
                string sortProperty = Information.ExtractPropertyPath<AuditRecord>(x => x.OperationTime);
                lcs.ColumnsSort = new[] { new ColumnsSortDef(sortProperty, SortOrder.Desc) };
                DataObject[] primaryAuditObjects = dataService.LoadObjects(lcs);
                DataObject[] ratifyingAuditObjects = GetRatifyingAuditObjects(dataService, primaryAuditObjects);
                string operationType = AuditOperationType.Update.ToString();
                AuditRecord primaryAuditRecord = primaryAuditObjects.Cast<AuditRecord>().FirstOrDefault(x => x.OperationType == operationType);

                // TODO: проверка полей аудита.

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
                lcs.LimitFunction = langDef.GetFunction(
                    langDef.funcEQ,
                    new VariableDef(langDef.GuidType, Information.ExtractPropertyPath<AuditRecord>(x => x.ObjectPrimaryKey)),
                    class1.__PrimaryKey);
                string sortProperty = Information.ExtractPropertyPath<AuditRecord>(x => x.OperationTime);
                lcs.ColumnsSort = new[] { new ColumnsSortDef(sortProperty, SortOrder.Desc) };
                DataObject[] primaryAuditObjects = dataService.LoadObjects(lcs);
                DataObject[] ratifyingAuditObjects = GetRatifyingAuditObjects(dataService, primaryAuditObjects);
                string operationType = AuditOperationType.Delete.ToString();
                AuditRecord primaryAuditRecord = primaryAuditObjects.Cast<AuditRecord>().FirstOrDefault(x => x.OperationType == operationType);

                // TODO: проверка полей аудита.

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
