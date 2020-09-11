pub struct Object {
    width: u32,
    height: u32,
}

pub trait AreaObject {
    fn new(width: u32, height: u32) -> Self;
    fn area(&self) -> u32;
    fn show(&self);
}

impl AreaObject for Object {
    fn new(width: u32, height: u32) -> Self {
        Object { width, height }
    }

    fn area(&self) -> u32 {
        self.width * self.height
    }

    fn show(&self) {
        println!("area : {} x {} = {}", self.width, self.height, self.area());
    }
}