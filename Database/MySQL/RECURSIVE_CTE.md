```sql
WITH RECURSIVE cte (num) AS
(
   SELECT 1
   UNION ALL
   SELECT num + 1 FROM cte WHERE num < 5
 )
SELECT * FROM cte;
```

```sql
-- SHOW VARIABLES LIKE 'max_recursive_iterations';
SET SESSION max_recursive_iterations = 4000; -- mariadb
-- mysql : cte_max_recursion_depth

WITH RECURSIVE date_generator(date_id) AS (
    SELECT '20230101'
    UNION ALL
    SELECT DATE_FORMAT(DATE_ADD(date_id, INTERVAL 1 DAY), '%Y%m%d')
    FROM date_generator
    WHERE date_id < '20321231'
)

SELECT date_id FROM date_generator;
```
