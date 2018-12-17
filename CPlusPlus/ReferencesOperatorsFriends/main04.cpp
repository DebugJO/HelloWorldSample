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
    Dummy operator-(Dummy a)
    {
        cout << "Addition occured!" << endl;
        return Dummy(this->value - a.value);
    }    
    friend ostream &operator<<(ostream &o, Dummy a)
    {
        o << a.value;
        return o;   
    }
    friend istream &operator>>(istream &i, Dummy a)
    {
        i >> a.value;
        return i;   
    }    
    inline int getValue() { return value; }
};

int main(void)
{
    Dummy a(5);
    Dummy b = 10;
    Dummy c = a.operator-(b);
    cout << c << endl;
    return 0;
}
