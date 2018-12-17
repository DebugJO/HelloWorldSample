#include <iostream>
using namespace std;

void increase(int &p)
{
    p++;
}

int main(void)
{
    int a = 10;
    cout << a << endl;
    increase(a);
    cout << a << endl;
    return 0;
}
