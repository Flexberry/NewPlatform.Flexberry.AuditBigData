



CREATE TABLE "Class2"
(

	"primaryKey" RAW(16) NOT NULL,

	"Field21" NVARCHAR2(255) NULL,

	"Field22" NVARCHAR2(255) NULL,

	"CreateTime" DATE NULL,

	"Creator" NVARCHAR2(255) NULL,

	"EditTime" DATE NULL,

	"Editor" NVARCHAR2(255) NULL,

	"Class1" RAW(16) NULL,

	"Class4" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Class1"
(

	"primaryKey" RAW(16) NOT NULL,

	"Field11" NVARCHAR2(255) NULL,

	"Field12" NVARCHAR2(255) NULL,

	"CreateTime" DATE NULL,

	"Creator" NVARCHAR2(255) NULL,

	"EditTime" DATE NULL,

	"Editor" NVARCHAR2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Class4"
(

	"primaryKey" RAW(16) NOT NULL,

	"Field41" NVARCHAR2(255) NULL,

	"Field42" NVARCHAR2(255) NULL,

	"CreateTime" DATE NULL,

	"Creator" NVARCHAR2(255) NULL,

	"EditTime" DATE NULL,

	"Editor" NVARCHAR2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Class3"
(

	"primaryKey" RAW(16) NOT NULL,

	"Field31" NVARCHAR2(255) NULL,

	"Field32" NVARCHAR2(255) NULL,

	"CreateTime" DATE NULL,

	"Creator" NVARCHAR2(255) NULL,

	"EditTime" DATE NULL,

	"Editor" NVARCHAR2(255) NULL,

	"Class2" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMNETLOCKDATA"
(

	"LockKey" NVARCHAR2(300) NOT NULL,

	"UserName" NVARCHAR2(300) NOT NULL,

	"LockDate" DATE NULL,

	 PRIMARY KEY ("LockKey")
) ;


CREATE TABLE "STORMSETTINGS"
(

	"primaryKey" RAW(16) NOT NULL,

	"Module" nvarchar2(1000) NULL,

	"Name" nvarchar2(255) NULL,

	"Value" CLOB NULL,

	"User" nvarchar2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMAdvLimit"
(

	"primaryKey" RAW(16) NOT NULL,

	"User" nvarchar2(255) NULL,

	"Published" NUMBER(1) NULL,

	"Module" nvarchar2(255) NULL,

	"Name" nvarchar2(255) NULL,

	"Value" CLOB NULL,

	"HotKeyData" NUMBER(10) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMFILTERSETTING"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" nvarchar2(255) NOT NULL,

	"DataObjectView" nvarchar2(255) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMWEBSEARCH"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" nvarchar2(255) NOT NULL,

	"Order" NUMBER(10) NOT NULL,

	"PresentView" nvarchar2(255) NOT NULL,

	"DetailedView" nvarchar2(255) NOT NULL,

	"FilterSetting_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMFILTERDETAIL"
(

	"primaryKey" RAW(16) NOT NULL,

	"Caption" nvarchar2(255) NOT NULL,

	"DataObjectView" nvarchar2(255) NOT NULL,

	"ConnectMasterProp" nvarchar2(255) NOT NULL,

	"OwnerConnectProp" nvarchar2(255) NULL,

	"FilterSetting_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMFILTERLOOKUP"
(

	"primaryKey" RAW(16) NOT NULL,

	"DataObjectType" nvarchar2(255) NOT NULL,

	"Container" nvarchar2(255) NULL,

	"ContainerTag" nvarchar2(255) NULL,

	"FieldsToView" nvarchar2(255) NULL,

	"FilterSetting_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "UserSetting"
(

	"primaryKey" RAW(16) NOT NULL,

	"AppName" nvarchar2(256) NULL,

	"UserName" nvarchar2(512) NULL,

	"UserGuid" RAW(16) NULL,

	"ModuleName" nvarchar2(1024) NULL,

	"ModuleGuid" RAW(16) NULL,

	"SettName" nvarchar2(256) NULL,

	"SettGuid" RAW(16) NULL,

	"SettLastAccessTime" DATE NULL,

	"StrVal" nvarchar2(256) NULL,

	"TxtVal" CLOB NULL,

	"IntVal" NUMBER(10) NULL,

	"BoolVal" NUMBER(1) NULL,

	"GuidVal" RAW(16) NULL,

	"DecimalVal" NUMBER(20,10) NULL,

	"DateTimeVal" DATE NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "ApplicationLog"
(

	"primaryKey" RAW(16) NOT NULL,

	"Category" nvarchar2(64) NULL,

	"EventId" NUMBER(10) NULL,

	"Priority" NUMBER(10) NULL,

	"Severity" nvarchar2(32) NULL,

	"Title" nvarchar2(256) NULL,

	"Timestamp" DATE NULL,

	"MachineName" nvarchar2(32) NULL,

	"AppDomainName" nvarchar2(512) NULL,

	"ProcessId" nvarchar2(256) NULL,

	"ProcessName" nvarchar2(512) NULL,

	"ThreadName" nvarchar2(512) NULL,

	"Win32ThreadId" nvarchar2(128) NULL,

	"Message" nvarchar2(2000) NULL,

	"FormattedMessage" nvarchar2(2000) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMAG"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" nvarchar2(80) NOT NULL,

	"Login" nvarchar2(50) NULL,

	"Pwd" nvarchar2(50) NULL,

	"IsUser" NUMBER(1) NOT NULL,

	"IsGroup" NUMBER(1) NOT NULL,

	"IsRole" NUMBER(1) NOT NULL,

	"ConnString" nvarchar2(255) NULL,

	"Enabled" NUMBER(1) NULL,

	"Email" nvarchar2(80) NULL,

	"CreateTime" DATE NULL,

	"Creator" nvarchar2(255) NULL,

	"EditTime" DATE NULL,

	"Editor" nvarchar2(255) NULL,

	 PRIMARY KEY ("primaryKey"),

	"Comment" nvarchar2(255) NULL,
) ;


CREATE TABLE "STORMLG"
(

	"primaryKey" RAW(16) NOT NULL,

	"Group_m0" RAW(16) NOT NULL,

	"User_m0" RAW(16) NOT NULL,

	"CreateTime" DATE NULL,

	"Creator" nvarchar2(255) NULL,

	"EditTime" DATE NULL,

	"Editor" nvarchar2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMAuObjType"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" nvarchar2(255) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMAuEntity"
(

	"primaryKey" RAW(16) NOT NULL,

	"ObjectPrimaryKey" nvarchar2(38) NOT NULL,

	"OperationTime" DATE NOT NULL,

	"OperationType" nvarchar2(100) NOT NULL,

	"ExecutionResult" nvarchar2(12) NOT NULL,

	"Source" nvarchar2(255) NOT NULL,

	"SerializedField" nvarchar2(2000) NULL,

	"User_m0" RAW(16) NOT NULL,

	"ObjectType_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMAuField"
(

	"primaryKey" RAW(16) NOT NULL,

	"Field" nvarchar2(100) NOT NULL,

	"OldValue" nvarchar2(2000) NULL,

	"NewValue" nvarchar2(2000) NULL,

	"MainChange_m0" RAW(16) NULL,

	"AuditEntity_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMI"
(

	"primaryKey" RAW(16) NOT NULL,

	"User_m0" RAW(16) NOT NULL,

	"Agent_m0" RAW(16) NOT NULL,

	"CreateTime" DATE NULL,

	"Creator" nvarchar2(255) NULL,

	"EditTime" DATE NULL,

	"Editor" nvarchar2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Session"
(

	"primaryKey" RAW(16) NOT NULL,

	"UserKey" RAW(16) NULL,

	"StartedAt" DATE NULL,

	"LastAccess" DATE NULL,

	"Closed" NUMBER(1) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMS"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" nvarchar2(100) NOT NULL,

	"Type" nvarchar2(100) NULL,

	"IsAttribute" NUMBER(1) NOT NULL,

	"IsOperation" NUMBER(1) NOT NULL,

	"IsView" NUMBER(1) NOT NULL,

	"IsClass" NUMBER(1) NOT NULL,

	"SharedOper" NUMBER(1) NULL,

	"CreateTime" DATE NULL,

	"Creator" nvarchar2(255) NULL,

	"EditTime" DATE NULL,

	"Editor" nvarchar2(255) NULL,

	 PRIMARY KEY ("primaryKey"),

	"Comment" nvarchar2(255) NULL,
) ;


CREATE TABLE "STORMP"
(

	"primaryKey" RAW(16) NOT NULL,

	"Subject_m0" RAW(16) NOT NULL,

	"Agent_m0" RAW(16) NOT NULL,

	"CreateTime" DATE NULL,

	"Creator" nvarchar2(255) NULL,

	"EditTime" DATE NULL,

	"Editor" nvarchar2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMF"
(

	"primaryKey" RAW(16) NOT NULL,

	"FilterText" CLOB NULL,

	"Name" nvarchar2(255) NULL,

	"FilterTypeNView" nvarchar2(255) NULL,

	"Subject_m0" RAW(16) NULL,

	"CreateTime" DATE NULL,

	"Creator" nvarchar2(255) NULL,

	"EditTime" DATE NULL,

	"Editor" nvarchar2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMAC"
(

	"primaryKey" RAW(16) NOT NULL,

	"TypeAccess" nvarchar2(7) NULL,

	"Filter_m0" RAW(16) NULL,

	"Permition_m0" RAW(16) NOT NULL,

	"CreateTime" DATE NULL,

	"Creator" nvarchar2(255) NULL,

	"EditTime" DATE NULL,

	"Editor" nvarchar2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMLO"
(

	"primaryKey" RAW(16) NOT NULL,

	"Class_m0" RAW(16) NOT NULL,

	"Operation_m0" RAW(16) NOT NULL,

	"CreateTime" DATE NULL,

	"Creator" nvarchar2(255) NULL,

	"EditTime" DATE NULL,

	"Editor" nvarchar2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMLA"
(

	"primaryKey" RAW(16) NOT NULL,

	"View_m0" RAW(16) NOT NULL,

	"Attribute_m0" RAW(16) NOT NULL,

	"CreateTime" DATE NULL,

	"Creator" nvarchar2(255) NULL,

	"EditTime" DATE NULL,

	"Editor" nvarchar2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMLV"
(

	"primaryKey" RAW(16) NOT NULL,

	"Class_m0" RAW(16) NOT NULL,

	"View_m0" RAW(16) NOT NULL,

	"CreateTime" DATE NULL,

	"Creator" nvarchar2(255) NULL,

	"EditTime" DATE NULL,

	"Editor" nvarchar2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMLR"
(

	"primaryKey" RAW(16) NOT NULL,

	"StartDate" DATE NULL,

	"EndDate" DATE NULL,

	"Agent_m0" RAW(16) NOT NULL,

	"Role_m0" RAW(16) NOT NULL,

	"CreateTime" DATE NULL,

	"Creator" nvarchar2(255) NULL,

	"EditTime" DATE NULL,

	"Editor" nvarchar2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;



ALTER TABLE "Class2"
	ADD CONSTRAINT "Class2_FClass1_0" FOREIGN KEY ("Class1") REFERENCES "Class1" ("primaryKey");

CREATE INDEX "Class2_IClass1" on "Class2" ("Class1");

ALTER TABLE "Class2"
	ADD CONSTRAINT "Class2_FClass4_0" FOREIGN KEY ("Class4") REFERENCES "Class4" ("primaryKey");

CREATE INDEX "Class2_IClass4" on "Class2" ("Class4");

ALTER TABLE "Class3"
	ADD CONSTRAINT "Class3_FClass2_0" FOREIGN KEY ("Class2") REFERENCES "Class2" ("primaryKey");

CREATE INDEX "Class3_IClass2" on "Class3" ("Class2");

ALTER TABLE "STORMWEBSEARCH"
	ADD CONSTRAINT "STORMWEBSEARCH_FSTORMFILT_6521" FOREIGN KEY ("FilterSetting_m0") REFERENCES "STORMFILTERSETTING" ("primaryKey");

ALTER TABLE "STORMFILTERDETAIL"
	ADD CONSTRAINT "STORMFILTERDETAIL_FSTORMF_2900" FOREIGN KEY ("FilterSetting_m0") REFERENCES "STORMFILTERSETTING" ("primaryKey");

ALTER TABLE "STORMFILTERLOOKUP"
	ADD CONSTRAINT "STORMFILTERLOOKUP_FSTORMF_1583" FOREIGN KEY ("FilterSetting_m0") REFERENCES "STORMFILTERSETTING" ("primaryKey");

ALTER TABLE "STORMLG"
	ADD CONSTRAINT "STORMLG_FSTORMAG_0" FOREIGN KEY ("Group_m0") REFERENCES "STORMAG" ("primaryKey");

ALTER TABLE "STORMLG"
	ADD CONSTRAINT "STORMLG_FSTORMAG_1" FOREIGN KEY ("User_m0") REFERENCES "STORMAG" ("primaryKey");

ALTER TABLE "STORMAuEntity"
	ADD CONSTRAINT "STORMAuEntity_FSTORMAG_0" FOREIGN KEY ("User_m0") REFERENCES "STORMAG" ("primaryKey");

ALTER TABLE "STORMAuEntity"
	ADD CONSTRAINT "STORMAuEntity_FSTORMAuObj_3287" FOREIGN KEY ("ObjectType_m0") REFERENCES "STORMAuObjType" ("primaryKey");

ALTER TABLE "STORMAuField"
	ADD CONSTRAINT "STORMAuField_FSTORMAuField_0" FOREIGN KEY ("MainChange_m0") REFERENCES "STORMAuField" ("primaryKey");

ALTER TABLE "STORMAuField"
	ADD CONSTRAINT "STORMAuField_FSTORMAuEntity_0" FOREIGN KEY ("AuditEntity_m0") REFERENCES "STORMAuEntity" ("primaryKey");

ALTER TABLE "STORMI"
	ADD CONSTRAINT "STORMI_FSTORMAG_0" FOREIGN KEY ("User_m0") REFERENCES "STORMAG" ("primaryKey");

ALTER TABLE "STORMI"
	ADD CONSTRAINT "STORMI_FSTORMAG_1" FOREIGN KEY ("Agent_m0") REFERENCES "STORMAG" ("primaryKey");

ALTER TABLE "STORMP"
	ADD CONSTRAINT "STORMP_FSTORMS_0" FOREIGN KEY ("Subject_m0") REFERENCES "STORMS" ("primaryKey");

ALTER TABLE "STORMP"
	ADD CONSTRAINT "STORMP_FSTORMAG_0" FOREIGN KEY ("Agent_m0") REFERENCES "STORMAG" ("primaryKey");

ALTER TABLE "STORMF"
	ADD CONSTRAINT "STORMF_FSTORMS_0" FOREIGN KEY ("Subject_m0") REFERENCES "STORMS" ("primaryKey");

ALTER TABLE "STORMAC"
	ADD CONSTRAINT "STORMAC_FSTORMF_0" FOREIGN KEY ("Filter_m0") REFERENCES "STORMF" ("primaryKey");

ALTER TABLE "STORMAC"
	ADD CONSTRAINT "STORMAC_FSTORMP_0" FOREIGN KEY ("Permition_m0") REFERENCES "STORMP" ("primaryKey");

ALTER TABLE "STORMLO"
	ADD CONSTRAINT "STORMLO_FSTORMS_0" FOREIGN KEY ("Class_m0") REFERENCES "STORMS" ("primaryKey");

ALTER TABLE "STORMLO"
	ADD CONSTRAINT "STORMLO_FSTORMS_1" FOREIGN KEY ("Operation_m0") REFERENCES "STORMS" ("primaryKey");

ALTER TABLE "STORMLA"
	ADD CONSTRAINT "STORMLA_FSTORMS_0" FOREIGN KEY ("View_m0") REFERENCES "STORMS" ("primaryKey");

ALTER TABLE "STORMLA"
	ADD CONSTRAINT "STORMLA_FSTORMS_1" FOREIGN KEY ("Attribute_m0") REFERENCES "STORMS" ("primaryKey");

ALTER TABLE "STORMLV"
	ADD CONSTRAINT "STORMLV_FSTORMS_0" FOREIGN KEY ("Class_m0") REFERENCES "STORMS" ("primaryKey");

ALTER TABLE "STORMLV"
	ADD CONSTRAINT "STORMLV_FSTORMS_1" FOREIGN KEY ("View_m0") REFERENCES "STORMS" ("primaryKey");

ALTER TABLE "STORMLR"
	ADD CONSTRAINT "STORMLR_FSTORMAG_0" FOREIGN KEY ("Agent_m0") REFERENCES "STORMAG" ("primaryKey");

ALTER TABLE "STORMLR"
	ADD CONSTRAINT "STORMLR_FSTORMAG_1" FOREIGN KEY ("Role_m0") REFERENCES "STORMAG" ("primaryKey");


