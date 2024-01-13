using Symbolism.DegreeGpe;
using Symbolism.LeadingCoefficientGpe;
using Symbolism.AlgebraicExpand;

namespace Symbolism.PolynomialDivision;

public static class Extensions
{
    public static (MathObject quotient, MathObject remainder) PolynomialDivision(MathObject u, MathObject v, MathObject x)
    {
        var q = (MathObject)0;
        var r = u;
        var m = r.DegreeGpe([x]);
        var n = v.DegreeGpe([x]);
        var lcv = v.LeadingCoefficientGpe(x);

        while (m >= n && r != 0)
        {
            var lcr = r.LeadingCoefficientGpe(x);
            var s = lcr / lcv;

            q += s * (x ^ (m - n));

            r = ((r - (lcr * (x ^ m))) - (v - lcv * (x ^ n)) * s * (x ^ (m - n))).AlgebraicExpand();

            m = r.DegreeGpe([x]);
        }

        return (q, r);
    }
}
