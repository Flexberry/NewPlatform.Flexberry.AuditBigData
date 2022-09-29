﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NewPlatform.Flexberry.AuditBigData
{
    using System;
    using System.Xml;
    using ICSSoft.STORMNET;
    
    
    // *** Start programmer edit section *** (Using statements)

    // *** End programmer edit section *** (Using statements)


    /// <summary>
    /// AuditRecord.
    /// </summary>
    // *** Start programmer edit section *** (AuditRecord CustomAttributes)

    // *** End programmer edit section *** (AuditRecord CustomAttributes)
    [ClassStorage("Audit")]
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.@this)]
    [View("AllFields", new string[] {
            "UserName",
            "UserLogin",
            "ObjectType",
            "ObjectPrimaryKey",
            "OperationTime",
            "OperationType",
            "ExecutionStatus",
            "Source",
            "SerializedFields",
            "HeadAuditEntity"})]
    [View("AuditRecordE", new string[] {
            "ObjectPrimaryKey",
            "ObjectType",
            "OperationTime",
            "OperationType",
            "ExecutionStatus",
            "Source",
            "UserName",
            "UserLogin",
            "SerializedFields"})]
    [View("AuditRecordL", new string[] {
            "ObjectPrimaryKey",
            "ObjectType",
            "OperationTime",
            "OperationType",
            "Source",
            "UserName",
            "UserLogin"})]
    public class AuditRecord : ICSSoft.STORMNET.DataObject
    {
        
        private string fUserName;
        
        private string fUserLogin;
        
        private string fObjectType;
        
        private object fObjectPrimaryKey;
        
        private System.DateTime fOperationTime;
        
        private string fOperationType;
        
        private NewPlatform.Flexberry.AuditBigData.ExecutionStatus fExecutionStatus;
        
        private string fSource;
        
        private string fSerializedFields;
        
        private NewPlatform.Flexberry.AuditBigData.AuditRecord fHeadAuditEntity;
        
        // *** Start programmer edit section *** (AuditRecord CustomMembers)

        // *** End programmer edit section *** (AuditRecord CustomMembers)

        
        /// <summary>
        /// UserName.
        /// </summary>
        // *** Start programmer edit section *** (AuditRecord.UserName CustomAttributes)

        // *** End programmer edit section *** (AuditRecord.UserName CustomAttributes)
        [StrLen(1024)]
        public virtual string UserName
        {
            get
            {
                // *** Start programmer edit section *** (AuditRecord.UserName Get start)

                // *** End programmer edit section *** (AuditRecord.UserName Get start)
                string result = this.fUserName;
                // *** Start programmer edit section *** (AuditRecord.UserName Get end)

                // *** End programmer edit section *** (AuditRecord.UserName Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (AuditRecord.UserName Set start)

                // *** End programmer edit section *** (AuditRecord.UserName Set start)
                this.fUserName = value;
                // *** Start programmer edit section *** (AuditRecord.UserName Set end)

                // *** End programmer edit section *** (AuditRecord.UserName Set end)
            }
        }
        
        /// <summary>
        /// UserLogin.
        /// </summary>
        // *** Start programmer edit section *** (AuditRecord.UserLogin CustomAttributes)

        // *** End programmer edit section *** (AuditRecord.UserLogin CustomAttributes)
        [StrLen(1024)]
        public virtual string UserLogin
        {
            get
            {
                // *** Start programmer edit section *** (AuditRecord.UserLogin Get start)

                // *** End programmer edit section *** (AuditRecord.UserLogin Get start)
                string result = this.fUserLogin;
                // *** Start programmer edit section *** (AuditRecord.UserLogin Get end)

                // *** End programmer edit section *** (AuditRecord.UserLogin Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (AuditRecord.UserLogin Set start)

                // *** End programmer edit section *** (AuditRecord.UserLogin Set start)
                this.fUserLogin = value;
                // *** Start programmer edit section *** (AuditRecord.UserLogin Set end)

                // *** End programmer edit section *** (AuditRecord.UserLogin Set end)
            }
        }
        
        /// <summary>
        /// ObjectType.
        /// </summary>
        // *** Start programmer edit section *** (AuditRecord.ObjectType CustomAttributes)

        // *** End programmer edit section *** (AuditRecord.ObjectType CustomAttributes)
        [StrLen(1024)]
        public virtual string ObjectType
        {
            get
            {
                // *** Start programmer edit section *** (AuditRecord.ObjectType Get start)

                // *** End programmer edit section *** (AuditRecord.ObjectType Get start)
                string result = this.fObjectType;
                // *** Start programmer edit section *** (AuditRecord.ObjectType Get end)

                // *** End programmer edit section *** (AuditRecord.ObjectType Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (AuditRecord.ObjectType Set start)

                // *** End programmer edit section *** (AuditRecord.ObjectType Set start)
                this.fObjectType = value;
                // *** Start programmer edit section *** (AuditRecord.ObjectType Set end)

                // *** End programmer edit section *** (AuditRecord.ObjectType Set end)
            }
        }
        
        /// <summary>
        /// ObjectPrimaryKey.
        /// </summary>
        // *** Start programmer edit section *** (AuditRecord.ObjectPrimaryKey CustomAttributes)

        // *** End programmer edit section *** (AuditRecord.ObjectPrimaryKey CustomAttributes)
        public virtual object ObjectPrimaryKey
        {
            get
            {
                // *** Start programmer edit section *** (AuditRecord.ObjectPrimaryKey Get start)

                // *** End programmer edit section *** (AuditRecord.ObjectPrimaryKey Get start)
                object result = this.fObjectPrimaryKey;
                // *** Start programmer edit section *** (AuditRecord.ObjectPrimaryKey Get end)

                // *** End programmer edit section *** (AuditRecord.ObjectPrimaryKey Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (AuditRecord.ObjectPrimaryKey Set start)

                // *** End programmer edit section *** (AuditRecord.ObjectPrimaryKey Set start)
                this.fObjectPrimaryKey = value;
                // *** Start programmer edit section *** (AuditRecord.ObjectPrimaryKey Set end)

                // *** End programmer edit section *** (AuditRecord.ObjectPrimaryKey Set end)
            }
        }
        
        /// <summary>
        /// OperationTime.
        /// </summary>
        // *** Start programmer edit section *** (AuditRecord.OperationTime CustomAttributes)

        // *** End programmer edit section *** (AuditRecord.OperationTime CustomAttributes)
        [NotNull()]
        public virtual System.DateTime OperationTime
        {
            get
            {
                // *** Start programmer edit section *** (AuditRecord.OperationTime Get start)

                // *** End programmer edit section *** (AuditRecord.OperationTime Get start)
                System.DateTime result = this.fOperationTime;
                // *** Start programmer edit section *** (AuditRecord.OperationTime Get end)

                // *** End programmer edit section *** (AuditRecord.OperationTime Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (AuditRecord.OperationTime Set start)

                // *** End programmer edit section *** (AuditRecord.OperationTime Set start)
                this.fOperationTime = value;
                // *** Start programmer edit section *** (AuditRecord.OperationTime Set end)

                // *** End programmer edit section *** (AuditRecord.OperationTime Set end)
            }
        }
        
        /// <summary>
        /// OperationType.
        /// </summary>
        // *** Start programmer edit section *** (AuditRecord.OperationType CustomAttributes)

        // *** End programmer edit section *** (AuditRecord.OperationType CustomAttributes)
        [StrLen(255)]
        [NotNull()]
        public virtual string OperationType
        {
            get
            {
                // *** Start programmer edit section *** (AuditRecord.OperationType Get start)

                // *** End programmer edit section *** (AuditRecord.OperationType Get start)
                string result = this.fOperationType;
                // *** Start programmer edit section *** (AuditRecord.OperationType Get end)

                // *** End programmer edit section *** (AuditRecord.OperationType Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (AuditRecord.OperationType Set start)

                // *** End programmer edit section *** (AuditRecord.OperationType Set start)
                this.fOperationType = value;
                // *** Start programmer edit section *** (AuditRecord.OperationType Set end)

                // *** End programmer edit section *** (AuditRecord.OperationType Set end)
            }
        }
        
        /// <summary>
        /// ExecutionStatus.
        /// </summary>
        // *** Start programmer edit section *** (AuditRecord.ExecutionStatus CustomAttributes)

        // *** End programmer edit section *** (AuditRecord.ExecutionStatus CustomAttributes)
        [NotNull()]
        public virtual NewPlatform.Flexberry.AuditBigData.ExecutionStatus ExecutionStatus
        {
            get
            {
                // *** Start programmer edit section *** (AuditRecord.ExecutionStatus Get start)

                // *** End programmer edit section *** (AuditRecord.ExecutionStatus Get start)
                NewPlatform.Flexberry.AuditBigData.ExecutionStatus result = this.fExecutionStatus;
                // *** Start programmer edit section *** (AuditRecord.ExecutionStatus Get end)

                // *** End programmer edit section *** (AuditRecord.ExecutionStatus Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (AuditRecord.ExecutionStatus Set start)

                // *** End programmer edit section *** (AuditRecord.ExecutionStatus Set start)
                this.fExecutionStatus = value;
                // *** Start programmer edit section *** (AuditRecord.ExecutionStatus Set end)

                // *** End programmer edit section *** (AuditRecord.ExecutionStatus Set end)
            }
        }
        
        /// <summary>
        /// Source.
        /// </summary>
        // *** Start programmer edit section *** (AuditRecord.Source CustomAttributes)

        // *** End programmer edit section *** (AuditRecord.Source CustomAttributes)
        [StrLen(255)]
        public virtual string Source
        {
            get
            {
                // *** Start programmer edit section *** (AuditRecord.Source Get start)

                // *** End programmer edit section *** (AuditRecord.Source Get start)
                string result = this.fSource;
                // *** Start programmer edit section *** (AuditRecord.Source Get end)

                // *** End programmer edit section *** (AuditRecord.Source Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (AuditRecord.Source Set start)

                // *** End programmer edit section *** (AuditRecord.Source Set start)
                this.fSource = value;
                // *** Start programmer edit section *** (AuditRecord.Source Set end)

                // *** End programmer edit section *** (AuditRecord.Source Set end)
            }
        }
        
        /// <summary>
        /// SerializedFields.
        /// </summary>
        // *** Start programmer edit section *** (AuditRecord.SerializedFields CustomAttributes)

        // *** End programmer edit section *** (AuditRecord.SerializedFields CustomAttributes)
        public virtual string SerializedFields
        {
            get
            {
                // *** Start programmer edit section *** (AuditRecord.SerializedFields Get start)

                // *** End programmer edit section *** (AuditRecord.SerializedFields Get start)
                string result = this.fSerializedFields;
                // *** Start programmer edit section *** (AuditRecord.SerializedFields Get end)

                // *** End programmer edit section *** (AuditRecord.SerializedFields Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (AuditRecord.SerializedFields Set start)

                // *** End programmer edit section *** (AuditRecord.SerializedFields Set start)
                this.fSerializedFields = value;
                // *** Start programmer edit section *** (AuditRecord.SerializedFields Set end)

                // *** End programmer edit section *** (AuditRecord.SerializedFields Set end)
            }
        }
        
        /// <summary>
        /// AuditRecord.
        /// </summary>
        // *** Start programmer edit section *** (AuditRecord.HeadAuditEntity CustomAttributes)

        // *** End programmer edit section *** (AuditRecord.HeadAuditEntity CustomAttributes)
        [PropertyStorage(new string[] {
                "HeadAuditEntity"})]
        public virtual NewPlatform.Flexberry.AuditBigData.AuditRecord HeadAuditEntity
        {
            get
            {
                // *** Start programmer edit section *** (AuditRecord.HeadAuditEntity Get start)

                // *** End programmer edit section *** (AuditRecord.HeadAuditEntity Get start)
                NewPlatform.Flexberry.AuditBigData.AuditRecord result = this.fHeadAuditEntity;
                // *** Start programmer edit section *** (AuditRecord.HeadAuditEntity Get end)

                // *** End programmer edit section *** (AuditRecord.HeadAuditEntity Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (AuditRecord.HeadAuditEntity Set start)

                // *** End programmer edit section *** (AuditRecord.HeadAuditEntity Set start)
                this.fHeadAuditEntity = value;
                // *** Start programmer edit section *** (AuditRecord.HeadAuditEntity Set end)

                // *** End programmer edit section *** (AuditRecord.HeadAuditEntity Set end)
            }
        }
        
        /// <summary>
        /// Class views container.
        /// </summary>
        public class Views
        {
            
            /// <summary>
            /// "AllFields" view.
            /// </summary>
            public static ICSSoft.STORMNET.View AllFields
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("AllFields", typeof(NewPlatform.Flexberry.AuditBigData.AuditRecord));
                }
            }

            /// <summary>
            /// "AuditRecordE" view.
            /// </summary>
            public static ICSSoft.STORMNET.View AuditRecordE
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("AuditRecordE", typeof(NewPlatform.Flexberry.AuditBigData.AuditRecord));
                }
            }

            /// <summary>
            /// "AuditRecordL" view.
            /// </summary>
            public static ICSSoft.STORMNET.View AuditRecordL
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("AuditRecordL", typeof(NewPlatform.Flexberry.AuditBigData.AuditRecord));
                }
            }
        }
    }
}
