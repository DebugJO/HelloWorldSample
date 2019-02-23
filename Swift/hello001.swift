import Swift

// 상수선언
// let 이름: 타입 = 값
// 변수선언
// var 이름: 타입 = 값
// 타입 생략 가능

let constant: String = "차후에 변경이 불가능한 상수"
var variable: String = "차후에 변경이 가능한 변수"

print(constant)
print(variable)

variable = "변수값 변경"
print(variable)
// constant = "상수값 변경" -> 에러발생
// print(constant)

let sum: Int
let inputA: Int = 100
let inputB: Int = 200

sum = inputA + inputB
print(sum)

// dump(variable) 
