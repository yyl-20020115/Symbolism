/* Copyright 2013 Eduardo Cavazos

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License. */

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using static Symbolism.ListConstructor;

namespace Symbolism;

public abstract class MathObject
{
    //////////////////////////////////////////////////////////////////////
    public static implicit operator MathObject(int n) => new Integer(n);

    public static implicit operator MathObject(BigInteger n) => new Integer(n);

    public static implicit operator MathObject(bool val) => new Bool(val);

    public static implicit operator MathObject(double val) => new DoubleFloat(val);
    //////////////////////////////////////////////////////////////////////
    #region overloads for 'int'
    public static MathObject operator +(MathObject a, int b) => a + new Integer(b);
    public static MathObject operator -(MathObject a, int b) => a - new Integer(b);
    public static MathObject operator *(MathObject a, int b) => a * new Integer(b);
    public static MathObject operator /(MathObject a, int b) => a / new Integer(b);
    public static MathObject operator ^(MathObject a, int b) => a ^ new Integer(b);
    public static MathObject operator +(int a, MathObject b) => new Integer(a) + b;
    public static MathObject operator -(int a, MathObject b) => new Integer(a) - b;
    public static MathObject operator *(int a, MathObject b) => new Integer(a) * b;
    public static MathObject operator /(int a, MathObject b) => new Integer(a) / b;
    public static MathObject operator ^(int a, MathObject b) => new Integer(a) ^ b;
    #endregion
    //////////////////////////////////////////////////////////////////////
    #region overloads for 'BigInteger'
    public static MathObject operator +(MathObject a, BigInteger b) => a + new Integer(b);
    public static MathObject operator -(MathObject a, BigInteger b) => a - new Integer(b);
    public static MathObject operator *(MathObject a, BigInteger b) => a * new Integer(b);
    public static MathObject operator /(MathObject a, BigInteger b) => a / new Integer(b);
    public static MathObject operator ^(MathObject a, BigInteger b) => a ^ new Integer(b);
    public static MathObject operator +(BigInteger a, MathObject b) => new Integer(a) + b;
    public static MathObject operator -(BigInteger a, MathObject b) => new Integer(a) - b;
    public static MathObject operator *(BigInteger a, MathObject b) => new Integer(a) * b;
    public static MathObject operator /(BigInteger a, MathObject b) => new Integer(a) / b;
    public static MathObject operator ^(BigInteger a, MathObject b) => new Integer(a) ^ b;
    #endregion
    //////////////////////////////////////////////////////////////////////
    #region overloads for 'double'

    public static MathObject operator +(MathObject a, double b) => a + new DoubleFloat(b);
    public static MathObject operator -(MathObject a, double b) => a - new DoubleFloat(b);
    public static MathObject operator *(MathObject a, double b) => a * new DoubleFloat(b);
    public static MathObject operator /(MathObject a, double b) => a / new DoubleFloat(b);
    public static MathObject operator ^(MathObject a, double b) => a ^ new DoubleFloat(b);
    public static MathObject operator +(double a, MathObject b) => new DoubleFloat(a) + b;
    public static MathObject operator -(double a, MathObject b) => new DoubleFloat(a) - b;
    public static MathObject operator *(double a, MathObject b) => new DoubleFloat(a) * b;
    public static MathObject operator /(double a, MathObject b) => new DoubleFloat(a) / b;
    public static MathObject operator ^(double a, MathObject b) => new DoubleFloat(a) ^ b;

    #endregion
    //////////////////////////////////////////////////////////////////////
    public static Equation operator ==(MathObject a, MathObject b) => new(a, b);
    public static Equation operator !=(MathObject a, MathObject b) => new(a, b, Equation.Operators.NotEqual);
    public static Equation operator <(MathObject a, MathObject b) => new(a, b, Equation.Operators.LessThan);
    public static Equation operator >(MathObject a, MathObject b) => new(a, b, Equation.Operators.GreaterThan);

    public static Equation operator ==(MathObject a, double b) => new(a, new DoubleFloat(b));
    public static Equation operator ==(double a, MathObject b) => new(new DoubleFloat(a), b);

    public static Equation operator !=(MathObject a, double b) => new(a, new DoubleFloat(b), Equation.Operators.NotEqual);
    public static Equation operator !=(double a, MathObject b) => new(new DoubleFloat(a), b, Equation.Operators.NotEqual);

    public static Equation operator ==(MathObject a, int b) => new(a, new Integer(b));
    public static Equation operator ==(int a, MathObject b) => new(new Integer(a), b);
    public static Equation operator !=(MathObject a, int b) => new(a, new Integer(b), Equation.Operators.NotEqual);
    public static Equation operator !=(int a, MathObject b) => new(new Integer(a), b, Equation.Operators.NotEqual);
    //////////////////////////////////////////////////////////////////////
    public static MathObject operator +(MathObject a, MathObject b) => new Sum(a, b).Simplify();
    public static MathObject operator -(MathObject a, MathObject b) => new Difference(a, b).Simplify();
    public static MathObject operator *(MathObject a, MathObject b) => new Product(a, b).Simplify();
    public static MathObject operator /(MathObject a, MathObject b) => new Quotient(a, b).Simplify();
    public static MathObject operator ^(MathObject a, MathObject b) => new Power(a, b).Simplify();

