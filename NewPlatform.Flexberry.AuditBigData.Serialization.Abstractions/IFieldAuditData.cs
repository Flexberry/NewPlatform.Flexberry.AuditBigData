namespace NewPlatform.Flexberry.AuditBigData
{
    /// <summary>
    /// Интерфейс структуры хранения данных аудита свойств объекта.
    /// </summary>
    public interface IFieldAuditData
    {
        /// <summary>
        /// Field value getter.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// MainChange value getter.
        /// </summary>
        public IFieldAuditData MainChange { get; set; }

        /// <summary>
        /// NewValue value getter.
        /// </summary>
        public string NewValue { get; set; }

        /// <summary>
        /// OldValue value getter.
        /// </summary>
        public string OldValue { get; set; }
    }
}
