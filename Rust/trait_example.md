### mithradates, 060 Easy Rust in Korean: A simple trait
```rust
struct Animal {
    name: String,
}

trait Canine {
    fn bark(&self) {
        println!("Woof woof!");
    }

    fn run(&self) {
        println!("I am running!")
    }
}

impl Canine for Animal {
    fn bark(&self) {
        println!("멍멍! {}", self.name);
    }
}

fn main() {
    let my_animal = Animal {
        name: "Mr. Mantle".to_string(),
    };

    my_animal.bark();
    my_animal.run();
}
```

### mithradates, 061 Easy Rust in Korean: Implementing a trait
```rust
use std::fmt;

#[derive(Debug)]
struct Cat {
    name: String,
    age: u8,
}

impl fmt::Display for Cat {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        let name = &self.name.to_string();
        let age = self.age;
        write!(f, "My cat's name is {} and it is {} years old", name, age)
    }
}

fn main() {
    let mr_mantle = Cat {
        name: "Reggie Mantle".to_string(),
        age: 4,
    };

    println!("{}", mr_mantle);
}
```

### mithradates, 062 Easy Rust in Korean: Another small trait
```rust
struct Monster {
    health: i32,
}

struct Wizard {}
struct Ranger {}

trait FightClose {
    fn attack_with_sword(&self, opponent: &mut Monster) {
        opponent.health -= 10;
        println!("Sword! Opponent health {}", opponent.health);
    }

    fn attack_with_hand(&self, opponent: &mut Monster) {
        opponent.health -= 2;
        println!("Fist! Opponent health {}", opponent.health);
    }
}

impl FightClose for Wizard {}
impl FightClose for Ranger {}

trait FightfromDistance {
    fn attack_with_bow(&self, opponent: &mut Monster, distance: u32) {
        if distance < 10 {
            opponent.health -= 10;
            println!("Bow! Opponent health {}", opponent.health);
        }
    }

    fn attack_with_rock(&self, opponent: &mut Monster, distance: u32) {
        if distance < 3 {
            opponent.health -= 4;
            println!("Rock! Opponent health {}", opponent.health);
        }
    }
}

impl FightfromDistance for Ranger {}

fn main() {
    let radagast = Wizard {};
    let aragorn = Ranger {};
    let mut uruk_hai = Monster { health: 40 };

    radagast.attack_with_sword(&mut uruk_hai);
    aragorn.attack_with_bow(&mut uruk_hai, 7);
}
```

### mithradates, 063 Easy Rust in Korean: Traits as bounds
```rust
use std::fmt::Debug;

struct Monster {
    health: i32,
}

#[derive(Debug)]
struct Wizard {
    health: i32,
}

#[derive(Debug)]
struct Ranger {
    health: i32,
}

trait Magic {}
trait FightClose {}
trait FightFromDistance {}

impl Magic for Wizard {}
impl FightClose for Ranger {}
impl FightClose for Wizard {}
impl FightFromDistance for Ranger {}

fn attack_with_bow<T>(character: &T, opponent: &mut Monster, distance: u32)
where
    T: FightFromDistance + Debug,
{
    if distance < 10 {
        opponent.health -= 10;
        println!("Bow! Opponent health {}. You {:?}", opponent.health, character);
    }
}

fn attack_with_sword<T>(character: &T, opponent: &mut Monster)
where
    T: FightClose + Debug,
{
    opponent.health -= 10;
    println!("Sword! Opponent health {}. You {:?}", opponent.health, character);
}

fn fireball<T>(character: &T, opponent: &mut Monster, distance: u32)
where
    T: Magic + Debug,
{
    if distance < 15 {
        opponent.health -= 20;
        println!("Bow! Opponent health {}. You {:?}", opponent.health, character);
    }
}

fn main() {
    let radagast = Wizard { health: 60 };
    let aragon = Ranger { health: 80 };
    let mut uruk_hai = Monster { health: 40 };

    println!("{}", radagast.health);
    println!("{}", aragon.health);

    attack_with_sword(&radagast, &mut uruk_hai);
    attack_with_bow(&aragon, &mut uruk_hai, 7);
    fireball(&radagast, &mut uruk_hai, 12);
}
```

### mithradates, 064 Easy Rust in Korean: Implementing From
```rust
struct City {
    name: String,
    population: u32,
}

struct Country {
    cities: Vec<City>,
}

impl City {
    fn new(name: &str, population: u32) -> Self {
        Self { name: name.to_string(), population }
    }
}

impl From<Vec<City>> for Country {
    fn from(cities: Vec<City>) -> Self {
        Self { cities }
    }
}

impl Country {
    fn print_cities(&self) {
        for city in &self.cities {
            println!("{} has a population of {}", city.name, city.population);
        }
    }
}

fn main() {
    let helsinki = City::new("Helsinki", 631_695);
    let turku = City::new("Turku", 186_756);

    let finland_cities = vec![helsinki, turku];

    // let finland = Country::from(finland_cities);
    let finland: Country = finland_cities.into();
    finland.print_cities();
}
```
