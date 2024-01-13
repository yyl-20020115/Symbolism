﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Symbolism.SimplifyLogical;

public static class Extensions
{
    public static bool HasDuplicates(this IEnumerable<MathObject> ls)
    {
        foreach (var elt in ls) if (ls.Count(item => item.Equals(elt)) > 1) return true;
        
        return false;
    }

    public static IEnumerable<MathObject> RemoveDuplicates(this IEnumerable<MathObject> seq)
    {
        var ls = new List<MathObject>();

        foreach (var elt in seq)
            if (ls.Any(item => item.Equals(elt)) == false)
                ls.Add(elt);

        return ls;
    }

    public static MathObject SimplifyLogical(this MathObject expr)
    {

        if (expr is And a&& a.args.HasDuplicates())
            return And.FromRange(a.args.RemoveDuplicates());
                    
        if (expr is Or o && o.args.HasDuplicates())
            return
                Or.FromRange(o.args.RemoveDuplicates())
                .SimplifyLogical();

        if (expr is Or o2) return o2.Map(elt => elt.SimplifyLogical());

        return expr;
    }
}
