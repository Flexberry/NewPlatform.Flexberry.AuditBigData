namespace NewPlatform.Flexberry.AuditBigData
{
    using System;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business.Audit.Objects;

    /// <summary>
    /// Simplifies interconversions between the type values
    /// in legacy <see cref="ICSSoft.STORMNET.Business.Audit.Objects"/> and <see cref="AuditBigData"/> namespaces.
    /// </summary>
    public static class LegacyAuditConverterHelper
    {
        private const string CustomOperation = "CustomOperation";

        /// <summary>
        /// Converts <see cref="ExecutionStatus"/> type value to <see cref="tExecutionVariant"/> type value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>Converted value.</returns>
        public static tExecutionVariant ExecutionStatus2ExecutionVariant(ExecutionStatus value)
        {
            return (tExecutionVariant)Enum.Parse(typeof(tExecutionVariant), value.ToString());
        }

        /// <summary>
        /// Converts <see cref="tExecutionVariant"/> type value to <see cref="ExecutionStatus"/> type value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>Converted value.</returns>
        public static ExecutionStatus ExecutionVariant2ExecutionStatus(tExecutionVariant value)
        {
            return (ExecutionStatus)Enum.Parse(typeof(ExecutionStatus), value.ToString());
        }

        /// <summary>
        /// Converts an operation type string representation in legacy <see cref="ICSSoft.STORMNET.Business.Audit.Objects"/> namespace
        /// to operation type string representation in <see cref="AuditBigData"/> namespace.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>Converted value.</returns>
        public static string OperationType2BigDataOperationType(string value)
        {
            object o = EnumCaption.GetValueFor(value, typeof(tTypeOfAuditOperation));
            if (o != null)
            {
                var typeOfAuditOperation = (tTypeOfAuditOperation)o;
                AuditOperationType auditOperationType = TypeOfAuditOperation2AuditOperationType(typeOfAuditOperation);

                return EnumCaption.GetCaptionFor(auditOperationType);
            }

            if (value == CustomOperation)
            {
                return EnumCaption.GetCaptionFor(AuditOperationType.Custom);
            }

            return value;
        }

        /// <summary>
        /// Converts <see cref="tTypeOfAuditOperation"/> type value to <see cref="AuditOperationType"/> type value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>Converted value.</returns>
        internal static AuditOperationType TypeOfAuditOperation2AuditOperationType(tTypeOfAuditOperation value)
        {
            return (AuditOperationType)Enum.Parse(typeof(AuditOperationType), value.ToString(), true);
        }
    }
}
