#include <iostream>

using namespace std;

template<typename T, typename U>
class Person
{
    public:
        T height;
        U weight;
        static int numOfPeople;
        Person(T h, U w)
        {
            height = h,
            weight = w,
            numOfPeople++;
        }
        void GetData()
        {
            cout << "Height : " << height << " and Weight : " << weight << endl;
        }
};

template<typename T, typename U> int Person<T, U>::numOfPeople;


int main(void)
{
    Person<double, int> mikeTyson(5.83, 216);
    mikeTyson.GetData();
    cout << "Number of People : " << mikeTyson.numOfPeople << endl;

    return 0;
}