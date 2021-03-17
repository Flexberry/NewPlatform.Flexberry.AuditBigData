﻿namespace NewPlatform.Flexberry.AuditBigData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Audit.Exceptions;
    using ICSSoft.STORMNET.Business.Audit.HelpStructures;
    using ICSSoft.STORMNET.Business.Audit.Objects;
    using ICSSoft.STORMNET.KeyGen;

    /// <summary>
    /// Audit manager.
    /// </summary>
    public class AuditManager : IAudit
    {
        private readonly IAuditSerializer auditSerializer;
        private readonly IDataService dataService;

        /// <summary>
        /// Initialize new instance <see cref="AuditManager"/>.
        /// </summary>
        /// <param name="dataService">IDataSerice instance to write <see cref="AuditRecord"/>.</param>
        /// <param name="auditSerializer">IAuditSerializer instance to serialize fields audit data.</param>
        public AuditManager(IDataService dataService, IAuditSerializer auditSerializer)
        {
            if (dataService == null)
            {
                throw new ArgumentNullException(nameof(dataService));
            }

            if (auditSerializer == null)
            {
                throw new ArgumentNullException(nameof(auditSerializer));
            }

            this.dataService = dataService;
            this.auditSerializer = auditSerializer;
        }

        /// <summary>
        /// Flag indicates that this service uses <see cref="LogService.LogInfo(object)" /> and <see cref="LogService.LogInfoFormat" /> to log audit operation information.
        /// Default is <see langword="true" />.
        /// </summary>
        public bool DetailedLogEnabled { get; set; } = true;

        /// <inheritdoc/>
        public void RatifyAuditOperation(RatificationAuditParameters ratificationAuditParameters)
        {
            if (ratificationAuditParameters == null)
            {
                throw new ArgumentNullException(nameof(ratificationAuditParameters));
            }

            if (ratificationAuditParameters.AuditOperationInfoList == null)
            {
                throw new Exception("Invalid audit parameters: null audit operation info list.");
            }

            var failedGuids = new List<Guid>();

            // Создаём соответствующие записи аудита.
            foreach (var auditAdditionalInfo in ratificationAuditParameters.AuditOperationInfoList)
            {
                try
                {
                    AuditRecord auditRecord = CreateRatifyingAuditRecord(
                        ratificationAuditParameters.CurrentTime,
                        ratificationAuditParameters.ExecutionResult,
                        auditSerializer.Serialize(auditAdditionalInfo),
                        auditAdditionalInfo.AuditRecordPrimaryKey);

                    dataService.UpdateObject(auditRecord);

                    if (DetailedLogEnabled)
                    {
                        LogService.LogInfoFormat("Audit, RatifyAuditOperation: у записи с ключом {0} статус изменён на {1}", auditAdditionalInfo.AuditRecordPrimaryKey, ratificationAuditParameters.ExecutionResult);
                    }
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    LogService.LogError("Audit, RatifyAuditOperation: " + ex.Message, ex);
                    failedGuids.Add(auditAdditionalInfo.AuditRecordPrimaryKey);
                }
            }

            if (failedGuids.Count > 0)
            {
                throw new RatifyExecutionFailedAuditException(failedGuids);
            }
        }

        /// <inheritdoc/>
        public Guid WriteCommonAuditOperation(CommonAuditParameters commonAuditParameters)
        {
            if (commonAuditParameters == null)
            {
                throw new ArgumentNullException(nameof(commonAuditParameters));
            }

            if (commonAuditParameters.OperatedObject == null)
            {
                throw new Exception("Invalid audit parameters: null operated object.");
            }

            try
            {
                AuditRecord auditRecord = CreatePrimaryAuditRecord(
                    commonAuditParameters.UserName,
                    commonAuditParameters.FullUserLogin,
                    commonAuditParameters.OperatedObject.GetType().AssemblyQualifiedName,
                    commonAuditParameters.OperatedObject.__PrimaryKey,
                    commonAuditParameters.CurrentTime,
                    TypeOfAuditOperation2AuditOperationType(commonAuditParameters.TypeOfAuditOperation).ToString(),
                    commonAuditParameters.ExecutionResult,
                    commonAuditParameters.OperationSource,
                    auditSerializer.Serialize(commonAuditParameters),
                    commonAuditParameters.AuditEntityGuid);

                dataService.UpdateObject(auditRecord);

                if (DetailedLogEnabled)
                {
                    LogService.LogInfoFormat(
                    "Audit, WriteCommonAuditOperation: объект {0}:{1} записан в аудит с ключом {2}",
                    auditRecord.ObjectPrimaryKey,
                    commonAuditParameters.OperatedObject,
                    auditRecord.__PrimaryKey);
                }

                return ((KeyGuid)auditRecord.__PrimaryKey).Guid;
            }
            catch (Exception ex)
            {
                LogService.LogError("Audit, WriteCommonAuditOperation: " + ex.Message, ex);
                throw new ExecutionFailedAuditException(ex);
            }
        }

        /// <inheritdoc/>
        public Guid WriteCustomAuditOperation(CheckedCustomAuditParameters checkedCustomAuditParameters)
        {
            if (checkedCustomAuditParameters == null)
            {
                throw new ArgumentNullException(nameof(checkedCustomAuditParameters));
            }

            try
            {
                AuditRecord auditRecord = CreatePrimaryAuditRecord(
                    checkedCustomAuditParameters.UserName,
                    checkedCustomAuditParameters.FullUserLogin,
                    checkedCustomAuditParameters.AuditObjectTypeOrDescription,
                    checkedCustomAuditParameters.AuditObjectPrimaryKey,
                    checkedCustomAuditParameters.CurrentTime,
                    CustomOperation2BigDataCustomOperation(checkedCustomAuditParameters.CustomOperation),
                    checkedCustomAuditParameters.ExecutionResult,
                    checkedCustomAuditParameters.OperationSource,
                    auditSerializer.Serialize(checkedCustomAuditParameters.CustomAuditFieldList),
                    checkedCustomAuditParameters.AuditEntityGuid);

                dataService.UpdateObject(auditRecord);

                if (DetailedLogEnabled)
                {
                    LogService.LogInfoFormat(
                        "Audit, WriteCustomAuditOperation: объект {0}:{1} записан в аудит с ключом {2}",
                        checkedCustomAuditParameters.AuditObjectPrimaryKey,
                        checkedCustomAuditParameters.AuditObjectTypeOrDescription,
                        auditRecord.__PrimaryKey);
                }

                return ((KeyGuid)auditRecord.__PrimaryKey).Guid;
            }
            catch (Exception ex)
            {
                LogService.LogError("Audit, WriteCustomAuditOperation: " + ex.Message, ex);
                throw new ExecutionFailedAuditException(ex);
            }
        }

        /// <summary>
        /// Создать и заполнить ратифицирующую запись аудита данными из параметров.
        /// </summary>
        /// <param name="operationTime">OperationTime.</param>
        /// <param name="executionStatus">ExecutionStatus.</param>
        /// <param name="serializedFields">SerializedFields.</param>
        /// <param name="headAuditEntityPrimaryKey">Head audit entity primary key.</param>
        /// <returns>Запись аудита.</returns>
        protected virtual AuditRecord CreateRatifyingAuditRecord(
            DateTime operationTime,
            ExecutionStatus executionStatus,
            string serializedFields,
            object headAuditEntityPrimaryKey)
        {
            var primaryAuditRecord = new AuditRecord();
            primaryAuditRecord.SetExistObjectPrimaryKey(headAuditEntityPrimaryKey);

            return new AuditRecord()
            {
                OperationTime = operationTime,
                OperationType = AuditOperationType.Ratify.ToString(),
                ExecutionStatus = executionStatus,
                SerializedFields = serializedFields,
                HeadAuditEntity = primaryAuditRecord,
            };
        }

        /// <summary>
        /// Создать и заполнить ратифицирующую запись аудита данными из параметров.
        /// </summary>
        /// <param name="operationTime">OperationTime.</param>
        /// <param name="executionVariant">ExecutionResult.</param>
        /// <param name="serializedFields">SerializedFields.</param>
        /// <param name="headAuditEntityPrimaryKey">Head audit entity primary key.</param>
        /// <returns>Запись аудита.</returns>
        protected virtual AuditRecord CreateRatifyingAuditRecord(
            DateTime operationTime,
            tExecutionVariant executionVariant,
            string serializedFields,
            object headAuditEntityPrimaryKey)
        {
            return CreateRatifyingAuditRecord(
                operationTime,
                ExecutionVariant2ExecutionStatus(executionVariant),
                serializedFields,
                headAuditEntityPrimaryKey);
        }

        /// <summary>
        /// Создать и заполнить первичную запись аудита данными из параметров.
        /// </summary>
        /// <param name="userName">UserName.</param>
        /// <param name="userLogin">UserLogin.</param>
        /// <param name="objectType">ObjectType.</param>
        /// <param name="objectPrimaryKey">ObjectPrimaryKey.</param>
        /// <param name="operationTime">OperationTime.</param>
        /// <param name="operationType">OperationType.</param>
        /// <param name="executionStatus">ExecutionStatus.</param>
        /// <param name="source">Source.</param>
        /// <param name="serializedFields">SerializedFields.</param>
        /// <param name="auditEntityGuid">AuditEntityGuid.</param>
        /// <returns>Запись аудита.</returns>
        protected virtual AuditRecord CreatePrimaryAuditRecord(
            string userName,
            string userLogin,
            string objectType,
            object objectPrimaryKey,
            DateTime operationTime,
            string operationType,
            ExecutionStatus executionStatus,
            string source,
            string serializedFields,
            Guid? auditEntityGuid)
        {
            var auditRecord = new AuditRecord()
            {
                UserName = userName,
                UserLogin = userLogin,
                ObjectType = objectType,
                ObjectPrimaryKey = objectPrimaryKey,
                OperationTime = operationTime,
                OperationType = operationType,
                ExecutionStatus = executionStatus,
                Source = source,
                SerializedFields = serializedFields,
            };

            if (auditEntityGuid != null)
            {
                auditRecord.__PrimaryKey = auditEntityGuid.Value;
            }

            return auditRecord;
        }

        /// <summary>
        /// Создать и заполнить первичную запись аудита данными из параметров.
        /// </summary>
        /// <param name="userName">UserName.</param>
        /// <param name="userLogin">UserLogin.</param>
        /// <param name="objectType">ObjectType.</param>
        /// <param name="objectPrimaryKey">ObjectPrimaryKey.</param>
        /// <param name="operationTime">OperationTime.</param>
        /// <param name="operationType">OperationType.</param>
        /// <param name="executionVariant">ExecutionResult.</param>
        /// <param name="source">Source.</param>
        /// <param name="serializedFields">SerializedFields.</param>
        /// <param name="auditEntityGuid">AuditEntityGuid.</param>
        /// <returns>Запись аудита.</returns>
        protected virtual AuditRecord CreatePrimaryAuditRecord(
            string userName,
            string userLogin,
            string objectType,
            object objectPrimaryKey,
            DateTime operationTime,
            string operationType,
            tExecutionVariant executionVariant,
            string source,
            string serializedFields,
            Guid? auditEntityGuid)
        {
            return CreatePrimaryAuditRecord(
                userName,
                userLogin,
                objectType,
                objectPrimaryKey,
                operationTime,
                operationType,
                ExecutionVariant2ExecutionStatus(executionVariant),
                source,
                serializedFields,
                auditEntityGuid);
        }

        private static string CustomOperation2BigDataCustomOperation(string value)
        {
            const string CustomOperation = "CustomOperation";

            if (Enum.TryParse<tTypeOfAuditOperation>(value, out var operationType))
            {
                return TypeOfAuditOperation2AuditOperationType(operationType).ToString();
            }

            if (value == CustomOperation)
            {
                return AuditOperationType.Custom.ToString();
            }

            return value;
        }

        private static ExecutionStatus ExecutionVariant2ExecutionStatus(tExecutionVariant value)
        {
            return (ExecutionStatus)Enum.Parse(typeof(ExecutionStatus), value.ToString());
        }

        private static AuditOperationType TypeOfAuditOperation2AuditOperationType(tTypeOfAuditOperation value)
        {
            return (AuditOperationType)Enum.Parse(typeof(AuditOperationType), value.ToString(), true);
        }
    }
}
