﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NewPlatform.Flexberry.Audit.Tests
{
    using System;
    using System.Xml;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Audit.Objects;
    
    
    // *** Start programmer edit section *** (Using statements)

    // *** End programmer edit section *** (Using statements)


    /// <summary>
    /// Class3.
    /// </summary>
    // *** Start programmer edit section *** (Class3 CustomAttributes)

    // *** End programmer edit section *** (Class3 CustomAttributes)
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    [View("AuditView", new string[] {
            "Field32",
            "CreateTime",
            "Creator",
            "EditTime",
            "Editor",
            "Class2",
            "Class2.Field21",
            "Class2.Field22",
            "Class2.Class1",
            "Class2.Class1.Field11",
            "Class2.Class1.Field12"})]
    [View("Class3E", new string[] {
            "Field31 as \'Поле31\'",
            "Field32 as \'Поле32\'"})]
    public class Class3 : ICSSoft.STORMNET.DataObject, IDataObjectWithAuditFields
    {
        
        private string fField31;
        
        private string fField32;
        
        private System.Nullable<System.DateTime> fCreateTime;
        
        private string fCreator;
        
        private System.Nullable<System.DateTime> fEditTime;
        
        private string fEditor;
        
        private NewPlatform.Flexberry.Audit.Tests.Class2 fClass2;
        
        // *** Start programmer edit section *** (Class3 CustomMembers)

        // *** End programmer edit section *** (Class3 CustomMembers)

        
        /// <summary>
        /// Field31.
        /// </summary>
        // *** Start programmer edit section *** (Class3.Field31 CustomAttributes)

        // *** End programmer edit section *** (Class3.Field31 CustomAttributes)
        [StrLen(255)]
        public virtual string Field31
        {
            get
            {
                // *** Start programmer edit section *** (Class3.Field31 Get start)

                // *** End programmer edit section *** (Class3.Field31 Get start)
                string result = this.fField31;
                // *** Start programmer edit section *** (Class3.Field31 Get end)

                // *** End programmer edit section *** (Class3.Field31 Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Class3.Field31 Set start)

                // *** End programmer edit section *** (Class3.Field31 Set start)
                this.fField31 = value;
                // *** Start programmer edit section *** (Class3.Field31 Set end)

                // *** End programmer edit section *** (Class3.Field31 Set end)
            }
        }
        
        /// <summary>
        /// Field32.
        /// </summary>
        // *** Start programmer edit section *** (Class3.Field32 CustomAttributes)

        // *** End programmer edit section *** (Class3.Field32 CustomAttributes)
        [StrLen(255)]
        public virtual string Field32
        {
            get
            {
                // *** Start programmer edit section *** (Class3.Field32 Get start)

                // *** End programmer edit section *** (Class3.Field32 Get start)
                string result = this.fField32;
                // *** Start programmer edit section *** (Class3.Field32 Get end)

                // *** End programmer edit section *** (Class3.Field32 Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Class3.Field32 Set start)

                // *** End programmer edit section *** (Class3.Field32 Set start)
                this.fField32 = value;
                // *** Start programmer edit section *** (Class3.Field32 Set end)

                // *** End programmer edit section *** (Class3.Field32 Set end)
            }
        }
        
        /// <summary>
        /// Время создания объекта.
        /// </summary>
        // *** Start programmer edit section *** (Class3.CreateTime CustomAttributes)

        // *** End programmer edit section *** (Class3.CreateTime CustomAttributes)
        public virtual System.Nullable<System.DateTime> CreateTime
        {
            get
            {
                // *** Start programmer edit section *** (Class3.CreateTime Get start)

                // *** End programmer edit section *** (Class3.CreateTime Get start)
                System.Nullable<System.DateTime> result = this.fCreateTime;
                // *** Start programmer edit section *** (Class3.CreateTime Get end)

                // *** End programmer edit section *** (Class3.CreateTime Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Class3.CreateTime Set start)

                // *** End programmer edit section *** (Class3.CreateTime Set start)
                this.fCreateTime = value;
                // *** Start programmer edit section *** (Class3.CreateTime Set end)

                // *** End programmer edit section *** (Class3.CreateTime Set end)
            }
        }
        
        /// <summary>
        /// Создатель объекта.
        /// </summary>
        // *** Start programmer edit section *** (Class3.Creator CustomAttributes)

        // *** End programmer edit section *** (Class3.Creator CustomAttributes)
        [StrLen(255)]
        public virtual string Creator
        {
            get
            {
                // *** Start programmer edit section *** (Class3.Creator Get start)

                // *** End programmer edit section *** (Class3.Creator Get start)
                string result = this.fCreator;
                // *** Start programmer edit section *** (Class3.Creator Get end)

                // *** End programmer edit section *** (Class3.Creator Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Class3.Creator Set start)

                // *** End programmer edit section *** (Class3.Creator Set start)
                this.fCreator = value;
                // *** Start programmer edit section *** (Class3.Creator Set end)

                // *** End programmer edit section *** (Class3.Creator Set end)
            }
        }
        
        /// <summary>
        /// Время последнего редактирования объекта.
        /// </summary>
        // *** Start programmer edit section *** (Class3.EditTime CustomAttributes)

        // *** End programmer edit section *** (Class3.EditTime CustomAttributes)
        public virtual System.Nullable<System.DateTime> EditTime
        {
            get
            {
                // *** Start programmer edit section *** (Class3.EditTime Get start)

                // *** End programmer edit section *** (Class3.EditTime Get start)
                System.Nullable<System.DateTime> result = this.fEditTime;
                // *** Start programmer edit section *** (Class3.EditTime Get end)

                // *** End programmer edit section *** (Class3.EditTime Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Class3.EditTime Set start)

                // *** End programmer edit section *** (Class3.EditTime Set start)
                this.fEditTime = value;
                // *** Start programmer edit section *** (Class3.EditTime Set end)

                // *** End programmer edit section *** (Class3.EditTime Set end)
            }
        }
        
        /// <summary>
        /// Последний редактор объекта.
        /// </summary>
        // *** Start programmer edit section *** (Class3.Editor CustomAttributes)

        // *** End programmer edit section *** (Class3.Editor CustomAttributes)
        [StrLen(255)]
        public virtual string Editor
        {
            get
            {
                // *** Start programmer edit section *** (Class3.Editor Get start)

                // *** End programmer edit section *** (Class3.Editor Get start)
                string result = this.fEditor;
                // *** Start programmer edit section *** (Class3.Editor Get end)

                // *** End programmer edit section *** (Class3.Editor Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Class3.Editor Set start)

                // *** End programmer edit section *** (Class3.Editor Set start)
                this.fEditor = value;
                // *** Start programmer edit section *** (Class3.Editor Set end)

                // *** End programmer edit section *** (Class3.Editor Set end)
            }
        }
        
        /// <summary>
        /// мастеровая ссылка на шапку NewPlatform.Flexberry.Audit.Tests.Class2.
        /// </summary>
        // *** Start programmer edit section *** (Class3.Class2 CustomAttributes)

        // *** End programmer edit section *** (Class3.Class2 CustomAttributes)
        [Agregator()]
        [NotNull()]
        [PropertyStorage(new string[] {
                "Class2"})]
        public virtual NewPlatform.Flexberry.Audit.Tests.Class2 Class2
        {
            get
            {
                // *** Start programmer edit section *** (Class3.Class2 Get start)

                // *** End programmer edit section *** (Class3.Class2 Get start)
                NewPlatform.Flexberry.Audit.Tests.Class2 result = this.fClass2;
                // *** Start programmer edit section *** (Class3.Class2 Get end)

                // *** End programmer edit section *** (Class3.Class2 Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Class3.Class2 Set start)

                // *** End programmer edit section *** (Class3.Class2 Set start)
                this.fClass2 = value;
                // *** Start programmer edit section *** (Class3.Class2 Set end)

                // *** End programmer edit section *** (Class3.Class2 Set end)
            }
        }
        
        /// <summary>
        /// Class views container.
        /// </summary>
        public class Views
        {
            
            /// <summary>
            /// "AuditView" view.
            /// </summary>
            public static ICSSoft.STORMNET.View AuditView
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("AuditView", typeof(NewPlatform.Flexberry.Audit.Tests.Class3));
                }
            }
            
            /// <summary>
            /// "Class3E" view.
            /// </summary>
            public static ICSSoft.STORMNET.View Class3E
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("Class3E", typeof(NewPlatform.Flexberry.Audit.Tests.Class3));
                }
            }
        }
        
        /// <summary>
        /// Audit class settings.
        /// </summary>
        public class AuditSettings
        {
            
            /// <summary>
            /// Включён ли аудит для класса.
            /// </summary>
            public static bool AuditEnabled = true;
            
            /// <summary>
            /// Использовать имя представления для аудита по умолчанию.
            /// </summary>
            public static bool UseDefaultView = false;
            
            /// <summary>
            /// Включён ли аудит операции чтения.
            /// </summary>
            public static bool SelectAudit = false;
            
            /// <summary>
            /// Имя представления для аудирования операции чтения.
            /// </summary>
            public static string SelectAuditViewName = "AuditView";
            
            /// <summary>
            /// Включён ли аудит операции создания.
            /// </summary>
            public static bool InsertAudit = true;
            
            /// <summary>
            /// Имя представления для аудирования операции создания.
            /// </summary>
            public static string InsertAuditViewName = "AuditView";
            
            /// <summary>
            /// Включён ли аудит операции изменения.
            /// </summary>
            public static bool UpdateAudit = true;
            
            /// <summary>
            /// Имя представления для аудирования операции изменения.
            /// </summary>
            public static string UpdateAuditViewName = "AuditView";
            
            /// <summary>
            /// Включён ли аудит операции удаления.
            /// </summary>
            public static bool DeleteAudit = true;
            
            /// <summary>
            /// Имя представления для аудирования операции удаления.
            /// </summary>
            public static string DeleteAuditViewName = "AuditView";
            
            /// <summary>
            /// Путь к форме просмотра результатов аудита.
            /// </summary>
            public static string FormUrl = "";
            
            /// <summary>
            /// Режим записи данных аудита (синхронный или асинхронный).
            /// </summary>
            public static ICSSoft.STORMNET.Business.Audit.Objects.tWriteMode WriteMode = ICSSoft.STORMNET.Business.Audit.Objects.tWriteMode.Synchronous;
            
            /// <summary>
            /// Максимальная длина сохраняемого значения поля (если 0, то строка обрезаться не будет).
            /// </summary>
            public static int PrunningLength = 0;
            
            /// <summary>
            /// Показывать ли пользователям в изменениях первичные ключи.
            /// </summary>
            public static bool ShowPrimaryKey = false;
            
            /// <summary>
            /// Сохранять ли старое значение.
            /// </summary>
            public static bool KeepOldValue = true;
            
            /// <summary>
            /// Сжимать ли сохраняемые значения.
            /// </summary>
            public static bool Compress = false;
            
            /// <summary>
            /// Сохранять ли все значения атрибутов, а не только изменяемые.
            /// </summary>
            public static bool KeepAllValues = false;
        }
    }
    
    /// <summary>
    /// Detail array of Class3.
    /// </summary>
    // *** Start programmer edit section *** (DetailArrayDetailArrayOfClass3 CustomAttributes)

    // *** End programmer edit section *** (DetailArrayDetailArrayOfClass3 CustomAttributes)
    public class DetailArrayOfClass3 : ICSSoft.STORMNET.DetailArray
    {
        
        // *** Start programmer edit section *** (NewPlatform.Flexberry.Audit.Tests.DetailArrayOfClass3 members)

        // *** End programmer edit section *** (NewPlatform.Flexberry.Audit.Tests.DetailArrayOfClass3 members)

        
        /// <summary>
        /// Construct detail array.
        /// </summary>
        /// <summary>
        /// Returns object with type Class3 by index.
        /// </summary>
        /// <summary>
        /// Adds object with type Class3.
        /// </summary>
        public DetailArrayOfClass3(NewPlatform.Flexberry.Audit.Tests.Class2 fClass2) : 
                base(typeof(Class3), ((ICSSoft.STORMNET.DataObject)(fClass2)))
        {
        }
        
        public NewPlatform.Flexberry.Audit.Tests.Class3 this[int index]
        {
            get
            {
                return ((NewPlatform.Flexberry.Audit.Tests.Class3)(this.ItemByIndex(index)));
            }
        }
        
        public virtual void Add(NewPlatform.Flexberry.Audit.Tests.Class3 dataobject)
        {
            this.AddObject(((ICSSoft.STORMNET.DataObject)(dataobject)));
        }
    }
}
