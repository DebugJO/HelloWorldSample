# Database
Sample, Reference

## SQLite Query Sample
```sql
-- milliseconds
select cast((strftime('%Y%m%d%H%M%S', datetime('now')) || substr(strftime('%f','now'),4)) as varchar) as today

-- localtime
select cast(strftime('%Y%m%d%H%M%S', datetime('now', 'localtime')) as varchar) as today

 -- LPAD 000~999
 select substr('000' || cast((abs(random()) % 1000) as varchar), -3, 3)
 
 -- RPAD 000~999
 select substr(cast((abs(random()) % 1000) as varchar) || '000', 1, 3)
```
