#include <iostream>
#include <vector>

using namespace std;

class Cat
{
  public:
    explicit Cat(const string &name) : mName(name)
    {
        cout << mName << " : Cat constructor" << endl;
    }

    virtual ~Cat() noexcept // [*]
    {
        cout << mName << " : ~Cat()" << endl;
    }

    Cat(const Cat &other) : mName(other.mName)
    {
        cout << mName << " : Copy constructor" << endl;
    }

    Cat(const Cat &&other) noexcept : mName{move(other.mName)} // [*]
    {
        cout << mName << " : Move constructor" << endl;
    }

  private:
    string mName;
};

int main()
{
    vector<Cat> cats;
    cats.reserve(2); // [*]
    cats.emplace_back("Kitty");
    cats.emplace_back("Nabi");

    cout << "-sizeof " << sizeof(cats) << endl;
    cout << "-size " << cats.size() << endl;
    cout << "-capacity " << cats.capacity() << endl;

    return 0;
}

/*
    1. noexcept, reserve 키워드를 사용하지 않을 때
    2. noexcept 키워드 사용하고  reserve 키워드 사용하지 않을 때
    3. reserve 키워드 사용
*/

// [Reference] 코드없는 프로그래밍, "벡터 메모리 ,noexcept", https://www.youtube.com/watch?v=HDOzrf_lebU
