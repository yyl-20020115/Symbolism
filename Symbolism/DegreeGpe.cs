using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using Symbolism.Has;

namespace Symbolism.DegreeGpe
{
    public static class Extensions
    {
        public static BigInteger DegreeMonomialGpe(this MathObject u, List<MathObject> v) => v.All(u.FreeOf)
                ? BigInteger.Zero
                : v.Contains(u)
                ? BigInteger.One
                : u switch
                {
                    Power power when power.exp is Integer integer && integer.val > 1 => integer.val,
                    Product product => product.elts.Select(elt => elt.DegreeMonomialGpe(v)).Aggregate(BigInteger.Add),
                    _ => 0
                };

        public static BigInteger DegreeGpe(this MathObject u, List<MathObject> v) 
            => u is Sum sum ? sum.elts.Select(elt => elt.DegreeMonomialGpe(v)).Max() : u.DegreeMonomialGpe(v);
    }

}