    public static MathObject operator -(MathObject a) { return new Difference(a).Simplify(); }

    // Precedence is used for printing purposes.
    // Thus, the precedence values below do not necessarily reflect 
    // the C# operator precedence values.
    // For example, in C#, the precedence of ^ is lower than +.
    // But for printing purposes, we'd like ^ to have a 
    // higher precedence than +.

    public int Precedence()
    {
        switch (this)
        {
            case Integer:
                return 1000;
            case DoubleFloat:
                return 1000;
            case Symbol:
                return 1000;
            case Function:
                return 1000;
            case Fraction:
                return 1000;
            case Power:
                return 130;
            case Product:
                return 120;
            case Sum:
                return 110;
        }

        Console.WriteLine(this.GetType().Name);

        throw new Exception();
    }

    public enum ToStringForms { Full, Standard }

    public ToStringForms ToStringForm = ToStringForms.Full;

    public virtual string FullForm() => base.ToString();

    public virtual string StandardForm() => FullForm();

    public override string ToString() => ToStringForm switch
    {
        ToStringForms.Full => FullForm(),
        ToStringForms.Standard => StandardForm(),
        _ => throw new Exception()
    };

    public virtual MathObject Numerator() => this;

    public virtual MathObject Denominator() => 1;

    public override bool Equals(object obj)
    { throw new Exception("MathObject.Equals called - abstract class"); }

    public override int GetHashCode() => base.GetHashCode();
}

public class Equation : MathObject
{
    public enum Operators { Equal, NotEqual, LessThan, GreaterThan }

    public readonly MathObject a;
    public readonly MathObject b;

    public Operators Operator;

    public Equation(MathObject x, MathObject y)
    { a = x; b = y; Operator = Operators.Equal; }

    public Equation(MathObject x, MathObject y, Operators op)
    { a = x; b = y; Operator = op; }

    public override string FullForm() => Operator switch
    {
        Operators.Equal => a + " == " + b,
        Operators.NotEqual => a + " != " + b,
        Operators.LessThan => a + " < " + b,
        Operators.GreaterThan => a + " > " + b,
        _ => throw new Exception()
    };

    public override bool Equals(object obj) =>
        obj is Equation e&&
        a.Equals(e.a) &&
        b.Equals(e.b) &&
        Operator == e.Operator;

    bool ToBoolean()
    {
        if (a is Bool && b is Bool) return (a as Bool).Equals(b);

        if (a is Equation && b is Equation) return (a as Equation).Equals(b);

        switch (a)
        {
            case Integer _ when b is Integer:
                return ((Integer)a).Equals(b);
            case DoubleFloat _ when b is DoubleFloat:
                return ((DoubleFloat)a).Equals(b);
            case Symbol _ when b is Symbol:
                return ((Symbol)a).Equals(b);
            case Sum _ when b is Sum:
                return ((Sum)a).Equals(b);
            case Product _ when b is Product:
                return ((Product)a).Equals(b);
            case Fraction _ when b is Fraction:
                return ((Fraction)a).Equals(b);
            case Power _ when b is Power:
                return ((Power)a).Equals(b);
            case Function _ when b is Function:
                return ((Function)a).Equals(b);
            case null when b is null:
                return true;
            case null:
                return false;
        }

        if (((object)b) == null) return false;

        if (a.GetType() != b.GetType()) return false;

        Console.WriteLine("" + a.GetType() + " " + b.GetType());

        throw new Exception();
    }

    public static implicit operator Boolean(Equation eq)
    {
        switch (eq.Operator)
        {
            case Operators.Equal:
                return (eq.a == eq.b).ToBoolean();
            case Operators.NotEqual:
                return !(eq.a == eq.b).ToBoolean();
            case Operators.LessThan:
                if (eq.a is Number && eq.b is Number)
                    return (eq.a as Number).ToDouble().val < (eq.b as Number).ToDouble().val;
                break;
        }

        if (eq.Operator == Operators.GreaterThan)
            if (eq.a is Number && eq.b is Number)
                return (eq.a as Number).ToDouble().val > (eq.b as Number).ToDouble().val;

        throw new Exception();
    }

    public MathObject Simplify()
    {
        if (a is Number && b is Number) return (bool)this;

        return this;
    }

    public override int GetHashCode() => new { a, b }.GetHashCode();

}

public class Bool(bool b) : MathObject
{
    public readonly bool val = b;

    public override string FullForm() => val.ToString();

    public override bool Equals(object obj) => val == (obj as Bool)?.val;

    public override int GetHashCode() => val.GetHashCode();
}

//public class NotEqual
//{
//    public MathObject a;
//    public MathObject b;

//    public NotEqual(MathObject x, MathObject y)
//    { a = x; b = y; }

//    public static implicit operator Boolean(NotEqual eq)
//    { return !((eq.a == eq.b).ToBoolean()); }
//}

public abstract class Number : MathObject
{
    public abstract DoubleFloat ToDouble();
}

public class Integer : Number
{
    public readonly BigInteger val;

    public Integer(int n) { val = n; }

    public Integer(BigInteger n) { val = n; }
                    
