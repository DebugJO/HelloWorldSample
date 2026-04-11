using ConsoleAppLib;
using System;
using System.Threading.Tasks;

namespace ConsoleApp;

internal class Program
{
    private static async Task Main()
    {
        string messageCpp = ConsoleHelper.GetMessageCpp();
        string resultCpp = !string.IsNullOrWhiteSpace(messageCpp)
            ? $"받은 메시지(Cpp): {messageCpp}"
            : "메시지(cpp)가 없거나 읽기에 실패했습니다.";

        Console.WriteLine(resultCpp);
        LogHelper.Info(resultCpp);
        
        string messageRust = ConsoleHelper.GetMessageRust();
        string resultRust = !string.IsNullOrWhiteSpace(messageRust)
            ? $"받은 메시지(Rust): {messageRust}"
            : "메시지(rust)가 없거나 읽기에 실패했습니다.";

        Console.WriteLine(resultRust);
        LogHelper.Info(resultRust);
        
        string messageZig = ConsoleHelper.GetMessageZig();
        string resultZig = !string.IsNullOrWhiteSpace(messageZig)
            ? $"받은 메시지(Zig): {messageZig}"
            : "메시지(Zig)가 없거나 읽기에 실패했습니다.";

        Console.WriteLine(resultZig);
        LogHelper.Info(resultZig);

        bool isLogClose = await Task.Run(() =>
        {
            Console.WriteLine("로그 Flush ...");
            LogHelper.Shutdown();
            return true;
        });

        Console.WriteLine(isLogClose ? "로그 Flush 완료" : "로그 Flush 에러");
    }
}

/* 싱글 파일 배포
dotnet publish -c Release : csproj에서 설정한 경우
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
*/