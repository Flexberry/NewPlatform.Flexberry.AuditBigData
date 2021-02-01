namespace NewPlatform.Flexberry.AuditBigData
{
    /// <summary>
    /// Интерфейс структуры хранения данных аудита свойства объекта <see cref="ICSSoft.STORMNET.DataObject"/>.
    /// </summary>
    public interface IFieldAuditData
    {
        /// <summary>
        /// Field.
        /// </summary>
        public string Field { get; }

        /// <summary>
        /// MainChange.
        /// </summary>
        public IFieldAuditData MainChange { get; }

        /// <summary>
        /// NewValue.
        /// </summary>
        public string NewValue { get; }

        /// <summary>
        /// OldValue.
        /// </summary>
        public string OldValue { get; }
    }
}
