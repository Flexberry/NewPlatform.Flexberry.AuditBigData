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
        /// <summary>
        /// Количество создаваемых записей для теста.
        /// </summary>
        private const int _recordsCount = 1000;

        /// <summary>
        /// Экземпляр класса для вывода сообщений.
        /// </summary>
        private readonly ITestOutputHelper output;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="output"> Экземпляр класса для вывода сообщений.</param>
        public AuditOperationsPerformanceTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        /// <summary>
        /// Вставка записей с использованием аудита на основе БД clickhouse.
        /// </summary>
        [Fact(Skip = "ClickHouseDataService is not implemented yet. Skip performance test.")]
        public void PerformaceTestClickHouse()
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
                List<DataObject> createdDataObjects = new List<DataObject>();

                Stopwatch timerForInsertData = new Stopwatch();
                timerForInsertData.Start();

                // Создаем записи.
                for (int i = 1; i <= _recordsCount; i++)
                {
                    // Два класса детейлов.
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

                    class2.Class3.Add(detailClass1);
                    class2.Class3.Add(detailClass2);

                    createdDataObjects.Add(class2);
                    DataObject[] dataObjects = new DataObject[] { class2 };

                    IDataService clickHousedataServices = ClickHouseDataServices;
                    dataService.AuditService.Audit = clickHousedataServices.AuditService.Audit;
                    dataService.UpdateObjects(ref dataObjects);
                }

                timerForInsertData.Stop();
                float insertTime = timerForInsertData.ElapsedMilliseconds;
                int averageInsertTime = (int)insertTime / _recordsCount;

                string messageForInsert = $"{dataService.GetType()} Insert {_recordsCount} records takes milliseconds - {insertTime}. Average Insert Time - {averageInsertTime}";
                output.WriteLine(messageForInsert);
            }
        }

        /// <summary>
        /// Вставка, изменение и удаление записей без использования аудита.
        /// </summary>
        [Fact(Skip = "Skip performance test.")]
        public void PerformaceTestWithoutAudit()
        {
            ExecuteOperations(true);
        }

        /// <summary>
        /// Вставка, изменение и удаление записей с использованием аудита.
        /// </summary>
        [Fact(Skip = "Skip performance test.")]
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
        /// <param name="disableAudit)">Флаг отключения аудита.</param>
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
                    dataService.AuditService.DisableAudit();
                    dataService.AuditService.AppSetting.AuditConnectionStringName = string.Empty;
                }

                List<DataObject> createdDataObjects = new List<DataObject>();

                Stopwatch timerForInsertData = new Stopwatch();
                timerForInsertData.Start();

                // Создаем записи.
                for (int i = 1; i <= _recordsCount; i++)
                {
                    // Два класса детейлов.
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

                    class2.Class3.Add(detailClass1);
                    class2.Class3.Add(detailClass2);

                    createdDataObjects.Add(class2);
                    DataObject[] dataObjects = new DataObject[] { class2 };
                    dataService.UpdateObjects(ref dataObjects);
                }

                timerForInsertData.Stop();
                float insertTime = timerForInsertData.ElapsedMilliseconds;
                int averageInsertTime = (int)insertTime / _recordsCount;

                string auditCaption = disableAudit ? " without audit" : "with audit";
                string messageForInsert = $"{dataService.GetType()} Insert {_recordsCount} records {auditCaption} takes milliseconds - {insertTime}. Average Insert Time - {averageInsertTime}";
                output.WriteLine(messageForInsert);

                // Операция изменения данных
                Stopwatch timerForUpdateData = new Stopwatch();
                timerForUpdateData.Start();

                foreach (Class2 createdClass in createdDataObjects)
                {
                    Class2 updatedClass = createdClass;

                    updatedClass.Field21 = RandomStringGenerator(randomizer);
                    updatedClass.Class1 = randomizer.Next(0, 2) != 0 ? masterClass1 : masterClass2;

                    foreach (Class3 detail in updatedClass.Class3)
                    {
                        detail.Field31 = string.Concat("detail_", RandomStringGenerator(randomizer));
                        detail.Field32 = string.Concat("detail_", RandomStringGenerator(randomizer));
                    }

                    DataObject[] dataObjects = new DataObject[] { updatedClass };
                    dataService.UpdateObjects(ref dataObjects);
                }

                timerForUpdateData.Stop();
                float updateTime = timerForUpdateData.ElapsedMilliseconds;
                int averageUpdateTime = (int)updateTime / _recordsCount;

                string messageForUpdate = $"{dataService.GetType()} Update {_recordsCount} records {auditCaption} takes milliseconds - {updateTime}. Average Update Time - {averageUpdateTime}";
                output.WriteLine(messageForUpdate);

                // Операция удаления данных
                Stopwatch timerForDeleteData = new Stopwatch();
                timerForDeleteData.Start();

                foreach (Class2 createdClass in createdDataObjects)
                {
                    Class2 deletedClass = createdClass;
                    DataObject[] dataObjects = new DataObject[] { deletedClass };
                    dataObjects[0].SetStatus(ObjectStatus.Deleted);
                    dataService.UpdateObjects(ref dataObjects);
                }

                timerForDeleteData.Stop();
                float deleteTime = timerForDeleteData.ElapsedMilliseconds;
                int averageDeleteTime = (int)deleteTime / _recordsCount;

                string messageForDelete = $"{dataService.GetType()} Delete {_recordsCount} records {auditCaption} takes milliseconds - {deleteTime}. Average Delete Time - {averageDeleteTime}";
                output.WriteLine(messageForDelete);
            }
        }

        /// <summary>
        /// Формирует случайную строку для значений полей.
        /// </summary>
        /// <param name="random"> Экземпляр генератора, созданного заранее.</param>
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
