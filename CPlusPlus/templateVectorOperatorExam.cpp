#include <iostream>
#include <vector>

using namespace std;

template <typename T> ostream &operator<<(ostream &o, const vector<T> &v);
template <typename T> vector<T> operator+(const vector<T> &x, const vector<T> &y);

template <typename T> vector<T> &operator<<(vector<T> &v, const T &val)
{
    v.push_back(val);
    return v;
}

int main()
{
    vector<int> v, w, z;
    v << 1 << 2 << 3 << 4 << 5;
    w << 6 << 7 << 8 << 9 << 10;
    z = v + w;
    cout << v << endl;
    cout << w << endl;
    cout << z << endl;
    return 0;
}

template <typename T> ostream &operator<<(ostream &o, const vector<T> &v)
{
    typename vector<T>::const_iterator itr;
    for (itr = v.begin(); itr != v.end(); ++itr) {
        o << *itr << ' ';
    }
    return o;
}

template <typename T> vector<T> operator+(const vector<T> &x, const vector<T> &y)
{
    vector<T> r;
    typename vector<T>::const_iterator itr;
    for (itr = x.begin(); itr != x.end(); ++itr) {
        r.push_back(*itr);
    }
    for (itr = y.begin(); itr != y.end(); ++itr) {
        r.push_back(*itr);
    }
    return r;
}
