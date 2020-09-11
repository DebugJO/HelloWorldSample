mod object_info;

use object_info::*;

fn main() {
    let obj = object_info::Object::new(20, 30);
    obj.show();
}
