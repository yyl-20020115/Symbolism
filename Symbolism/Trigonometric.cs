using System;
using System.Linq;

using static Symbolism.ListConstructor;
using static Symbolism.Trigonometric.Constructors;

namespace Symbolism.Trigonometric;

public class Sin(MathObject param) : Function("sin", SinProc, new[] { param })
{
    public static MathObject Mod(MathObject x, MathObject y)
    {
        if (x is Number && y is Number)
        {
            var result = Convert.ToInt32(Math.Floor(((x / y) as Number).ToDouble().val));

            return x - y * result;
        }

        throw new Exception();
    }

    static MathObject SinProc(params MathObject[] ls)
    {
        var Pi = new Symbol("Pi");

        var half = new Integer(1) / 2;

        var u = ls[0];
        
        if (u == 0) return 0;

        if (u == Pi) return 0;
        
        if (u is DoubleFloat @float)
            return new DoubleFloat(Math.Sin(@float.val));
        
        if (u is Number && u < 0) return -Sin(-u);
        
        if (u is Product &&
            (u as Product).elts[0] is Number &&
            (u as Product).elts[0] < 0)
            return -Sin(-u);

        if (u is Product &&
            ((u as Product).elts[0] is Integer || (u as Product).elts[0] is Fraction) &&
            (u as Product).elts[0] > half &&
            (u as Product).elts[1] == Pi)
        {
            var n = (u as Product).elts[0];

            if (n > 2) return Sin(Mod(n, 2) * Pi);

            if (n > 1) return -Sin(n * Pi - Pi);

            if (n > half) return Sin((1 - n) * Pi);

            return new Sin(n * Pi);
        }

        // sin(k/n pi)
        // n is one of 1 2 3 4 6

        if (u is Product &&
            List<MathObject>(1, 2, 3, 4, 6).Any(elt =>
                elt == (u as Product).elts[0].Denominator()) &&
            (u as Product).elts[0].Numerator() is Integer &&
            (u as Product).elts[1] == Pi)
        {
            var k = (u as Product).elts[0].Numerator();
            var n = (u as Product).elts[0].Denominator();

            if (n == 1) return 0;

            if (n == 2)
            {
                if (Mod(k, 4) == 1) return 1;

                if (Mod(k, 4) == 3) return -1;
            }

            if (n == 3)
            {
                if (Mod(k, 6) == 1) return (3 ^ half) / 2;
                if (Mod(k, 6) == 2) return (3 ^ half) / 2;

                if (Mod(k, 6) == 4) return -(3 ^ half) / 2;
                if (Mod(k, 6) == 5) return -(3 ^ half) / 2;
            }

            if (n == 4)
            {
                if (Mod(k, 8) == 1) return 1 / (2 ^ half);
                if (Mod(k, 8) == 3) return 1 / (2 ^ half);

                if (Mod(k, 8) == 5) return -1 / (2 ^ half);
                if (Mod(k, 8) == 7) return -1 / (2 ^ half);
            }

            if (n == 6)
            {
                if (Mod(k, 12) == 1) return half;
                if (Mod(k, 12) == 5) return half;

                if (Mod(k, 12) == 7) return -half;
                if (Mod(k, 12) == 11) return -half;
            }
        }
        
        // sin(Pi + x + y + ...)   ->   -sin(x + y + ...)
        
        if (u is Sum && (u as Sum).elts.Any(elt => elt == Pi))
            return -Sin(u - Pi);

        // sin(x + n pi)

        bool Product_n_Pi(MathObject elt) =>
                (elt is Product) &&
                (
                    (elt as Product).elts[0] is Integer ||
                    (elt as Product).elts[0] is Fraction
                ) &&
                Math.Abs(((elt as Product).elts[0] as Number).ToDouble().val) >= 2.0 &&

                (elt as Product).elts[1] == Pi;

        if (u is Sum && (u as Sum).elts.Any(Product_n_Pi))
        {
            var pi_elt = (u as Sum).elts.First(Product_n_Pi);

            var n = (pi_elt as Product).elts[0];

            return Sin((u - pi_elt) + Mod(n, 2) * Pi);
        }

        // sin(a + b + ... + n/2 * Pi)

        Func<MathObject, bool> Product_n_div_2_Pi = elt =>
            elt is Product &&
            (
                (elt as Product).elts[0] is Integer ||
                (elt as Product).elts[0] is Fraction
            ) &&
            (elt as Product).elts[0].Denominator() == 2 &&
            (elt as Product).elts[1] == Pi;

        if (u is Sum && (u as Sum).elts.Any(Product_n_div_2_Pi))
        {
            var n_div_2_Pi = (u as Sum).elts.First(Product_n_div_2_Pi);

            var other_elts = u - n_div_2_Pi;

            var n = (n_div_2_Pi as Product).elts[0].Numerator();

            if (Mod(n, 4) == 1) return new Cos(other_elts);
            if (Mod(n, 4) == 3) return -new Cos(other_elts);
        }

        return new Sin(u);
    }
}
public class Cos(MathObject param) : Function("cos", CosProc, new[] { param })
{
    public static MathObject Mod(MathObject x, MathObject y)
    {
        if (x is Number && y is Number)
        {
            var result = Convert.ToInt32(Math.Floor(((x / y) as Number).ToDouble().val));

            return x - y * result;
        }

        throw new Exception();
    }

