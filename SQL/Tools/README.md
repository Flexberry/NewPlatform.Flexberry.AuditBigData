# NewPlatform.Flexberry.AuditBigData

## Каталог вспомогательных скриптов.

PostgreSql.update.texttojsonb.sql - скрипт для конвертации типа столбца SerializedFields из TEXT в JSONB. Изначально в AuditBigData тип был TEXT и сейчас,
на БД, которые были размернуты по старому типу, может потребоваться перевести столбец в новый. Данные при этом сохраняются.
