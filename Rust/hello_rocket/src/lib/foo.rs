use serde_derive::Serialize;

#[derive(Serialize, Debug)]
pub struct Foo {
    pub message: &'static str,
}