    static MathObject CosProc(params MathObject[] ls)
    {
        var Pi = new Symbol("Pi");

        var half = new Integer(1) / 2;

        var u = ls[0];

        if (ls[0] == 0) return 1;

        if (ls[0] == new Symbol("Pi")) return -1;

        switch (ls[0])
        {
            case DoubleFloat:
                return new DoubleFloat(Math.Cos(((DoubleFloat)ls[0]).val));
            case Number when ls[0] < 0:
                return new Cos(-ls[0]);
            case Product when (ls[0] as Product).elts[0] is Number && ((ls[0] as Product).elts[0] as Number) < 0:
                return new Cos(-ls[0]).Simplify();
            case Product when (
                            (ls[0] as Product).elts[0] is Integer ||
                            (ls[0] as Product).elts[0] is Fraction
                        ) && (ls[0] as Product).elts[0] as Number > new Integer(1) / 2 && (ls[0] as Product).elts[1] == Pi:
                {
                    var n = (ls[0] as Product).elts[0];

                    if (n > 2) return Cos(Mod(n, 2) * Pi);

                    if (n > 1) return -Cos(n * Pi - Pi);

                    if (n > half) return -Cos(Pi - n * Pi);

                    return new Cos(n * Pi);
                }

            case Product when List<MathObject>(1, 2, 3, 4, 6)
                            .Any(elt => elt == (ls[0] as Product).elts[0].Denominator()) &&
                        (ls[0] as Product).elts[0].Numerator() is Integer && (ls[0] as Product).elts[1] == Pi:
                {
                    var k = (ls[0] as Product).elts[0].Numerator();
                    var n = (ls[0] as Product).elts[0].Denominator();

                    if (n == 1)
                    {
                        if (Mod(k, 2) == 1) return -1;
                        if (Mod(k, 2) == 0) return 1;
                    }

                    if (n == 2)
                    {
                        if (Mod(k, 2) == 1) return 0;
                    }

                    if (n == 3)
                    {
                        if (Mod(k, 6) == 1) return half;
                        if (Mod(k, 6) == 5) return half;

                        if (Mod(k, 6) == 2) return -half;
                        if (Mod(k, 6) == 4) return -half;
                    }

                    if (n == 4)
                    {
                        if (Mod(k, 8) == 1) return 1 / (2 ^ half);
                        if (Mod(k, 8) == 7) return 1 / (2 ^ half);

                        if (Mod(k, 8) == 3) return -1 / (2 ^ half);
                        if (Mod(k, 8) == 5) return -1 / (2 ^ half);
                    }

                    if (n == 6)
                    {
                        if (Mod(k, 12) == 1) return (3 ^ half) / 2;
                        if (Mod(k, 12) == 11) return (3 ^ half) / 2;

                        if (Mod(k, 12) == 5) return -(3 ^ half) / 2;
                        if (Mod(k, 12) == 7) return -(3 ^ half) / 2;
                    }

                    break;
                }
        }



        // cos(Pi + x + y + ...)   ->   -cos(x + y + ...)

        if (u is Sum && (u as Sum).elts.Any(elt => elt == Pi))
            return -Cos(u - Pi);
        
        // cos(n Pi + x + y)

        // n * Pi where n is Exact && abs(n) >= 2

        Func<MathObject, bool> Product_n_Pi = elt =>
                (elt is Product) &&
                (
                    (elt as Product).elts[0] is Integer ||
                    (elt as Product).elts[0] is Fraction
                ) &&
                Math.Abs(((elt as Product).elts[0] as Number).ToDouble().val) >= 2.0 &&

                (elt as Product).elts[1] == Pi;

        if (ls[0] is Sum && (ls[0] as Sum).elts.Any(Product_n_Pi))
        {
            var pi_elt = (ls[0] as Sum).elts.First(Product_n_Pi);

            var n = (pi_elt as Product).elts[0];

            return Cos((ls[0] - pi_elt) + Mod(n, 2) * Pi);
        }

        Func<MathObject, bool> Product_n_div_2_Pi = elt =>
            elt is Product &&
            (
                (elt as Product).elts[0] is Integer ||
                (elt as Product).elts[0] is Fraction
            ) &&
            (elt as Product).elts[0].Denominator() == 2 &&
            (elt as Product).elts[1] == Pi;

        // cos(a + b + ... + n/2 * Pi) -> sin(a + b + ...)

        if (ls[0] is Sum && (ls[0] as Sum).elts.Any(Product_n_div_2_Pi))
        {
            var n_div_2_Pi = (ls[0] as Sum).elts.First(Product_n_div_2_Pi);

            var other_elts = ls[0] - n_div_2_Pi;

            var n = (n_div_2_Pi as Product).elts[0].Numerator();

            if (Mod(n, 4) == 1) return -new Sin(other_elts);
            if (Mod(n, 4) == 3) return new Sin(other_elts);
        }

        return new Cos(ls[0]);
    }
}
public class Tan(MathObject param) : Function("tan", TanProc, new[] { param })
{
    static MathObject TanProc(params MathObject[] ls)
    {
        if (ls[0] is DoubleFloat @float)
            return new DoubleFloat(Math.Tan(@float.val));

        return new Tan(ls[0]);
    }
}
public class Asin(MathObject param) : Function("asin", AsinProc, new[] { param })
{
    static MathObject AsinProc(params MathObject[] ls) => ls[0] is DoubleFloat @float ? new DoubleFloat(Math.Asin(@float.val)) : new Asin(ls[0]);
}
public class Atan(MathObject param) : Function("atan", AtanProc, new[] { param })
{
    static MathObject AtanProc(params MathObject[] ls) 
        => ls[0] is DoubleFloat @float ? new DoubleFloat(Math.Atan(@float.val)) : (MathObject)new Atan(ls[0]);
}
public class Atan2(MathObject a, MathObject b) : Function("atan2", Atan2Proc, new[] { a, b })
{
    static MathObject Atan2Proc(params MathObject[] ls) =>
        //if (
        //    (ls[0] is DoubleFloat || ls[0] is Integer)
        //    &&
        //    (ls[1] is DoubleFloat || ls[1] is Integer)
        //    )
        //    return new DoubleFloat(
        //        Math.Atan2(
        //            (ls[0] as Number).ToDouble().val,
        //            (ls[1] as Number).ToDouble().val));