    public static implicit operator Integer(BigInteger n) => new(n);

    // public static MathObject operator *(MathObject a, MathObject b) => new Product(a, b).Simplify();

    public static Integer operator +(Integer a, Integer b) => a.val + b.val;
    public static Integer operator -(Integer a, Integer b) => a.val - b.val;
    public static Integer operator *(Integer a, Integer b) => a.val * b.val;

    public override string FullForm() => val.ToString();

    public override bool Equals(object obj) => val == (obj as Integer)?.val;

    public override int GetHashCode() => val.GetHashCode();

    public override DoubleFloat ToDouble() => new((double)val);
}

public class DoubleFloat(double n) : Number
{
    public static double? tolerance = null;

    public readonly double val = n;

    public override string FullForm() => val.ToString("R");

    //public bool EqualWithinTolerance(DoubleFloat obj)
    //{
    //    if (tolerance.HasValue)
    //        return Math.Abs(val - obj.val) < tolerance;

    //    throw new Exception();
    //}

    public override bool Equals(object obj)
    {
        if (obj is DoubleFloat f&& tolerance.HasValue)
            return Math.Abs(val - f.val) < tolerance;

        if (obj is DoubleFloat @float) return val == @float.val;

        return false;
    }

    public override int GetHashCode() => val.GetHashCode();

    public override DoubleFloat ToDouble() => this;
}

public class Fraction(Integer a, Integer b) : Number
{
    public readonly Integer numerator = a;
    public readonly Integer denominator = b;

    public override string FullForm() => numerator + "/" + denominator;

    public override DoubleFloat ToDouble() => new((double)numerator.val / (double)denominator.val);
    //////////////////////////////////////////////////////////////////////

    public override bool Equals(object obj) =>
        numerator == (obj as Fraction)?.numerator
        &&
        denominator == (obj as Fraction)?.denominator;

    public override int GetHashCode() => new { numerator, denominator }.GetHashCode();

    public override MathObject Numerator() => numerator;

    public override MathObject Denominator() => denominator;
}

public static class Rational
{
    static BigInteger Div(BigInteger a, BigInteger b)
    {
        return BigInteger.DivRem(a, b, out _); }
            
    static BigInteger Rem(BigInteger a, BigInteger b)
    { _ = BigInteger.DivRem(a, b, out BigInteger rem); return rem; }
            
    static BigInteger Gcd(BigInteger a, BigInteger b)
    {
        BigInteger r;
        while (b != 0)
        {
            r = Rem(a, b);
            a = b;
            b = r;
        }
        return BigInteger.Abs(a);
    }

    public static MathObject SimplifyRationalNumber(MathObject u)
    {
        if (u is Integer) return u;

        if (u is Fraction u_)
        {
            var n = u_.numerator.val;
            var d = u_.denominator.val;

            if (Rem(n, d) == 0) return Div(n, d);

            var g = Gcd(n, d);

            if (d > 0) return new Fraction(Div(n, g), Div(d, g));

            if (d < 0) return new Fraction(Div(-n, g), Div(-d, g));
        }

        throw new Exception();
    }

    public static Integer Numerator(MathObject u)
    {
        // (a / b) / (c / d)
        // (a / b) * (d / c)
        // (a * d) / (b * c)

        if (u is Integer integer) return integer;

        if (u is Fraction u_)
        {
            //return
            //    Numerator(u_.numerator).val
            //    *
            //    Denominator(u_.denominator).val;

            return
                Numerator(u_.numerator)
                *
                Denominator(u_.denominator);
        }

        throw new Exception();
    }

    public static Integer Denominator(MathObject u)
    {
        // (a / b) / (c / d)
        // (a / b) * (d / c)
        // (a * d) / (b * c)

        if (u is Integer) return new Integer(1);

        if (u is Fraction u_)
        {
            return
                Denominator(u_.numerator)
                *
                Numerator(u_.denominator);
        }

        throw new Exception();
    }

    public static Fraction EvaluateSum(MathObject v, MathObject w) =>        

        // a / b + c / d
        // a d / b d + c b / b d
        // (a d + c b) / (b d)

        new (
            Numerator(v) * Denominator(w) + Numerator(w) * Denominator(v),
            Denominator(v) * Denominator(w));
    
    public static Fraction EvaluateDifference(MathObject v, MathObject w) =>
        new (
            Numerator(v) * Denominator(w) - Numerator(w) * Denominator(v),
            Denominator(v) * Denominator(w));

    public static Fraction EvaluateProduct(MathObject v, MathObject w) => 
        new (
            Numerator(v) * Numerator(w),
            Denominator(v) * Denominator(w));

    public static MathObject EvaluateQuotient(MathObject v, MathObject w) => Numerator(w).val == 0
            ? new Undefined()
            : new Fraction(
                Numerator(v) * Denominator(w),
                Numerator(w) * Denominator(v));

    public static MathObject EvaluatePower(MathObject v, BigInteger n)
    {
        if (Numerator(v).val != 0)
        {
            if (n > 0) return EvaluateProduct(EvaluatePower(v, n - 1), v);

            if (n == 0) return 1;
            
            if (n == -1) return new Fraction(Denominator(v), Numerator(v));

            if (n < -1)
            {
                var s = new Fraction(Denominator(v), Numerator(v));

                return EvaluatePower(s, -n);
            }
        }
                    
        if (n >= 1) return 0;
        if (n <= 0) return new Undefined();

        throw new Exception();
    }

