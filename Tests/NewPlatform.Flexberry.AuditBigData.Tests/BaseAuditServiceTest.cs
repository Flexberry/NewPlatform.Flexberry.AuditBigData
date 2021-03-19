namespace ICSSoft.STORMNET.Business.Audit.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Security;
    using ICSSoft.STORMNET.Windows.Forms;
    using NewPlatform.Flexberry.AuditBigData;

    public abstract class BaseAuditServiceTest : BaseIntegratedTest
    {
        protected BaseAuditServiceTest()
            : base("AuditTests")
        {
        }

        /// <summary>
        /// Returns ratifying audit records list for the specified primary audit records list.
        /// </summary>
        /// <param name="dataService">Audit data service.</param>
        /// <param name="primaryAuditObjects">Primary audit records list.</param>
        /// <returns>Ratifying audit records as <see cref="DataObject"/> array.</returns>
        protected static DataObject[] GetRatifyingAuditObjects(IDataService dataService, DataObject[] primaryAuditObjects)
        {
            ExternalLangDef langDef = ExternalLangDef.LanguageDef;
            DataObject[] result = null;

            if (primaryAuditObjects.Any())
            {
                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(AuditRecord), AuditRecord.Views.AllFields);
                List<object> prmList = primaryAuditObjects.Select(x => x.__PrimaryKey).ToList();
                prmList.Add(new VariableDef(langDef.GuidType, Information.ExtractPropertyPath<AuditRecord>(x => x.HeadAuditEntity)));
                prmList.Reverse();
                lcs.LimitFunction = langDef.GetFunction(
                    langDef.funcIN,
                    prmList.ToArray());
                result = dataService.LoadObjects(lcs);
            }

            return result ?? Array.Empty<AuditRecord>();

        }

        /// <summary>
        /// Creates the <see cref="MSSQLDataService" /> instance for temp database with audit service for testing.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The <see cref="MSSQLDataService" /> instance.</returns>
        protected override MSSQLDataService CreateMssqlDataService(string connectionString)
        {
            return new MSSQLDataService(new EmptySecurityManager(), GetAuditServiceForTest()) { CustomizationString = connectionString };
        }

        /// <summary>
        /// Creates the <see cref="PostgresDataService" /> instance for temp database with audit service for testing.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The <see cref="PostgresDataService" /> instance.</returns>
        protected override PostgresDataService CreatePostgresDataService(string connectionString)
        {
            return new PostgresDataService(new EmptySecurityManager(), GetAuditServiceForTest()) { CustomizationString = connectionString };
        }

        /// <summary>
        /// Creates the <see cref="OracleDataService" /> instance for temp database with audit service for testing.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The <see cref="OracleDataService" /> instance.</returns>
        protected override OracleDataService CreateOracleDataService(string connectionString)
        {
            return new OracleDataService(new EmptySecurityManager(), GetAuditServiceForTest()) { CustomizationString = connectionString };
        }

        /// <summary>
        /// Gets the audit service for the test.
        /// </summary>
        /// <returns>Returns instance of the <see cref="AuditService"/> class that will be used for the test.</returns>
        protected abstract AuditService GetAuditServiceForTest();
    }
}