import Swift

func sum(a: Int, b: Int) ->  Int {
    return a + b
}
print(sum(a:1, b:2))

func printName()  -> String{
    return "헬로우월드"
}
print(printName())

func bye() {print("Bye")}
bye()

// 매개변수 기본값
 func greeting(friend: String, me: String = "msjo") {
     print("Hello \(friend)! I'm \(me)")
 }
greeting(friend: "debugjo")
greeting(friend: "AAA", me: "BBB")

// 전자인자 레이블
func greeting1(to friend: String, from me: String) {
    print("Hello \(friend)! I'm \(me)")
}
greeting1(to: "CCC", from: "DDD")

// 함수 타입
var someFunction: (String, String) -> Void = greeting1(to:from:)
someFunction("가나다", "마바사")
