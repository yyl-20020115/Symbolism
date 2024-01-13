using System;
using System.Numerics;

using Symbolism.Has;

namespace Symbolism.CoefficientGpe;

public static class Extensions
{
    public static Tuple<MathObject, BigInteger> CoefficientMonomialGpe(this MathObject u, MathObject x)
    {
        if (u == x) return Tuple.Create((MathObject)1, (BigInteger)1);

        switch (u)
        {
            case Power p when p.bas == x && p.exp is Integer i && i.val > 1:
                return Tuple.Create((MathObject)1, i.val);
            case Product p2:
                {
                    var m = (BigInteger)0;
                    var c = u;

                    foreach (var elt in p2.elts)
                    {
                        var f = elt.CoefficientMonomialGpe(x);

                        if (f == null) return null;

                        if (f.Item2 != 0)
                        {
                            m = f.Item2;
                            c = u / (x ^ m);
                        }
                    }

                    return Tuple.Create(c, m);
                }
        }

        return u.FreeOf(x) ? Tuple.Create(u, (BigInteger)0) : null;
    }

    public static MathObject CoefficientGpe(this MathObject u, MathObject x, BigInteger j)
    {
        if (u is not Sum)
        {
            var f = u.CoefficientMonomialGpe(x);

            if (f == null) return null;

            if (f.Item2 == j) return f.Item1;

            return 0;
        }

        if (u == x) return j == 1 ? 1 : 0;

        var c = (MathObject)0;

        foreach (var elt in (u as Sum).elts)
        {
            var f = elt.CoefficientMonomialGpe(x);

            if (f == null) return null;

            if (f.Item2 == j) c += f.Item1;
        }

        return c;
    }
}
