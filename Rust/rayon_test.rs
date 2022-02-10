use separator::Separatable;
use rayon::prelude::*;

fn main() {
    let some_number = 1..10000000000u128;

    let result_number:u128 = some_number.into_par_iter().sum();

    println!("{}", result_number.separated_string());
}

// [dependencies]
// separator = "0.4.1"
// rayon = "1.5.1"
