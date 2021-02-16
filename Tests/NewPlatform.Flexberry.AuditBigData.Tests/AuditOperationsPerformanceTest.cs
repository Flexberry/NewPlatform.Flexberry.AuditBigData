namespace ICSSoft.STORMNET.Business.Audit.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Audit.Objects;
    using NewPlatform.Flexberry.Audit.Tests;
    using Xunit;

    /// <summary>
    /// Проверка быстродействия операций с данными. С использование аудита и без.
    /// </summary>
    public class AuditOperationsPerformanceTest : BaseAuditServiceTest
    {
        /// <summary>
        /// Вставка записей без использования аудита.
        /// </summary>
        [Fact]
        public void InsertPerformaceTestWithoutAudit()
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
                List<DataObject> dataObjects = new List<DataObject>();

                for (int i = 0; i < 1000; i++)
                {
                    // Два класса детейлов. будут назначаться рандомно.
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

                    dataObjects.Add(class2);

                    if (i % 100 == 0)
                    {
                        DataObject[] dataObjectsArray = dataObjects.ToArray();
                        dataService.UpdateObjects(ref dataObjectsArray);

                        dataObjects.Clear();
                    }
                }
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
