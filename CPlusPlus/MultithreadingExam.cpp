// https://www.youtube.com/watch?v=eeSC43KQdVI&list=PL_dsdStdDXbrzGQUMh2sy6T8GcCCst3Nm

#include <future>
#include <initializer_list>
#include <iostream>
#include <thread>
#include <vector>
#include <windows.h>

using namespace std;

void ThreadFn(int &v1, int v2)
{
    cout << "a thread function" << endl;
    cout << "Value V1 : " << v1++ << endl;
    cout << "Value V2 : " << v2 << endl;
}

void ThreadFn1()
{
    cout << "a thread function 1" << endl;
    cout << this_thread::get_id() << endl;
}

template <typename T> void ThreadFn2() { cout << "Type is : " << typeid(T).name() << endl; }

void ThreadFn3(initializer_list<int> il) { cout << "size of list : " << il.size() << endl; }

void ThreadFn4(vector<int> il)
{
    cout << "size of list : " << il.size() << endl;
    for (vector<int>::iterator itr = il.begin(); itr != il.end(); ++itr) {
        cout << *itr << endl;
    }
}

int AsyncFunc5(int value)
{
    cout << "Main Thread5 : " << this_thread::get_id() << endl;
    cout << "async..." << endl;
    return value + 100;
}

void ThreadFn6(promise<int> &value)
{
    this_thread::sleep_for(chrono::seconds(2));
    value.set_value(200);
}

/**************************************************************************/

int main()
{
    // 콘솔 한글
    SetConsoleOutputCP(CP_UTF8);

    // 기본 thread
    int localvalue = 100;
    thread t{ThreadFn, ref(localvalue), 2000};
    /*
    thread t1{[](int &v1) {
                  cout << "a thread function" << endl;
                  cout << "Value V1 : " << v1++ << endl;
              },
              ref(localvalue)};
    */
    t.join();
    cout << "Value in Main Thread : " << localvalue << endl;
    cout << "----------------" << endl;

    // [1] thread stl
    thread t1{ThreadFn1};
    cout << this_thread::get_id() << endl;
    t1.join();
    cout << "----------------" << endl;

    // [2] thread Template
    thread t2{ThreadFn2<int>};
    this_thread::sleep_for(chrono::milliseconds(1000));
    thread t21{ThreadFn2<float>};
    t2.join();
    t21.join();
    cout << "----------------" << endl;

    // [3] thread template and STL
    initializer_list<int> il = {1, 2, 3, 4, 5, 6};
    thread t3{ThreadFn3, il};
    t3.join();
    cout << "----------------" << endl;

    // [4] thread vector
    vector<int> il2 = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
    thread t4{ThreadFn4, il2};
    t4.join();
    cout << "----------------" << endl;

    // [5] Async
    cout << "Main Thread : " << this_thread::get_id() << endl;
    future<int> fn = async(launch::async, AsyncFunc5, 200);
    // future<void> fn = async(launch::deferred, AsyncFunc5); //동일한 thread id에서 실행
    if (fn.valid())
        cout << fn.get() << endl;
    if (fn.valid())
        fn.get();
    else
        cout << "Invalid" << endl;
    cout << "----------------" << endl;

    // [6] Promise
    promise<int> myPromise;
    future<int> fut = myPromise.get_future();
    // myPromise.set_value(100);
    cout << "Main..." << endl;
    thread t6{ThreadFn6, ref(myPromise)};
    cout << "Main Thread : " << fut.get() << endl;
    t6.join();
    cout << "----------------" << endl;

    return 0;
}
