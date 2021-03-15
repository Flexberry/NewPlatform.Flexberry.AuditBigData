CREATE TABLE "Audit" (

 "primaryKey" UUID,

 "UserName" String,

 "UserLogin" String,

 "OperationId" Nullable(UUID),

 "OperationTags" String,

 "ObjectType" String,

 "ObjectPrimaryKey" NVARCHAR(38),

 "OperationTime" DateTime,

 "OperationType" String,

 "ExecutionStatus" String,

 "Source" String,

 "SerializedFields" String,

 "HeadAuditEntity" UUID

) ENGINE = MergeTree() ORDER BY ("primaryKey");
