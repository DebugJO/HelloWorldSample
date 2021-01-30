fn main() {
    let s1 = "hello";
    let len1 = calculate_length1(&s1);
    println!("The length of {} is {}.", s1, len1);

    let s2 = String::from("hello");
    let len2 = calculate_length2(&s2);
    println!("The length of {} is {}.", s2, len2);

    let mut s = String::from("hello");
    change_string(&mut s);
    println!("{}", s);
}

fn calculate_length1(s: &str) -> usize {
    s.len()
}

fn calculate_length2(s: &String) -> usize {
    s.len()
}

fn change_string(s: &mut String) {
    s.push_str(", world")
}
