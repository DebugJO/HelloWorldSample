WITH CTE (NUM) AS
(
  SELECT 1
  UNION ALL
  SELECT NUM + 1
  FROM CTE
  WHERE NUM < 1000)

SELECT NUM
FROM CTE
OPTION (MAXRECURSION 1000)


declare @Result table
(
   cDate varchar(8),
   rDate date
)

declare @Date date;
set @Date = convert(date, '20220101', 112)
declare @cDate varchar(8)

while @Date <= convert(date, '20991231', 112)
begin
    set @cDate = convert(char(8), @Date, 112);
    insert into @Result(cDate, rDate) values (@cDate, @Date);
    set @Date = DATEADD(dd, 1, @Date);
END


select * from @Result where cDate between '20220101' and '20991231'


DECLARE @dStart DATE,
        @dEnd DATE
SET @dStart =  convert(datetime, '2022-01-01 00:00:00', 120)
SET @dEnd = DATEADD (YEAR, 80, @dStart)
;WITH CTE AS
(
    SELECT @dStart AS dDay
    UNION ALL
    SELECT DATEADD (DAY, 1, dDay)
    FROM CTE
    WHERE dDay < @dEnd
)
SELECT dDay, convert(varchar(8), dDay, 112) FROM CTE
OPTION (MaxRecursion 32767)


select convert(datetime, '2022-01-01 00:00:00', 120)

DECLARE @dStart DATE,
        @dEnd DATE
SET @dStart = GETDATE ()
SET @dEnd = DATEADD (YEAR, 80, @dStart)
;WITH CTE AS
(
    SELECT @dStart AS dDay
    UNION ALL
    SELECT DATEADD (DAY, 1, dDay)
    FROM CTE
    WHERE dDay < @dEnd
)
SELECT * FROM CTE
OPTION (MaxRecursion 32767)
