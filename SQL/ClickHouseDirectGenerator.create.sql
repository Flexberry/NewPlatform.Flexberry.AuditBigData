CREATE TABLE Audit
(
    "primaryKey" UUID,
    "UserName" Nullable(String),
    "UserLogin" Nullable(String),
    "OperationId" Nullable(UUID),
    "OperationTags" String,
    "ObjectType" Nullable(String),
    "ObjectPrimaryKey" Nullable(String),
    "OperationTime" DateTime,
    "OperationType" String,
    "ExecutionStatus" String,
    "Source" Nullable(String),
    "SerializedFields" Nullable(String),
    "HeadAuditEntity" Nullable(UUID)
)
ENGINE = MergeTree()
ORDER BY OperationTime
SETTINGS index_granularity = 8192;


CREATE TABLE AuditBuffer AS Audit ENGINE = Buffer(currentDatabase(), Audit, 16, 0.1, 2, 10, 10000, 100000, 1000000);
