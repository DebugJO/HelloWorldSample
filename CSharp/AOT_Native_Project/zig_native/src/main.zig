const std = @import("std");

const SALT: i32 = 0x1234;
const VALID_KEY: i32 = 0x7777;
const XOR_KEY: u8 = 0x5A;
const MAX_INTERNAL_BUFFER: usize = 1024;

fn obfuscate(comptime str: []const u8) [str.len]u8 {
    var res: [str.len]u8 = undefined;
    for (str, 0..) |c, i| {
        res[i] = c ^ XOR_KEY;
    }
    return res;
}

const secret_data = obfuscate("zig 보안 문자열");

export fn get_secret_data(buffer: [*]u8, buffer_len: i32, obfuscated_key: i32) callconv(.c) i32 {
    if (buffer_len <= 0) {
        return 0;
    }

    if ((obfuscated_key ^ SALT) != VALID_KEY) {
        return -1;
    }

    const safe_limit = @as(usize, @intCast(buffer_len));

    const internal_limit =
        if (safe_limit < MAX_INTERNAL_BUFFER)
            safe_limit
        else
            MAX_INTERNAL_BUFFER;

    const copy_len =
        if (secret_data.len < internal_limit)
            secret_data.len
        else
            internal_limit - 1;

    var i: usize = 0;

    while (i < copy_len) : (i += 1) {
        buffer[i] = secret_data[i] ^ XOR_KEY;
    }

    buffer[copy_len] = 0;
    return @as(i32, @intCast(copy_len));
}
