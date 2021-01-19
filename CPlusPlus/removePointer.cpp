#include <iostream>

using namespace std;

// void removePointer(int **pointer)
//{
//    delete *pointer;
//    *pointer = nullptr;
//}

// int main()
//{
//    int *pointer = new int;
//    *pointer = 5;
//    cout << *pointer << endl;
//    removePointer(&pointer);
//    if (pointer == nullptr) {
//        cout << "pointer removed" << endl;
//    } else {
//        cout << "pointer that has not been removed" << endl;
//    }

//    return 0;
//}

void removePointer(int* &pointer)
{
    delete pointer;
    pointer = nullptr;
}

int main()
{
    int *pointer = new int;
    *pointer = 5;
    cout << *pointer << endl;
    removePointer(pointer);
    if (pointer == nullptr) {
        cout << "pointer removed" << endl;
    } else {
        cout << "pointer that has not been removed" << endl;
    }

    return 0;
}
