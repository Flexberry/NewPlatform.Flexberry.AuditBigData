



CREATE TABLE "Audit" (

 "primaryKey" UUID,

 "UserName" String,

 "UserLogin" String,

 "ObjectType" String,

 "ObjectPrimaryKey" String,

 "OperationTime" DateTime,

 "OperationType" String,

 "ExecutionStatus" String,

 "Source" String,

 "SerializedFields" StringMax,

 "HeadAuditEntity" UUID

) ENGINE = MergeTree() ORDER BY ("primaryKey");




