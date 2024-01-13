using System;

namespace Symbolism.DeepSelect;

public static class Extensions
{
    public static MathObject DeepSelect(this MathObject obj, Func<MathObject, MathObject> proc)
    {
        var r = proc(obj);
        return r switch
        {
            Power p => p.bas.DeepSelect(proc) ^ p.exp.DeepSelect(proc),
            Or o => o.Map(elt => elt.DeepSelect(proc)),
            And a => a.Map(elt => elt.DeepSelect(proc)),
            Equation e => new Equation(
                    e.a.DeepSelect(proc),
                    e.b.DeepSelect(proc),
                    e.Operator),
            Sum s => s.Map(elt => elt.DeepSelect(proc)),
            Product t => t.Map(elt => elt.DeepSelect(proc)),
            _ => r
        };
    }
}
