### SQL 에서 GROUP_CONCAT 사용하기

##### MySQL
```sql
SELECT student_name, GROUP_CONCAT(test_score)
FROM student
GROUP BY student_name;

// SEPARATOR는 구분자(',', #13 등) 사용
SELECT B.id, group_concat(B.value SEPARATOR '-')
FROM A, B
WHERE B.id = A.id
GROUP BY B.id
```

