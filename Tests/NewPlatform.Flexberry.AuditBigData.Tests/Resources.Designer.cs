﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NewPlatform.Flexberry.AuditBigData.Tests {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NewPlatform.Flexberry.AuditBigData.Tests.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///
        ///
        ///
        ///CREATE TABLE [Class2] (
        ///
        ///	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,
        ///
        ///	 [Field21] VARCHAR(255)  NULL,
        ///
        ///	 [Field22] VARCHAR(255)  NULL,
        ///
        ///	 [CreateTime] DATETIME  NULL,
        ///
        ///	 [Creator] VARCHAR(255)  NULL,
        ///
        ///	 [EditTime] DATETIME  NULL,
        ///
        ///	 [Editor] VARCHAR(255)  NULL,
        ///
        ///	 [Class1] UNIQUEIDENTIFIER  NULL,
        ///
        ///	 [Class4] UNIQUEIDENTIFIER  NULL,
        ///
        ///	 PRIMARY KEY ([primaryKey]))
        ///
        ///
        ///CREATE TABLE [Class1] (
        ///
        ///	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,
        ///
        ///	 [Field11] VARCHAR(255)  NULL,
        ///
        ///	 [Fiel [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string MssqlScript {
            get {
                return ResourceManager.GetString("MssqlScript", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///
        ///
        ///
        ///CREATE TABLE &quot;Class2&quot;
        ///(
        ///
        ///	&quot;primaryKey&quot; RAW(16) NOT NULL,
        ///
        ///	&quot;Field21&quot; NVARCHAR2(255) NULL,
        ///
        ///	&quot;Field22&quot; NVARCHAR2(255) NULL,
        ///
        ///	&quot;CreateTime&quot; DATE NULL,
        ///
        ///	&quot;Creator&quot; NVARCHAR2(255) NULL,
        ///
        ///	&quot;EditTime&quot; DATE NULL,
        ///
        ///	&quot;Editor&quot; NVARCHAR2(255) NULL,
        ///
        ///	&quot;Class1&quot; RAW(16) NULL,
        ///
        ///	&quot;Class4&quot; RAW(16) NULL,
        ///
        ///	 PRIMARY KEY (&quot;primaryKey&quot;)
        ///) ;
        ///
        ///
        ///CREATE TABLE &quot;Class1&quot;
        ///(
        ///
        ///	&quot;primaryKey&quot; RAW(16) NOT NULL,
        ///
        ///	&quot;Field11&quot; NVARCHAR2(255) NULL,
        ///
        ///	&quot;Field12&quot; NVARCHAR2(255) NULL,
        ///
        ///	&quot;CreateTime&quot; DATE NUL [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string OracleScript {
            get {
                return ResourceManager.GetString("OracleScript", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///
        ///
        ///
        ///CREATE TABLE Class2 (
        ///
        /// primaryKey UUID NOT NULL,
        ///
        /// Field21 VARCHAR(255) NULL,
        ///
        /// Field22 VARCHAR(255) NULL,
        ///
        /// CreateTime TIMESTAMP(3) NULL,
        ///
        /// Creator VARCHAR(255) NULL,
        ///
        /// EditTime TIMESTAMP(3) NULL,
        ///
        /// Editor VARCHAR(255) NULL,
        ///
        /// Class1 UUID NULL,
        ///
        /// Class4 UUID NULL,
        ///
        /// PRIMARY KEY (primaryKey));
        ///
        ///
        ///CREATE TABLE Class1 (
        ///
        /// primaryKey UUID NOT NULL,
        ///
        /// Field11 VARCHAR(255) NULL,
        ///
        /// Field12 VARCHAR(255) NULL,
        ///
        /// CreateTime TIMESTAMP(3) NULL,
        ///
        /// Creator VARCHAR(255) NULL,
        ///
        ///  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PostgresScript {
            get {
                return ResourceManager.GetString("PostgresScript", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE &quot;Class2&quot; (
        ///
        /// &quot;primaryKey&quot; UUID,
        ///
        /// &quot;Field21&quot; String,
        ///
        /// &quot;Field22&quot; String,
        ///
        /// &quot;CreateTime&quot; DATETIME,
        ///
        /// &quot;Creator&quot; String,
        ///
        /// &quot;EditTime&quot; DATETIME,
        ///
        /// &quot;Editor&quot; String,
        ///
        /// &quot;Class1&quot; UUID,
        ///
        /// &quot;Class4&quot; UUID
        ///
        ///) ENGINE = MergeTree() ORDER BY (&quot;primaryKey&quot;);
        ///
        ///
        ///CREATE TABLE &quot;Class1&quot; (
        ///
        /// &quot;primaryKey&quot; UUID,
        ///
        /// &quot;Field11&quot; String,
        ///
        /// &quot;Field12&quot; String,
        ///
        /// &quot;CreateTime&quot; DATETIME,
        ///
        /// &quot;Creator&quot; String,
        ///
        /// &quot;EditTime&quot; DATETIME,
        ///
        /// &quot;Editor&quot; String
        ///
        ///) ENGINE = MergeTree() ORDER BY (&quot;primaryKey&quot; [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ClickHouseScript
        {
            get
            {
                return ResourceManager.GetString("ClickHouseScript", resourceCulture);
            }
        }
    }
}
