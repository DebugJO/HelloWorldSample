extern crate reqwest;
extern crate serde;

use reqwest::Error;
use serde::Deserialize;
use std::collections::HashMap;

#[derive(Deserialize, Debug)]
struct IpInfo {
    ip: String,
    city: String,
    region: String,
    country: String,
    loc: String,
    org: String,
    postal: String,
    timezone: String,
    readme: String,
}

#[derive(Deserialize, Debug)]
struct HttpBin {
    slideshow: Show,
}

#[derive(Deserialize, Debug)]
struct Show {
    author: String,
    date: String,
    slides: Vec<Slide>,
    title: String,
}

#[derive(Deserialize, Debug)]
struct Slide {
    items: Option<Vec<String>>,
    title: String,
    r#type: String,
}

#[tokio::main]
async fn main() -> Result<(), Error> {
    let res_ipinfo1 = reqwest::get("http://ipinfo.io/json").await?.text().await?;

    println!("{}\n", res_ipinfo1);

    let res_ipinfo2 = reqwest::get("http://ipinfo.io/json")
        .await?
        .json::<HashMap<String, String>>()
        .await?;

    let mut items: Vec<_> = res_ipinfo2.iter().collect();
    items.sort();

    for (key, value) in items.iter() {
        println!("{} : {}", key, value);
    }

    println!("\n-----------------------------------------------\n");

    let res = reqwest::get("https://httpbin.org/json")
        .await?
        .text()
        .await?;
    println!("{}", res);

    let res_httpbin = reqwest::get("https://httpbin.org/json")
        .await?
        .json::<HttpBin>()
        .await?;

    // println!("slideshow : {:?}\n", res_httpbin.slideshow);
    println!("author : {}", res_httpbin.slideshow.author);
    println!("date : {}", res_httpbin.slideshow.date);

    for (k1, v1) in res_httpbin.slideshow.slides.iter().enumerate() {
        for v2 in v1.items.iter() {
            for (k3, v3) in v2.iter().enumerate() {
                println!("slides[{}] : items[{}] : {}", k1, k3, v3);
            }
        }

        println!("slides[{}] : title : {}", k1, v1.title);
        println!("slides[{}] : type : {}", k1, v1.r#type);
    }

    println!("title : {}", res_httpbin.slideshow.title);

    Ok(())
}
