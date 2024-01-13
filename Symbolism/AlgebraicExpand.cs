using Symbolism.ExpandProduct;
using Symbolism.ExpandPower;

namespace Symbolism.AlgebraicExpand;

public static class Extensions
{
    public static MathObject AlgebraicExpand(this MathObject u)
    {
        switch (u)
        {
            case Equation eq:
                return eq.a.AlgebraicExpand() == eq.b.AlgebraicExpand();
            case Sum s:
                return s.Map(elt => elt.AlgebraicExpand());
            case Product p:
                {
                    var v = p.elts[0];

                    return v.AlgebraicExpand()
                        .ExpandProduct((u / v).AlgebraicExpand());
                }

            case Power p:
                {
                    var bas = p.bas;
                    var exp = p.exp;

                    return exp is Integer i && i.val >= 2 ? bas.AlgebraicExpand().ExpandPower(i.val) : u;
                }

            case Function f:
                return new Function(
                    f.name,
                    f.proc,
                    f.args.ConvertAll(elt => elt.AlgebraicExpand()))
                .Simplify();

            default:
                return u;
        }
    }
}
