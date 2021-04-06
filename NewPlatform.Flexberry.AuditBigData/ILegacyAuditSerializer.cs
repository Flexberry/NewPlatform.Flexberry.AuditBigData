namespace NewPlatform.Flexberry.AuditBigData
{
    using System.Collections.Generic;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Audit.HelpStructures;
    using ICSSoft.STORMNET.Business.Audit.Objects;

    /// <summary>
    /// Legacy audit serializer interface.
    /// </summary>
    public interface ILegacyAuditSerializer
    {
        /// <summary>
        /// Deserializes the specified string value to <see cref="IEnumerable{IFieldAuditData}"/>.
        /// </summary>
        /// <param name="value">Serialized value.</param>
        /// <returns>The enumerator over a collection of <see cref="IFieldAuditData"/>.</returns>
        IEnumerable<IFieldAuditData> Deserialize(string value);

        /// <summary>
        /// Serializes the specified <see cref="AuditAdditionalInfo"/> value to string.
        /// </summary>
        /// <param name="auditAdditionalInfo">Сведения об автогенерируемых полях.</param>
        /// <returns>Serialized value.</returns>
        string Serialize(AuditAdditionalInfo auditAdditionalInfo);

        /// <summary>
        /// Serializes the specified <see cref="IEnumerable{IFieldAuditData}"/> value to string.
        /// </summary>
        /// <param name="items">The enumerator over a collection of <see cref="CustomAuditField"/>.</param>
        /// <returns>Serialized value.</returns>
        string Serialize(IEnumerable<CustomAuditField> items);

        /// <summary>
        /// Serializes the specified <see cref="CommonAuditParameters"/> value to string.
        /// </summary>
        /// <param name="commonAuditParameters">Объект, содержащий данные для аудита <see cref="CustomAuditParameters"/>.</param>
        /// <returns>Serialized value.</returns>
        string Serialize(CommonAuditParameters commonAuditParameters);

        /// <summary>
        /// Serializes the specified <see cref="IEnumerable{IFieldAuditData}"/> value to string.
        /// </summary>
        /// <param name="items">The enumerator over a collection of <see cref="IFieldAuditData"/>.</param>
        /// <returns>Serialized value.</returns>
        string Serialize(IEnumerable<IFieldAuditData> items);
    }
}
