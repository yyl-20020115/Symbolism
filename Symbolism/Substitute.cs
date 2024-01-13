using System;
using System.Collections.Generic;
using System.Linq;

namespace Symbolism.Substitute;

public static class Extensions
{
    public static MathObject Substitute(this MathObject m, MathObject a, MathObject b) => m == a
            ? b
            : m switch
            {
                Equation e => e.Operator switch
                {
                    Equation.Operators.Equal => (e.a.Substitute(a, b) == e.b.Substitute(a, b)).Simplify(),
                    Equation.Operators.NotEqual => (e.a.Substitute(a, b) != e.b.Substitute(a, b)).Simplify(),
                    Equation.Operators.LessThan => (e.a.Substitute(a, b) < e.b.Substitute(a, b)).Simplify(),
                    Equation.Operators.GreaterThan => (e.a.Substitute(a, b) > e.b.Substitute(a, b)).Simplify(),
                    _ => throw new Exception()
                },
                Power p => p.bas.Substitute(a, b) ^ (m as Power).exp.Substitute(a, b),
                Product t => t.Map(elt => elt.Substitute(a, b)),
                Sum s => s.Map(elt => elt.Substitute(a, b)),
                Function obj_ => new Function(
                    obj_.name,
                    obj_.proc,
                    obj_.args.ConvertAll(arg => arg.Substitute(a, b)))
                .Simplify(),
                _ => m,
            };

    public static MathObject SubstituteEq(this MathObject m, Equation eq) =>
        m.Substitute(eq.a, eq.b);

    public static MathObject SubstituteEqLs(this MathObject m, List<Equation> eqs) =>
        eqs.Aggregate(m, (a, eq) => a.SubstituteEq(eq));

    public static MathObject Substitute(this MathObject m, MathObject a, int b) =>
        m.Substitute(a, new Integer(b));

    public static MathObject Substitute(this MathObject m, MathObject a, double b) =>
        m.Substitute(a, new DoubleFloat(b));

}