    public static MathObject SimplifyRNERec(MathObject u)
    {
        if (u is Fraction f && Denominator(f).val == 0) 
            return new Undefined();

        switch (u)
        {
            case Sum _ when ((Sum)u).elts.Count == 1:
                return SimplifyRNERec(((Sum)u).elts[0]);
            case Difference _ when ((Difference)u).elts.Count == 1:
                {
                    var v = SimplifyRNERec(((Difference)u).elts[0]);

                    if (v == new Undefined()) return v;

                    return EvaluateProduct(-1, v);
                }

            case Sum sum when sum.elts.Count == 2:
                {
                    var v = SimplifyRNERec(sum.elts[0]);
                    var w = SimplifyRNERec(sum.elts[1]);

                    if (v == new Undefined() || w == new Undefined())
                        return new Undefined();

                    return EvaluateSum(v, w);
                }

            case Product product when product.elts.Count == 2:
                {
                    var v = SimplifyRNERec(product.elts[0]);
                    var w = SimplifyRNERec(product.elts[1]);

                    if (v == new Undefined() || w == new Undefined())
                        return new Undefined();

                    return EvaluateProduct(v, w);
                }

            case Difference difference when difference.elts.Count == 2:
                {
                    var v = SimplifyRNERec(difference.elts[0]);
                    var w = SimplifyRNERec(difference.elts[1]);

                    if (v == new Undefined() || w == new Undefined())
                        return new Undefined();

                    return EvaluateDifference(v, w);
                }

            case Fraction fraction:
                {
                    var v = SimplifyRNERec(fraction.numerator);
                    var w = SimplifyRNERec(fraction.denominator);

                    if (v == new Undefined() || w == new Undefined())
                        return new Undefined();

                    return EvaluateQuotient(v, w);
                }

            case Power power:
                {
                    var v = SimplifyRNERec(power.bas);

                    if (v == new Undefined()) return v;

                    return EvaluatePower(v, ((Integer)power.exp).val);
                }

            default:
                throw new Exception();
        }
    }

    public static MathObject SimplifyRNE(MathObject u)
    {
        var v = SimplifyRNERec(u);
        if (v is Undefined) return v;
        return SimplifyRationalNumber(v);
    }
}

public class Undefined : MathObject { }

public static class MiscUtils { }

public class Symbol(string str) : MathObject
{
    public readonly string name = str;

    public override string FullForm() => name;

    public override int GetHashCode() => name.GetHashCode();

    public override bool Equals(object o) =>
        o is Symbol s && name == s.name;
}

public static class ListConstructor
{
    public static List<T> List<T>(params T[] items) => new(items);

    public static ImmutableList<T> ImList<T>(params T[] items) => ImmutableList.Create(items);
}

public static class ListUtils
{
    public static ImmutableList<MathObject> Cons(this ImmutableList<MathObject> obj, MathObject elt) => 
        obj.Insert(0, elt);
    
    public static ImmutableList<MathObject> Cdr(this ImmutableList<MathObject> obj) => obj.RemoveAt(0);

    public static bool Equal(ImmutableList<MathObject> a, ImmutableList<MathObject> b)
    {
        if (a.Count == 0 && b.Count == 0) return true;

        if (a.Count == 0) return false;

        if (b.Count == 0) return false;

        if (a[0] == b[0]) return Equal(a.Cdr(), b.Cdr());

        return false;
    }
}

public class Function(string name, Function.Proc proc, IEnumerable<MathObject> args) : MathObject
{
    public delegate MathObject Proc(params MathObject[] ls);

    public readonly String name = name;

    public readonly Proc proc = proc;
            
    public readonly ImmutableList<MathObject> args = ImmutableList.CreateRange(args);

    public override bool Equals(object o) =>
        GetType() == o.GetType() && o is Function f && 
        name == f.name &&
        ListUtils.Equal(args, f.args);

    public MathObject Simplify() => proc == null ? this : proc([.. args]);

    public override string FullForm() => $"{name}({string.Join(", ", args)})";

    public MathObject Clone() => MemberwiseClone() as MathObject;

    public override int GetHashCode() => new { name, args }.GetHashCode();
}

public static class FunctionExtensions
{
    //public static MathObject Map<T>(this T obj, Func<MathObject, MathObject> proc) where T : Function, new()
    //{
    //    // return new T() { args = obj.args.Select(proc).ToList() }.Simplify();

    //    // return 
        
    //}
}

public class And : Function
{
    static MathObject AndProc(MathObject[] ls)
    {
        if (ls.Length == 0) return true;

        if (ls.Length == 1) return ls.First();

        if (ls.Any(elt => elt == false)) return false;

        if (ls.Any(elt => elt == true))
            return new And(ls.Where(elt => elt != true).ToArray()).Simplify();
                    
        if (ls.Any(elt => elt is And))
        {
            var items = new List<MathObject>();

            foreach (var elt in ls)
            {
                if (elt is And) items.AddRange((elt as And).args);

                else items.Add(elt);
            }

            return And.FromRange(items).Simplify();
        }

        return new And(ls);
    }
            
