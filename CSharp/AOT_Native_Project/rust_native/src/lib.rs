use obfstr::obfbytes;
use std::ptr;

const SALT: i32 = 0x1234;
const VALID_KEY: i32 = 0x7777;
const MAX_INTERNAL_BUFFER: usize = 1024;

#[unsafe(no_mangle)]
pub extern "C" fn get_secret_data(buffer: *mut u8, buffer_len: i32, obfuscated_key: i32) -> i32 {
    if buffer.is_null() || buffer_len <= 0 {
        return 0;
    }

    if (obfuscated_key ^ SALT) != VALID_KEY {
        return -1;
    }

    let bytes: &[u8; 21] = obfbytes!("rust 보안 문자열".as_bytes());
    let safe_limit: usize = (buffer_len as usize).min(MAX_INTERNAL_BUFFER);
    let copy_len: usize = bytes.len().min(safe_limit - 1);

    unsafe {
        ptr::copy_nonoverlapping(bytes.as_ptr(), buffer, copy_len);
        ptr::write(buffer.add(copy_len), 0);
    }

    copy_len as i32
}

#[cfg(test)]
mod tests {
    #[test]
    fn check_string_len() {
        let s: &str = "rust 보안 문자열";
        println!("바이트 길이: {}", s.len());
        //assert_eq!(s.len(), 21);
    }
}

// cargo test -- --nocapture
// cargo build --release
