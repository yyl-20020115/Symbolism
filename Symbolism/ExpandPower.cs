using System.Numerics;
using Symbolism.ExpandProduct;

namespace Symbolism.ExpandPower;

public static class Extensions
{
    public static BigInteger Factorial(BigInteger n)
    {
        var result = (BigInteger)1;

        for (var i = 1; i <= n; i++)
        {
            result *= i;
        }

        return result;
        // return Enumerable.Range(1, n).Aggregate((acc, elt) => acc * elt);
    }

    public static MathObject ExpandPower(this MathObject u, BigInteger n)
    {
        if (u is Sum)
        {
            var f = (u as Sum).elts[0];

            var r = u - f;

            MathObject s = 0;

            var k = 0;

            while (true)
            {
                if (k > n) return s;
                else
                {
                    var c =
                        Factorial(n)
                        /
                        (Factorial(k) * Factorial(n - k));

                    s += (c * (f ^ (n - k))).ExpandProduct(r.ExpandPower(k));

                    k++;
                }
            }
        }
        else return u ^ n;
    }
}
