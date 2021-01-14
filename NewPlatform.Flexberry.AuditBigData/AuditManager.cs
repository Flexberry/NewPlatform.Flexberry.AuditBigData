namespace NewPlatform.Flexberry.AuditBigData
{
    using System;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Audit.Exceptions;
    using ICSSoft.STORMNET.KeyGen;

    /// <summary>
    /// Audit manager.
    /// </summary>
    public class AuditManager : IAudit
    {
        private readonly IDataService dataService;

        /// <summary>
        /// Initialize new instance <see cref="AuditManager"/>.
        /// </summary>
        /// <param name="dataService">IDataSerice instance to write <see cref="AuditRecord"/>.</param>
        public AuditManager(IDataService dataService)
        {
            if (dataService == null)
            {
                throw new ArgumentNullException(nameof(dataService));
            }

            this.dataService = dataService;
        }

        /// <inheritdoc/>
        public void RatifyAuditOperation(RatificationAuditParameters ratificationAuditParameters)
        {
            throw new NotImplementedException();
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
                AuditRecord auditRecord = new AuditRecord()
                {
                    UserName = commonAuditParameters.UserName,
                    UserLogin = commonAuditParameters.FullUserLogin,
                    ObjectType = commonAuditParameters.OperatedObject.GetType().FullName,
                    ObjectPrimaryKey = commonAuditParameters.OperatedObject.__PrimaryKey,
                    OperationTime = commonAuditParameters.CurrentTime,
                    OperationType = commonAuditParameters.TypeOfAuditOperation.ToString(),
                    ExecutionStatus = ExecutionStatus.Unexecuted,
                    Source = commonAuditParameters.OperationSource,
                    SerializedFields = string.Empty,
                };

                dataService.UpdateObject(auditRecord);

                LogService.LogInfoFormat(
                    "Audit, WriteCommonAuditOperation: объект {0}:{1} записан в аудит с ключом {2}",
                    auditRecord.ObjectPrimaryKey,
                    commonAuditParameters.OperatedObject,
                    auditRecord.__PrimaryKey);

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
            throw new NotImplementedException();
        }
    }
}
