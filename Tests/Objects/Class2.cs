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
    /// Class2.
    /// </summary>
    // *** Start programmer edit section *** (Class2 CustomAttributes)

    // *** End programmer edit section *** (Class2 CustomAttributes)
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    [View("AuditView", new string[] {
            "Field21",
            "Field22",
            "Class1 as \'Class1\'",
            "Class1.Field11",
            "Class1.Field12",
            "Class4",
            "Class4.Field41",
            "Class4.Field42",
            "CreateTime",
            "Creator",
            "EditTime",
            "Editor"})]
    [AssociatedDetailViewAttribute("AuditView", "Class3", "AuditView", true, "", "Class3", true, new string[] {
            ""})]
    [View("Class2E", new string[] {
            "Field21 as \'Поле21\'",
            "Field22 as \'Поле22\'",
            "Class1 as \'Class1\'",
            "Class1.Field11 as \'Поле11\'",
            "Class1.Field12 as \'Поле12\'",
            "Class4 as \'Class4\'",
            "Class4.Field41 as \'Поле41\'",
            "Class4.Field42 as \'Поле42\'"})]
    [AssociatedDetailViewAttribute("Class2E", "Class3", "Class3E", true, "", "Class3", true, new string[] {
            ""})]
    [View("Class2L", new string[] {
            "Field21 as \'Поле21\'",
            "Field22 as \'Поле22\'",
            "Class1",
            "Class1.Field11 as \'Поле11\'",
            "Class1.Field12 as \'Поле12\'"})]
    public class Class2 : ICSSoft.STORMNET.DataObject, IDataObjectWithAuditFields
    {
        
        private string fField21;
        
        private string fField22;
        
        private System.Nullable<System.DateTime> fCreateTime;
        
        private string fCreator;
        
        private System.Nullable<System.DateTime> fEditTime;
        
        private string fEditor;
        
        private NewPlatform.Flexberry.Audit.Tests.Class1 fClass1;
        
        private NewPlatform.Flexberry.Audit.Tests.Class4 fClass4;
        
        private NewPlatform.Flexberry.Audit.Tests.DetailArrayOfClass3 fClass3;
        
        // *** Start programmer edit section *** (Class2 CustomMembers)

        // *** End programmer edit section *** (Class2 CustomMembers)

        
        /// <summary>
        /// Field21.
        /// </summary>
        // *** Start programmer edit section *** (Class2.Field21 CustomAttributes)

        // *** End programmer edit section *** (Class2.Field21 CustomAttributes)
        [StrLen(255)]
        public virtual string Field21
        {
            get
            {
                // *** Start programmer edit section *** (Class2.Field21 Get start)

                // *** End programmer edit section *** (Class2.Field21 Get start)
                string result = this.fField21;
                // *** Start programmer edit section *** (Class2.Field21 Get end)

                // *** End programmer edit section *** (Class2.Field21 Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Class2.Field21 Set start)

                // *** End programmer edit section *** (Class2.Field21 Set start)
                this.fField21 = value;
                // *** Start programmer edit section *** (Class2.Field21 Set end)

                // *** End programmer edit section *** (Class2.Field21 Set end)
            }
        }
        
        /// <summary>
        /// Field22.
        /// </summary>
        // *** Start programmer edit section *** (Class2.Field22 CustomAttributes)

        // *** End programmer edit section *** (Class2.Field22 CustomAttributes)
        [StrLen(255)]
        public virtual string Field22
        {
            get
            {
                // *** Start programmer edit section *** (Class2.Field22 Get start)

                // *** End programmer edit section *** (Class2.Field22 Get start)
                string result = this.fField22;
                // *** Start programmer edit section *** (Class2.Field22 Get end)

                // *** End programmer edit section *** (Class2.Field22 Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Class2.Field22 Set start)

                // *** End programmer edit section *** (Class2.Field22 Set start)
                this.fField22 = value;
                // *** Start programmer edit section *** (Class2.Field22 Set end)

                // *** End programmer edit section *** (Class2.Field22 Set end)
            }
        }
        
        /// <summary>
        /// Время создания объекта.
        /// </summary>
        // *** Start programmer edit section *** (Class2.CreateTime CustomAttributes)

        // *** End programmer edit section *** (Class2.CreateTime CustomAttributes)
        public virtual System.Nullable<System.DateTime> CreateTime
        {
            get
            {
                // *** Start programmer edit section *** (Class2.CreateTime Get start)

                // *** End programmer edit section *** (Class2.CreateTime Get start)
                System.Nullable<System.DateTime> result = this.fCreateTime;
                // *** Start programmer edit section *** (Class2.CreateTime Get end)

                // *** End programmer edit section *** (Class2.CreateTime Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Class2.CreateTime Set start)

                // *** End programmer edit section *** (Class2.CreateTime Set start)
                this.fCreateTime = value;
                // *** Start programmer edit section *** (Class2.CreateTime Set end)

                // *** End programmer edit section *** (Class2.CreateTime Set end)
            }
        }
        
        /// <summary>
        /// Создатель объекта.
        /// </summary>
        // *** Start programmer edit section *** (Class2.Creator CustomAttributes)

        // *** End programmer edit section *** (Class2.Creator CustomAttributes)
        [StrLen(255)]
        public virtual string Creator
        {
            get
            {
                // *** Start programmer edit section *** (Class2.Creator Get start)

                // *** End programmer edit section *** (Class2.Creator Get start)
                string result = this.fCreator;
                // *** Start programmer edit section *** (Class2.Creator Get end)

                // *** End programmer edit section *** (Class2.Creator Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Class2.Creator Set start)

                // *** End programmer edit section *** (Class2.Creator Set start)
                this.fCreator = value;
                // *** Start programmer edit section *** (Class2.Creator Set end)

                // *** End programmer edit section *** (Class2.Creator Set end)
            }
        }
        
        /// <summary>
        /// Время последнего редактирования объекта.
        /// </summary>
        // *** Start programmer edit section *** (Class2.EditTime CustomAttributes)

        // *** End programmer edit section *** (Class2.EditTime CustomAttributes)
        public virtual System.Nullable<System.DateTime> EditTime
        {
            get
            {
                // *** Start programmer edit section *** (Class2.EditTime Get start)

                // *** End programmer edit section *** (Class2.EditTime Get start)
                System.Nullable<System.DateTime> result = this.fEditTime;
                // *** Start programmer edit section *** (Class2.EditTime Get end)

                // *** End programmer edit section *** (Class2.EditTime Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Class2.EditTime Set start)

                // *** End programmer edit section *** (Class2.EditTime Set start)
                this.fEditTime = value;
                // *** Start programmer edit section *** (Class2.EditTime Set end)

                // *** End programmer edit section *** (Class2.EditTime Set end)
            }
        }
        
        /// <summary>
        /// Последний редактор объекта.
        /// </summary>
        // *** Start programmer edit section *** (Class2.Editor CustomAttributes)

        // *** End programmer edit section *** (Class2.Editor CustomAttributes)
        [StrLen(255)]
        public virtual string Editor
        {
            get
            {
                // *** Start programmer edit section *** (Class2.Editor Get start)

                // *** End programmer edit section *** (Class2.Editor Get start)
                string result = this.fEditor;
                // *** Start programmer edit section *** (Class2.Editor Get end)

                // *** End programmer edit section *** (Class2.Editor Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Class2.Editor Set start)

                // *** End programmer edit section *** (Class2.Editor Set start)
                this.fEditor = value;
                // *** Start programmer edit section *** (Class2.Editor Set end)

                // *** End programmer edit section *** (Class2.Editor Set end)
            }
        }
        
        /// <summary>
        /// Class2.
        /// </summary>
        // *** Start programmer edit section *** (Class2.Class1 CustomAttributes)

        // *** End programmer edit section *** (Class2.Class1 CustomAttributes)
        [PropertyStorage(new string[] {
                "Class1"})]
        public virtual NewPlatform.Flexberry.Audit.Tests.Class1 Class1
        {
            get
            {
                // *** Start programmer edit section *** (Class2.Class1 Get start)

                // *** End programmer edit section *** (Class2.Class1 Get start)
                NewPlatform.Flexberry.Audit.Tests.Class1 result = this.fClass1;
                // *** Start programmer edit section *** (Class2.Class1 Get end)

                // *** End programmer edit section *** (Class2.Class1 Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Class2.Class1 Set start)

                // *** End programmer edit section *** (Class2.Class1 Set start)
                this.fClass1 = value;
                // *** Start programmer edit section *** (Class2.Class1 Set end)

                // *** End programmer edit section *** (Class2.Class1 Set end)
            }
        }
        
        /// <summary>
        /// Class2.
        /// </summary>
        // *** Start programmer edit section *** (Class2.Class4 CustomAttributes)

        // *** End programmer edit section *** (Class2.Class4 CustomAttributes)
        [PropertyStorage(new string[] {
                "Class4"})]
        public virtual NewPlatform.Flexberry.Audit.Tests.Class4 Class4
        {
            get
            {
                // *** Start programmer edit section *** (Class2.Class4 Get start)

                // *** End programmer edit section *** (Class2.Class4 Get start)
                NewPlatform.Flexberry.Audit.Tests.Class4 result = this.fClass4;
                // *** Start programmer edit section *** (Class2.Class4 Get end)

                // *** End programmer edit section *** (Class2.Class4 Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Class2.Class4 Set start)

                // *** End programmer edit section *** (Class2.Class4 Set start)
                this.fClass4 = value;
                // *** Start programmer edit section *** (Class2.Class4 Set end)

                // *** End programmer edit section *** (Class2.Class4 Set end)
            }
        }
        
        /// <summary>
        /// Class2.
        /// </summary>
        // *** Start programmer edit section *** (Class2.Class3 CustomAttributes)

        // *** End programmer edit section *** (Class2.Class3 CustomAttributes)
        public virtual NewPlatform.Flexberry.Audit.Tests.DetailArrayOfClass3 Class3
        {
            get
            {
                // *** Start programmer edit section *** (Class2.Class3 Get start)

                // *** End programmer edit section *** (Class2.Class3 Get start)
                if ((this.fClass3 == null))
                {
                    this.fClass3 = new NewPlatform.Flexberry.Audit.Tests.DetailArrayOfClass3(this);
                }
                NewPlatform.Flexberry.Audit.Tests.DetailArrayOfClass3 result = this.fClass3;
                // *** Start programmer edit section *** (Class2.Class3 Get end)

                // *** End programmer edit section *** (Class2.Class3 Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Class2.Class3 Set start)

                // *** End programmer edit section *** (Class2.Class3 Set start)
                this.fClass3 = value;
                // *** Start programmer edit section *** (Class2.Class3 Set end)

                // *** End programmer edit section *** (Class2.Class3 Set end)
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
                    return ICSSoft.STORMNET.Information.GetView("AuditView", typeof(NewPlatform.Flexberry.Audit.Tests.Class2));
                }
            }
            
            /// <summary>
            /// "Class2E" view.
            /// </summary>
            public static ICSSoft.STORMNET.View Class2E
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("Class2E", typeof(NewPlatform.Flexberry.Audit.Tests.Class2));
                }
            }
            
            /// <summary>
            /// "Class2L" view.
            /// </summary>
            public static ICSSoft.STORMNET.View Class2L
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("Class2L", typeof(NewPlatform.Flexberry.Audit.Tests.Class2));
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
}
