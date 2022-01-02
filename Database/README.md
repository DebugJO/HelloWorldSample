# Database
에이디치히얌 개발소스

## SQLite Query Sample
```sql
-- milliseconds
SELECT cast((strftime('%Y%m%d%H%M%S', datetime('now')) || substr(strftime('%f','now'),4)) as varchar) as today

-- localtime
SELECT cast(strftime('%Y%m%d%H%M%S', datetime('now', 'localtime')) as varchar) as today
```