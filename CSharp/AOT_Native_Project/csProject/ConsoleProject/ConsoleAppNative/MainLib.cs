using System;
using System.Runtime.InteropServices;

namespace ConsoleAppNative;

public static partial class MainLib
{
    [LibraryImport("CppNative.dll", EntryPoint = "GetSecretData", StringMarshalling = StringMarshalling.Utf8)]
    private static unsafe partial int GetSecretDataCpp(byte* buffer, int obfuscatedKey);

    [LibraryImport("rust_native.dll", EntryPoint = "get_secret_data", StringMarshalling = StringMarshalling.Utf8)]
    private static unsafe partial int GetSecretDataRust(byte* buffer, int bufferLen, int obfuscatedKey);
    
    [LibraryImport("zig_native.dll", EntryPoint = "get_secret_data", StringMarshalling = StringMarshalling.Utf8)]
    private static unsafe partial int GetSecretDataZig(byte* buffer, int bufferLen, int obfuscatedKey);

    [UnmanagedCallersOnly(EntryPoint = "GetDecryptedCpp")]
    public static unsafe int GetDecryptedCpp(byte* outputBuffer, int outputLen)
    {
        const int fixed_buffer_size = 1024;
        const int real_key = 0x7777;
        const int salt = 0x1234;
        const int encoded_key = real_key ^ salt;

        byte* tempBuffer = stackalloc byte[fixed_buffer_size];
        int result = GetSecretDataCpp(tempBuffer, encoded_key);

        if (result <= 0)
        {
            return result;
        }

        int copyLen = Math.Min(result, outputLen);
        NativeMemory.Copy(tempBuffer, outputBuffer, (nuint)copyLen);
        return copyLen;
    }

    [UnmanagedCallersOnly(EntryPoint = "GetDecryptedRust")]
    public static unsafe int GetDecryptedRust(byte* outputBuffer, int outputLen)
    {
        const int fixed_buffer_size = 1024;
        const int real_key = 0x7777;
        const int salt = 0x1234;
        const int encoded_key = real_key ^ salt;

        byte* tempBuffer = stackalloc byte[fixed_buffer_size];
        int result = GetSecretDataRust(tempBuffer, fixed_buffer_size, encoded_key);

        if (result <= 0)
        {
            return result;
        }

        int copyLen = Math.Min(result, outputLen);
        NativeMemory.Copy(tempBuffer, outputBuffer, (nuint)copyLen);
        return copyLen;
    }
    
    [UnmanagedCallersOnly(EntryPoint = "GetDecryptedZig")]
    public static unsafe int GetDecryptedZig(byte* outputBuffer, int outputLen)
    {
        const int fixed_buffer_size = 1024;
        const int real_key = 0x7777;
        const int salt = 0x1234;
        const int encoded_key = real_key ^ salt;

        byte* tempBuffer = stackalloc byte[fixed_buffer_size];
        int result = GetSecretDataZig(tempBuffer, fixed_buffer_size, encoded_key);

        if (result <= 0)
        {
            return result;
        }

        int copyLen = Math.Min(result, outputLen);
        NativeMemory.Copy(tempBuffer, outputBuffer, (nuint)copyLen);
        return copyLen;
    }
}