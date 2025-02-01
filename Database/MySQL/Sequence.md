```sql
DELIMITER //

CREATE FUNCTION GenerateUniqueKey()
RETURNS CHAR(20) DETERMINISTIC
BEGIN
    DECLARE unique_key CHAR(20);
    
    -- 날짜 14자리 + UUID_SHORT() 기반 6자리 SEQ
    SET unique_key = CONCAT(
        DATE_FORMAT(NOW(), '%Y%m%d%H%i%s'),
        LPAD(UUID_SHORT() MOD 1000000, 6, '0')
    );

    RETURN unique_key;
END //

DELIMITER ;
```

```sql
CREATE TABLE my_table (
    unique_key CHAR(20) PRIMARY KEY NOT NULL,
    name VARCHAR(100) NOT NULL
);
```

```sql
INSERT INTO my_table (unique_key, name) VALUES (GenerateUniqueKey(), 'Alice');
INSERT INTO my_table (unique_key, name) VALUES (GenerateUniqueKey(), 'Bob');
INSERT INTO my_table (unique_key, name) VALUES (GenerateUniqueKey(), 'Charlie');

SELECT * FROM my_table;
```
