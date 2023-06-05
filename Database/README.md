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

## SQL Server DATETIME where clause
2023-06-01 오전 11:08:37
```sql
select getdate(), dateadd(hh, -1, getdate())

select k.* from OcsResultSendCheck k where format(k.RegDate, 'yyyy-MM-dd HH:mm:ss') = '2023-06-01 11:08:37'

select k.* from OcsResultSendCheck k where k.RegDate >= '2023-06-01 11:08:37' and k.RegDate < '2023-06-01 11:08:38'
select k.* from OcsResultSendCheck k where k.RegDate between '2023-06-01 11:08:37' and '2023-06-01 11:08:38'
```
