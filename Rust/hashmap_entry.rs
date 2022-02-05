use std::collections::HashMap;

fn main() {
    let data: Vec<(&str, i32)> = vec![
        ("male", 9),
        ("female", 5),
        ("male", 0),
        ("female", 6),
        ("female", 5),
        ("male", 10),
    ];

    let mut survey_hash = HashMap::new();

    for (gender, number) in data {
        survey_hash.entry(gender).or_insert(Vec::new()).push(number);
    }

    for (male_or_female, numbers) in survey_hash {
        println!("{:?}, {:?}, {}", male_or_female, numbers, numbers.iter().sum::<i32>());
    }
}