    public And(params MathObject[] ls) : base("and", AndProc, ls) { }

    public And() : base("and", AndProc, new List<MathObject>()) { }

    public static And FromRange(IEnumerable<MathObject> ls) => new(ls.ToArray());

    public MathObject Add(MathObject obj) =>
        And.FromRange(args.Add(obj)).Simplify();

    public MathObject AddRange(IEnumerable<MathObject> ls) =>
        And.FromRange(args.AddRange(ls)).Simplify();
            
    public MathObject Map(Func<MathObject, MathObject> proc) => 
        And.FromRange(args.Select(proc)).Simplify();
}

public class Or : Function
{
    static MathObject OrProc(params MathObject[] ls)
    {
        if (ls.Length == 1) return ls.First();

        // 10 || false || 20   ->   10 || 20

        if (ls.Any(elt => elt == false))
            return Or.FromRange(ls.Where(elt => elt != false)).Simplify();

        if (ls.Any(elt => (elt is Bool) && (elt as Bool).val)) return new Bool(true);

        if (ls.All(elt => (elt is Bool) && (elt as Bool).val == false)) return new Bool(false);
                    
        if (ls.Any(elt => elt is Or))
        {
            var items = new List<MathObject>();

            foreach (var elt in ls)
            {
                if (elt is Or) items.AddRange((elt as Or).args);
                else items.Add(elt);
            }
                            
            return Or.FromRange(items).Simplify();
        }

        return new Or(ls);
    }
            
    public Or(params MathObject[] ls) : base("or", OrProc, ls) { }

    public Or() : base("or", OrProc, new List<MathObject>()) { }

    public static Or FromRange(IEnumerable<MathObject> ls) => new(ls.ToArray());

    public MathObject Map(Func<MathObject, MathObject> proc) => Or.FromRange(args.Select(proc)).Simplify();
}

public static class OrderRelation
{
    public static MathObject Base(MathObject u) => u is Power ? (u as Power).bas : u;

    public static MathObject Exponent(MathObject u) => u is Power ? (u as Power).exp : 1;

    public static MathObject Term(this MathObject u)
    {
        if (u is Product product && product.elts[0] is Number)
            return Product.FromRange((u as Product).elts.Cdr());
            // return (u as Product).Cdr()

        if (u is Product) return u;

        return new Product(u);
    }

    public static MathObject Const(this MathObject u) =>
        (u is Product && (u as Product).elts[0] is Number) ? (u as Product).elts[0] : 1;

    public static bool O3(ImmutableList<MathObject> uElts, ImmutableList<MathObject> vElts)
    {
        if (uElts.IsEmpty) return true;
        if (vElts.IsEmpty) return false;

        var u = uElts.First();
        var v = vElts.First();

        return (!(u == v)) ?
            Compare(u, v) :
            O3(uElts.Cdr(), vElts.Cdr());
    }

    public static bool Compare(MathObject u, MathObject v)
    {
        if (u is DoubleFloat @float && v is DoubleFloat float1)
        {
            return @float.val < float1.val;
        }

        // if (u is DoubleFloat && v is Integer) return ((DoubleFloat)u).val < ((Integer)v).val;

        if (u is DoubleFloat float2 && v is Integer integer) return float2.val < ((double)integer.val);

        if (u is DoubleFloat float3 && v is Fraction fraction) return
            float3.val < ((double)fraction.numerator.val) / ((double)fraction.denominator.val);

        switch (u)
        {
            case Integer _ when v is DoubleFloat float5:
                return (double)((Integer)u).val < float5.val;
            case Fraction _ when v is DoubleFloat float4:
                return
                            (double)((Fraction)u).numerator.val / (double)((Fraction)u).denominator.val < float4.val;
            case Integer _:
                return Compare(new Fraction((Integer)u, new Integer(1)), v);
        }

        if (v is Integer integer1)
            return Compare(u, new Fraction(integer1, new Integer(1)));

        switch (u)
        {
            case Fraction _ when v is Fraction fraction1:
                {
                    var u_ = (Fraction)u;
                    var v_ = fraction1;

                    // a / b   <   c / d
                    //
                    // (a d) / (b d)   <   (c b) / (b d)

                    return
                        u_.numerator.val * v_.denominator.val
                        <
                        v_.numerator.val * u_.denominator.val;
                }

            case Symbol _ when v is Symbol symbol:
                return
                                string.Compare(
                                    ((Symbol)u).name,
                                    symbol.name) < 0;
            case Product _ when v is Product:
                return O3(
                                (u as Product).elts.Reverse(),
                                (v as Product).elts.Reverse());
            case Sum _ when v is Sum:
                return O3(
                                (u as Sum).elts.Reverse(),
                                (v as Sum).elts.Reverse());
            case Power _ when v is Power power:
                {
                    var u_ = (Power)u;
                    var v_ = power;

                    return u_.bas == v_.bas ?
                        Compare(u_.exp, v_.exp) :
                        Compare(u_.bas, v_.bas);
                }

            case Function _ when v is Function function:
                {
                    var u_ = (Function)u;
                    var v_ = function;

                    return u_.name == v_.name ?
                        O3(u_.args, v_.args) :
                        string.Compare(u_.name, v_.name) < 0;
                }

            case Number _ when v is not Number:
                return true;
            case Product _ when v is Power || v is Sum || v is Function || v is Symbol:
                return Compare(u, new Product(v));
            case Power _ when v is Sum || v is Function || v is Symbol:
                return Compare(u, new Power(v, new Integer(1)));
            case Sum _ when v is Function || v is Symbol:
                return Compare(u, new Sum(v));
            case Function _ when v is Symbol symbol1:
                {
                    var u_ = (Function)u;
                    var v_ = symbol1;

                    return u_.name != v_.name && Compare(new Symbol(u_.name), v);
                }

            default:
                return !Compare(v, u);
        }
    }
}

