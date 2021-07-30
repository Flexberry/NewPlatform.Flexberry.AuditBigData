namespace NewPlatform.Flexberry.AuditBigData
{
    using System.Collections.Generic;

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
        /// Serializes the specified <see cref="IEnumerable{IFieldAuditData}"/> value to string.
        /// </summary>
        /// <param name="items">The enumerator over a collection of <see cref="IFieldAuditData"/>.</param>
        /// <returns>Serialized value.</returns>
        string Serialize(IEnumerable<IFieldAuditData> items);
    }
}
