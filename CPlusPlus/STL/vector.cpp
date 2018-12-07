#include <iostream>
#include <vector>
#include <string>
#include <map>
#include <iterator>

using namespace std;

int main(void)
{
    // VECTOR
    // vector<string> names;
    // names.push_back("peter");
    // names.push_back("Easy");
    
    // for (int i = 0; i < names.size(); i++)
    // {
    //     cout << names[i] << endl;
    // }

    // MAP ITERATOR
    // map<string, int> names;
    // map<string, int>::iterator iterator;
    // names["peter"] = 22;
    // names["Easy"] = 33;

    // for (iterator = names.begin(); iterator != names.end(); iterator++)
    // {
    //     cout << iterator->first << " = " << iterator->second << endl;
    // }

    // VECTOR ITERATOR
    vector<string> names;
    vector<string>::iterator iterator;
    names.push_back("peter");
    names.push_back("Easy");

    for (iterator = names.begin(); iterator != names.end(); *iterator++)
    {
        cout << *iterator << endl;
    }    

    return 0;
}