public class Power(MathObject a, MathObject b) : MathObject
{
    public readonly MathObject bas = a;
    public readonly MathObject exp = b;

    public override string FullForm() =>
        string.Format("{0} ^ {1}",
            bas.Precedence() < Precedence() ? $"({bas})" : $"{bas}",
            exp.Precedence() < Precedence() ? $"({exp})" : $"{exp}");

    public override string StandardForm()
    {
        // x ^ 1/2   ->   sqrt(x)

        if (exp == new Integer(1) / new Integer(2)) return $"sqrt({bas})";

        return string.Format("{0} ^ {1}",
            bas.Precedence() < Precedence() ? $"({bas})" : $"{bas}",
            exp.Precedence() < Precedence() ? $"({exp})" : $"{exp}");
    }

    public override bool Equals(object obj) =>
        obj is Power && bas == (obj as Power).bas && exp == (obj as Power).exp;

    public MathObject Simplify()
    {
        var v = bas;
        var w = exp;

        if (v == 0) return 0;
        if (v == 1) return 1;
        if (w == 0) return 1;
        if (w == 1) return v;

        // Logic from MPL/Scheme:
        //
        //if (v is Integer && w is Integer)
        //    return
        //        new Integer(
        //            (int)Math.Pow(((Integer)v).val, ((Integer)w).val));

        // C# doesn't have built-in rationals. So:
        // 1 / 3 -> 3 ^ -1 -> 0.333... -> (int)... -> 0

        //if (v is Integer && w is Integer && ((Integer)w).val > 1)
        //    return
        //        new Integer(
        //            (int)Math.Pow(((Integer)v).val, ((Integer)w).val));

        var n = w;

        if ((v is Integer || v is Fraction) && n is Integer)
            return Rational.SimplifyRNE(new Power(v, n));

        if (v is DoubleFloat @float && w is Integer integer)
            return new DoubleFloat(Math.Pow(@float.val, (double) integer.val));

        return v switch
        {
            DoubleFloat _ when w is Fraction fraction => new DoubleFloat(Math.Pow(((DoubleFloat)v).val, fraction.ToDouble().val)),
            Integer _ when w is DoubleFloat float1 => new DoubleFloat(Math.Pow((double)((Integer)v).val, float1.val)),
            Fraction _ when w is DoubleFloat float2 => new DoubleFloat(Math.Pow(((Fraction)v).ToDouble().val, float2.val)),
            Power _ when w is Integer => ((Power)v).bas ^ ((Power)v).exp * w,
            Product _ when w is Integer => (v as Product).Map(elt => elt ^ w),
            _ => new Power(v, w),
        };
    }

    public override MathObject Numerator()
    {
        if (exp is Integer && exp < 0) return 1;

        if (exp is Fraction && exp < 0) return 1;

        return this;
    }

    public override MathObject Denominator()
    {
        if (exp is Integer && exp < 0) return this ^ -1;

        if (exp is Fraction && exp < 0) return this ^ -1;

        return 1;
    }

    public override int GetHashCode() => new { bas, exp }.GetHashCode();
}

public class Product(params MathObject[] ls) : MathObject
{
    public readonly ImmutableList<MathObject> elts = ImmutableList.Create(ls);

    public static Product FromRange(IEnumerable<MathObject> ls) => new (ls.ToArray());

    public override string FullForm() =>
        string.Join(" * ", elts.ConvertAll(elt => elt.Precedence() < Precedence() ? $"({elt})" : $"{elt}"));

    public override string StandardForm()
    {
        if (this.Denominator() == 1)
        {
            if (this.Const() < 0 && this / this.Const() is Sum) return $"-({this * -1})";

            if (this.Const() < 0) return $"-{this * -1}";

            return string.Join(" * ",
                elts.ConvertAll(elt => elt.Precedence() < Precedence() || (elt is Power && (elt as Power).exp != new Integer(1) / 2) ? $"({elt})" : $"{elt}"));
        }

        var expr_a = this.Numerator();
        var expr_b = this.Denominator();

        var expr_a_ = expr_a is Sum || (expr_a is Power && (expr_a as Power).exp != new Integer(1) / 2) ? $"({expr_a})" : $"{expr_a}";

        var expr_b_ = expr_b is Sum || expr_b is Product || (expr_b is Power && (expr_b as Power).exp != new Integer(1) / 2) ? $"({expr_b})" : $"{expr_b}";

        return $"{expr_a_} / {expr_b_}";
    }

