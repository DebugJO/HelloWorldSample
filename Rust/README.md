### Rust WebAPI

* Actix
* Rocket

### OpenSSL 라이브러리 설정 : Eclipse Paho MQTT Rust Client Library

다운로드(Win32/Win64 OpenSSL) : https://slproweb.com/products/Win32OpenSSL.html

```bat
set OPENSSL_ROOT_DIR=C:\A_Library\OpenSSL64
set OPENSSL_INCLUDE_DIR=C:\A_Library\OpenSSL64\include\openssl
set OPENSSL_LIBRARIES=C:\A_Library\OpenSSL64\lib\VC
```

### [dependencies] paho-mqtt = "0.7.1"

* https://crates.io/crates/paho-mqtt
* https://github.com/eclipse/paho.mqtt.rust

### rust get json from webSite
* [ust_get_json.rs](https://github.com/DebugJO/HelloWorldSample/blob/master/Rust/rust_get_json.rs)

### String : &str
```rust
// helper.rs

struct Person {
    first_name: String,
    last_name: String,
}

impl Person {
    fn new(first: &str, last: &str) -> Self {
        Self {
            first_name: first.to_string(),
            last_name: last.to_string(),
        }
    }

    fn get_name_string(&self) -> String {
        return self.last_name.to_string() + &self.first_name.to_string();
    }

    fn get_name_str(&self) -> &str {
        let b = self.last_name.to_string() + &self.first_name.to_string();
        return string_to_static_str(b);
    }
}

fn string_to_static_str(s: String) -> &'static str {
    Box::leak(s.into_boxed_str())
}

pub fn run() {
    let p = Person::new("길동", "홍");
    println!("Person: {} / {}", p.get_name_string(), p.get_name_str());
}
``
