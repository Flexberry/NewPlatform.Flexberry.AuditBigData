CREATE TABLE Audit
(
    "primaryKey" UUID,
    "UserName" String,
    "UserLogin" String,
    "OperationId" Nullable(UUID),
    "OperationTags" String,
    "ObjectType" String,
    "ObjectPrimaryKey" String,
    "OperationTime" DateTime,
    "OperationType" String,
    "ExecutionStatus" String,
    "Source" String,
    "SerializedFields" String,
    "HeadAuditEntity" Nullable(UUID)
)
ENGINE = MergeTree()
ORDER BY OperationTime
SETTINGS index_granularity = 8192;


CREATE TABLE AuditBuffer AS Audit ENGINE = Buffer(test, Audit, 16, 0.1, 2, 10, 10000, 100000, 1000000);
