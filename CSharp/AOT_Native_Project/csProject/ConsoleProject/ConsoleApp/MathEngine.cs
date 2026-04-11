namespace ConsoleApp;

public unsafe class MathEngine
{
    private readonly delegate*<double, double, double>[] mOperationTable;

    public MathEngine()
    {
        mOperationTable = new delegate*<double, double, double>[4];
        mOperationTable[(int)OpType.Add] = &Add;
        mOperationTable[(int)OpType.Subtract] = &Subtract;
        mOperationTable[(int)OpType.Multiply] = &Multiply;
        mOperationTable[(int)OpType.Divide] = &Divide;
    }

    private static double Add(double a, double b) => a + b;

    private static double Subtract(double a, double b) => a - b;

    private static double Multiply(double a, double b) => a * b;

    private static double Divide(double a, double b) => b != 0 ? a / b : double.NaN;

    public double Execute(OpType op, double a, double b)
    {
        int opIndex = (int)op;

        if ((uint)opIndex >= (uint)mOperationTable.Length)
        {
            return 0;
        }

        delegate*<double, double, double> func = mOperationTable[opIndex];
        return func != null ? func(a, b) : 0;

        // if (func != null)
        // {
        //     return mOperationTable[opIndex](a, b);
        // }
        //
        // return 0;
    }
}

public enum OpType
{
    Add = 0,
    Subtract = 1,
    Multiply = 2,
    Divide = 3
}