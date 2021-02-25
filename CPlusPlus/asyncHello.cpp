#include <chrono>
#include <future>
#include <iostream>

using namespace std;

int foo(int a)
{
    this_thread::sleep_for(std::chrono::milliseconds(a * 1000));
    return a;
}

int main()
{
    cout << "Start foo()..." << endl;

    auto a1 = async(launch::async, foo, 2);
    cout << "a1 : " << a1.get() << endl;

    auto a2 = async(launch::async, foo, 5);
    cout << "a2 : " << a2.get() << endl;

    cout << "Stop foo()..." << endl;
    return 0;
}
