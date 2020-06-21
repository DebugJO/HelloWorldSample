#![feature(proc_macro_hygiene, decl_macro)]

// use rocket::http::hyper::header::{Authorization, Basic, Headers};
use rocket::{get, response::NamedFile, routes};
use rocket_contrib::{json::Json, serve::StaticFiles, templates::Template};
use std::path::Path;

mod lib;

use self::lib::{bar::Bar, foo::Foo};

#[get("/")]
fn index() -> &'static str {
    "Hello World"
}

#[get("/foo")]
fn foo() -> Template {
    let context = Foo {
        message: "This is the foo page",
    };
    Template::render("index", &context)
}

#[get("/bar")]
fn bar() -> Json<Bar> {
    Json(Bar {
        bar: 123,
        such_wow: "such wow",
    })
}

#[get("/static")]
fn static_file() -> NamedFile {
    NamedFile::open(Path::new("static/foobar.html")).unwrap()
}

fn main() {
    rocket::ignite()
        .mount("/", routes![index, foo, bar, static_file])
        .mount("/", StaticFiles::from("static"))
        .attach(Template::fairing())
        .launch();
}
