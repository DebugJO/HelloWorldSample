use serde::Serialize;

#[derive(Serialize)]
pub struct Student {
    pub id: String,
    pub name: String,
    pub email: String,
}
