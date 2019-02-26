### mysql, oracle 페이징 쿼리의 차이점

게시판을 만들 때 주로 사용하는 페이징 기법을 Oracle과 MySQL의 Query를 비교해서 살펴보도록 하겠습니다. 아주 기초사항만을 다루기 때문에 고급 Query는 스스로 학습을 통하여 살펴보는 것을 권장합니다.

#### 간단한 row출력
```sql
m : select * from mTable
o : select * from oTable
```

#### 처음 다섯개의 row만 출력
```sql
m : select * from mTable limit 5
o : select * from oTable where rownum <= 5 또는
    select rownum, id, name from oTable where rownum <=5
```

#### id컬럼에 의한 오름차순으로 row출력
```sql
m : select * from mTable order by id
o : select * from oTable order by id 또는
    select * from oTable where id > 0
```

#### id 컬럼 오름차순 정렬상태에서 위로부터 5개의 row만 출력
```sql
m : select * from mTable order by id limit 5
o : select id, name from oTable where rownum <=5 order by id
```

#### N번째 row부터 아래로 3개의 row만 출력
```sql
m : select * from mTable order by id limit 5,3
o : select id, name from (select rownum as rnum, id, name from oTable where id > 0) where rnum > 5 and rownum <=3
```

#### id 컬럼의 내림차순으로 역정렬 출력
```sql
m : select * from mTable order by id desc
o : select * from oTable order by id desc 또는
    select /*+ index_desc(oTable id_pk) */ id, name from oTable where thid > 0
```

#### N번째 row부터 역정렬 상태로, 아래로 3 row 만 출력
```sql
m : select * from mTable order by id desc limit 5,3
o : select id, name 
    from (select /*+ index_desc(oTable id_pk) */ 
                 id, name, rownum as rnum from oTable where id > 0) 
    where rnum >= 6 and rownum <= 3
```

#### 결론 : 몇개의 row로 한 묶음으로 하여 한 페이지 단위로 끊어서 본다면
```sql
// 예, PHP, $page:페이지넘버, $PostNum:페이지당 출력할 row수
m : select * from mTable order by id desc limit (($page-1)*PostNum), $PostNum
o : select id, name 
    from (select /*+ index_desc(oTable id_pk) */ 
                 id, name, rownum as rnum 
                 from oTable 
                 where id > 0) 
    where rnum >= (($page-1)*$PostNum+1) and rownum <= $PostNum
    
--최종쿼리(oracle)
SELECT  *
FROM
        (
        SELECT ROWNUM AS RNUM
              , A.*
        FROM (
            {검색쿼리 - 정렬이 필요할 경우 정렬조건 포함}
        ) A
        WHERE   ROWNUM <= {범위까지}
        )
WHERE   RNUM > {범위부터};
  
--또는
  
SELECT  *
FROM
        (
        SELECT
            /*+ INDEX_ASC or INDEX_DESC(A {정렬조건 인덱스명}) */
            ROWNUM AS RNUM
              , A.*
        FROM (
            {검색쿼리 - 정렬이 필요한 경우 정렬조건을 포함하지 않고 ORACLE 힌트사용}
        ) A
        WHERE   ROWNUM <= {범위까지}
        )
WHERE   RNUM > {범위부터};
```
