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

### SQL Server Snapshot 트랙잭션

```sql
-- 스냅샷을 해당 데이터베이스에 혀용했다고 가정
-- ALTER DATABASE YourDatabaseName SET ALLOW_SNAPSHOT_ISOLATION ON;

CREATE PROCEDURE usp_UpdateWithTimeoutHandling
    @ErrorMsg NVARCHAR(4000) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- 타임아웃 시간을 5000 밀리초(5초)로 설정합니다.
    SET LOCK_TIMEOUT 5000;

    -- 트랜잭션 격리 수준을 스냅샷으로 설정합니다.
    SET TRANSACTION ISOLATION LEVEL SNAPSHOT;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- 여기에 UPDATE 문을 작성합니다.
        UPDATE YourTable
        SET Column1 = 'NewValue'
        WHERE SomeCondition = 'SomeValue';

        -- 예제 SELECT 문 (타임아웃을 유도할 수 있습니다)
        -- 실제 상황에 맞게 쿼리를 조정하십시오.
        SELECT Column1, Column2
        FROM YourTable WITH (ROWLOCK, UPDLOCK)
        WHERE SomeCondition = 'SomeValue';

        COMMIT TRANSACTION;
        SET @ErrorMsg = NULL; -- 오류 메시지가 없음을 나타냅니다.
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

        -- 타임아웃 오류가 발생했을 때 처리할 코드입니다.
        IF ERROR_NUMBER() = 1222
        BEGIN
            SET @ErrorMsg = '타임아웃 오류가 발생했습니다.';
        END
        ELSE
        BEGIN
            -- 다른 오류 처리
            SET @ErrorMsg = ERROR_MESSAGE();
        END
    END CATCH
END
```
### SQL Server Snapshot 트랙잭션, C# 사용예제

```cs
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

public class DatabaseContext
{
    private readonly string _connectionString;

    public DatabaseContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<string> UpdateWithTimeoutHandlingAsync(CancellationToken cancellationToken)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var parameters = new DynamicParameters();
        parameters.Add("@ErrorMsg", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            "usp_UpdateWithTimeoutHandling",
            parameters,
            commandType: CommandType.StoredProcedure,
            commandTimeout: 5000, // 5초 타임아웃 설정
            cancellationToken: cancellationToken
        );

        return parameters.Get<string>("@ErrorMsg");
    }
}

public class Program
{
    private static async Task Main(string[] args)
    {
        var connectionString = "your_connection_string_here";
        var dbContext = new DatabaseContext(connectionString);

        using var cts = new CancellationTokenSource();

        // 사용자가 원하는 시점에 작업을 취소할 수 있도록 설정합니다.
        cts.CancelAfter(TimeSpan.FromSeconds(10));

        try
        {
            string errorMsg = await dbContext.UpdateWithTimeoutHandlingAsync(cts.Token);

            if (!string.IsNullOrEmpty(errorMsg))
            {
                Console.WriteLine($"오류 발생: {errorMsg}");
            }
            else
            {
                Console.WriteLine("작업이 성공적으로 완료되었습니다.");
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("작업이 취소되었습니다.");
        }
    }
}
```
