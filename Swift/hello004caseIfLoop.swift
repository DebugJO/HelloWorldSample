import Swift

let someInteger = 100

if someInteger < 100 {
    print("100미만")
} else if someInteger > 100 {
    print("100 초과")
} else {
    print("100")
}

switch someInteger {
    case 0:
        print("Zero")
    case 1..<100:
        print("1~99")
    case 100:
        print("100")
    case 101...Int.max:
        print("over 100")
    default:
        print("unknown")
}

switch "AAA" {
    case "BBB":
        print("BBB")
    case "AAA":
        print("AAA:OK")
    default:
        print("unknown")
}


var integers = [1, 2, 3]
let people = ["AAA": 10, "BBB": 15, "CCC": 12]

for integer in integers {
    print(integer)
}

for(name, age) in people {
    print("\(name): \(age)")
}

while integers.count > 1 {
    integers.removeLast()
}
for integer in integers {
    print(integer)
}

repeat {
    integers.removeLast()
} while integers.count > 0

var chk: Bool = false
if integers.count == 0 {
        chk = true
}

if !chk {
    for integer in integers { 
        print(integer)
    }
} else {
    print("Null")
}
