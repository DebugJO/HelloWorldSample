SELECT cast((strftime('%Y%m%d%H%M%S', datetime('now')) || substr(strftime('%f','now'),4)) as varchar) as today
union all
SELECT cast(strftime('%Y%m%d%H%M%S', datetime('now', 'localtime')) as varchar) as today
