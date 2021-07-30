namespace NewPlatform.Flexberry.AuditBigData.Serialization
{
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    /// <summary>
    /// <see cref="ILegacyAuditSerializer"/> implementation using Newtonsoft.Json.
    /// </summary>
    public class JsonLegacyAuditSerializer : ILegacyAuditSerializer
    {
        /// <inheritdoc />
        public IEnumerable<IFieldAuditData> Deserialize(string value)
        {
            return JsonConvert.DeserializeObject<JsonFieldAuditData[]>(value ?? string.Empty) ?? new JsonFieldAuditData[] { }.Cast<IFieldAuditData>();
        }

        /// <inheritdoc />
        public string Serialize(IEnumerable<IFieldAuditData> items)
        {
            return items != null && items.Any() ? JsonConvert.SerializeObject(items) : null;
        }
    }
}
