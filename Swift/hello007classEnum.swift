import Swift

class Sample {
    var mutableProperty: Int = 100
    let immuableProperty: Int = 100

    static var typeProperty: Int = 100

    // 인스턴스 메서드
    func instanceMethod() {
        print("instance method \(mutableProperty)")
    }
    
    // 타입 메서드 - 재정의 불가
    static func typeMethod() {
        print("type method \(typeProperty)")
    }

    // 재정의 가능 타입 메서드
    class func classMethod() {
        print("type method - class")
    }
}

var mutableReference: Sample = Sample()

mutableReference.mutableProperty = 200
// mutableReference.immuableProperty = 200 // 에러
mutableReference.instanceMethod()


// let immutable: Sample = Sample()
// immutable.mutableProperty = 200 // 에러

Sample.typeProperty = 300
Sample.typeMethod()

Sample.classMethod()

class Student {
    var name: String = "unKnown"
    var `class`: String = "Swift"

    class func selfIntroduce() {
        print("학생타입입니다")
    }

    func selfIntroduce() {
        print("저는 \(self.class)반 \(name)입니다")
    }
}

Student.selfIntroduce()

var msjo: Student = Student()
msjo.name = "MSJO"
msjo.class = "스위프트"
msjo.selfIntroduce()

let other: Student = Student()
other.name = "abc" 
other.selfIntroduce()


/*****  열거형 *****/

enum Weekday {
    case mon
    case tue
    case wed
    case thu, fri, sat, sun
}

var day: Weekday = Weekday.mon
print(day)
day = .tue
print(day)

switch day {
    case .mon, .tue, .wed, .thu:
        print("평일입니다")
    case Weekday.fri:
        print("불금입니다")
    case .sat, .sun:
        print("신나는 주말")
}

enum Fruit: Int {
    case apple = 0
    case grape = 1
    case peach // 자동으로 2
}
print("Fruit.peach.rawValue == \(Fruit.peach.rawValue)")

enum School: String {
    case elementary = "초등"
    case middle = "중학"
    case high = "중등"
    case university
}
print("School.middle.rawValue == \(School.middle.rawValue)")
print("School.university.rawValue == \(School.university.rawValue)")

// let apple: Fruit = Fruit(rawValue: 0) //Optional 타입이기 때문에 오류
let apple: Fruit? = Fruit(rawValue: 0)

if let orange: Fruit = Fruit(rawValue: 5) {
    print("rawValue 5에 해당하는 케이스는 \(orange)입니다")
} else {
    print("rawValue 5에 해당하는 케이스가 없습니다")
}

enum Month {
    case dec, jan, feb
    case mar, apr, may 
    case jun, jul, aug
    case sep, oct, nov

    func printMessage() {
        switch self {
            case .mar, .apr, .may:
                print("봄")
            case .jun, .jul, .aug:
                print("여름")
            case .sep, .oct, .nov:
                print("가을")
            case .dec, .jan, .feb:
                print("겨울")
        }
    }
}

Month.mar.printMessage()
