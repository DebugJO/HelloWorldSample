use serde_derive::Serialize;

#[derive(Serialize, Debug)]
#[serde(rename_all = "camelCase")]
pub struct Bar {
    pub bar: u8,
    pub such_wow: &'static str,
}