    public override int GetHashCode() => elts.GetHashCode();

    public override bool Equals(object obj) =>
        obj is Product && ListUtils.Equal(elts, (obj as Product).elts);

    static ImmutableList<MathObject> MergeProducts(ImmutableList<MathObject> pElts, ImmutableList<MathObject> qElts)
    {
        if (pElts.Count == 0) return qElts;
        if (qElts.Count == 0) return pElts;

        var p = pElts[0];
        var ps = pElts.Cdr();

        var q = qElts[0];
        var qs = qElts.Cdr();

        var res = RecursiveSimplify(ImList(p, q));

        if (res.Count == 0) return MergeProducts(ps, qs);

        if (res.Count == 1) return MergeProducts(ps, qs).Cons(res[0]);

        if (ListUtils.Equal(res, ImList(p, q))) return MergeProducts(ps, qElts).Cons(p);

        if (ListUtils.Equal(res, ImList(q, p))) return MergeProducts(pElts, qs).Cons(q);

        throw new Exception();
    }

    static ImmutableList<MathObject> SimplifyDoubleNumberProduct(DoubleFloat a, Number b)
    {
        double val = 0.0;

        if (b is DoubleFloat @float) val = a.val * @float.val;

        if (b is Integer integer) val = a.val * (double)integer.val;

        if (b is Fraction fraction) val = a.val * fraction.ToDouble().val;
                    
        if (val == 1.0) return [];

        return ImList<MathObject>(new DoubleFloat(val));
    }

    public static ImmutableList<MathObject> RecursiveSimplify(ImmutableList<MathObject> elts)
    {
        if (elts.Count == 2)
        {
            switch (elts[0])
            {
                case Product _ when elts[1] is Product product2:
                    return MergeProducts(
                                    ((Product)elts[0]).elts,
                                    product2.elts);
                case Product product2:
                    return MergeProducts(product2.elts, ImList(elts[1]));
            }

            if (elts[1] is Product product1) return MergeProducts(ImList(elts[0]), product1.elts);

            //////////////////////////////////////////////////////////////////////

            if (elts[0] is DoubleFloat float1 && elts[1] is Number number1)
                return SimplifyDoubleNumberProduct(float1, number1);

            if (elts[0] is Number number && elts[1] is DoubleFloat @float)
                return SimplifyDoubleNumberProduct(@float, number);

            //////////////////////////////////////////////////////////////////////

            if ((elts[0] is Integer || elts[0] is Fraction)
                &&
                (elts[1] is Integer || elts[1] is Fraction))
            {
                var P = Rational.SimplifyRNE(new Product(elts[0], elts[1]));
                                    
                if (P == 1) return [];

                return ImList(P);
            }

            if (elts[0] == 1) return ImList(elts[1]);
            if (elts[1] == 1) return ImList(elts[0]);

            var p = elts[0];
            var q = elts[1];

            if (OrderRelation.Base(p) == OrderRelation.Base(q))
            {
                var res = OrderRelation.Base(p) ^ (OrderRelation.Exponent(p) + OrderRelation.Exponent(q));
                                    
                if (res == 1) return [];

                return ImList(res);
            }

            if (OrderRelation.Compare(q, p)) return ImList(q, p);

            return ImList(p, q);
        }

        if (elts[0] is Product product)
            return
                MergeProducts(
                    product.elts,
                    RecursiveSimplify(elts.Cdr()));

        return MergeProducts(
            ImList(elts[0]),
            RecursiveSimplify(elts.Cdr()));

        throw new Exception();
    }

    public MathObject Simplify()
    {
        if (elts.Count == 1) return elts[0];

        if (elts.Any(elt => elt == 0)) return 0;

        var res = RecursiveSimplify(elts);

        if (res.IsEmpty) return 1;

        if (res.Count == 1) return res[0];

        // Without the below, the following throws an exception:
        // sqrt(a * b) * (sqrt(a * b) / a) / c
                    
        if (res.Any(elt => elt is Product)) return Product.FromRange(res).Simplify();
                    
        return Product.FromRange(res);
    }

    public override MathObject Numerator() =>
        Product.FromRange(elts.Select(elt => elt.Numerator())).Simplify();

    public override MathObject Denominator() =>
        Product.FromRange(elts.Select(elt => elt.Denominator())).Simplify();

    public MathObject Map(Func<MathObject, MathObject> proc) =>
        Product.FromRange(elts.Select(proc)).Simplify();
}

public class Sum(params MathObject[] ls) : MathObject
{
    public readonly ImmutableList<MathObject> elts = ImmutableList.Create(ls);

    public static Sum FromRange(IEnumerable<MathObject> ls) => new(ls.ToArray());

    public override int GetHashCode() => elts.GetHashCode();

    public override bool Equals(object obj) =>
        obj is Sum && ListUtils.Equal(elts, (obj as Sum).elts);

