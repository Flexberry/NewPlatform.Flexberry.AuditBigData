ALTER TABLE Audit
ALTER COLUMN SerializedFields TYPE JSONB using SerializedFields::JSON;