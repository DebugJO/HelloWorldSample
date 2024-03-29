use rayon::prelude::*;
use separator::Separatable;

fn main() {
    let some_number = 1..=10_000_000_000u128;

    let result_number: u128 = some_number.into_par_iter().sum();

    // let result_number = 10_000_000_000u128 * (10_000_000_000u128 + 1) / 2;

    println!("{}", result_number.separated_string());
}

// [dependencies]
// separator = "0.4.1"
// rayon = "1.5.1"
// num = "0.4.0"

// use num::BigUint;
// use rayon::prelude::*;
// use std::ops::Mul;
// 
// fn factorial() -> BigUint {    
//    (1..=100000u32).into_par_iter().map(BigUint::from).reduce_with(Mul::mul).unwrap()
// }
// 
// fn main() {
//     println!("{}", factorial());
// }
