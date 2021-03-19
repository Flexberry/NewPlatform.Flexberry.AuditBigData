namespace NewPlatform.Flexberry.AuditBigData
{
    /// <summary>
    /// Интерфейс структуры хранения данных аудита свойства объекта <see cref="ICSSoft.STORMNET.DataObject"/>.
    /// </summary>
    public interface IFieldAuditData
    {
        /// <summary>
        /// Field value getter.
        /// </summary>
        public string Field { get; }

        /// <summary>
        /// MainChange value getter.
        /// </summary>
        public IFieldAuditData MainChange { get; }

        /// <summary>
        /// NewValue value getter.
        /// </summary>
        public string NewValue { get; }

        /// <summary>
        /// OldValue value getter.
        /// </summary>
        public string OldValue { get; }
    }
}
