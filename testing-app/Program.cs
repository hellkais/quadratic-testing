using System.Runtime.InteropServices;

internal static class NativeMethods
{
    [DllImport("quadratic.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void setA(double a);

    [DllImport("quadratic.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void setB(double b);

    [DllImport("quadratic.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void setC(double c);

    [DllImport("quadratic.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int getSolution(out double x1, out double x2);
}

internal class Program
{
    private const int SOLUTION_OK = 0;
    private const int ERROR_A_IS_ZERO = 1;
    private const int ERROR_NO_REAL_ROOTS = 2;

    static void Main()
    {
        var i = 4;
        while (i > 0) 
        {
            RunTest("x^2 - 4 = 0", 1.0, 0.0, -4.0, SOLUTION_OK, -2.0, 2.0);
            i--;
        }
        
        RunTest("a = 0", 0.0, 2.0, 1.0, ERROR_A_IS_ZERO, null, null);
    }

    static void RunTest(string name, double a, double b, double c, int expectedCode, double? expectedX1, double? expectedX2)
    {
        NativeMethods.setA(a);
        NativeMethods.setB(b);
        NativeMethods.setC(c);

        int code = NativeMethods.getSolution(out double x1, out double x2);

        bool codeOk = code == expectedCode;
        bool rootsOk = true;

        if (expectedCode == SOLUTION_OK && expectedX1.HasValue && expectedX2.HasValue)
        {
            rootsOk = SameRoots(x1, x2, expectedX1.Value, expectedX2.Value);
        }

        if (codeOk && rootsOk)
        {
            Console.WriteLine($"[PASS] {name}");
            Console.WriteLine($"       return={code}, x1={x1}, x2={x2}");
        }
        else
        {
            Console.WriteLine($"[FAIL] {name}");
            Console.WriteLine($"       expected return={expectedCode}, x1={expectedX1}, x2={expectedX2}");
            Console.WriteLine($"       actual   return={code}, x1={x1}, x2={x2}");
        }
    }

    static bool SameRoots(double actual1, double actual2, double expected1, double expected2)
    {
        const double epsilon = 1e-9;

        bool direct =
            Math.Abs(actual1 - expected1) < epsilon &&
            Math.Abs(actual2 - expected2) < epsilon;

        bool swapped =
            Math.Abs(actual1 - expected2) < epsilon &&
            Math.Abs(actual2 - expected1) < epsilon;

        return direct || swapped;
    }
}