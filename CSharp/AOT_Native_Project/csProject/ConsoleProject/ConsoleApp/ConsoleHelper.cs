using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleApp;

internal partial class ConsoleHelper
{
    private const int FIXED_BUFFER_SIZE = 1024;
    private const string DLL_NAME = "ConsoleAppNative.dll";
    private static readonly bool _isLoaded;

    static ConsoleHelper()
    {
        _isLoaded = NativeLibrary.TryLoad(DLL_NAME, Assembly.GetExecutingAssembly(), null, out _);
    }

    [LibraryImport(DLL_NAME, EntryPoint = "GetDecryptedCpp", StringMarshalling = StringMarshalling.Utf8)]
    private static partial int GetDecryptedCpp(Span<byte> buffer, int bufferLen);
    
    [LibraryImport(DLL_NAME, EntryPoint = "GetDecryptedRust", StringMarshalling = StringMarshalling.Utf8)]
    private static partial int GetDecryptedRust(Span<byte> buffer, int bufferLen);
    
    [LibraryImport(DLL_NAME, EntryPoint = "GetDecryptedZig", StringMarshalling = StringMarshalling.Utf8)]
    private static partial int GetDecryptedZig(Span<byte> buffer, int bufferLen);

    public static string GetMessageCpp()
    {
        if (!_isLoaded)
        {
            Console.WriteLine($"Error: {DLL_NAME} 모듈을 찾을 수 없습니다.");
            return string.Empty;
        }

        Span<byte> buffer = stackalloc byte[FIXED_BUFFER_SIZE];

        try
        {
            int result = GetDecryptedCpp(buffer, buffer.Length);
            return result <= 0 ? string.Empty : Encoding.UTF8.GetString(buffer[..result]).TrimEnd('\0');
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return string.Empty;
        }
    }
    
    public static string GetMessageRust()
    {
        if (!_isLoaded)
        {
            Console.WriteLine($"Error: {DLL_NAME} 모듈을 찾을 수 없습니다.");
            return string.Empty;
        }

        Span<byte> buffer = stackalloc byte[FIXED_BUFFER_SIZE];

        try
        {
            int result = GetDecryptedRust(buffer, buffer.Length);
            return result <= 0 ? string.Empty : Encoding.UTF8.GetString(buffer[..result]).TrimEnd('\0');
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return string.Empty;
        }
    }
    
    public static string GetMessageZig()
    {
        if (!_isLoaded)
        {
            Console.WriteLine($"Error: {DLL_NAME} 모듈을 찾을 수 없습니다.");
            return string.Empty;
        }

        Span<byte> buffer = stackalloc byte[FIXED_BUFFER_SIZE];

        try
        {
            int result = GetDecryptedZig(buffer, buffer.Length);
            return result <= 0 ? string.Empty : Encoding.UTF8.GetString(buffer[..result]).TrimEnd('\0');
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return string.Empty;
        }
    }
}