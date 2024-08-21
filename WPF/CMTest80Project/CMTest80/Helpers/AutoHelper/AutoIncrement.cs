using System;

namespace CMTest80.Helpers.AutoHelper;

public class AutoIncrement(int number = 0) : IDisposable
{
    private bool mIsDispose;
    private int mNumber = number;

    ~AutoIncrement() => Dispose(false);

    protected virtual void Dispose(bool disposing)
    {
        if (mIsDispose)
        {
            return;
        }

        if (disposing)
        {
            mNumber = 0;
        }

        mIsDispose = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public int GetSEQ()
    {
        return mNumber += 1;
    }
}