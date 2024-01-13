﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Symbolism.LogicalExpand;

public static class Extensions
{
    public static MathObject LogicalExpand(this MathObject obj)
    {
        if (obj is Or)
        {
            return (obj as Or).Map(elt => elt.LogicalExpand());
        }

        if (obj is And a &&
            a.args.Any(elt => elt is Or) &&
            a.args.Count > 1)
        {
            var before = new List<MathObject>();
            Or or = null;
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

        return obj;
    }
}
