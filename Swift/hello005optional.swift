import Swift

func printName(_ name: String) {
    print(name)
}
// 언더바로 명시적 파라미터 사용 생략
printName("가나다")

var myName: String? = nil
//printName(myName) : 오류

if let name: String = myName {
    printName(name)
} else {
    print("myName == nil")
}

var yourName: String! = nil

if let name: String = yourName {
    printName(name)
} else {
    print("yourName == nil")
}

var optionalString: String? = "Hello"
print(optionalString!)
