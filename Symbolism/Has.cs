using System;
using System.Linq;

namespace Symbolism.Has;

public static class Extensions
{
    public static bool Has(this MathObject obj, MathObject a) => obj == a
            ? true
            : obj switch
            {
                Equation e => e.a.Has(a) || e.b.Has(a),
                Power power => power.bas.Has(a) || power.exp.Has(a),
                Product product => product.elts.Any(elt => elt.Has(a)),
                Sum sum => sum.elts.Any(elt => elt.Has(a)),
                Function function => function.args.Any(elt => elt.Has(a)),
                _ => false
            };

    public static bool Has(this MathObject obj, Func<MathObject, bool> proc) => proc(obj) 
        | obj switch
            {
                Equation e => e.a.Has(proc) || e.b.Has(proc),
                Power p => p.bas.Has(proc) || p.exp.Has(proc),
                Product t => t.elts.Any(elt => elt.Has(proc)),
                Sum s => s.elts.Any(elt => elt.Has(proc)),
                Function f => f.args.Any(elt => elt.Has(proc)),
                _ => false
            };

    public static bool FreeOf(this MathObject obj, MathObject a) => !obj.Has(a);
}
