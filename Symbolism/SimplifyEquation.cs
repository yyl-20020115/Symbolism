using System.Linq;

namespace Symbolism.SimplifyEquation;

public static class Extensions
{
    public static MathObject SimplifyEquation(this MathObject expr)
    {
        // 10 * x == 0   ->   x == 0
        // 10 * x != 0   ->   x == 0

        if (expr is Equation e&&
            e.a is Product p&&
            p.elts.Any(elt => elt is Number) &&
            (e.b == 0))
            return new Equation(
                Product.FromRange((e.a as Product).elts.Where(elt => !(elt is Number))).Simplify(),
                0,
                e.Operator).Simplify();

        // x ^ 2 == 0   ->   x == 0
        // x ^ 2 != 0   ->   x == 0

        if (expr is Equation e2&&
            e2.b == 0 &&
            e2.a is Power p2&&
            p2.exp is Integer i&&
            i.val > 0)
            return p2.bas == 0;

        if (expr is And a) return a.Map(elt => elt.SimplifyEquation());

        if (expr is Or o) return o.Map(elt => elt.SimplifyEquation());

        return expr;
    }
}
