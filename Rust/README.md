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
    
    fn get_name_str1(&self) -> &str {
        let a = self.last_name.to_string() + &self.first_name.to_string();
        return  Box::leak(a.into_boxed_str());
    }    

    fn get_name_str2(&self) -> &str {
        let b = self.last_name.to_string() + &self.first_name.to_string();
        return string_to_static_str(b);
    }
}

fn string_to_static_str(s: String) -> &'static str {
    Box::leak(s.into_boxed_str())
}

pub fn run() {
    let p = Person::new("길동", "홍");
    println!("Person: {} / {}", p.get_name_string(), p.get_name_str1(), p.get_name_str2());
}
```

### Option : [mithradates, "047 Easy Rust in Korean: More Option"](https://www.youtube.com/watch?v=uhMO2b13bUA)
```rust
fn take_fifth(value: Vec<i32>) -> Option<i32> {
    if value.len() < 5 {
        None
    } else {
        Some(value[4])
    }
}

fn main() {
    let new_vec1 = vec![1, 2, 3, 4, 5];
    let index1 = take_fifth(new_vec1);

    match index1 {
        Some(number) => println!("I got a number: {}", number),
        None => println!("There was nothing inside")
    }

    let new_vec2 = vec![1, 2];
    let index2 = take_fifth(new_vec2);

    if index2.is_some() {
        println!("I got a number: {}", index2.unwrap());
    } else {
        println!("There was nothing inside");
    }
}
```

### Result : [mithradates, "Easy Rust Korean"](https://www.youtube.com/playlist?list=PLfllocyHVgsSJf1zO6k6o3SX2mbZjAqYE)
```rust
fn check_error(input: i32) -> Result<(), ()> {
    if input % 2 == 0 {
        Ok(())
    } else {
        Err(())
    }
}

fn main() {
    if check_error(5).is_ok() {
        println!("It's okay, guys!");
    } else {
        println!("It's an error, guys!");
    }

    match check_error(4) {
        Ok(..) => println!("It's okay, guys!"),
        Err(..) => println!("It's an error, guys!")
    }
}
```

```rust
fn check_if_five(number: i32) -> Result<i32, String> {
    match number {
        5 => Ok(number),
        _ => Err("Sorry, the number wasn't five.".to_string())
    }
}

fn main() {
    let mut result_vec = Vec::new(); // Vec<Result<i32, String>>

    for number in 2..=7 {
        result_vec.push(check_if_five(number));
    }

    println!("{:#?}", result_vec);
}
```

```rust
fn parse_number(number: &str) -> Result<i32, std::num::ParseIntError> {
    number.parse()
}

fn main() {
    let mut result_vec: Vec<Result<i32, std::num::ParseIntError>> = vec![];
    result_vec.push(parse_number("8"));
    result_vec.push(parse_number("one"));
    result_vec.push(parse_number("7"));

    for number in result_vec {
        if number.is_ok() {
            println!("Ok: {:?}", number.unwrap())
        } else {
            println!("Err: {:?}", number.unwrap_err().kind())
        }
    }
}
```

```rust
fn parse_number(number: &str) -> Result<i32, std::num::ParseIntError> {
    number.parse()
}

fn main() {
    let mut result_vec: Vec<Result<i32, std::num::ParseIntError>> = vec![];
    result_vec.push(parse_number("8"));
    result_vec.push(parse_number("one"));
    result_vec.push(parse_number("7"));

    for index in 0..result_vec.iter().count() {
        if let Some(number) = result_vec.get(index) {
            println!("{:?}", number.as_ref().unwrap_or(&0));
        }
    }
}
```

```rust
fn main() {
    let item_vec = vec![
        vec!["홍길동", "홍길서", "홍길남", "홍길북", "10"],
        vec!["가나닭", "20", "30"],
    ];

    for mut item in item_vec {
        while let Some(info) = item.pop() {
            if let Ok(number) = info.parse::<i32>() {
                println!("The number is: {}", number);
            }
        }
    }
}
```
