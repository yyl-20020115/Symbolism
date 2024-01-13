using System.Linq;

namespace Symbolism.SimplifyEquation;

public static class Extensions
{
    public static MathObject SimplifyEquation(this MathObject expr) =>
        // 10 * x == 0   ->   x == 0
        // 10 * x != 0   ->   x == 0

        expr switch
        {
            Equation e when e.a is Product p && p.elts.Any(elt => elt is Number) && e.b == 0 => new Equation(
                                    Product.FromRange((e.a as Product).elts.Where(elt => elt is not Number)).Simplify(),
                                    0,
                                    e.Operator).Simplify(),
            _ => expr is Equation e2 &&
                                e2.b == 0 &&
                                e2.a is Power p2 &&
                                p2.exp is Integer i &&
                                i.val > 0
                                ? p2.bas == 0
                                : expr is And a ? a.Map(elt => elt.SimplifyEquation())
                                : expr is Or o ? o.Map(elt => elt.SimplifyEquation()) 
                                : expr,// x ^ 2 == 0   ->   x == 0
                                                                                                                                                    // x ^ 2 != 0   ->   x == 0
        };
}
