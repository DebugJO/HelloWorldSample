#include <iostream>
using namespace std;

int main(void)
{
    // 01
    auto func = []() {
        cout << "Hello World 01" << endl;
    };

    func();

    // 02
    []() {
        cout << "Hello World 02" << endl;
    }();

    // 03
    [](int i, int j) {
        cout << "Hello World 03 : i + j = " << i + j << endl;
    }(1, 2);

    // 04
    int sum = [](int i, int j) -> int {
        return i + j;
    }(1, 2);

    cout << "Hello World 04 : i + j = " << sum << endl;

    // 05
    cout << "Hello World 05 : i + j = " << [](int i, int j) -> int {
        return i + j;
    }(1, 2) << endl;

    // 06 
    int i = 1;
    int j = 2;
    [i, j]() {
        cout << "Hello World 06 : i + j = " << i + j << endl;
    }();

    // 07 copy
    int k = 10;
    auto func1 = [k]() {
        cout << "Innter value : " << k << endl;
    };
    for (int i = 0; i < 5; i++)
    {
        cout << "Outer value [" << i << "] : " <<  k << endl;
        func1();
        k = k + 1;
    }

    // 08 reference
    int c = 10;
    auto func2 = [&c]() {
        cout << "Innter value : " << c << endl;
    };
    for (int i = 0; i < 5; i++)
    {
        cout << "Outer value [" << i << "] : " <<  c << endl;
        func2();
        c = c + 1;
    }

    return 0;
}
