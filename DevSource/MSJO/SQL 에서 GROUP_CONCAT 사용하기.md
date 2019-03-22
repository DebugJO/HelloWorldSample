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

##### Oracle
```sql
SELECT
	deptno,
	LISTAGG(name, ',') within group (order by name) name
FROM test_tb
WHERE deptno = 1
GROUP BY deptno;

// WM_CONCAT
SELECT to_char(wm_concat(emp_name))
FROM dept_tb
GROUP BY dept_name;​

// 추가로 TRANSLATE('대상문자열', '비교문자', '바꿀문자')​ 또는 replace
SELECT translate(wm_concat(emp_name),'a,','a ')
FROM dept_tb
GROUP BY dept_name;
```

##### SQL Server
```sql
SELECT p.uid, p.person,
       stuff (g.group_name, 1, 1, '') as group_name
FROM   People p
CROSS APPLY
(
    SELECT ',' + s.group_name
    FROM   Teams t
           INNER JOIN Sports s ON t.gid = s.gid
    WHERE  t.uid = p.uid
    ORDER BY s.group_name
    FOR XML PATH ('')
) g (group_name)

// 또는 다음과 같이 EX1
SELECT table_name,
		LEFT(column_names , LEN(column_names )-1) AS column_names
FROM information_schema.columns AS extern
CROSS APPLY
(
    SELECT column_name + ','
    FROM information_schema.columns AS intern
    WHERE extern.table_name = intern.table_name
    FOR XML PATH('')
) pre_trimmed (column_names)
GROUP BY table_name, column_names;

// EX2
SELECT t1.SalesOrderNo,
    STUFF((SELECT ', '+t2.PartNo 
	FROM dbo.SOCuttersUsed t2 
	WHERE t1.SalesOrderNo = t2.SalesOrderNo
		FOR XML PATH('')),1,1,'') AS PartNo
FROM dbo.SOCuttersUsed t1
GROUP BY t1.SalesOrderNo
ORDER BY SalesOrderNo
```

##### SQL Server에서 SQLCLR로 작성한 사용자 함수(CONCAT) 사용하기
```
1. 다운로드
홈페이지 : https://archive.codeplex.com/?p=groupconcat
다운로드 : https://groupconcat.codeplex.com/downloads/get/1502310

2. 설치
내려받은 파일의 압축을 풀면 다음과 같이 4개의 파일로 구성되어 있다.
GroupConcatInstallation.sql : 설치 쿼리
GroupConcatUninstallation.sql : 삭제 쿼리
Test.1.BuildTestData.sql : 설치 확인 쿼리
Test.2.DemoFunctions.sql : 사용 예제
```

3. 사용방법
```sql
-- Test.2.DemoFunctions.sql에 포함된 예제
SELECT  DocID, dbo.GROUP_CONCAT(ErrorDetail) AS FieldTypeDetail
FROM dbo.GroupConcatTestData
GROUP BY DocID
ORDER BY DocID;

-- DISTINCT를 사용한다면
SELECT  DocID,
	dbo.GROUP_CONCAT_DS(DISTINCT ErrorDetail, N',', 1) AS FieldTypeDetail
FROM dbo.GroupConcatTestData
GROUP BY DocID
ORDER BY DocID ;

-- 홈페이지에 소개된 사용법 예제
SELECT some_id,
	dbo.GROUP_CONCAT(some_column) AS as delimited_list
FROM dbo.some_table
GROUP BY some_id;

/*결과
some_id    delimited_list
--------   -----------------------
1          red,green,blue
2          cyan,magenta,yellow,key
*/
```
