namespace NewPlatform.Flexberry.AuditBigData
{
    using System.Collections.Generic;

    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Audit.HelpStructures;

    /// <summary>
    /// Legacy audit converter interface.
    /// </summary>
    public interface ILegacyAuditConverter
    {
        /// <summary>
        /// Converts the specified <see cref="AuditAdditionalInfo"/> value to <see cref="IEnumerable{IFieldAuditData}"/>.
        /// </summary>
        /// <param name="auditAdditionalInfo">Сведения об автогенерируемых полях.</param>
        /// <returns>Converted collection of <see cref="IFieldAuditData" />.</returns>
        IEnumerable<IFieldAuditData> Convert(AuditAdditionalInfo auditAdditionalInfo);

        /// <summary>
        /// Converts the specified <see cref="IEnumerable{CustomAuditField}"/> value to <see cref="IEnumerable{IFieldAuditData}"/>.
        /// </summary>
        /// <param name="items">The enumerator over a collection of <see cref="CustomAuditField"/>.</param>
        /// <returns>Converted collection of <see cref="IFieldAuditData" />.</returns>
        IEnumerable<IFieldAuditData> Convert(IEnumerable<CustomAuditField> items);

        /// <summary>
        /// Converts the specified <see cref="CommonAuditParameters"/> value to <see cref="IEnumerable{IFieldAuditData}"/>.
        /// </summary>
        /// <param name="commonAuditParameters">Объект, содержащий данные для аудита <see cref="CustomAuditParameters"/>.</param>
        /// <returns>Converted collection of <see cref="IFieldAuditData" />.</returns>
        IEnumerable<IFieldAuditData> Convert(CommonAuditParameters commonAuditParameters);
    }
}
