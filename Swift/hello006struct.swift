import Swift

struct Sample {
    var mutableProperty: Int = 100
    let immuableProperty: Int = 100

    static var typeProperty: Int = 100

    // 인스턴스 메서드
    func instanceMethod() {
        print("instance method")
    }
    
    // 타입 메서드
    static func typeMethod() {
        print("type method \(typeProperty)")
    }
}

var mutable: Sample = Sample()

mutable.mutableProperty = 200
// mutable.immuableProperty = 200 // 에러

let immutable: Sample = Sample()
// immutable.mutableProperty = 200 // 에러

Sample.typeProperty = 300
Sample.typeMethod()

struct Student {
    var name: String = "unKnown"
    var `class`: String = "Swift"

    static func selfIntroduce() {
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
//other.name = "abc" //에러
other.selfIntroduce()
