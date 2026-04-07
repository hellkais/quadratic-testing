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
        //INSERT YOUR TEST CASES HERE
        //USE RunTest() TO TEST YOUR INPUT

        //TC01
        RunTest("TC01", 1.0, 0.0, -4.0, 0);
        //TC02
        RunTest("TC02", 1.0, -3.0, 2.0, 0);
    }

    static void RunTest(string name, double a, double b, double c, int expectedCode, double? expectedX1 = null, double? expectedX2 = null)
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
            Console.WriteLine($"       code={code}, x1={x1}, x2={x2}");
        }
        else
        {
            Console.WriteLine($"[FAIL] {name}");
            Console.WriteLine($"       expected code={expectedCode}, x1={expectedX1}, x2={expectedX2}");
            Console.WriteLine($"       actual   code={code}, x1={x1}, x2={x2}");
        }
    }

    static bool SameRoots(double actual1, double actual2, double expected1, double expected2)
    {
        var actual = new[] { actual1, actual2 };
        var expected = new[] { expected1, expected2 };

        Array.Sort(actual);
        Array.Sort(expected);

        return NearlyEqual(actual[0], expected[0]) 
            && NearlyEqual(actual[1], expected[1]);
    }

    static bool NearlyEqual(double a, double b, double epsilon = 1e-9)
    {
        return Math.Abs(a - b) <= epsilon * Math.Max(1.0, Math.Max(Math.Abs(a), Math.Abs(b)));
    }
}