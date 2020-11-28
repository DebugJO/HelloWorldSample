#include <crtdbg.h>

// 컴파일 : cl /std:c++17 /Zi /MDd /EHsc /O2 /utf-8 %1 /link /out:%2
#ifdef _DEBUG
#define new new (_CLIENT_BLOCK, __FILE__, __LINE__)
#endif

void StartDebug()
{
    _CrtSetDbgFlag(_CRTDBG_LEAK_CHECK_DF | _CRTDBG_ALLOC_MEM_DF);
}

int main()
{
    StartDebug();

    char *c1;
    char *c2;

    c1 = new char[8];

    c2 = new char[16]; // Detected memory leaks!

    delete[] c1;
    // delete[] c2; // Detected memory leaks!
    return 0;
}
