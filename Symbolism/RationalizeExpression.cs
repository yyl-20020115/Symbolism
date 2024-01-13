namespace Symbolism.RationalizeExpression
{
    public static class Extensions
    {
        public static MathObject RationalizeSum(MathObject u, MathObject v)
        {
            var m = u.Numerator();
            var r = u.Denominator();
            var n = v.Numerator();
            var s = v.Denominator();

            if (r == 1 && s == 1) return u + v;

            return RationalizeSum(m * s, n * r) / (r * s);
        }

        public static MathObject RationalizeExpression(this MathObject u)
        {
            switch (u)
            {
                case Equation e:
                    return new Equation(
                                e.a.RationalizeExpression(),
                                e.b.RationalizeExpression(),
                                e.Operator);
                case Power p:
                    return p.bas.RationalizeExpression() ^ p.exp;
                case Product t:
                    return
                                    t.Map(elt => elt.RationalizeExpression());
                case Sum s:
                    {
                        var f = s.elts[0];

                        var g = f.RationalizeExpression();
                        var r = (u - f).RationalizeExpression();

                        return RationalizeSum(g, r);
                    }

                default:
                    return u;
            }
        }
    }

}
