CREATE TABLE AuditMergeTree
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
ORDER BY primaryKey
SETTINGS index_granularity = 8192;


CREATE TABLE Audit AS AuditMergeTree ENGINE = Buffer(test, AuditMergeTree, 16, 1, 2, 10, 10000, 100000, 1000000);
