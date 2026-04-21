using System;

namespace MyAppLib.Helpers;

public interface IResult<out TSelf, in TValue> where TSelf : IResult<TSelf, TValue>
{
    static abstract TSelf Success(TValue value);

    static abstract TSelf Failure(string message, int errorCode = 500);
}

public record Result<T> : IResult<Result<T>, T>
{
    public T? Value { get; }
    public string? ErrorMessage { get; }
    public int ErrorCode { get; }
    public ResultState State { get; }

    public enum ResultState
    {
        Success,
        None,
        Failure
    }

    private Result(ResultState state, T? value, string? error, int code)
    {
        State = state;
        Value = value;
        ErrorMessage = error;
        ErrorCode = code;
    }

    public static Result<T> Success(T value) => new(ResultState.Success, value, null, 0);

    public static Result<T> Failure(string message, int errorCode = 500)
        => new(ResultState.Failure, default, message, errorCode);

    public static Result<T> None() => new(ResultState.None, default, null, 0);

    public static implicit operator Result<T>(T value) => Success(value);

    public static implicit operator Result<T>(None _) => None();

    public TResult Match<TResult>(Func<T, TResult> success, Func<TResult> none, Func<string, int, TResult> failure)
        => State switch
        {
            ResultState.Success => success(Value!),
            ResultState.None => none(),
            ResultState.Failure => failure(ErrorMessage!, ErrorCode),
            _ => throw new InvalidOperationException()
        };

    public void Match(Action<T> success, Action none, Action<string, int> failure)
    {
        switch (State)
        {
            case ResultState.Success: success(Value!); break;
            case ResultState.None: none(); break;
            case ResultState.Failure: failure(ErrorMessage!, ErrorCode); break;
            default: throw new InvalidOperationException();
        }
    }
}

public readonly struct None
{
    public static None Value => new();
}

public static class ResultExtensions
{
    // 공통 실패 처리 (400 Bad Request)
    public static TResult ToBadRequest<TResult, TValue>(string message)
        where TResult : IResult<TResult, TValue>
        => TResult.Failure(message, 400);

    // 공통 실패 처리 (401 Unauthorized)
    public static TResult ToUnauthorized<TResult, TValue>(string message = "인증되지 않았습니다.")
        where TResult : IResult<TResult, TValue>
        => TResult.Failure(message, 401);

    // 공통 성공 처리 (단순 값을 결과 객체로 변환)
    public static TResult ToSuccess<TResult, TValue>(this TValue value)
        where TResult : IResult<TResult, TValue>
        => TResult.Success(value);
}

// public Result<string> GetUsername(int id)
// {
//     return id switch
//     {
//         < 0 => Result<string>.Failure("잘못된 접근입니다.", 400),
//         0 => None.Value,
//         _ => "John Doe"
//     };
// }
// Result<string> result = GetUsername(-1);
//
// string message = result.Match(
//     success: name => $"사용자: {name}",
//     none: () => "사용자를 찾을 수 없습니다.",
//     failure: (msg, code) => $" 오류({code}): {msg}"
// );
//
// LogHelper.Debug($"xxxxxxxxxxxxxxxxx : message = {message}");
// ====================================================================================================
// public Result<string> ProcessPayment(decimal amount)
// {
//     switch (amount)
//     {
//         // if (!User.IsAuthenticated) 
//         case > 30:
//             return ResultExtensions.ToUnauthorized<Result<string>, string>();
//         case <= 0:
//             return ResultExtensions.ToBadRequest<Result<string>, string>("금액이 올바르지 않습니다.");
//         default:
//         {
//             string receipt = $"결제 완료: {amount}원";
//             return receipt.ToSuccess<Result<string>, string>();
//         }
//     }
// }
// Result<string> result1 = ProcessPayment(1000);
// result1.Match(
//     success: s => LogHelper.Debug($"결과 : {s}"),
//     none: () => LogHelper.Debug("결과 없음"),
//     failure: (msg, code) => LogHelper.Debug($"Error {code}: {msg}")
// );
// ====================================================================================================
// public static class ErrorCodes
// {
//     public const int InvalidInput = 400;
//     public const int Unauthorized = 401;
//     public const int NotFound = 404;
//     public const int ServerError = 500;
// }
//
// public Result<User> GetUserAccount(int id)
// {
//     if (id < 0) 
//         return Result<User>.Failure("아이디는 음수일 수 없습니다.", ErrorCodes.InvalidInput);
//
//     if (id == 999) // 특정 금지된 아이디 가정
//         return Result<User>.Failure("접근 권한이 없는 계정입니다.", ErrorCodes.Unauthorized);
//
//     var user = _repository.Find(id);
//     if (user == null) return None.Value;
//
//     return user; // 성공 (자동 형변환)
// }
// ====================================================================================================
// public interface IUserService 
// {
//     Result<string> GetUserName(int id);
// }
//
// public class UserService : IUserService
// {
//     public Result<string> GetUserName(int id)
//     {
//         if (id == 0) return None.Value;
//         return "John Doe";
//     }
// }
// builder.Services.AddScoped<IUserService, UserService>();
// public class UserController(IUserService userService) : ControllerBase
// {
//     public IActionResult Get(int id)
//     {
//         // 서비스에서 Result<string>을 받아와서 Match로 처리
//         return userService.GetUserName(id).Match<IActionResult>(
//             success: name => Ok(name),
//             none: () => NotFound(),
//             failure: (msg, code) => StatusCode(code, msg)
//         );
//     }
// }
// public Result<string> DoSomething()
// {
//     return ResultExtensions.ToBadRequest<Result<string>, string>("에러!");
// }
// public IActionResult Process(int id)
// {
//     var userResult = _userService.GetUserName(id);
//     
//     // 패턴 매칭을 통해 아주 선언적으로 작성 가능
//     return userResult.Match<IActionResult>(
//         success: name => Ok($"어서오세요, {name}님"),
//         none: () => NotFound("사용자를 찾을 수 없습니다."),
//         failure: (msg, code) => StatusCode(code, msg)
//     );
// }