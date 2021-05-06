CREATE TABLE [Audit] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [UserName] NVARCHAR(1024)  NULL,

	 [UserLogin] NVARCHAR(1024)  NULL,

	 [ObjectType] NVARCHAR(1024)  NULL,

	 [ObjectPrimaryKey] NVARCHAR(38)  NULL,

	 [OperationTime] DATETIME  NOT NULL,

	 [OperationType] VARCHAR(255)  NOT NULL,

	 [ExecutionStatus] VARCHAR(10)  NOT NULL,

	 [Source] VARCHAR(255)  NULL,

	 [SerializedFields] NVARCHAR(MAX)  NULL,

	 [HeadAuditEntity] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY NONCLUSTERED ([primaryKey]))
	 
ALTER TABLE [Audit] ADD CONSTRAINT [Audit_FAudit_0] FOREIGN KEY ([HeadAuditEntity]) REFERENCES [Audit]
CREATE INDEX Audit_IHeadAuditEntity on [Audit] ([HeadAuditEntity])
CREATE CLUSTERED INDEX Audit_CIOperationTime on [Audit] ([OperationTime])

