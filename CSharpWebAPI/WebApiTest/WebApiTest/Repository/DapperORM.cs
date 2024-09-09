using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest.Helpers;

namespace WebApiTest.Repository;

public static class DapperORM
{
    private static readonly string SQLITE = @$"Data Source={Environment.CurrentDirectory}\Database\OLLPass.db;Mode=ReadWrite";

    public static async Task<DbResult<List<T>>> ReturnListAsyncLite<T>(string sql, DynamicParameters parameters) where T : class, new()
    {
        try
        {
            using IDbConnection DapperLite = new SqliteConnection(SQLITE);
            List<T> obj = (await DapperLite.QueryAsync<T>(sql, parameters, commandType: CommandType.Text)).ToList();

            return new DbResult<List<T>>
            {
                Kind = obj.Count > 0 ? EResultKind.OK : EResultKind.EMPTY,
                Result = obj
            };
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"DapperORM(ReturnListAsyncLite) : ERROR : {ex.Message.ToSingleLine()}");
            return new DbResult<List<T>> { Kind = EResultKind.ERROR, Error = ex.Message };
        }
    }

    public static async Task<DbResult<T>> ReturnSingleAsyncLite<T>(string sql, DynamicParameters parameters) where T : class, new()
    {
        try
        {
            using IDbConnection DapperLite = new SqliteConnection(SQLITE);
            T? obj = await DapperLite.QuerySingleOrDefaultAsync<T>(sql, parameters, commandType: CommandType.Text);

            return new DbResult<T>
            {
                Kind = obj != null ? EResultKind.OK : EResultKind.EMPTY,
                Result = obj ?? new T()
            };
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"DapperORM(ReturnSingleAsyncLite) : ERROR : {ex.Message.ToSingleLine()}");
            return new DbResult<T> { Kind = EResultKind.ERROR, Error = ex.Message };

        }
    }
}

public class DbResult<T> where T : class, new()
{
    public EResultKind Kind { get; set; }
    public string Error { get; set; } = "처리 결과가 없습니다";
    public T Result { get; set; } = new();
}

public enum EResultKind
{
    OK,
    EMPTY,
    ERROR
}