    static ImmutableList<MathObject> MergeSums(ImmutableList<MathObject> pElts, ImmutableList<MathObject> qElts)
    {
        if (pElts.Count == 0) return qElts;
        if (qElts.Count == 0) return pElts;

        var p = pElts[0];
        var ps = pElts.Cdr();

        var q = qElts[0];
        var qs = qElts.Cdr();

        var res = RecursiveSimplify(ImList(p, q));

        if (res.Count == 0) return MergeSums(ps, qs);

        if (res.Count == 1) return MergeSums(ps, qs).Cons(res[0]);

        if (ListUtils.Equal(res, ImList(p, q))) return MergeSums(ps, qElts).Cons(p);

        if (ListUtils.Equal(res, ImList(q, p))) return MergeSums(pElts, qs).Cons(q);

        throw new Exception();
    }

    static ImmutableList<MathObject> SimplifyDoubleNumberSum(DoubleFloat a, Number b)
    {
        double val = 0.0;

        if (b is DoubleFloat @float) val = a.val + @float.val;

        if (b is Integer integer) val = a.val + (double)integer.val;

        if (b is Fraction fraction) val = a.val + fraction.ToDouble().val;

        if (val == 0.0) return [];
                    
        return [new DoubleFloat(val)];
    }

    static ImmutableList<MathObject> RecursiveSimplify(ImmutableList<MathObject> elts)
    {
        if (elts.Count == 2)
        {
            if (elts[0] is Sum sum2 && elts[1] is Sum sum3)
                return MergeSums(
                    sum2.elts,
                    sum3.elts);

            if (elts[0] is Sum sum1)
                return MergeSums(
                    sum1.elts,
                    ImList(elts[1]));

            if (elts[1] is Sum sums)
                return MergeSums(
                    ImList(elts[0]),
                    sums.elts);

            //////////////////////////////////////////////////////////////////////

            if (elts[0] is DoubleFloat float1 && elts[1] is Number number1)
                return SimplifyDoubleNumberSum(float1, number1);

            if (elts[0] is Number number && elts[1] is DoubleFloat @float)
            {
                return SimplifyDoubleNumberSum(@float, number);
            }

            //////////////////////////////////////////////////////////////////////

            if ((elts[0] is Integer || elts[0] is Fraction)
                &&
                (elts[1] is Integer || elts[1] is Fraction))
            {
                var P = Rational.SimplifyRNE(new Sum(elts[0], elts[1]));
                                    
                if (P == 0) return [];

                return ImList(P);
            }

            if (elts[0] == 0) return ImList(elts[1]);

            if (elts[1] == 0) return ImList(elts[0]);

            var p = elts[0];
            var q = elts[1];

            if (p.Term() == q.Term())
            {
                var res = p.Term() * (p.Const() + q.Const());

                if (res == 0) return [];

                return ImList(res);
            }

            if (OrderRelation.Compare(q, p)) return ImList(q, p);

            return ImList(p, q);
        }

        return elts[0] is Sum sum
            ? MergeSums(
                    sum.elts, RecursiveSimplify(elts.Cdr()))
            : MergeSums(
            ImList(elts[0]), RecursiveSimplify(elts.Cdr()));
    }

    public MathObject Simplify()
    {
        if (elts.Count == 1) return elts[0];

        var res = RecursiveSimplify(elts);

        if (res.Count == 0) return 0;
        if (res.Count == 1) return res[0];

        return Sum.FromRange(res);
    }

    public override string FullForm() =>
        String.Join(" + ", elts.ConvertAll(elt => elt.Precedence() < Precedence() ? $"({elt})" : $"{elt}"));

    public override string StandardForm()
    {
        var result = string.Join(" ",
            elts
                .ConvertAll(elt =>
                {
                    var elt_ = elt.Const() < 0 ? elt * -1 : elt;

                    var elt__ = elt.Const() < 0 && elt_ is Sum || (elt is Power && (elt as Power).exp != new Integer(1) / 2) ? $"({elt_})" : $"{elt_}";

                    return elt.Const() < 0 ? $"- {elt__}" : $"+ {elt__}";
                }));

        if (result.StartsWith("+ ")) return result.Remove(0, 2); // "+ x + y"   ->   "x + y"

        if (result.StartsWith("- ")) return result.Remove(1, 1); // "- x + y"   ->   "-x + y"

        return result;
    }

    public MathObject Map(Func<MathObject, MathObject> proc) =>
        Sum.FromRange(elts.Select(proc)).Simplify();
}

class Difference(params MathObject[] ls) : MathObject
{
    public readonly ImmutableList<MathObject> elts = ImmutableList.Create(ls);

    public MathObject Simplify()
    {
        if (elts.Count == 1) return -1 * elts[0];

        if (elts.Count == 2) return elts[0] + -1 * elts[1];

        throw new Exception();
    }
}

class Quotient(params MathObject[] ls) : MathObject
{
    public readonly ImmutableList<MathObject> elts = ImmutableList.Create(ls);

    public MathObject Simplify() => elts[0] * (elts[1] ^ -1);
}

public static class Constructors
{
    public static MathObject Sqrt(MathObject obj) => obj ^ (new Integer(1) / new Integer(2));

    public static MathObject And(params MathObject[] ls) => Symbolism.And.FromRange(ls).Simplify();

    public static MathObject Or(params MathObject[] ls) => Symbolism.Or.FromRange(ls).Simplify();
}
