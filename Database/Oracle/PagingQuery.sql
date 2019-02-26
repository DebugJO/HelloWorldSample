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
  
OR
  
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

-- 출처: https://yangyag.tistory.com/295?category=712845 [Hello Brother!]
