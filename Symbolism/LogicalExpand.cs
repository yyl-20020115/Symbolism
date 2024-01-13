using System;
using System.Collections.Generic;
using System.Linq;

namespace Symbolism.LogicalExpand;

public static class Extensions
{
    public static MathObject LogicalExpand(this MathObject m)
    {
        switch (m)
        {
            case Or o:
                return o.Map(elt => elt.LogicalExpand());

            case And a when a.args.Any(elt => elt is Or) && a.args.Count > 1:
                {
                    Or or = null;
                    var before = new List<MathObject>();
                    var after = new List<MathObject>();

                    foreach (var elt in a.args)
                    {
                        if (elt is Or && or == null) or = elt as Or;
                        else if (or == null) before.Add(elt);
                        else after.Add(elt);
                    }

                    return
                        or.Map(or_elt =>
                            new And(
                                And.FromRange(before).Simplify().LogicalExpand(),
                                or_elt,
                                And.FromRange(after).Simplify().LogicalExpand()).Simplify()).LogicalExpand();
                }

            default:
                return m;
        }
    }
}
