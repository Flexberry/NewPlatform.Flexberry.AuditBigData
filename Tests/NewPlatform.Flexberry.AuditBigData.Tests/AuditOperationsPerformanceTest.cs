namespace ICSSoft.STORMNET.Business.Audit.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Audit.Objects;
    using NewPlatform.Flexberry.Audit.Tests;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Проверка быстродействия операций с данными. С использование аудита и без.
    /// </summary>
    public class AuditOperationsPerformanceTest : BaseAuditServiceTest
    {
        // Количество создаваемых записей для теста.
        private const int RECORDS_COUNT = 10000;

        private readonly ITestOutputHelper output;

        public AuditOperationsPerformanceTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        /// <summary>
        /// Вставка записей без использования аудита.
        /// </summary>
        [Fact]
        public void PerformaceTestWithoutAudit()
        {
            ExecuteOperations(true);
        }

        /// <summary>
        /// Вставка записей с использованием аудита.
        /// </summary>
        [Fact]
        public void PerformaceTestWithAudit()
        {
            ExecuteOperations();
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

        /// <summary>
        /// Выполнить операции создания, изменения и удаления записей.
        /// </summary>
        /// <param name="disableAudit)">Флаг отключения.</param>
        private void ExecuteOperations(bool disableAudit = false)
        {
            // Два класса мастеров. Будут назначаться рандомно.
            Class1 masterClass1 = new Class1
            {
                Field11 = "MasterClass_1_Field_1",
                Field12 = "MasterClass_1_Field_2",
            };

            Class1 masterClass2 = new Class1
            {
                Field11 = "MasterClass_2_Field_1",
                Field12 = "MasterClass_2_Field_2",
            };

            Random randomizer = new Random();

            foreach (IDataService dataService in DataServices)
            {
                if (disableAudit)
                {
                    // Отключаем аудит.
                    dataService.AuditService.AppSetting.AuditConnectionStringName = string.Empty;
                }

                List<DataObject> createdDataObjects = new List<DataObject>();

                Stopwatch timerForInsertData = new Stopwatch();
                timerForInsertData.Start();

                // Создаем записи.
                for (int i = 1; i <= RECORDS_COUNT; i++)
                {
                    // Два класса детейлов. Будут назначаться рандомно.
                    Class3 detailClass1 = new Class3
                    {
                        Field31 = string.Concat("detail_1_", RandomStringGenerator(randomizer)),
                        Field32 = string.Concat("detail_1_", RandomStringGenerator(randomizer)),
                    };

                    Class3 detailClass2 = new Class3
                    {
                        Field31 = string.Concat("detail_2_", RandomStringGenerator(randomizer)),
                        Field32 = string.Concat("detail_2_", RandomStringGenerator(randomizer)),
                    };

                    Class2 class2 = new Class2
                    {
                        Field21 = RandomStringGenerator(randomizer),
                        Field22 = RandomStringGenerator(randomizer),
                        Class1 = randomizer.Next(0, 2) != 0 ? masterClass1 : masterClass2,
                    };

                    Class3 detail = randomizer.Next(0, 2) != 0 ? detailClass1 : detailClass2;
                    class2.Class3.Add(detail);

                    createdDataObjects.Add(class2);
                    DataObject[] dataObjects = new DataObject[] { class2 };
                    dataService.UpdateObjects(ref dataObjects);
                }

                timerForInsertData.Stop();
                float insertTime = timerForInsertData.ElapsedMilliseconds;

                string auditCaption = disableAudit ? " without audit" : "with audit";
                string messageForInsert = $"{dataService.GetType()} Insert {RECORDS_COUNT} records {auditCaption} takes milliseconds - {insertTime}";
                output.WriteLine(messageForInsert);

                // Операция изменения данных
                Stopwatch timerForUpdateData = new Stopwatch();
                timerForUpdateData.Start();

                foreach (Class2 createdClass in createdDataObjects)
                {
                    Class2 updatedClass = createdClass;

                    updatedClass.Field21 = RandomStringGenerator(randomizer);
                    DataObject[] dataObjects = new DataObject[] { updatedClass };
                    dataService.UpdateObjects(ref dataObjects);
                }

                timerForUpdateData.Stop();
                float updateTime = timerForUpdateData.ElapsedMilliseconds;

                string messageForUpdate = $"{dataService.GetType()} Update {RECORDS_COUNT} records {auditCaption} takes milliseconds - {updateTime}";
                output.WriteLine(messageForUpdate);

                // Операция удаления данных
                Stopwatch timerForDeleteData = new Stopwatch();
                timerForDeleteData.Start();

                foreach (Class2 createdClass in createdDataObjects)
                {
                    Class2 updatedClass = createdClass;

                    updatedClass.Field21 = RandomStringGenerator(randomizer);
                    DataObject[] dataObjects = new DataObject[] { updatedClass };
                    dataObjects[0].SetStatus(ObjectStatus.Deleted);
                    dataService.UpdateObjects(ref dataObjects);
                }

                timerForDeleteData.Stop();
                float deleteTime = timerForDeleteData.ElapsedMilliseconds;

                string messageForDelete = $"{dataService.GetType()} Delete {RECORDS_COUNT} records {auditCaption} takes milliseconds - {deleteTime}";
                output.WriteLine(messageForDelete);
            }
        }

        /// <summary>
        /// Формирует случайную строку для значений полей.
        /// </summary>
        private string RandomStringGenerator(Random random)
        {
            int length = 7;

            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }

            return result.ToString();
        }
    }
}
