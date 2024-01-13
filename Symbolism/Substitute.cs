using System;
using System.Collections.Generic;
using System.Linq;

namespace Symbolism.Substitute;

public static class Extensions
{
    public static MathObject Substitute(this MathObject obj, MathObject a, MathObject b)
    {
        if (obj == a) return b;

        switch (obj)
        {
            case Equation e:
                if (e.Operator == Equation.Operators.Equal)
                    return (e.a.Substitute(a, b) == e.b.Substitute(a, b)).Simplify();

                if (e.Operator == Equation.Operators.NotEqual)
                    return (e.a.Substitute(a, b) != e.b.Substitute(a, b)).Simplify();

                if (e.Operator == Equation.Operators.LessThan)
                    return (e.a.Substitute(a, b) < e.b.Substitute(a, b)).Simplify();

                if (e.Operator == Equation.Operators.GreaterThan)
                    return (e.a.Substitute(a, b) > e.b.Substitute(a, b)).Simplify();

                throw new Exception();
            case Power p:
                return p.bas.Substitute(a, b) ^ (obj as Power).exp.Substitute(a, b);
            case Product t:
                return
                            t.Map(elt => elt.Substitute(a, b));
            case Sum s:
                return
                            s.Map(elt => elt.Substitute(a, b));
            case Function obj_:
                {
                    return new Function(
                        obj_.name,
                        obj_.proc,
                        obj_.args.ConvertAll(arg => arg.Substitute(a, b)))
                    .Simplify();
                }

            default:
                return obj;
        }
    }

    public static MathObject SubstituteEq(this MathObject obj, Equation eq) =>
        obj.Substitute(eq.a, eq.b);

    public static MathObject SubstituteEqLs(this MathObject obj, List<Equation> eqs) =>
        eqs.Aggregate(obj, (a, eq) => a.SubstituteEq(eq));

    public static MathObject Substitute(this MathObject obj, MathObject a, int b) =>
        obj.Substitute(a, new Integer(b));

    public static MathObject Substitute(this MathObject obj, MathObject a, double b) =>
        obj.Substitute(a, new DoubleFloat(b));

}

