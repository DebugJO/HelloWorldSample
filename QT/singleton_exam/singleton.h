#ifndef SINGLETON_H
#define SINGLETON_H

#include <assert.h>

template <class T> class Singleton
{
  public:
    static T *Instance()
    {
        if (!mInstance)
            mInstance = new T;
        assert(mInstance != nullptr);
        return mInstance;
    }

  protected:
    Singleton();
    ~Singleton();

  private:
    Singleton(Singleton const &);
    Singleton &operator=(Singleton const &);
    static T *mInstance;
};

template <class T> T *Singleton<T>::mInstance = nullptr;

#endif // SINGLETON_H
