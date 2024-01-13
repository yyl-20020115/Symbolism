namespace Symbolism.ExpandProduct;

public static class Extensions
{
    public static MathObject ExpandProduct(this MathObject r, MathObject s)
    {
        if (r is Sum ss)
        {
            var f = ss.elts[0];

            return f.ExpandProduct(s) + (r - f).ExpandProduct(s);
        }

        return s is Sum ? s.ExpandProduct(r) : r * s;
    }
}
