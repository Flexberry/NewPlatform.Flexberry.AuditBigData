[assembly: Xunit.CollectionBehavior(DisableTestParallelization = true)]

namespace ICSSoft.STORMNET.Business.Audit.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading;
    using ClickHouse.Ado;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.Audit.Objects;
    using NewPlatform.Flexberry.AuditBigData;
    using NewPlatform.Flexberry.AuditBigData.Tests;
    using NewPlatform.Flexberry.ORM;
    using Npgsql;
    using Oracle.ManagedDataAccess.Client;

    public abstract class BaseIntegratedTest : IDisposable
    {
        private const string poolingFalseConst = "Pooling=false;";

        /// <summary>
        /// The temporary database name prefix.
        /// </summary>
        private readonly string _tempDbNamePrefix;

        private string _databaseName;

        private string _tmpUserNameOracle;

        /// <summary>
        /// The data services for temp databases (for <see cref="DataServices"/>).
        /// </summary>
        private readonly List<IDataService> _dataServices = new List<IDataService>();

        /// <summary>
        /// The data services for temp clickhouse database (for <see cref="DataServices"/>).
        /// </summary>
        private readonly IDataService _clickHousedataServices;

        /// <summary>
        /// Flag: Indicates whether "Dispose" has already been called.
        /// </summary>
        private bool _disposed;

        protected virtual string MssqlScript
        {
            get
            {
                return Resources.MssqlScript;
            }
        }

        protected virtual string PostgresScript
        {
            get
            {
                return Resources.PostgresScript;
            }
        }

        protected virtual string OracleScript
        {
            get
            {
                return Resources.OracleScript;
            }
        }

        protected virtual string ClickHouseScript
        {
            get
            {
                return Resources.ClickHouseScript;
            }
        }

        /// <summary>
        /// Data services for temp databases.
        /// </summary>
        protected IEnumerable<IDataService> DataServices
        {
            get
            {
                if (_disposed)
                    throw new ObjectDisposedException(null);

                return _dataServices;
            }
        }

        /// <summary>
        /// Data services for temp databases.
        /// </summary>
        protected IDataService ClickHouseDataServices
        {
            get
            {
                if (_disposed)
                    throw new ObjectDisposedException(null);

                return _clickHousedataServices;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseIntegratedTest" /> class.
        /// </summary>
        /// <param name="tempDbNamePrefix">Prefix for temp database name.</param>
        protected BaseIntegratedTest(string tempDbNamePrefix)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string connectionStringsConfigSectionName = "connectionStrings";
            ConnectionStringsSection connectionStringsSection = (ConnectionStringsSection)configuration.GetSection(connectionStringsConfigSectionName);

            if (!(tempDbNamePrefix != null))
                throw new ArgumentNullException();
            if (!(tempDbNamePrefix != string.Empty))
                throw new ArgumentException();
            if (!tempDbNamePrefix.All(char.IsLetterOrDigit))
                throw new ArgumentException();
            _tempDbNamePrefix = tempDbNamePrefix;
            _databaseName = _tempDbNamePrefix + "_" + DateTime.Now.ToString("yyyyMMddHHmmssff") + "_" + Guid.NewGuid().ToString("N");
            if (!string.IsNullOrWhiteSpace(PostgresScript) && ConnectionStringPostgres != poolingFalseConst)
            {
                if (!(tempDbNamePrefix.Length <= 12)) // Max length is 63 (-18 -32).
                    throw new ArgumentException();
                if (!char.IsLetter(tempDbNamePrefix[0])) // Database names must have an alphabetic first character.
                    throw new ArgumentException();
                using (var conn = new NpgsqlConnection(ConnectionStringPostgres))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(string.Format("CREATE DATABASE \"{0}\" ENCODING = 'UTF8' CONNECTION LIMIT = -1;", _databaseName), conn))
                        cmd.ExecuteNonQuery();
                }

                using (var conn = new NpgsqlConnection($"{ConnectionStringPostgres};Database={_databaseName}"))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(PostgresScript, conn))
                        cmd.ExecuteNonQuery();
                    string connStr = $"{ConnectionStringPostgres};Database={_databaseName}";
                    PostgresDataService dataService = CreatePostgresDataService(connStr);
                    _dataServices.Add(dataService);

                    InitAuditService(dataService);

                    connectionStringsSection.ConnectionStrings.Add(new ConnectionStringSettings($"{dataService.AuditService.AppSetting.AppName}_{dataService.AuditService.AppSetting.AuditConnectionStringName}", connStr));
                }
            }

            if (!string.IsNullOrWhiteSpace(MssqlScript) && ConnectionStringMssql != poolingFalseConst)
            {
                if (!(tempDbNamePrefix.Length <= 64))// Max is 128.
                    throw new ArgumentException();
                using (var connection = new SqlConnection(ConnectionStringMssql))
                {
                    connection.Open();
                    using (var command = new SqlCommand($"CREATE DATABASE {_databaseName} COLLATE Cyrillic_General_CI_AS", connection))
                        command.ExecuteNonQuery();
                }

                using (var connection = new SqlConnection($"{ConnectionStringMssql};Database={_databaseName}"))
                {
                    connection.Open();
                    using (var command = new SqlCommand(MssqlScript, connection))
                    {
                        command.CommandTimeout = 180;
                        command.ExecuteNonQuery();
                    }

                    string connStr = $"{ConnectionStringMssql};Database={_databaseName}";
                    MSSQLDataService dataService = CreateMssqlDataService(connStr);
                    _dataServices.Add(dataService);
                    InitAuditService(dataService);
                    connectionStringsSection.ConnectionStrings.Add(new ConnectionStringSettings($"{dataService.AuditService.AppSetting.AppName}_{dataService.AuditService.AppSetting.AuditConnectionStringName}", connStr));
                }
            }

            if (!string.IsNullOrWhiteSpace(OracleScript) && ConnectionStringOracle != poolingFalseConst)
            {
                if (!(tempDbNamePrefix.Length <= 8)) // Max length is 30 (-18 -4).
                    throw new ArgumentException();

                using (var connection = new OracleConnection(ConnectionStringOracle))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        // "CREATE USER" privileges required.
                        var doWhile = true;
                        while (doWhile)
                        {
                            _tmpUserNameOracle = tempDbNamePrefix + "_" + DateTime.Now.ToString("yyyyMMddHHmmssff") + "_" + new Random().Next(9999);
                            command.CommandText = $"CREATE USER {_tmpUserNameOracle} IDENTIFIED BY {_tmpUserNameOracle} DEFAULT TABLESPACE users  quota unlimited on users  TEMPORARY TABLESPACE temp";
                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch (OracleException ex)
                            {
                                Thread.Sleep(1000);
                                if (ex.Message.Contains("conflicts with another user or role name "))
                                    continue;
                                throw;
                            }

                            doWhile = false;
                        }

                        // "CREATE SESSION WITH ADMIN OPTION" privileges required.
                        command.CommandText = $"GRANT CREATE SESSION TO {_tmpUserNameOracle}";
                        command.ExecuteNonQuery();
                        command.CommandText = $"GRANT CREATE TABLE TO {_tmpUserNameOracle}";
                        command.ExecuteNonQuery();
                    }
                }

                using (var connection = new OracleConnection($"{ConnectionStringOracleDataSource};User Id={_tmpUserNameOracle};Password={_tmpUserNameOracle};"))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        foreach (var cmdText in OracleScript.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            command.CommandText = cmdText.Trim();
                            if (!string.IsNullOrWhiteSpace(command.CommandText))
                                command.ExecuteNonQuery();
                        }

                        string connStr = $"{ConnectionStringOracleDataSource};User Id={_tmpUserNameOracle};Password={_tmpUserNameOracle};";
                        OracleDataService dataService = CreateOracleDataService(connStr);
                        _dataServices.Add(dataService);
                        InitAuditService(dataService);
                        connectionStringsSection.ConnectionStrings.Add(new ConnectionStringSettings($"{dataService.AuditService.AppSetting.AppName}_{dataService.AuditService.AppSetting.AuditConnectionStringName}", connStr));
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(ClickHouseScript) && ConnectionStringClickHouse != string.Empty)
            {
                if (!(tempDbNamePrefix.Length <= 12)) // Max length is 63 (-18 -32).
                    throw new ArgumentException();
                if (!char.IsLetter(tempDbNamePrefix[0])) // Database names must have an alphabetic first character.
                    throw new ArgumentException();

                var settings = new ClickHouseConnectionSettings(ConnectionStringClickHouse);

                using (var connection = new ClickHouseConnection(settings))
                {
                    connection.Open();

                    using (var cmd = new ClickHouseCommand(connection))
                    {
                        cmd.CommandText = $"CREATE DATABASE \"{_databaseName}\"";
                        cmd.ExecuteNonQuery();
                    }
                }

                settings = new ClickHouseConnectionSettings($"{ConnectionStringClickHouse};Database={_databaseName}");
                using (var connection = new ClickHouseConnection(settings))
                {
                    connection.Open();

                    string[] createTableCommands = ClickHouseScript.Split(';');

                    foreach (string createTableCommand in createTableCommands)
                    {
                        if (string.IsNullOrWhiteSpace(createTableCommand))
                        {
                            continue;
                        }

                        using (var cmd = new ClickHouseCommand(connection, createTableCommand))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                    string connectionString = $"{ConnectionStringClickHouse};Database={_databaseName}";
                    ClickHouseDataService dataService = CreateClickHouseDataService(connectionString);
                    _clickHousedataServices = dataService;

                    InitAuditService(dataService);

                    string appName = "_audit" + dataService.AuditService.AppSetting.AppName;
                    connectionStringsSection.ConnectionStrings.Add(new ConnectionStringSettings($"{appName}_{dataService.AuditService.AppSetting.AuditConnectionStringName}", connectionString));
                }
            }

            configuration.Save();
            ConfigurationManager.RefreshSection(connectionStringsConfigSectionName);
        }

        /// <summary>
        /// Creates the <see cref="MSSQLDataService"/> instance for temp database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The <see cref="MSSQLDataService"/> instance.</returns>
        protected virtual MSSQLDataService CreateMssqlDataService(string connectionString)
        {
            return new MSSQLDataService { CustomizationString = connectionString };
        }

        /// <summary>
        /// Creates the <see cref="PostgresDataService"/> instance for temp database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The <see cref="PostgresDataService"/> instance.</returns>
        protected virtual PostgresDataService CreatePostgresDataService(string connectionString)
        {
            return new PostgresDataService { CustomizationString = connectionString };
        }

        /// <summary>
        /// Creates the <see cref="OracleDataService"/> instance for temp database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The <see cref="OracleDataService"/> instance.</returns>
        protected virtual OracleDataService CreateOracleDataService(string connectionString)
        {
            return new OracleDataService { CustomizationString = connectionString };
        }

        /// <summary>
        /// Creates the <see cref="ClickHouseDataService"/> instance for temp database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The <see cref="ClickHouseDataService"/> instance.</returns>
        protected virtual ClickHouseDataService CreateClickHouseDataService(string connectionString)
        {
            return new ClickHouseDataService { CustomizationString = connectionString };
        }

        /// <summary>
        /// Deletes the temporary databases and perform other cleaning.
        /// </summary>
        /// <param name="disposing">Flag: indicates whether method is calling from "Dispose()" or not.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                try
                {
                    foreach (var ds in _dataServices)
                    {
                        ExecuteDisposeForDataservice(ds);
                    }
                }
                catch (Exception)
                {
                }
            }

            _disposed = true;
        }

        /// <summary>
        /// Execute dispose for target service.
        /// </summary>
        /// <param name="service">Disposing service.</param>
        protected virtual void ExecuteDisposeForDataservice(IDataService service)
        {
            if (service is PostgresDataService)
            {
                using (var conn = new NpgsqlConnection(ConnectionStringPostgres))
                {
                    conn.Open();
                    using (var command = new NpgsqlCommand($"DROP DATABASE \"{_databaseName}\";", conn))
                        command.ExecuteNonQuery();
                }
            }

            if (service is MSSQLDataService)
            {
                using (var connection = new SqlConnection(ConnectionStringMssql))
                {
                    connection.Open();
                    using (var command = new SqlCommand($"DROP DATABASE {_databaseName}", connection))
                        command.ExecuteNonQuery();
                }
            }

            if (service is OracleDataService)
            {
                using (var connection = new OracleConnection(ConnectionStringOracle))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = $"DROP USER {_tmpUserNameOracle} CASCADE";
                        command.ExecuteNonQuery();
                    }
                }
            }

            if (service is ClickHouseDataService)
            {
                var settings = new ClickHouseConnectionSettings(ConnectionStringClickHouse);

                using (var connection = new ClickHouseConnection(settings))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        /*command.CommandText = $"DROP USER {_tmpUserNameOracle} CASCADE";
                        command.ExecuteNonQuery();*/
                    }
                }
            }

            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string connectionStringsConfigSectionName = "connectionStrings";
            ConnectionStringsSection connectionStringsSection = (ConnectionStringsSection)configuration.GetSection(connectionStringsConfigSectionName);
            connectionStringsSection.ConnectionStrings.Remove($"{service.AuditService.AppSetting.AppName}_{service.AuditService.AppSetting.AuditConnectionStringName}");
            configuration.Save();
            ConfigurationManager.RefreshSection(connectionStringsConfigSectionName);
        }

        /// <summary>
        /// Deletes the temporary databases and perform other cleaning.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        private static string ConnectionStringPostgres
        {
            get
            {
                // ADO.NET doesn't close the connection with pooling. We have to disable it explicitly.
                // http://stackoverflow.com/questions/9033356/connection-still-idle-after-close
                return $"{poolingFalseConst}{ConfigurationManager.ConnectionStrings["ConnectionStringPostgres"]}";
            }
        }

        private static string ConnectionStringMssql
        {
            get
            {
                // ADO.NET doesn't close the connection with pooling. We have to disable it explicitly.
                // http://stackoverflow.com/questions/9033356/connection-still-idle-after-close
                return $"{poolingFalseConst}{ConfigurationManager.ConnectionStrings["ConnectionStringMssql"]}";
            }
        }

        private static string ConnectionStringOracle
        {
            get
            {
                // ADO.NET doesn't close the connection with pooling. We have to disable it explicitly.
                // http://stackoverflow.com/questions/9033356/connection-still-idle-after-close
                return $"{poolingFalseConst}{ConfigurationManager.ConnectionStrings["ConnectionStringOracle"]}";
            }
        }

        private static string ConnectionStringClickHouse
        {
            get
            {
                // ADO.NET doesn't close the connection with pooling. We have to disable it explicitly.
                // http://stackoverflow.com/questions/9033356/connection-still-idle-after-close
                return $"{ConfigurationManager.ConnectionStrings["ConnectionStringClickHouse"]}";
            }
        }

        private static string ConnectionStringOracleDataSource
        {
            get
            {
                var dataSource = ConnectionStringOracle.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(t => t.Trim().ToLower().IndexOf("data") == 0 && t.ToLower().IndexOf("source") != -1);

                // ADO.NET doesn't close the connection with pooling. We have to disable it explicitly.
                // http://stackoverflow.com/questions/9033356/connection-still-idle-after-close
                return $"{poolingFalseConst}{dataSource};";
            }
        }

        /// <summary>
        /// Провести инициализацию сервиса аудита.
        /// </summary>
        /// <param name="dataService">Сервис данных аудита.</param>
        private static void InitAuditService(IDataService dataService)
        {
            var auditAppSetting = new AuditAppSetting
            {
                AppName = "Tests",
                AuditEnabled = true,
                IsDatabaseLocal = true,
                AuditConnectionStringName = dataService.AuditService.AppSetting.AuditConnectionStringName,
                AuditWinServiceUrl = null,
                WriteSessions = false,
                DefaultWriteMode = tWriteMode.Synchronous,
            };

            var auditDsSetting = new AuditDSSetting(dataService, $"{auditAppSetting.AppName}_{auditAppSetting.AuditConnectionStringName}");
            auditAppSetting.AuditDSSettings.Add(auditDsSetting);

            AuditService.InitAuditService(auditAppSetting, new LegacyAuditManager(dataService, new LegacyAuditSerializer()), dataService.AuditService);
        }
    }
}
