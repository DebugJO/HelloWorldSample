import Swift

//기본타입
// Bool, Int, UInt, Float, Double, Character, String 등

//Any 타입
var someAny: Any = 100
someAny = "한글입니다."
print(someAny)
someAny = 123.12
print(someAny)

class SomeClass {}
var someAnyObject: AnyObject = SomeClass()
print(someAnyObject)

//비어있는 값은 들어올 수 없다.(Error)
//someAny = nil;
//print(someAny) 

/* 컬렉션 타입 
Array - 순서가 있는 리스트 컬렉션
Ditionary - 키와 값의 쌍으로 이루어진 컬렉션
Set - 순서가 없고, 멤버가 유일한 컬렉션
*/

var integers: Array<Int> = Array<Int>()
integers.append(1)
integers.append(100)
integers.append(101)
print(integers.contains(100))
//integers.remove(at: 0)
integers.removeLast() 
//integers.removeAll()
print(integers.count)

//var strings: [String] = [String]()

let immutableArray = [1, 2, 3]
print(immutableArray)

//Key가 String 타입이고 Value가 Any인 빈 Dictionary 생성
var anyDictionary: Dictionary<String, Any> = [String: Any]()
anyDictionary["A"] = "value"
anyDictionary["B"] = 100
print(anyDictionary)
anyDictionary.removeValue(forKey: "A")
print(anyDictionary)
anyDictionary["B"] = nil
print(anyDictionary)

// Dictionary 출력 및 순서
let numberOfLegs = ["spider": 8, "ant": 6, "cat": 4]

for (animalName, legCount) in numberOfLegs {
    print("\(animalName)s have \(legCount) legs")
}

let order1 = numberOfLegs.keys.sorted(by: <)
print(order1)

let order2 = numberOfLegs.values.sorted(by: <)
print(order2)

let order3 = numberOfLegs.sorted(by: <)
print(order3)

let emptyDictionary: [String: String] = [:]

var initDictionary: Dictionary = [1:"가나다", 2:"마바사"]
var someValue1 = initDictionary.values.joined()
print(someValue1)

let joinedStr = initDictionary.values.joined(separator: " , ")
print(joinedStr) 

var iSet: Set<Int> = Set<Int>()
iSet.insert(1)
iSet.insert(2)
iSet.insert(3)
print(iSet)

let setA: Set<Int> = [1, 2, 3, 4, 5]
let setB: Set<Int> = [3, 4, 5, 6, 7]
let union: Set<Int> = setA.union(setB)
let sortedUnion: [Int] = union.sorted()
print(sortedUnion)
let intersection: Set<Int> = setA.intersection(setB)
print(intersection.sorted())
let subtracting: Set<Int> = setA.subtracting(setB)
print(subtracting.sorted())
