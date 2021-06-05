namespace NewPlatform.Flexberry.AuditBigData.Serialization
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// <see cref="IFieldAuditData"/> implementation.
    /// </summary>
    [JsonObject(IsReference = true)]
    public sealed class JsonFieldAuditData : IFieldAuditData
    {
        /// <inheritdoc />
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public string Field { get; set; }

        /// <summary>
        /// MainChange value setter.
        /// </summary>
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public JsonFieldAuditData MainChange { get; set; }

        /// <inheritdoc />
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public string NewValue { get; set; }

        /// <inheritdoc />
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public string OldValue { get; set; }

        /// <inheritdoc/>
        IFieldAuditData IFieldAuditData.MainChange
        {
            get => MainChange;
            set => MainChange = value as JsonFieldAuditData;
        }
    }
}
