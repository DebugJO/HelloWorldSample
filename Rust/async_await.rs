// [dependencies]
// tokio = { version = "1", features = ["full"] }

use tokio::net::TcpStream;
use tokio::macros::support::Future;
use tokio::io::AsyncWriteExt;

fn write_data<'a>(data:&'a [u8]) -> impl Future<Output = ()> + 'a {
    async move{
        let mut stream = TcpStream::connect("127.0.0.1:8080").await.unwrap();
        stream.write_all(data).await.unwrap()
    }
}

#[tokio::main]
async fn main() {
    write_data(b"HelloWorld").await;    
    println!("End...");
}      
