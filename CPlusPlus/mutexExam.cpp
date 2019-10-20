// https://www.youtube.com/watch?v=eeSC43KQdVI&list=PL_dsdStdDXbrzGQUMh2sy6T8GcCCst3Nm

#include <iostream>
#include <mutex>
#include <thread>

using namespace std;

void ThreadFn(mutex &mtx)
{
    lock_guard<mutex> fnLock(mtx);
    cout << "I locked the Mutex..." << endl;
    this_thread::sleep_for(chrono::seconds(2));
}

static int value;
static mutex value_mutex;

void increase_value()
{
    value_mutex.lock();
    value++;
    cout << value << endl;
    value_mutex.unlock();
}

int main()
{
    mutex mtx;
    thread th{ThreadFn, ref(mtx)};
    this_thread::sleep_for(chrono::seconds(1));
    unique_lock<mutex> fnLock(mtx);
    cout << "I am inside the Main Thread." << endl;
    fnLock.unlock();
    fnLock.lock();
    th.join();
    cout << "-----------------------------" << endl;

    value = 0;
    thread t[10];

    for (auto i = 0; i < 10; i++) {
        t[i] = thread(increase_value);
    }

    for (int i = 0; i < 10; i++) {
        t[i].join();
    }
    cout << "-----------------------------" << endl;

    return 0;
}
