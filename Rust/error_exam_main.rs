// fn exit(x: i32) {
//     if x == 0 {
//         panic!("we got a 0");
//     }
//     println!("things are fine!");
// }

// fn main() {
//     exit(1);
//     exit(0); //error, panic
// }

// --------------------------------------

// fn exit(x: Option<i32>) {
//     match x {
//         Some(0) => panic!("we got a 0"),
//         Some(x) => println!("we got a {} things are fine!", x),
//         None => println!("we got nothing"),
//     }
// }

// fn main() {
//    exit(Some(1));
//    exit(Some(0)); // error, panic
//    exit(None);
// }

// --------------------------------------

// use std::{fs::File, io::ErrorKind};
// fn main() {
//     let f = File::open("test.txt");

//     let _f = match f {
//         Ok(file) => file,
//         Err(ref error) if error.kind() == ErrorKind::NotFound => match File::create("test.txt") {
//             Ok(fc) => fc,
//             Err(e) => {
//                 panic!("could not create file: {:?}", e)
//             }
//         },
//         Err(error) => {
//             panic!("could not open the file: {:?}", error);
//         }
//     };
// }

// --------------------------------------

// use std::fs::File;
// fn main() {
//     // let _f = File::open("test.txt").unwrap(); // error, panic
//     let _f = File::open("test.txt").expect("Could not open file"); // error, panic
// }

// --------------------------------------

// use std::fs::File;
// use std::io;
// use std::io::Read;
// fn read_file() -> Result<String, io::Error> {
//     let mut f = File::open("test.txt")?;
//     let mut s = String::new();
//     f.read_to_string(&mut s)?;
//     Ok(s)
// }
// fn main() {
//     let r = read_file();

//     match r {
//         Ok(s) => println!("{}", s),
//         Err(e) => println!("{}", e.to_string()),
//     }
// }

// --------------------------------------

use std::fs::File;
use std::io;
use std::io::Read;
fn read_file() -> Result<String, io::Error> {
    let mut s = String::new();
    File::open("test.txt")?.read_to_string(&mut s)?;
    Ok(s)
}
fn main() {
    let r = read_file();

    match r {
        Ok(s) => println!("{}", s),
        Err(e) => println!("{}", e.to_string()),
    }
}

/*
https://maeng-dev.tistory.com/25?category=833571
https://rinthel.github.io/rust-lang-book-ko/ch09-02-recoverable-errors-with-result.html
*/
