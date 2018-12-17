#include <iostream>
using namespace std;

class Dummy
{
  private:
    int value;

  public:
    Dummy()
    {
        value = 0;
    }
    Dummy(int val)
    {
        value = val;
    }
    Dummy operator+(Dummy a)
    {
        cout << "Addition occured!" << endl;
        return Dummy(this->value + a.value);
    }
    inline int getValue() { return value; }
};

int main(void)
{
    Dummy a(5);
    Dummy b = 10;
    Dummy c = a + b;
    cout << c.getValue() << endl;
    return 0;
}
