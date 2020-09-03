namespace NewPlatform.Flexberry.AuditBigData
{
    using System;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.KeyGen;

    /// <summary>
    /// Audit manager.
    /// </summary>
    public class AuditManager : IAudit
    {
        private IDataService dataService;

        /// <summary>
        /// Initialize new instance <see cref="AuditManager"/>.
        /// </summary>
        /// <param name="dataService">DataSerice for write <see cref="AuditRecord"/>.</param>
        public AuditManager(IDataService dataService)
        {
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

            return ((KeyGuid)auditRecord.__PrimaryKey).Guid;
        }

        /// <inheritdoc/>
        public Guid WriteCustomAuditOperation(CheckedCustomAuditParameters checkedCustomAuditParameters)
        {
            throw new NotImplementedException();
        }
    }
}
