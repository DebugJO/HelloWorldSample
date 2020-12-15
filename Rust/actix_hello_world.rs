use actix_web::http::StatusCode;
use actix_web::{web, App, Error, HttpResponse, HttpServer};

async fn home() -> Result<HttpResponse, Error> {
    Ok(HttpResponse::build(StatusCode::OK).content_type("text/html; charset=utf-8").body(include_str!("../templates/index.html")))
}

#[actix_rt::main]
async fn main() -> std::io::Result<()> {
    println!("Staring Server ... To exit, Press ^C");
    HttpServer::new(move || App::new().route("/", web::get().to(home))).bind("127.0.0.1:8888")?.run().await
}

/*
[dependencies]
actix-web = "3.3.2"
actix-rt = "1.1.1"
*/
