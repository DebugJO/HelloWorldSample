mod config;
mod models;

use crate::models::Student;

use actix_web::{dev::ServiceRequest, web, App, Error, HttpServer, Responder};

use actix_web_httpauth::extractors::basic::{BasicAuth, Config};
use actix_web_httpauth::extractors::AuthenticationError;
use actix_web_httpauth::middleware::HttpAuthentication;

use dotenv::dotenv;
use std::io;

fn validate_credentials(user_id: &str, user_password: &str) -> Result<bool, std::io::Error> {
    // Basic Auth (Username, Password)
    if user_id.eq("abc") && user_password.eq("123") {
        return Ok(true);
    }
    return Err(std::io::Error::new(std::io::ErrorKind::Other, "Authentication failed!"));
}

async fn basic_auth_validator(req: ServiceRequest, credentials: BasicAuth) -> Result<ServiceRequest, Error> {
    let config = req.app_data::<Config>().map(|data| data.get_ref().clone()).unwrap_or_else(Default::default);
    match validate_credentials(credentials.user_id(), credentials.password().unwrap().trim()) {
        Ok(res) => {
            if res == true {
                Ok(req)
            } else {
                Err(AuthenticationError::from(config).into())
            }
        }
        Err(_) => Err(AuthenticationError::from(config).into()),
    }
}

async fn student() -> impl Responder {
    web::HttpResponse::Ok().json(Student {
        id: "1000".to_string(),
        name: "홍길동".to_string(),
        email: "1000@gmail.com".to_string(),
    })
}

#[actix_rt::main]
async fn main() -> io::Result<()> {
    dotenv().ok();

    let config = crate::config::Config::from_env().unwrap();

    println!("Staring server at http://{}:{}/", config.server.host, config.server.port);

    HttpServer::new(|| {
        let auth = HttpAuthentication::basic(basic_auth_validator);
        App::new().wrap(auth).route("/", web::get().to(student))
    })
    .bind(format!("{}:{}", config.server.host, config.server.port))?
    .run()
    .await
}