        ls[0] switch
        {
            DoubleFloat @float when ls[1] is DoubleFloat float1 => new DoubleFloat(
                Math.Atan2(
                    @float.val,
                    float1.val)),
            Integer integer when ls[1] is DoubleFloat float2 => new DoubleFloat(
                Math.Atan2(
                    (double)integer.val,
                    float2.val)),
            DoubleFloat float3 when ls[1] is Integer integer2 => new DoubleFloat(
                Math.Atan2(
                    float3.val,
                    (double)integer2.val)),
            Integer integer3 when ls[1] is Integer integer1 => new DoubleFloat(
                Math.Atan2(
                    (double)integer3.val,
                    (double)integer1.val)),
            _ => new Atan2(ls[0], ls[1])
        };
}

public static class Constructors
{
    public static MathObject Sin(MathObject obj) => new Sin(obj).Simplify();
    public static MathObject Cos(MathObject obj) => new Cos(obj).Simplify();
    public static MathObject Tan(MathObject obj) => new Tan(obj).Simplify();

    public static MathObject Asin(MathObject obj) => new Asin(obj).Simplify();
    public static MathObject Atan(MathObject obj) => new Atan(obj).Simplify();
}

public static class Extensions
{
    public static readonly Symbol Pi = new ("Pi");

    public static MathObject ToRadians(this MathObject n) => n * Pi / 180;

    public static MathObject ToDegrees(this MathObject n) => 180 * n / Pi;

    public static MathObject ToRadians(this int n) => new Integer(n) * Pi / 180;

    public static MathObject ToDegrees(this int n) => 180 * new Integer(n) / Pi;

    // (Integer) 180 * n / Pi
}
