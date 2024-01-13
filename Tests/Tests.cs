﻿/* Copyright 2013 Eduardo Cavazos

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
using System.Linq;
using Xunit;

using Symbolism;
using Symbolism.Has;
using Symbolism.Substitute;
using Symbolism.SimplifyLogical;
using Symbolism.LogicalExpand;
using Symbolism.SimplifyEquation;
using Symbolism.DegreeGpe;
using Symbolism.CoefficientGpe;
using Symbolism.AlgebraicExpand;
using Symbolism.IsolateVariable;
using Symbolism.EliminateVariable;
using Symbolism.RationalExpand;
using Symbolism.LeadingCoefficientGpe;
using Symbolism.Trigonometric;
using Symbolism.DeepSelect;
using Symbolism.RationalizeExpression;

using static Symbolism.Constructors;
using static Symbolism.Trigonometric.Constructors;

using static Symbolism.PolynomialDivision.Extensions;
using static Symbolism.PolynomialGcd.Extensions;

namespace SymbolismTests;

public static class Extensions
{
    // public static void AssertEqTo(this MathObject a, MathObject b) => Assert.True(a == b);

    public static MathObject AssertEqTo(this MathObject a, MathObject b)
    {
        Assert.True(a == b);

        return a;
    }

    public static MathObject MultiplyBothSidesBy(this MathObject obj, MathObject item) =>
        //if (obj is Equation)
        //    return (obj as Equation).a * item == (obj as Equation).b * item;

        obj switch
        {
            Equation e => new Equation(
                e.a * item,
                e.b * item,
                e.Operator),
            And a => a.Map(elt => elt.MultiplyBothSidesBy(item)),
            _ => throw new Exception()
        };

    public static MathObject AddToBothSides(this MathObject obj, MathObject item) 
        => obj is Equation ? (MathObject)((obj as Equation).a + item == (obj as Equation).b + item) : throw new Exception();
}

public class Obj2(string name)
{
    public Symbol ΣFx = new($"{name}.ΣFx");
    public Symbol ΣFy = new($"{name}.ΣFy");
    public Symbol m = new($"{name}.m");
    public Symbol ax = new($"{name}.ax");
    public Symbol ay = new($"{name}.ay");

    public Symbol F1 = new($"{name}.F1"), F2 = new ($"{name}.F2");
    public Symbol th1 = new($"{name}.th1"), th2 = new ($"{name}.th2");
    public Symbol F1x = new($"{name}.F1x"), F2x = new ($"{name}.F2x");
    public Symbol F1y = new($"{name}.F1y"), F2y = new ($"{name}.F2y");

    public And Equations() => new(

            F1x == F1 * Cos(th1),
            F1y == F1 * Sin(th1),

            F2x == F2 * Cos(th2),
            F2y == F2 * Sin(th2),

            ΣFx == F1x + F2x,
            ΣFx == m * ax,

            ΣFy == F1y + F2y,
            ΣFy == m * ay

            );
}

public class Obj3(string name)
{
    public Symbol ΣFx = new($"{name}.ΣFx");
    public Symbol ΣFy = new ($"{name}.ΣFy");
    public Symbol m = new ($"{name}.m");
    public Symbol ax = new ($"{name}.ax");
    public Symbol ay = new ($"{name}.ay");

    public Symbol F1 = new ($"{name}.F1"), F2 = new ($"{name}.F2"), F3 = new ($"{name}.F3");
    public Symbol th1 = new ($"{name}.th1"), th2 = new ($"{name}.th2"), th3 = new ($"{name}.th3");
    public Symbol F1x = new ($"{name}.F1x"), F2x = new ($"{name}.F2x"), F3x = new ($"{name}.F3x");
    public Symbol F1y = new ($"{name}.F1y"), F2y = new ($"{name}.F2y"), F3y = new ($"{name}.F3y");

    public And Equations() => new (

            F1x == F1 * Cos(th1),
            F1y == F1 * Sin(th1),

            F2x == F2 * Cos(th2),
            F2y == F2 * Sin(th2),

            F3x == F3 * Cos(th3),
            F3y == F3 * Sin(th3),

            ΣFx == F1x + F2x + F3x,
            ΣFx == m * ax,

            ΣFy == F1y + F2y + F3y,
            ΣFy == m * ay

            );
}

public class Obj5(string name)
{
    public Symbol ΣFx = new ($"{name}.ΣFx");
    public Symbol ΣFy = new ($"{name}.ΣFy");
    public Symbol m = new ($"{name}.m");
    public Symbol ax = new ($"{name}.ax");
    public Symbol ay = new ($"{name}.ay");

    public Symbol F1 = new ($"{name}.F1"), F2 = new ($"{name}.F2"), F3 = new ($"{name}.F3"), F4 = new ($"{name}.F4"), F5 = new ($"{name}.F5");
    public Symbol th1 = new ($"{name}.th1"), th2 = new ($"{name}.th2"), th3 = new ($"{name}.th3"), th4 = new ($"{name}.th4"), th5 = new ($"{name}.th5");
    public Symbol F1x = new ($"{name}.F1x"), F2x = new ($"{name}.F2x"), F3x = new ($"{name}.F3x"), F4x = new ($"{name}.F4x"), F5x = new ($"{name}.F5x");
    public Symbol F1y = new ($"{name}.F1y"), F2y = new ($"{name}.F2y"), F3y = new ($"{name}.F3y"), F4y = new ($"{name}.F4y"), F5y = new ($"{name}.F5y");

    public And Equations() => new (

            F1x == F1 * Cos(th1),
            F1y == F1 * Sin(th1),

            F2x == F2 * Cos(th2),
            F2y == F2 * Sin(th2),

            F3x == F3 * Cos(th3),
            F3y == F3 * Sin(th3),

            F4x == F4 * Cos(th4),
            F4y == F4 * Sin(th4),

            F5x == F5 * Cos(th5),
            F5y == F5 * Sin(th5),

            ΣFx == F1x + F2x + F3x + F4x + F5x,
            ΣFx == m * ax,

            ΣFy == F1y + F2y + F3y + F4y + F5y,
            ΣFy == m * ay

            );
}

public class KinematicObjectABC(string name)
{
    public Symbol xA = new ($"{name}.xA"), yA = new ($"{name}.yA"), vxA = new ($"{name}.vxA"), vyA = new ($"{name}.vyA"), vA = new ($"{name}.vA"), thA = new ($"{name}.thA");
    public Symbol xB = new ($"{name}.xB"), yB = new ($"{name}.yB"), vxB = new ($"{name}.vxB"), vyB = new ($"{name}.vyB"), vB = new ($"{name}.vB"), thB = new ($"{name}.thB");
    public Symbol xC = new ($"{name}.xC"), yC = new ($"{name}.yC"), vxC = new ($"{name}.vxC"), vyC = new ($"{name}.vyC"), vC = new ($"{name}.vC"), thC = new ($"{name}.thC");

    public Symbol tAB = new ($"{name}.tAB"), tBC = new ($"{name}.tBC"), tAC = new ($"{name}.tAC");

    public Symbol ax = new ($"{name}.ax"), ay = new ($"{name}.ay");

    public And EquationsAB() => new(

            vxB == vxA + ax * tAB,
            vyB == vyA + ay * tAB,

            xB == xA + vxA * tAB + ax * (tAB ^ 2) / 2,
            yB == yA + vyA * tAB + ay * (tAB ^ 2) / 2

            );

    public And EquationsBC() => new (

            vxC == vxB + ax * tBC,
            vyC == vyB + ay * tBC,

            xC == xB + vxB * tBC + ax * (tBC ^ 2) / 2,
            yC == yB + vyB * tBC + ay * (tBC ^ 2) / 2

            );

    public And EquationsAC() =>
        new (

            vxC == vxA + ax * tAC,
            vyC == vyA + ay * tAC,

            xC == xA + vxA * tAC + ax * (tAC ^ 2) / 2,
            yC == yA + vyA * tAC + ay * (tAC ^ 2) / 2

            );

    public And TrigEquationsA() =>
        new (

            vxA == vA * Cos(thA),
            vyA == vA * Sin(thA)

            );

}

public class Tests
{
    readonly Symbol a = new ("a");
    readonly Symbol b = new ("b");
    readonly Symbol c = new ("c");
    readonly Symbol d = new ("d");
    readonly Symbol w = new ("w");

    readonly Symbol x = new ("x");
    readonly Symbol y = new ("y");
    readonly Symbol z = new ("z");

    static Integer Int(int n) => new(n);

    #region
    [Fact] public void Test1() => Assert.True(new DoubleFloat(1.2).Equals(new DoubleFloat(1.2)));
    [Fact] public void Test2() => Assert.False(new DoubleFloat(1.20000001).Equals(new DoubleFloat(1.20000002)));

    [Fact]
    public void Test3()
    {
        MathObject.DoubleFloatTolerance = 0.000000001;

        Assert.True(new DoubleFloat(1.2000000000001).Equals(new DoubleFloat(1.200000000002)));

        MathObject.DoubleFloatTolerance = null;
    }

    [Fact] public void Test4() => Assert.False(new DoubleFloat(1.2).Equals(new DoubleFloat(1.23)));
    #endregion

    #region Const

    [Fact] public void Test5() => Assert.Equal(2, (2 * x * y).Const());
    [Fact] public void Test6() => Assert.True((x * y / 2).Const() == new Integer(1) / 2);
    [Fact] public void Test7() => Assert.Equal(0.1, (0.1 * x * y).Const());
    [Fact] public void Test8() => Assert.Equal(1, (x * y).Const());

    #endregion

    #region Simplify

    [Fact] public void Test9() => Assert.True(x + x == 2 * x);
    [Fact] public void Test10() => Assert.True(x + x + x == 3 * x);
    [Fact] public void Test11() => Assert.True(5 + x + 2 == 7 + x);
    [Fact] public void Test12() => Assert.True(3 + x + 5 + x == 8 + 2 * x);
    [Fact] public void Test13() => Assert.True(4 * x + 3 * x == 7 * x);
    [Fact] public void Test14() => Assert.True(x + y + z + x + y + z == 2 * x + 2 * y + 2 * z);
    [Fact] public void Test15() => Assert.True(10 - x == 10 + x * -1);
    [Fact] public void Test16() => Assert.True(x * y / 3 == Int(1) / 3 * x * y);
    [Fact] public void Test17() => Assert.True(x / y == x * (y ^ -1));
    [Fact] public void Test18() => Assert.True(x / 3 == x * (Int(1) / 3));
    [Fact] public void Test19() => Assert.True(6 * x * y / 3 == 2 * x * y);
    [Fact] public void Test20() => Assert.True((((x ^ Int(1) / 2) ^ Int(1) / 2) ^ 8) == (x ^ 2));
    [Fact] public void Test21() => Assert.True(((((x * y) ^ (Int(1) / 2)) * (z ^ 2)) ^ 2) == (x * y * (z ^ 4)));
    [Fact] public void Test22() => Assert.Equal(1, x / x);
    [Fact] public void Test23() => Assert.Equal(1, x / y * y / x);
    [Fact] public void Test24() => Assert.True((x ^ 2) * (x ^ 3) == (x ^ 5));
    [Fact] public void Test25() => Assert.True(x + y + x + z + 5 + z == 5 + 2 * x + y + 2 * z);
    [Fact] public void Test26() => Assert.True(((Int(1) / 2) * x + (Int(3) / 4) * x) == Int(5) / 4 * x);
    [Fact] public void Test27() => Assert.True(1.2 * x + 3 * x == 4.2 * x);
    [Fact] public void Test28() => Assert.True(3 * x + 1.2 * x == 4.2 * x);
    [Fact] public void Test29() => Assert.True(1.2 * x * 3 * y == 3.5999999999999996 * x * y);
    [Fact] public void Test30() => Assert.True(3 * x * 1.2 * y == 3.5999999999999996 * x * y);
    [Fact] public void Test31() => Assert.True(3.4 * x * 1.2 * y == 4.08 * x * y);
    [Fact] public void Test32() => Assert.True((a == b) == (a == b));

    #endregion

    #region Power.Simplify
    [Fact] public void Test33() => Assert.Equal(0, (0 ^ x));
    [Fact] public void Test34() => Assert.Equal(1, (1 ^ x));
    [Fact] public void Test35() => Assert.Equal(1, (x ^ 0));
    [Fact] public void Test36() => Assert.True((x ^ 1) == x);
    #endregion

    // Product.Simplify
    [Fact] public void Test37() => Assert.Equal(0, x * 0);

    // Difference
    [Fact] public void Test38() => Assert.True(-x == -1 * x);
    [Fact] public void Test39() => Assert.True(x - y == x + -1 * y);

    #region Substitute

    [Fact] public void Test40() => Assert.Equal(20, Int(10).Substitute(Int(10), 20));
    [Fact] public void Test41() => Assert.Equal(10, Int(10).Substitute(Int(15), 20));

    [Fact] public void Test42() => Assert.Equal(2.0, new DoubleFloat(1.0).Substitute(new DoubleFloat(1.0), 2.0));
    [Fact] public void Test43() => Assert.Equal(1.0, new DoubleFloat(1.0).Substitute(new DoubleFloat(1.5), 2.0));

    [Fact] public void Test44() => Assert.True((Int(1) / 2).Substitute(Int(1) / 2, Int(3) / 4) == Int(3) / 4);
    [Fact] public void Test45() => Assert.True((Int(1) / 2).Substitute(Int(1) / 3, Int(3) / 4) == Int(1) / 2);

    [Fact] public void Test46() => Assert.True(x.Substitute(x, y) == y);
    [Fact] public void Test47() => Assert.True(x.Substitute(y, y) == x);

    [Fact] public void Test48() => Assert.True((x ^ y).Substitute(x, 10) == (10 ^ y));
    [Fact] public void Test49() => Assert.True((x ^ y).Substitute(y, 10) == (x ^ 10));

    [Fact] public void Test50() => Assert.Equal(10, (x ^ y).Substitute(x ^ y, 10));

    [Fact] public void Test51() => Assert.True((x * y * z).Substitute(x, y) == ((y ^ 2) * z));
    [Fact] public void Test52() => Assert.True((x * y * z).Substitute(x * y * z, x) == x);

    [Fact] public void Test53() => Assert.True((x + y + z).Substitute(x, y) == ((y * 2) + z));
    [Fact] public void Test54() => Assert.True((x + y + z).Substitute(x + y + z, x) == x);

    [Fact] public void Test55() => Assert.Equal(16200, ((((x * y) ^ (Int(1) / 2)) * (z ^ 2)) ^ 2).Substitute(x, 10).Substitute(y, 20).Substitute(z, 3));

    #region Equation.Substitute

    [Fact] public void Test56() => Assert.True((x == y).Substitute(y, z) == (x == z));

    [Fact] public void Test57() => Assert.True((x != y).Substitute(y, z) == (x != z));

    [Fact] public void Test58() => (x == 0).Substitute(x, 0).AssertEqTo(true);
    [Fact] public void Test59() => (x == 0).Substitute(x, 1).AssertEqTo(false);
    [Fact] public void Test60() => (x != 0).Substitute(x, 0).AssertEqTo(false);
    [Fact] public void Test61() => (x != 0).Substitute(x, 1).AssertEqTo(true);

    #endregion

    #endregion

    [Fact] public void Test62() => Assert.Equal(0.99999999999911982, Sin(new DoubleFloat(3.14159 / 2)));

    [Fact] public void Test63() => Assert.True(Sin(x + y) + Sin(x + y) == 2 * Sin(x + y));

    [Fact] public void Test64() => Assert.True(Sin(x + x) == Sin(2 * x));

    [Fact] public void Test65() => Assert.True(Sin(x + x).Substitute(x, 1) == Sin(Int(2)));

    [Fact] public void Test66() => Assert.Equal(0.90929742682568171, Sin(x + x).Substitute(x, 1.0));

    [Fact] public void Test67() => Assert.True(Sin(2 * x).Substitute(x, y) == Sin(2 * y));

    // Product.RecursiveSimplify

    [Fact] public void Test68() => Assert.True(1 * x == x);

    [Fact] public void Test69() => Assert.True(x * 1 == x);

    [Fact] public void Test70() => Assert.True(x != y);

    [Fact] public void Test71() => Assert.NotEqual("10", x.ToString());

    // ==(double a, MathObject b)

    [Fact] public void Test72() => Assert.Equal(1.0, new DoubleFloat(3.0) - 2.0);

    [Fact] public void Test73() => Assert.True((a == b) != (a != b));

    [Fact] public void Test74() => (Sqrt(a * b) * (Sqrt(a * b) / a) / c).AssertEqTo(b / c);

    static void AssertToStringMatch(MathObject obj, string str) => Assert.True(obj.ToString() == str);

    [Fact]
    public void Test75()
    {
        //MathObject.ToStringForm = MathObject.ToStringForms.Full;

        AssertToStringMatch(x + y + z, "x + y + z");

        AssertToStringMatch(x + y * z, "x + y * z");

        AssertToStringMatch((x + y) * z, "(x + y) * z");

        Assert.True((Sin(x) * Cos(y)).ToString() == "Cos(y) * Sin(x)", "(Sin(x) * Cos(y)).ToString()");

        AssertToStringMatch(And(x, y, z), "and(x, y, z)");

        AssertToStringMatch(x ^ y, "x ^ y");

        AssertToStringMatch((x * y) ^ (x + z), "(x * y) ^ (x + z)");

        Assert.True((x - y).ToString() == "x + -1 * y", "(x - y).ToString()");

        Assert.True((x - y - z).ToString() == "x + -1 * y + -1 * z", "(x - y - z).ToString()");

        Assert.True((x / y).ToString() == "x * y ^ -1", "(x / y).ToString()");

        Assert.True((x - (y - z)).ToString() == "x + -1 * (y + -1 * z)", "(x - (y - z)).ToString()");
    }

    [Fact]
    public void Test76()
    {
        //MathObject.ToStringForm = MathObject.ToStringForms.Standard;

        Assert.True((x + y).ToString() == "x + y", "(x + y).ToString()");

        Assert.True((x - y).ToString() == "x - y", "(x - y).ToString()");

        Assert.True((x - y - z).ToString() == "x - y - z", "(x - y - z).ToString()");

        Assert.True((-x - y - z).ToString() == "-x - y - z", "(x - y - z).ToString()");

        Assert.True((2 * x - 3 * y - 4 * z).ToString() == "2 * x - 3 * y - 4 * z", "(2 * x - 3 * y - 4 * z).ToString()");

        Assert.True((x - (y - z)).ToString() == "x - (y - z)", "(x - (y - z)).ToString()");

        Assert.True((x - y + z).ToString() == "x - y + z", "(x - y + z).ToString()");

        Assert.True((-x).ToString() == "-x", "(-x).ToString()");

        Assert.True((x / y).ToString() == "x / y", "(x / y).ToString()");

        Assert.True((x / (y + z)).ToString() == "x / (y + z)", "(x / (y + z)).ToString()");

        Assert.True(((x + y) / (x + z)).ToString() == "(x + y) / (x + z)", "((x + y) / (x + z)).ToString()");

        Assert.True((-x * y).ToString() == "-x * y", "(-x * y).ToString()");

        Assert.True((x * -y).ToString() == "-x * y", "(x * -y).ToString()");


        Assert.True(Sin(x / y).ToString() == "Sin(x / y)", "Sin(x / y).ToString()");

        Assert.True(
            (x == -Sqrt(2 * y * (-z * a + y * (b ^ 2) / 2 - c * y * d + c * y * z * Sin(x))) / y).ToString() ==
            "x == -sqrt(2 * y * ((b ^ 2) * y / 2 - c * d * y - a * z + c * Sin(x) * y * z)) / y",
            "(x == -sqrt(2 * y * (-z * a + y * (b ^ 2) / 2 - c * y * d + c * y * z * Sin(x))) / y).ToString()");

        Assert.True((x * (y ^ z)).ToString() == "x * (y ^ z)", "(x * (y ^ z)).ToString()");

        Assert.True((x + (y ^ z)).ToString() == "x + (y ^ z)", "((x + (y ^ z)).ToString()");

        Assert.True(Sqrt(x).ToString() == "sqrt(x)", "sqrt(x).ToString()");

        Assert.True(Sqrt(x).FullForm().ToString() == "x ^ 1/2", "sqrt(x).FullForm()");

        Assert.True((x ^ (new Integer(1) / 3)).ToString() == "x ^ 1/3", "(x ^ (new Integer(1) / 3)).ToString()");

        Assert.True(And(And(x, y), And(x, z)).SimplifyLogical().ToString() == "and(x, y, z)",
            "and(and(x, y), and(x, z)).SimplifyLogical().ToString()");

        AssertToStringMatch(x == Sqrt(2 * (y * z - Cos(a) * y * z)), "x == sqrt(2 * (y * z - Cos(a) * y * z))");

        AssertToStringMatch(
             a == (-c * Cos(d) - b * c * Sin(d) + x * y + b * x * z) / (-y - z),
            "a == (-c * Cos(d) - b * c * Sin(d) + x * y + b * x * z) / (-y - z)");

        AssertToStringMatch(
             x == -(Sin(y) / Cos(y) + Sqrt((Sin(y) ^ 2) / (Cos(y) ^ 2))) * (z ^ 2) / a,
            "x == -(Sin(y) / Cos(y) + sqrt((Sin(y) ^ 2) / (Cos(y) ^ 2))) * (z ^ 2) / a");

        AssertToStringMatch(x * Sqrt(y), "x * sqrt(y)");

        AssertToStringMatch(x / Sqrt(y), "x / sqrt(y)");

        AssertToStringMatch(Sqrt(y) / x, "sqrt(y) / x");

        AssertToStringMatch((x ^ 2) / (y ^ 3), "(x ^ 2) / (y ^ 3)");

        AssertToStringMatch(
             x == y * Sqrt(-8 * a / (y * (z ^ 2))) * (z ^ 2) / (4 * a),
            "x == y * sqrt(-8 * a / (y * (z ^ 2))) * (z ^ 2) / (4 * a)");

        AssertToStringMatch(-(-1 + x), "-(-1 + x)");
    }

    #region Equation.ToString

    [Fact] public void Test77() => Assert.True((x == y).ToString() == "x == y", "x == y");

    [Fact] public void Test78() => Assert.True((x != y).ToString() == "x != y", "x != y");

    #endregion

    #region Function.ToString
    [Fact] public void Test79() => Assert.True(new And().ToString() == "and()", "and()");

    #endregion

    #region Equation.Simplify

    [Fact] public void Test80() => (new Integer(0) == new Integer(0)).Simplify().AssertEqTo(true);
    [Fact] public void Test81() => (new Integer(0) == new Integer(1)).Simplify().AssertEqTo(false);
    [Fact] public void Test82() => (new Integer(0) != new Integer(1)).Simplify().AssertEqTo(true);
    [Fact] public void Test83() => (new Integer(0) != new Integer(0)).Simplify().AssertEqTo(false);

    #endregion

    #region And
    [Fact] public void Test84() => And().AssertEqTo(true);

    [Fact] public void Test85() => And(10).AssertEqTo(10);

    [Fact] public void Test86() => And(true).AssertEqTo(true);

    [Fact] public void Test87() => And(false).AssertEqTo(false);

    [Fact] public void Test88() => And(10, 20, 30).AssertEqTo(And(10, 20, 30));

    [Fact] public void Test89() => And(10, false, 20).AssertEqTo(false);

    [Fact] public void Test90() => And(10, true, 20).AssertEqTo(And(10, 20));

    [Fact] public void Test91() => And(10, And(20, 30), 40).AssertEqTo(And(10, 20, 30, 40));
    #endregion

    #region Or

    [Fact] public void Test92() => Or(10).AssertEqTo(10);

    [Fact] public void Test93() => Or(true).AssertEqTo(true);

    [Fact] public void Test94() => Or(false).AssertEqTo(false);

    [Fact] public void Test95() => Or(10, 20, false).AssertEqTo(Or(10, 20));

    [Fact] public void Test96() => Or(false, false).AssertEqTo(false);

    [Fact] public void Test97() => Or(10, true, 20, false).AssertEqTo(true);

    [Fact] public void Test98() => Or(10, false, 20).AssertEqTo(Or(10, 20));

    [Fact] public void Test99() => Or(10, Or(20, 30), 40).AssertEqTo(Or(10, 20, 30, 40));

    #endregion

    #region Function.Map

    [Fact]
    public void Test100() => new And(1, 2, 3, 4, 5, 6).Map(elt => elt * 2)
        .AssertEqTo(And(2, 4, 6, 8, 10, 12));


    [Fact]
    public void Test101() => new And(1, 2, 3, 4, 5, 6).Map(elt => (elt is Integer) && (elt as Integer).val % 2 == 0 ? elt : false)
        .AssertEqTo(false);

    [Fact]
    public void Test102() => new Or(1, 2, 3).Map(elt => elt * 2)
        .AssertEqTo(Or(2, 4, 6));

    [Fact]
    public void Test103() => new Or(1, 2, 3, 4, 5, 6).Map(elt => (elt is Integer) && (elt as Integer).val % 2 == 0 ? elt : false)
        .AssertEqTo(Or(2, 4, 6));

    #endregion Function.Map

    #region Sum

    [Fact]
    public void Test104() => Assert.False((x + y).Equals(x * y), "(x + y).Equals(x * y)");

    #endregion

    [Fact]
    public void Test105()
    {
        (x < y).Substitute(x, 10).Substitute(y, 20).AssertEqTo(true);
        (x > y).Substitute(x, 10).Substitute(y, 20).AssertEqTo(false);
    }

    readonly Symbol Pi = new ("Pi");

    readonly MathObject half = new Integer(1) / 2;

    #region Sin

    [Fact] public void Test106() => Sin(0).AssertEqTo(0);

    [Fact] public void Test107() => Sin(Pi).AssertEqTo(0);

    [Fact] public void Test108() => Sin(-10).AssertEqTo(-Sin(10));

    [Fact] public void Test109() => Sin(-x).AssertEqTo(-Sin(x));

    [Fact] public void Test110() => Sin(-5 * x).AssertEqTo(-Sin(5 * x));

    // Sin(k/n pi) for n = 1 2 3 4 6

    [Fact] public void Test111() => Sin(-2 * Pi).AssertEqTo(0);
    [Fact] public void Test112() => Sin(-1 * Pi).AssertEqTo(0);
    [Fact] public void Test113() => Sin(2 * Pi).AssertEqTo(0);
    [Fact] public void Test114() => Sin(3 * Pi).AssertEqTo(0);

    [Fact] public void Test115() => Sin(-7 * Pi / 2).AssertEqTo(1);
    [Fact] public void Test116() => Sin(-5 * Pi / 2).AssertEqTo(-1);
    [Fact] public void Test117() => Sin(-3 * Pi / 2).AssertEqTo(1);
    [Fact] public void Test118() => Sin(-1 * Pi / 2).AssertEqTo(-1);
    [Fact] public void Test119() => Sin(1 * Pi / 2).AssertEqTo(1);
    [Fact] public void Test120() => Sin(3 * Pi / 2).AssertEqTo(-1);
    [Fact] public void Test121() => Sin(5 * Pi / 2).AssertEqTo(1);
    [Fact] public void Test122() => Sin(7 * Pi / 2).AssertEqTo(-1);

    [Fact] public void Test123() => Sin(-4 * Pi / 3).AssertEqTo(Sqrt(3) / 2);
    [Fact] public void Test124() => Sin(-2 * Pi / 3).AssertEqTo(-Sqrt(3) / 2);
    [Fact] public void Test125() => Sin(-1 * Pi / 3).AssertEqTo(-Sqrt(3) / 2);
    [Fact] public void Test126() => Sin(1 * Pi / 3).AssertEqTo(Sqrt(3) / 2);
    [Fact] public void Test127() => Sin(2 * Pi / 3).AssertEqTo(Sqrt(3) / 2);
    [Fact] public void Test128() => Sin(4 * Pi / 3).AssertEqTo(-Sqrt(3) / 2);
    [Fact] public void Test129() => Sin(5 * Pi / 3).AssertEqTo(-Sqrt(3) / 2);
    [Fact] public void Test130() => Sin(7 * Pi / 3).AssertEqTo(Sqrt(3) / 2);

    [Fact] public void Test131() => Sin(-3 * Pi / 4).AssertEqTo(-1 / Sqrt(2));
    [Fact] public void Test132() => Sin(-1 * Pi / 4).AssertEqTo(-1 / Sqrt(2));
    [Fact] public void Test133() => Sin(1 * Pi / 4).AssertEqTo(1 / Sqrt(2));
    [Fact] public void Test134() => Sin(3 * Pi / 4).AssertEqTo(1 / Sqrt(2));
    [Fact] public void Test135() => Sin(5 * Pi / 4).AssertEqTo(-1 / Sqrt(2));
    [Fact] public void Test136() => Sin(7 * Pi / 4).AssertEqTo(-1 / Sqrt(2));
    [Fact] public void Test137() => Sin(9 * Pi / 4).AssertEqTo(1 / Sqrt(2));
    [Fact] public void Test138() => Sin(11 * Pi / 4).AssertEqTo(1 / Sqrt(2));

    [Fact] public void Test139() => Sin(-5 * Pi / 6).AssertEqTo(-half);
    [Fact] public void Test140() => Sin(-1 * Pi / 6).AssertEqTo(-half);
    [Fact] public void Test141() => Sin(1 * Pi / 6).AssertEqTo(half);
    [Fact] public void Test142() => Sin(5 * Pi / 6).AssertEqTo(half);
    [Fact] public void Test143() => Sin(7 * Pi / 6).AssertEqTo(-half);
    [Fact] public void Test144() => Sin(11 * Pi / 6).AssertEqTo(-half);
    [Fact] public void Test145() => Sin(13 * Pi / 6).AssertEqTo(half);
    [Fact] public void Test146() => Sin(17 * Pi / 6).AssertEqTo(half);

    // Sin(a/b pi) where a/b > 1/2 (i.e. not in first quadrant)

    [Fact] public void Test147() => Sin(15 * Pi / 7).AssertEqTo(Sin(1 * Pi / 7));
    [Fact] public void Test148() => Sin(8 * Pi / 7).AssertEqTo(-Sin(1 * Pi / 7));
    [Fact] public void Test149() => Sin(4 * Pi / 7).AssertEqTo(Sin(3 * Pi / 7));

    // Sin( a + b + ... + n * pi ) where abs(n) >= 2

    [Fact] public void Test150() => Sin(x - 3 * Pi).AssertEqTo(Sin(x + Pi));
    [Fact] public void Test151() => Sin(x - 2 * Pi).AssertEqTo(Sin(x));
    [Fact] public void Test152() => Sin(x + 2 * Pi).AssertEqTo(Sin(x));
    [Fact] public void Test153() => Sin(x + 3 * Pi).AssertEqTo(Sin(x + Pi));
    [Fact] public void Test154() => Sin(x + 7 * Pi / 2).AssertEqTo(Sin(x + 3 * Pi / 2));

    // Sin( a + b + ... + n/2 * pi )

    [Fact] public void Test155() => Sin(x - 3 * Pi / 2).AssertEqTo(Cos(x));
    [Fact] public void Test156() => Sin(x - 1 * Pi / 2).AssertEqTo(-Cos(x));
    [Fact] public void Test157() => Sin(x + 1 * Pi / 2).AssertEqTo(Cos(x));
    [Fact] public void Test158() => Sin(x + 3 * Pi / 2).AssertEqTo(-Cos(x));

    [Fact] public void Test159() => Sin(Pi + x).AssertEqTo(-Sin(x));

    [Fact] public void Test160() => Sin(Pi + x + y).AssertEqTo(-Sin(x + y));


    [Fact] public void Test161() => Cos(Pi + x).AssertEqTo(-Cos(x));

    [Fact] public void Test162() => Cos(Pi + x + y).AssertEqTo(-Cos(x + y));

    #endregion

    #region Cos

    [Fact] public void Test163() => Cos(0).AssertEqTo(1);

    [Fact] public void Test164() => Cos(Pi).AssertEqTo(-1);

    [Fact] public void Test165() => Cos(-10).AssertEqTo(Cos(10));

    [Fact] public void Test166() => Cos(-10 * x).AssertEqTo(Cos(10 * x));

    [Fact] public void Test167() => Cos(3 * Pi).AssertEqTo(-1);

    [Fact] public void Test168() => Cos(2 * Pi * 3 / 4).AssertEqTo(0);

    // Cos( a + b + ... + n * pi ) where abs(n) >= 2

    [Fact] public void Test169() => Cos(x - 3 * Pi).AssertEqTo(Cos(x + Pi));
    [Fact] public void Test170() => Cos(x + 3 * Pi).AssertEqTo(Cos(x + Pi));

    [Fact] public void Test171() => Cos(x - 2 * Pi).AssertEqTo(Cos(x));
    [Fact] public void Test172() => Cos(x + 2 * Pi).AssertEqTo(Cos(x));

    [Fact] public void Test173() => Cos(x + Pi * 7 / 2).AssertEqTo(Cos(x + Pi * 3 / 2));

    // Cos( a + b + ... + n/2 * pi )

    [Fact] public void Test174() => Cos(x - Pi * 3 / 2).AssertEqTo(-Sin(x));
    [Fact] public void Test175() => Cos(x - Pi * 1 / 2).AssertEqTo(Sin(x));
    [Fact] public void Test176() => Cos(x + Pi * 1 / 2).AssertEqTo(-Sin(x));
    [Fact] public void Test177() => Cos(x + Pi * 3 / 2).AssertEqTo(Sin(x));

    #endregion

    #region Has

    [Fact] public void Test178() => Assert.True(a.Has(elt => elt == a), "a.Has(elt => elt == a)");

    [Fact] public void Test179() => Assert.False(a.Has(elt => elt == b), "a.Has(elt => elt == b) == false");

    [Fact] public void Test180() => Assert.True((a == b).Has(elt => elt == a), "Has - 3");

    [Fact] public void Test181() => Assert.False((a == b).Has(elt => elt == c), "Has - 4");

    [Fact] public void Test182() => Assert.True(((a + b) ^ c).Has(elt => elt == a + b), "Has - 5");

    [Fact] public void Test183() => Assert.True(((a + b) ^ c).Has(elt => (elt is Power) && (elt as Power).exp == c), "Has - 6");

    [Fact] public void Test184() => Assert.True((x * (a + b + c)).Has(elt => (elt is Sum) && (elt as Sum).Has(b)), "Has - 7");

    [Fact] public void Test185() => Assert.True((x * (a + b + c)).Has(elt => (elt is Sum) && (elt as Sum).elts.Any(obj => obj == b)), "Has - 8");

    [Fact] public void Test186() => Assert.False((x * (a + b + c)).Has(elt => (elt is Product) && (elt as Product).elts.Any(obj => obj == b)), "Has - 9");

    #endregion

    #region FreeOf

    [Fact] public void Test187() => Assert.False((a + b).FreeOf(b), "(a + b).FreeOf(b)");
    [Fact] public void Test188() => Assert.True((a + b).FreeOf(c), "(a + b).FreeOf(c)");
    [Fact] public void Test189() => Assert.False(((a + b) * c).FreeOf(a + b), "((a + b) * c).FreeOf(a + b)");
    [Fact] public void Test190() => Assert.False((Sin(x) + 2 * x).FreeOf(Sin(x)), "(Sin(x) + 2 * x).FreeOf(Sin(x))");
    [Fact] public void Test191() => Assert.True(((a + b + c) * d).FreeOf(a + b), "((a + b + c) * d).FreeOf(a + b)");
    [Fact] public void Test192() => Assert.True(((y + 2 * x - y) / x).FreeOf(x), "((y + 2 * x - y) / x).FreeOf(x)");
    [Fact] public void Test193() => Assert.True(((x * y) ^ 2).FreeOf(x * y), "((x * y) ^ 2).FreeOf(x * y)");

    #endregion

    #region Numerator

    [Fact] public void Test194() => Assert.Equal(1, (x ^ -1).Numerator());

    [Fact] public void Test195() => Assert.Equal(1, (x ^ -half).Numerator());

    #endregion

    #region Denominator

    [Fact]
    public void Test196() =>
        ((new Integer(2) / 3) * ((x * (x + 1)) / (x + 2)) * (y ^ z)).Denominator().AssertEqTo(3 * (x + 2));

    #endregion

    #region LogicalExpand

    [Fact]
    public void Test197() =>
        And(Or(a, b), c).LogicalExpand()
        .AssertEqTo(
            Or(
                And(a, c),
                And(b, c)));

    [Fact]
    public void Test198() =>
            And(a, Or(b, c))
                .LogicalExpand()
                .AssertEqTo(Or(And(a, b), And(a, c)));

    [Fact]
    public void Test199() =>
            And(a, Or(b, c), d)
                .LogicalExpand()
                .AssertEqTo(
                    Or(
                        And(a, b, d),
                        And(a, c, d)));

    [Fact]
    public void Test200() =>
            And(Or(a == b, b == c), x == y)
                .LogicalExpand()
                .AssertEqTo(
                    Or(
                        And(a == b, x == y),
                        And(b == c, x == y)));

    [Fact]
    public void Test201() =>
            And(
                Or(a == b, b == c),
                Or(c == d, d == a),
                x == y)
                .LogicalExpand()
                .AssertEqTo(
                    Or(
                        And(a == b, c == d, x == y),
                        And(a == b, d == a, x == y),
                        And(b == c, c == d, x == y),
                        And(b == c, d == a, x == y)));

    #endregion

    #region SimplifyEquation

    [Fact]
    public void Test202() =>
    (2 * x == 0)
                .SimplifyEquation()
                .AssertEqTo(x == 0);

    [Fact]
    public void Test203() =>
    (2 * x != 0)
                .SimplifyEquation()
                .AssertEqTo(x != 0);

    [Fact]
    public void Test204() =>
    ((x ^ 2) == 0)
                .SimplifyEquation()
                .AssertEqTo(x == 0);

    #endregion

    #region SimplifyLogical

    [Fact]
    public void Test205() =>
    And(a, b, c, a)
                .SimplifyLogical()
                .AssertEqTo(And(a, b, c));

    #endregion SimplifyLogical

    #region DegreeGpe

    [Fact]
    public void Test206() => Assert.True(
        ((3 * w * x ^ 2) * (y ^ 3) * (z ^ 4)).DegreeGpe([x, z]) == 6,
        "((3 * w * x ^ 2) * (y ^ 3) * (z ^ 4)).DegreeGpe(new List<MathObject>() { x, z })");

    [Fact]
    public void Test207() => Assert.True(
        ((a * x ^ 2) + b * x + c).DegreeGpe([x]) == 2,
        "((a * x ^ 2) + b * x + c).DegreeGpe(new List<MathObject>() { x })");

    [Fact]
    public void Test208() => Assert.True(
        (a * (Sin(x) ^ 2) + b * Sin(x) + c).DegreeGpe([Sin(x)]) == 2,
        "(a * (Sin(x) ^ 2) + b * Sin(x) + c).DegreeGpe(new List<MathObject>() { Sin(x) })");

    [Fact]
    public void Test209() => Assert.True(
        (2 * (x ^ 2) * y * (z ^ 3) + w * x * (z ^ 6)).DegreeGpe([x, z]) == 7,
        "(2 * (x ^ 2) * y * (z ^ 3) + w * x * (z ^ 6)).DegreeGpe(new List<MathObject>() { x, z })");

    #endregion

    #region CoefficientGpe

    [Fact] public void Test210() => Assert.True((a * (x ^ 2) + b * x + c).CoefficientGpe(x, 2) == a);

    [Fact] public void Test211() => Assert.True((3 * x * (y ^ 2) + 5 * (x ^ 2) * y + 7 * x + 9).CoefficientGpe(x, 1) == 3 * (y ^ 2) + 7);

    [Fact] public void Test212() => Assert.Equal(0, (3 * x * (y ^ 2) + 5 * (x ^ 2) * y + 7 * x + 9).CoefficientGpe(x, 3));

    [Fact]
    public void Test213() => Assert.True(
        (3 * Sin(x) * (x ^ 2) + 2 * x + 4).CoefficientGpe(x, 2) == null,
        "(3 * Sin(x) * (x ^ 2) + 2 * x + 4).CoefficientGpe(x, 2) == null");

    #endregion

    #region AlgebraicExpand

    [Fact]
    public void Test214() => Assert.True(
        ((x + 2) * (x + 3) * (x + 4)).AlgebraicExpand()
        ==
        24 + 26 * x + 9 * (x ^ 2) + (x ^ 3));

    [Fact]
    public void Test215() => Assert.True(
        ((x + y + z) ^ 3).AlgebraicExpand()
        ==
        (x ^ 3) + (y ^ 3) + (z ^ 3) +
        3 * (x ^ 2) * y +
        3 * (y ^ 2) * x +
        3 * (x ^ 2) * z +
        3 * (y ^ 2) * z +
        3 * (z ^ 2) * x +
        3 * (z ^ 2) * y +
        6 * x * y * z);

    [Fact]
    public void Test216() => Assert.True(
        (((x + 1) ^ 2) + ((y + 1) ^ 2)).AlgebraicExpand()
        ==
        2 + 2 * x + (x ^ 2) + 2 * y + (y ^ 2));

    [Fact]
    public void Test217() => Assert.True(
        ((((x + 2) ^ 2) + 3) ^ 2).AlgebraicExpand()
        ==
        49 + 56 * x + 30 * (x ^ 2) + 8 * (x ^ 3) + (x ^ 4));

    [Fact]
    public void Test218() => Assert.True(
        Sin(x * (y + z)).AlgebraicExpand()
        ==
        Sin(x * y + x * z));


    [Fact]
    public void Test219() => Assert.True(
        (a * (b + c) == x * (y + z)).AlgebraicExpand()
        ==
        (a * b + a * c == x * y + x * z));

    [Fact]
    public void Test220() =>
        (5 * x * (500 / (x ^ 2) * (Sqrt(3.0) / 4) + 1) + 2 * (x ^ 2) + (Sqrt(3.0) / 2) * (x ^ 2))
        .AlgebraicExpand()
        .AssertEqTo(1082.5317547305483 / x + 5 * x + 2.8660254037844384 * (x ^ 2));

    #endregion

    #region IsolateVariable

    [Fact] public void Test221() => (x + y + z == 0).IsolateVariable(a).AssertEqTo(x + y + z == 0);

    // (x * a + x * b == 0).IsolateVariable(x).Disp();

    [Fact]
    public void Test222() => (x * (a + b) - x * a - x * b + x == c)
        .IsolateVariable(x)
        .AssertEqTo(x == c);

    [Fact]
    public void Test223() => And(x == y, a == b)
                .IsolateVariable(b)
                .AssertEqTo(And(x == y, b == a));

    [Fact]
    public void Test224() => Or(And(y == x, z == x), And(b == x, c == x))
                .IsolateVariable(x)
                .AssertEqTo(Or(And(x == y, x == z), And(x == b, x == c)));

    [Fact] public void Test225() => Assert.True((0 == x - y).IsolateVariableEq(x).Equals(x == y), "(0 == x - y).IsolateVariable(x).Equals(x == y)");



    [Fact]
    public void Test226() => (a * (x ^ 2) + b * x + c == 0)
        .IsolateVariable(x)
        .AssertEqTo(

            Or(

                And(
                    x == (-b + Sqrt((b ^ 2) + -4 * a * c)) / (2 * a),
                    a != 0
                ),

                And(
                    x == (-b - Sqrt((b ^ 2) + -4 * a * c)) / (2 * a),
                    a != 0
                ),

                And(x == -c / b, a == 0, b != 0),

                And(a == 0, b == 0, c == 0)
            )
        );

    [Fact]
    public void Test227() => (a * (x ^ 2) + c == 0)
        .IsolateVariable(x)
        .AssertEqTo(

            Or(
                And(
                    x == Sqrt(-4 * a * c) / (2 * a),
                    a != 0
                ),

                And(
                    x == -Sqrt(-4 * a * c) / (2 * a),
                    a != 0
                ),

                And(a == 0, c == 0)
            )
        );

    // a x^2 + b x + c == 0
    // a x^2 + c == - b x
    // (a x^2 + c) / x == - b

    [Fact]
    public void Test228() => ((a * (x ^ 2) + c) / x == -b)
        .IsolateVariable(x)
        .AssertEqTo(
            Or(
                And(
                    x == (-b + Sqrt((b ^ 2) + -4 * a * c)) / (2 * a),
                    a != 0
                ),

                And(
                    x == (-b - Sqrt((b ^ 2) + -4 * a * c)) / (2 * a),
                    a != 0
                ),

                And(x == -c / b, a == 0, b != 0),

                And(a == 0, b == 0, c == 0)
            ));

    [Fact] public void Test229() => (Sqrt(x + y) == z).IsolateVariable(x).AssertEqTo(x == (z ^ 2) - y);

    [Fact]
    public void Test230() => (a * b + a == c)
                .IsolateVariable(a)
                .AssertEqTo(a == c / (b + 1));

    [Fact]
    public void Test231() => (a * b + a * c == d)
                .IsolateVariable(a)
                .AssertEqTo(a == d / (b + c));

    [Fact]
    public void Test232() => (1 / Sqrt(x) == y)
                .IsolateVariable(x)
                .AssertEqTo(x == (y ^ -2));

    [Fact]
    public void Test233() => (y == Sqrt(x) / x)
                .IsolateVariable(x)
                .AssertEqTo(x == (y ^ -2));

    [Fact]
    public void Test234() => (-Sqrt(x) + z * x == y)
                .IsolateVariable(x)
                .AssertEqTo(-Sqrt(x) + z * x == y);

    [Fact]
    public void Test235() => (Sqrt(a + x) - z * x == -y)
                .IsolateVariable(x)
                .AssertEqTo(Sqrt(a + x) - z * x == -y);

    [Fact]
    public void Test236() => (Sqrt(2 + x) * Sqrt(3 + x) == y)
                .IsolateVariable(x)
                .AssertEqTo(Sqrt(2 + x) * Sqrt(3 + x) == y);

    [Fact]
    public void Test237() => ((x + 1) / (x + 2) == 3)
                .IsolateVariable(x)
                .AssertEqTo(x == -new Integer(5) / 2);

    [Fact]
    public void Test238() => ((1 + 2 * x) / (3 * x - 4) == 5)
                .IsolateVariable(x)
                .AssertEqTo(x == new Integer(21) / 13);

    #endregion

    #region EliminateVariable

    [Fact]
    public void Test239() => And((x ^ 3) == (y ^ 5), z == x)
        .EliminateVariable(x)
        .AssertEqTo((z ^ 3) == (y ^ 5));

    [Fact]
    public void Test241() => And((x ^ 3) == (y ^ 5), z == (x ^ 7))
        .EliminateVariable(x)
        .AssertEqTo(And((x ^ 3) == (y ^ 5), z == (x ^ 7)));

    #endregion

    [Fact] public void Test242() => And(x + y == z, x / y == 0, x != 0).CheckVariable(x).AssertEqTo(false);

    #region RationalExpand

    // EA: Example 6.62

    [Fact]
    public void Test243() => Assert.True(
        (((Sqrt(1 / (((x + y) ^ 2) + 1)) + 1)
        *
        (Sqrt(1 / (((x + y) ^ 2) + 1)) - 1))
        / (x + 1))
        .RationalExpand()

        ==

        (-(x ^ 2) - 2 * x * y - (y ^ 2)) / (1 + x + (x ^ 2) + (x ^ 3) + 2 * x * y + 2 * (x ^ 2) * y + (y ^ 2) + x * (y ^ 2))

    );

    // EA: page 269

    [Fact]
    public void Test244() => Assert.Equal(0, (1 / (1 / a + c / (a * b)) + (a * b * c + a * (c ^ 2)) / ((b + c) ^ 2) - a).RationalExpand());

    #endregion

    #region LeadingCoefficientGpe

    [Fact]
    public void Test245() => Assert.True(
        (3 * x * (y ^ 2) + 5 * (x ^ 2) * y + 7 * (x ^ 2) * (y ^ 3) + 9).LeadingCoefficientGpe(x)
        ==
        5 * y + 7 * (y ^ 3)
    );

    #endregion


    #region PolynomialDivision

    // MM: Example 4.4

    [Fact]
    public void Test246()
    {
        var (quotient, remainder) = PolynomialDivision(5 * (x ^ 2) + 4 * x + 1, 2 * x + 3, x);

        Assert.True(quotient == 5 * x / 2 - new Integer(7) / 4);
        Assert.True(remainder == new Integer(25) / 4);
    }

    // MM: Example 4.10

    [Fact]
    public void Test247()
    {
        var (quotient, remainder) = PolynomialDivision((x ^ 2) - 4, x + 2, x);

        Assert.True(quotient == x - 2);
        Assert.Equal(0, remainder);
    }

    [Fact]
    public void Test248()
    {
        var (quotient, remainder) = PolynomialDivision((x ^ 2) + 5 * x + 6, x + 2, x);

        Assert.True(quotient == x + 3);
        Assert.Equal(0, remainder);
    }

    // MM: Example 4.43

    [Fact]
    public void Test249()
    {
        var (_, remainder) = PolynomialDivision(3 * (x ^ 3) + (x ^ 2) - 4, (x ^ 2) - 4 * x + 2, x);

        Assert.True(remainder == 46 * x - 30);
    }

    // MM: Example 4.45

    [Fact]
    public void Test250()
    {
        var i = new Symbol("i");
        var (_, remainder) = PolynomialDivision(2 + 3 * i + 4 * (i ^ 2) + 5 * (i ^ 3) + 6 * (i ^ 4), (i ^ 2) + 1, i);

        Assert.True(remainder == 4 - 2 * i);
    }

    #endregion

    #region PolynomialGcd

    // MM: Example 4.20

    [Fact]
    public void Test251() => Assert.True(
        PolynomialGcd(
            2 * (x ^ 3) + 12 * (x ^ 2) + 22 * x + 12,
            2 * (x ^ 3) + 18 * (x ^ 2) + 52 * x + 48,
            x)

        ==

        (x ^ 2) + 5 * x + 6
    );

    // MM: Example 4.24

    [Fact]
    public void Test252() => Assert.True(
        PolynomialGcd(
            (x ^ 7) - 4 * (x ^ 5) - (x ^ 2) + 4,
            (x ^ 5) - 4 * (x ^ 3) - (x ^ 2) + 4,
            x)

        ==

        (x ^ 3) - (x ^ 2) - 4 * x + 4
    );

    #endregion
           
    [Fact]
    public void Test253_a()
    {
        var x = new Symbol("x");
        var y = new Symbol("y");
        var z = new Symbol("z");
        _ = And(
            (x ^ 2) - 4 == 0,
            y + x == 0,
            x + z == 10
        );

        var half = new Integer(1) / 2;

        ((x ^ 2) - 4 == 0)
            .IsolateVariableEq(x)
            .AssertEqTo(Or(x == half * Sqrt(16), x == -half * Sqrt(16)));
    }

    [Fact]
    public void Test253_b()
    {
        var x = new Symbol("x");
        var y = new Symbol("y");
        var z = new Symbol("z");

        var eqs = And(
            (x ^ 2) - 4 == 0,
            y + x == 0,
            x + z == 10
        );

        var half = new Integer(1) / 2;
                    
        eqs.EliminateVariable(x)
            .AssertEqTo(
                Or(
                    And(
                        half * Sqrt(16) + y == 0,
                        half * Sqrt(16) + z == 10
                    ),
                    And(
                        -half * Sqrt(16) + y == 0,
                        -half * Sqrt(16) + z == 10
                    )
                )
            );
    }

    [Fact]
    public void Test253()
    {
        var x = new Symbol("x");
        var y = new Symbol("y");
        var z = new Symbol("z");

        var eqs = And(
            (x ^ 2) - 4 == 0,
            y + x == 0,
            x + z == 10
        );

        var half = new Integer(1) / 2;

        ((x ^ 2) - 4 == 0)
            .IsolateVariableEq(x)
            .AssertEqTo(Or(x == half * Sqrt(16), x == -half * Sqrt(16)));

        eqs.EliminateVariable(x)
            .AssertEqTo(
                Or(
                    And(
                        half * Sqrt(16) + y == 0,
                        half * Sqrt(16) + z == 10
                    ),
                    And(
                        -half * Sqrt(16) + y == 0,
                        -half * Sqrt(16) + z == 10
                    )
                )
            );
    }

    static And Kinematic(Symbol s, Symbol u, Symbol v, Symbol a, Symbol t) =>
        new (
            v == u + a * t,
            s == (u + v) * t / 2
        );

    [Fact]
    public void PSE_Example_2_6()
    {
        var sAC = new Symbol("sAC");
        var sAB = new Symbol("sAB");

        var vA = new Symbol("vA");
        var vB = new Symbol("vB");
        var vC = new Symbol("vC");

        var a = new Symbol("a");

        var tAC = new Symbol("tAC");
        var tAB = new Symbol("tAB");

        var eqs = And(
            tAB == tAC / 2,
            Kinematic(sAC, vA, vC, a, tAC),
            Kinematic(sAB, vA, vB, a, tAB)
            );

        var vals = new List<Equation>() { vA == 10, vC == 30, tAC == 10 };

        eqs
            .EliminateVariables(tAB, sAC, vB, sAB)
            .IsolateVariable(a)
            .AssertEqTo(a == (vC - vA) / tAC)
            .SubstituteEqLs(vals)
            .AssertEqTo(a == 2);

        eqs
            .EliminateVariables(vB, a, tAB, sAC)
            .AssertEqTo(sAB == tAC / 4 * (2 * vA + (vC - vA) / 2))
            .SubstituteEqLs(vals)
            .AssertEqTo(sAB == 75);
    }

    [Fact]
    public void PSE_Example_2_7()
    {
        // s = 
        // u = 63
        // v =  0
        // a =
        // t =  2

        var s = new Symbol("s");
        var u = new Symbol("u");
        var v = new Symbol("v");
        var a = new Symbol("a");
        var t = new Symbol("t");

        var eqs = Kinematic(s, u, v, a, t);

        var vals = new List<Equation>() { u == 63, v == 0, t == 2.0 };

        eqs
            .EliminateVariable(s)
            .AssertEqTo(v == a * t + u)
            .IsolateVariable(a)
            .AssertEqTo(a == (v - u) / t)
            .SubstituteEqLs(vals)
            .AssertEqTo(a == -31.5);

        eqs
            .EliminateVariable(a)
            .SubstituteEqLs(vals)
            .AssertEqTo(s == 63.0);
    }

    [Fact]
    public void PSE_Example_2_8()
    {
        // car
        //
        // s1 =  
        // u1 = 45
        // v1 = 45
        // a1 =  0
        // t1 = 

        // officer
        //
        // s2 =  
        // u2 =  0
        // v2 = 
        // a2 =  3
        // t2

        var s1 = new Symbol("s1");
        var u1 = new Symbol("u1");
        var v1 = new Symbol("v1");
        var a1 = new Symbol("a1");
        var t1 = new Symbol("t1");

        var s2 = new Symbol("s2");
        var u2 = new Symbol("u2");
        var v2 = new Symbol("v2");
        var a2 = new Symbol("a2");
        var t2 = new Symbol("t2");

        var eqs = And(
            u1 == v1,
            s1 == s2,
            t2 == t1 - 1,
            Kinematic(s1, u1, v1, a1, t1),
            Kinematic(s2, u2, v2, a2, t2));

        var vals = new List<Equation>()
            {
                v1 == 45.0,
                u2 == 0,
                a2 == 3
            };

        eqs
            .EliminateVariables(s2, t1, a1, s1, v2, u1)
            .IsolateVariable(t2)
            .SubstituteEqLs(vals)
            .AssertEqTo(Or(t2 == -0.96871942267131317, t2 == 30.968719422671313));
    }

    static And Kinematic(Symbol sA, Symbol sB, Symbol vA, Symbol vB, Symbol a, Symbol tA, Symbol tB) =>
        new (
            vB == vA + a * (tB - tA),
            sB - sA == (vA + vB) * (tB - tA) / 2);

    [Fact]
    public void PSE_Example_2_12()
    {
        var yA = new Symbol("yA");
        var yB = new Symbol("yB");
        var yC = new Symbol("yC");
        var yD = new Symbol("yD");

        var tA = new Symbol("tA");
        var tB = new Symbol("tB");
        var tC = new Symbol("tC");
        var tD = new Symbol("tD");

        var vA = new Symbol("vA");
        var vB = new Symbol("vB");
        var vC = new Symbol("vC");
        var vD = new Symbol("vD");

        var a = new Symbol("a");

        var eqs = And(
            Kinematic(yA, yB, vA, vB, a, tA, tB),
            Kinematic(yB, yC, vB, vC, a, tB, tC),
            Kinematic(yC, yD, vC, vD, a, tC, tD));

        var vals = new List<Equation>()
            {
                yA == 50,
                yC == 50,
                vA == 20,
                vB == 0,
                a == -9.8,
                tA == 0,
                tD == 5
            };

        // velocity and position at t = 5.00 s

        DoubleFloat.DoubleFloatTolerance = 0.000000001;

        eqs
            .EliminateVariables(tB, tC, vC, yB, yD)
            .SubstituteEqLs(vals)
            .AssertEqTo(Or(vD == -29.000000000000004, vD == -29.000000000000007));

        eqs
            .EliminateVariables(tB, tC, vC, yB, vD)
            .IsolateVariable(yD)
            .SubstituteEqLs(vals)
            .AssertEqTo(Or(yD == 27.499999999, yD == 27.499999999));

        DoubleFloat.DoubleFloatTolerance = null;
    }

    [Fact]
    public void PSE_Example_4_3()
    {
        // A long-jumper leaves the ground at an angle of 20.0° above
        // the horizontal and at a speed of 11.0 m/s.

        // (a) How far does he jump in the horizontal direction?
        // (Assume his motion is equivalent to that of a particle.)

        // (b) What is the maximum height reached?

        var xA = new Symbol("xA");
        var xB = new Symbol("xB");
        var xC = new Symbol("xC");

        var yA = new Symbol("yA");
        var yB = new Symbol("yB");
        var yC = new Symbol("yC");

        var vxA = new Symbol("vxA");
        var vxB = new Symbol("vxB");
        var vxC = new Symbol("vxC");

        var vyA = new Symbol("vyA");
        var vyB = new Symbol("vyB");
        var vyC = new Symbol("vyC");

        var tAB = new Symbol("tAB");
        var tAC = new Symbol("tAC");

        var ax = new Symbol("ax");
        var ay = new Symbol("ay");

        var vA = new Symbol("vA");
        var thA = new Symbol("thA");

        var Pi = new Symbol("Pi");

        var eqs = And(

            vxA == vA * Cos(thA),
            vyA == vA * Sin(thA),

            tAC == 2 * tAB,


            vxB == vxA + ax * tAB,
            vyB == vyA + ay * tAB,

            xB == xA + vxA * tAB + ax * (tAB ^ 2) / 2,
            yB == yA + vyA * tAB + ay * (tAB ^ 2) / 2,


            vxC == vxA + ax * tAB,
            vyC == vyA + ay * tAB,

            xC == xA + vxA * tAC + ax * (tAC ^ 2) / 2,
            yC == yA + vyA * tAC + ay * (tAC ^ 2) / 2

            );

        var zeros = new List<Equation>() { xA == 0, yA == 0, ax == 0, vyB == 0 };

        var vals = new List<Equation>() { thA == (20).ToRadians(), vA == 11.0, ay == -9.8, Pi == Math.PI };

        eqs
            .EliminateVariables(xB, yC, vxB, vxC, vyC, yB, tAC, vxA, vyA, tAB)
            .SubstituteEqLs(zeros)
            .AssertEqTo(xC == -2 * Cos(thA) * Sin(thA) * (vA ^ 2) / ay)
            .SubstituteEqLs(vals)
            .AssertEqTo(xC == 7.9364592624562507);

        eqs
            .EliminateVariables(xB, yC, vxB, vxC, vyC, xC, vxA, tAC, vyA, tAB)
            .SubstituteEqLs(zeros)
            .AssertEqTo(yB == -(Sin(thA) ^ 2) * (vA ^ 2) / (2 * ay))
            .SubstituteEqLs(vals)
            .AssertEqTo(yB == 0.72215873425009314);
    }

    [Fact]
    public void PSE_Example_4_3_KinematicObjectABC()
    {
        // A long-jumper leaves the ground at an angle of 20.0° above
        // the horizontal and at a speed of 11.0 m/s.

        // (a) How far does he jump in the horizontal direction?
        // (Assume his motion is equivalent to that of a particle.)

        // (b) What is the maximum height reached?

        var obj = new KinematicObjectABC("obj");

        var yB = new Symbol("yB");
        var xC = new Symbol("xC");
        var ay = new Symbol("ay");
        var thA = new Symbol("thA");
        var vA = new Symbol("vA");

        var Pi = new Symbol("Pi");

        var eqs = And(

            obj.TrigEquationsA(),

            obj.tAC == 2 * obj.tAB,

            obj.EquationsAB(),
            obj.EquationsAC()

            );

        var vals = new List<Equation>()
            {
                obj.xA == 0,
                obj.yA == 0,

                obj.vA == vA,
                obj.thA == thA,

                obj.yB == yB,
                obj.vyB == 0,

                obj.xC == xC,

                obj.ax == 0,
                obj.ay == ay
            };

        var numerical_vals = new List<Equation>()
            {
                thA == (20).ToRadians(),
                vA == 11,
                ay == -9.8,
                Pi == Math.PI

            };

        // xC
        eqs
            .SubstituteEqLs(vals)

            .EliminateVariables(
                obj.vxA, obj.vyA, obj.vyC, obj.vxC, obj.vxB,
                obj.xB, yB, obj.yC,
                obj.tAC, obj.tAB
            )

            .AssertEqTo(xC == -2 * Cos(thA) * Sin(thA) * (vA ^ 2) / ay)

            .SubstituteEqLs(numerical_vals)

            .AssertEqTo(xC == 7.9364592624562507);

        // yB
        eqs
            .SubstituteEqLs(vals)

            .EliminateVariables(
                obj.tAB, obj.tAC,
                obj.vxA, obj.vxB, obj.vxC, obj.vyC, obj.vyA,
                obj.xB, xC, obj.yC
            )

            .AssertEqTo(yB == -(Sin(thA) ^ 2) * (vA ^ 2) / (2 * ay))

            .SubstituteEqLs(numerical_vals)

            .AssertEqTo(yB == 0.72215873425009314);

    }

    [Fact]
    public void PSE_5E_Example_4_5()
    {
        _ = new Symbol("xA");
        _ = new Symbol("xB");
        _ = new Symbol("xC");

        var yA = new Symbol("yA");
        _ = new Symbol("yB");
        var yC = new Symbol("yC");

        var vxA = new Symbol("vxA");
        _ = new Symbol("vxB");
        var vxC = new Symbol("vxC");

        var vyA = new Symbol("vyA");
        _ = new Symbol("vyB");
        var vyC = new Symbol("vyC");
        _ = new Symbol("tAB");
        var tAC = new Symbol("tAC");

        var ax = new Symbol("ax");
        var ay = new Symbol("ay");

        var vA = new Symbol("vA");
        var thA = new Symbol("thA");

        var vC = new Symbol("vC");

        var Pi = new Symbol("Pi");

        var eqs = And(

            vxA == vA * Cos(thA),
            vyA == vA * Sin(thA),

            // tAC == 2 * tAB,

            // vxB == vxA + ax * tAB,
            // vyB == vyA + ay * tAB,

            // xB == xA + vxA * tAB + ax * (tAB ^ 2) / 2,
            // yB == yA + vyA * tAB + ay * (tAB ^ 2) / 2,

            vxC == vxA + ax * tAC,
            vyC == vyA + ay * tAC,

            // xC == xA + vxA * tAC + ax * (tAC ^ 2) / 2,
            yC == yA + vyA * tAC + ay * (tAC ^ 2) / 2,

            vC == Sqrt((vxC ^ 2) + (vyC ^ 2)),

            ay != 0
        );

        var zeros = new List<Equation>() { ax == 0, yC == 0 };
        var vals = new List<Equation>() { yA == 45, vA == 20, thA == (30).ToRadians(), ay == -9.8, Pi == Math.PI };

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        eqs
            .EliminateVariables(vC, vxA, vxC, vyC, vyA)
            .IsolateVariable(tAC)
            .LogicalExpand().SimplifyEquation().SimplifyLogical()
            .CheckVariable(ay)
            .AssertEqTo(
                Or(
                    And(
                        tAC == -(Sin(thA) * vA + Sqrt((Sin(thA) ^ 2) * (vA ^ 2) + 2 * ay * (yC - yA))) / ay,
                        ay != 0),
                    And(
                        tAC == -(Sin(thA) * vA - Sqrt((Sin(thA) ^ 2) * (vA ^ 2) + 2 * ay * (yC - yA))) / ay,
                        ay != 0)))
            .SubstituteEqLs(zeros)
            .SubstituteEqLs(vals)
            .AssertEqTo(Or(tAC == 4.2180489012229376, tAC == -2.1772325746923267));

        eqs
            .SubstituteEqLs(zeros)
            .EliminateVariables(vxC, vxA, vyA, vyC, tAC)
            .SimplifyEquation().SimplifyLogical()
            .CheckVariable(ay)
            .AssertEqTo(
                Or(
                    And(
                        ay != 0,
                        vC == Sqrt((Cos(thA) ^ 2) * (vA ^ 2) + ((Sin(thA) * vA - (Sin(thA) * vA + Sqrt((Sin(thA) ^ 2) * (vA ^ 2) + -2 * ay * yA))) ^ 2))),
                    And(
                        ay != 0,
                        vC == Sqrt((Cos(thA) ^ 2) * (vA ^ 2) + ((Sin(thA) * vA - (Sin(thA) * vA - Sqrt((Sin(thA) ^ 2) * (vA ^ 2) + -2 * ay * yA))) ^ 2)))))
            .SubstituteEqLs(vals)
            .AssertEqTo(Or(vC == 35.805027579936315, vC == 35.805027579936322));

        DoubleFloat.DoubleFloatTolerance = null;
    }

    [Fact]
    public void PSE_5E_Example_4_6()
    {
        // An Alaskan rescue plane drops a package of emergency rations to 
        // a stranded party of explorers, as shown in Figure 4.13.
        // If the plane is traveling horizontally at 40.0 m/s and is
        // 100 m above the ground, where does the package strike the
        // ground relative to the point at which it was released?

        // What are the horizontal and vertical components
        // of the velocity of the package just before it hits the ground?

        var xA = new Symbol("xA");
        var xB = new Symbol("xB");

        var yA = new Symbol("yA");
        var yB = new Symbol("yB");

        var vxA = new Symbol("vxA");
        var vxB = new Symbol("vxB");

        var vyA = new Symbol("vyA");
        var vyB = new Symbol("vyB");

        var tAB = new Symbol("tAB");

        var ax = new Symbol("ax");
        var ay = new Symbol("ay");

        var Pi = new Symbol("Pi");

        var eqs = And(

            vxB == vxA + ax * tAB,
            vyB == vyA + ay * tAB,

            xB == xA + vxA * tAB + ax * (tAB ^ 2) / 2,
            yB == yA + vyA * tAB + ay * (tAB ^ 2) / 2,

            vxA != 0,

            ay != 0
        );

        var vals = new List<Equation>() { xA == 0, yA == 100, vxA == 40, vyA == 0, yB == 0, ax == 0, ay == -9.8, Pi == Math.PI };

        var zeros = vals.Where(eq => eq.b == 0).ToList();

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        eqs
            .EliminateVariables(vxB, vyB, tAB)
            .IsolateVariable(xB)
            .LogicalExpand().SimplifyEquation()
            .CheckVariable(ay)
            .CheckVariable(vxA).SimplifyLogical()
            .SubstituteEq(ax == 0)
            .AssertEqTo(
                Or(
                    And(
                        vxA != 0,
                        xB == -(vxA ^ 2) * (-(-vyA / vxA + ay / (vxA ^ 2) * xA) + Sqrt(((-vyA / vxA + ay * xA / (vxA ^ 2)) ^ 2) + 2 * ay * (vyA * xA / vxA - ay / 2 / (vxA ^ 2) * (xA ^ 2) - yA + yB) / (vxA ^ 2))) / ay,
                        ay / (vxA ^ 2) != 0,
                        ay != 0),
                    And(
                        vxA != 0,
                        xB == -(vxA ^ 2) * (-(-vyA / vxA + ay / (vxA ^ 2) * xA) - Sqrt(((-vyA / vxA + ay * xA / (vxA ^ 2)) ^ 2) + 2 * ay * (vyA * xA / vxA - ay / 2 / (vxA ^ 2) * (xA ^ 2) - yA + yB) / (vxA ^ 2))) / ay,
                        ay / (vxA ^ 2) != 0,
                        ay != 0)))
            .SubstituteEqLs(zeros)
            .AssertEqTo(
                Or(
                    And(
                        vxA != 0,
                        xB == -1 / ay * (vxA ^ 2) * Sqrt(-2 * ay * (vxA ^ -2) * yA),
                        ay / (vxA ^ 2) != 0,
                        ay != 0),
                    And(
                        vxA != 0,
                        xB == 1 / ay * (vxA ^ 2) * Sqrt(-2 * ay * (vxA ^ -2) * yA),
                        ay / (vxA ^ 2) != 0,
                        ay != 0)))
            .SubstituteEqLs(vals)
            .AssertEqTo(Or(xB == 180.70158058105022, xB == -180.70158058105022));

        eqs
            .EliminateVariables(vxB, xB, tAB)
            .IsolateVariable(vyB)
            .LogicalExpand().SimplifyEquation()
            .CheckVariable(ay)
            .AssertEqTo(
                Or(
                    And(
                        vyB == -1 * ay * Sqrt(2 * (ay ^ -1) * ((ay ^ -1) / 2 * (vyA ^ 2) + -1 * yA + yB)),
                        vxA != 0,
                        ay != 0),
                    And(
                        vyB == ay * Sqrt(2 * (ay ^ -1) * ((ay ^ -1) / 2 * (vyA ^ 2) + -1 * yA + yB)),
                        vxA != 0,
                        ay != 0)))
            .SubstituteEqLs(zeros)
            .AssertEqTo(
                Or(
                  And(
                      vyB == -ay * Sqrt(-2 / ay * yA),
                      vxA != 0,
                      ay != 0),
                  And(
                      vyB == ay * Sqrt(-2 / ay * yA),
                      vxA != 0,
                      ay != 0)))
            .SubstituteEqLs(vals)
            .AssertEqTo(Or(vyB == 44.271887242357309, vyB == -44.271887242357309));

        DoubleFloat.DoubleFloatTolerance = null;
    }

    [Fact]
    public void PSE_5E_Example_4_7()
    {
        var xA = new Symbol("xA");
        var yA = new Symbol("yA");

        var xB = new Symbol("xB");
        var yB = new Symbol("yB");

        var vxA = new Symbol("vxA");
        var vyA = new Symbol("vyA");

        var vxB = new Symbol("vxB");
        var vyB = new Symbol("vyB");

        var tAB = new Symbol("tAB");

        var ax = new Symbol("ax");
        var ay = new Symbol("ay");

        var th = new Symbol("th");
        var d = new Symbol("d");

        var Pi = new Symbol("Pi");

        var eqs = And(

            Cos(th) == (xB - xA) / d,
            Sin(th) == (yA - yB) / d,

            vxB == vxA + ax * tAB,
            vyB == vyA + ay * tAB,

            xB == xA + vxA * tAB + ax * (tAB ^ 2) / 2,
            yB == yA + vyA * tAB + ay * (tAB ^ 2) / 2,

            yB != 0,

            ay != 0
        );

        var vals = new List<Equation>() { xA == 0, yA == 0, vxA == 25, vyA == 0, ax == 0, ay == -9.8, th == (35).ToRadians(), Pi == Math.PI };

        var zeros = vals.Where(eq => eq.b == 0).ToList();

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        eqs
            .SubstituteEqLs(zeros)
            .EliminateVariables(vxB, vyB, d, yB, tAB)
            .IsolateVariable(xB)
            .LogicalExpand()
            .CheckVariable(ay)
            .SimplifyEquation()
            .AssertEqTo(
                Or(
                    And(
                        xB == -(Sin(th) / Cos(th) + Sqrt((Cos(th) ^ -2) * (Sin(th) ^ 2))) * (vxA ^ 2) / ay,
                        ay / (vxA ^ 2) != 0,
                        Sin(th) / Cos(th) * xB != 0,
                        ay != 0),
                    And(
                        xB == -(Sin(th) / Cos(th) - Sqrt((Cos(th) ^ -2) * (Sin(th) ^ 2))) * (vxA ^ 2) / ay,
                        ay / (vxA ^ 2) != 0,
                        Sin(th) / Cos(th) * xB != 0,
                        ay != 0)))
            .SubstituteEqLs(vals)
            .SimplifyEquation()
            .AssertEqTo(
                Or(
                    And(
                        xB == 89.312185996136435,
                        xB != 0),
                    And(
                        xB == 7.0805039835788038E-15,
                        xB != 0)));

        eqs
            .SubstituteEqLs(zeros)
            .EliminateVariables(vxB, vyB, d, xB, tAB)
            .IsolateVariable(yB)
            .LogicalExpand()
            .CheckVariable(yB)
            .AssertEqTo(
                And(
                    yB == 2 * (Sin(th) ^ 2) * (vxA ^ 2) / ay / (Cos(th) ^ 2),
                    -ay * (Cos(th) ^ 2) / (Sin(th) ^ 2) / (vxA ^ 2) / 2 != 0,
                    yB != 0,
                    ay != 0))
            .SubstituteEqLs(vals)
            .AssertEqTo(
                And(
                    yB == -62.537065888482395,
                    yB != 0));

        eqs
            .SubstituteEqLs(zeros)
            .EliminateVariables(vxB, vyB, d, xB, yB)
            .IsolateVariable(tAB)
            .LogicalExpand().CheckVariable(ay).SimplifyEquation().SimplifyLogical()
            .AssertEqTo(
                Or(
                    And(
                        tAB == -(Sin(th) * vxA / Cos(th) + Sqrt((Sin(th) ^ 2) * (vxA ^ 2) / (Cos(th) ^ 2))) / ay,
                        ay != 0,
                        Sin(th) * tAB * vxA / Cos(th) != 0),
                    And(
                        tAB == -(Sin(th) * vxA / Cos(th) - Sqrt((Sin(th) ^ 2) * (vxA ^ 2) / (Cos(th) ^ 2))) / ay,
                        ay != 0,
                        Sin(th) * tAB * vxA / Cos(th) != 0)))
            .SubstituteEqLs(vals)
            .CheckVariable(tAB).SimplifyEquation()
            .AssertEqTo(
                And(
                    tAB == 3.5724874398454571,
                    tAB != 0));

        eqs
            .SubstituteEqLs(zeros)
            .EliminateVariables(vxB, d, tAB, xB, yB)
            .IsolateVariable(vyB)
            .LogicalExpand()
            .CheckVariable(ay)
            .SimplifyEquation()
            .CheckVariable(ay)
            .AssertEqTo(
                Or(
                    And(
                        vyB == -ay * (Sin(th) * vxA / (ay * Cos(th)) + Sqrt((Sin(th) ^ 2) * (vxA ^ 2) / ((ay ^ 2) * (Cos(th) ^ 2)))),
                        Sin(th) * vxA * vyB / (ay * Cos(th)) != 0,
                        ay != 0),
                    And(
                        vyB == -ay * (Sin(th) * vxA / (ay * Cos(th)) - Sqrt((Sin(th) ^ 2) * (vxA ^ 2) / ((ay ^ 2) * (Cos(th) ^ 2)))),
                        Sin(th) * vxA * vyB / (ay * Cos(th)) != 0,
                        ay != 0)))
            .SubstituteEqLs(vals)
            .CheckVariable(vyB)
            .SimplifyEquation()
            .CheckVariable(vyB)
            .AssertEqTo(
                And(
                    vyB == -35.010376910485483,
                    vyB != 0));

        DoubleFloat.DoubleFloatTolerance = null;
    }

    [Fact]
    public void PSE_5E_P4_9()
    {
        // In a local bar, a customer slides an empty beer mug
        // down the counter for a refill. The bartender is momentarily 
        // distracted and does not see the mug, which slides
        // off the counter and strikes the floor 1.40 m from the
        // base of the counter. If the height of the counter is 
        // 0.860 m, (a) with what velocity did the mug leave the
        // counter and (b) what was the direction of the mug’s 
        // velocity just before it hit the floor?

        var xA = new Symbol("xA");
        var yA = new Symbol("yA");

        var xB = new Symbol("xB");
        var yB = new Symbol("yB");

        var vxA = new Symbol("vxA");
        var vyA = new Symbol("vyA");

        var vxB = new Symbol("vxB");
        var vyB = new Symbol("vyB");

        var tAB = new Symbol("tAB");

        var ax = new Symbol("ax");
        var ay = new Symbol("ay");

        var thB = new Symbol("thB");
        var vB = new Symbol("vB");

        var eqs = And(

            vxB == vxA + ax * tAB,
            vyB == vyA + ay * tAB,

            Tan(thB) == vyB / vxB,

            xB == xA + vxA * tAB + ax * (tAB ^ 2) / 2,
            yB == yA + vyA * tAB + ay * (tAB ^ 2) / 2,

            xB != 0
        );

        var vals = new List<Equation>() { xA == 0, yA == 0.86, /* vxA */ vyA == 0, xB == 1.4, yB == 0, /* vxB vyB vB thB */ /* tAB */ ax == 0, ay == -9.8 };

        var zeros = vals.Where(eq => eq.b == 0).ToList();

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        eqs
            .SubstituteEqLs(zeros)
            .EliminateVariables(thB, vxB, vyB, tAB)
            .IsolateVariable(vxA)
            .LogicalExpand()
            .AssertEqTo(
                Or(
                    And(
                        vxA == ay * (xB ^ 2) / yA / 4 * Sqrt(-8 / ay * (xB ^ -2) * yA),
                        2 / ay * (xB ^ -2) * yA != 0,
                        xB != 0),
                    And(
                        vxA == -ay * (xB ^ 2) / yA / 4 * Sqrt(-8 / ay * (xB ^ -2) * yA),
                        2 / ay * (xB ^ -2) * yA != 0,
                        xB != 0)))
            .SubstituteEqLs(vals)
            .AssertEqTo(Or(vxA == -3.3417722634053204, vxA == 3.3417722634053204));

        eqs
            .SubstituteEqLs(zeros)
            .EliminateVariables(vxB, vyB, tAB, vxA)
            .LogicalExpand()
            .CheckVariable(xB)
            .SimplifyLogical()
            .IsolateVariable(thB)
            .AssertEqTo(
                And(
                    -Tan(thB) / ay != 0,
                    thB == new Atan(-2 * yA / xB),
                    xB != 0))
            .SubstituteEqLs(vals)
            .AssertEqTo(
                And(
                    0.1020408163265306 * Tan(thB) != 0,
                    thB == -0.88760488150470185));

        DoubleFloat.DoubleFloatTolerance = null;
    }

    readonly Func<MathObject, MathObject> SumDifferenceFormulaFunc = elt =>
    {
        // Sin(u) Cos(v) - Cos(u) Sin(v) -> Sin(u - v)

        if (elt is Sum)
        {
            var items = new List<MathObject>();

            foreach (var item in (elt as Sum).elts)
            {
                if (
                    item is Product &&
                    (item as Product).elts[0] == -1 &&
                    (item as Product).elts[1] is Cos &&
                    (item as Product).elts[2] is Sin
                    )
                {
                    var u_ = ((item as Product).elts[1] as Cos).args[0];
                    var v_ = ((item as Product).elts[2] as Sin).args[0];

                    bool match(MathObject obj)
                    {
                        return obj is Product &&
                                                    (obj as Product).elts[0] is Cos &&
                                                    (obj as Product).elts[1] is Sin &&

                                                    ((obj as Product).elts[1] as Sin).args[0] == u_ &&
                                                    ((obj as Product).elts[0] as Cos).args[0] == v_;
                    }

                    if (items.Any(obj => match(obj)))
                    {
                        items = items.Where(obj => match(obj) == false).ToList();

                        items.Add(Sin(u_ - v_));
                    }
                    else items.Add(item);
                }
                else items.Add(item);
            }

            return Sum.FromRange(items).Simplify();
        }

        return elt;
    };

    [Fact]
    public void SumDifferenceFormulaFuncTest()
    {
        var u = new Symbol("u");
        var v = new Symbol("v");

        (Sin(u) * Cos(v) - Cos(u) * Sin(v))
            .DeepSelect(SumDifferenceFormulaFunc)
            .AssertEqTo(Sin(u - v));
    }

    readonly Func<MathObject, MathObject> SumDifferenceFormulaAFunc = elt =>
    {
        if (elt is Sum)
        {
            var items = new List<MathObject>();

            foreach (var item in (elt as Sum).elts)
            {
                if (
                    item is Product &&
                    (item as Product).elts[0] is Cos &&
                    (item as Product).elts[1] is Sin
                    )
                {
                    var u_ = ((item as Product).elts[0] as Cos).args[0];
                    var v_ = ((item as Product).elts[1] as Sin).args[0];

                    bool match(MathObject obj) =>
                        obj is Product &&
                        (obj as Product).elts[0] is Cos &&
                        (obj as Product).elts[1] is Sin &&

                        ((obj as Product).elts[1] as Sin).args[0] == u_ &&
                        ((obj as Product).elts[0] as Cos).args[0] == v_;

                    if (items.Any(obj => match(obj)))
                    {
                        items = items.Where(obj => match(obj) == false).ToList();

                        items.Add(Sin(u_ + v_));
                    }
                    else items.Add(item);
                }
                else items.Add(item);
            }

            return Sum.FromRange(items).Simplify();
        }

        return elt;
    };

    [Fact]
    public void SumDifferenceFormulaAFuncTest()
    {
        // Sin(u) Cos(v) + Cos(u) Sin(v) -> Sin(u + v)

        var u = new Symbol("u");
        var v = new Symbol("v");

        (Sin(u) * Cos(v) + Cos(u) * Sin(v))
            .DeepSelect(SumDifferenceFormulaAFunc)
            .AssertEqTo(Sin(u + v));
    }

    readonly Func<MathObject, MathObject> DoubleAngleFormulaFunc =
        elt =>
        {
            // Sin(u) Cos(u) -> Sin(2 u) / 2

            if (elt is Product)
            {
                var items = new List<MathObject>();

                foreach (var item in (elt as Product).elts)
                {
                    if (item is Sin)
                    {
                        var sym = (item as Sin).args.First();

                        if (items.Any(obj => (obj is Cos) && (obj as Cos).args.First() == sym))
                        {
                            items = items.Where(obj => ((obj is Cos) && (obj as Cos).args.First() == sym) == false).ToList();

                            items.Add(Sin(2 * sym) / 2);
                        }
                        else items.Add(item);
                    }

                    else if (item is Cos)
                    {
                        var sym = (item as Cos).args.First();

                        if (items.Any(obj => (obj is Sin) && (obj as Sin).args.First() == sym))
                        {
                            items = items.Where(obj => ((obj is Sin) && (obj as Sin).args.First() == sym) == false).ToList();

                            items.Add(Sin(2 * sym) / 2);
                        }
                        else items.Add(item);
                    }

                    else items.Add(item);

                }

                return Product.FromRange(items).Simplify();
            }
            return elt;
        };

    readonly Func<MathObject, MathObject> SinCosToTanFunc = elt =>
    {
        // Sin(x) / Cos(x) -> Tan(x)

        if (elt is Product)
        {
            if ((elt as Product).elts.Any(obj1 =>
                    obj1 is Sin &&
                    (elt as Product).elts.Any(obj2 => obj2 == 1 / Cos((obj1 as Sin).args[0]))))
            {
                var sin_ = (elt as Product).elts.First(obj1 =>
                    obj1 is Sin &&
                    (elt as Product).elts.Any(obj2 => obj2 == 1 / Cos((obj1 as Sin).args[0])));

                var arg = (sin_ as Sin).args[0];

                return elt * Cos(arg) / Sin(arg) * Tan(arg);
            }

            return elt;
        }

        return elt;
    };

    [Fact]
    public void SinCosToTanFuncTest()
    {
        var x = new Symbol("x");
        var y = new Symbol("y");

        (Sin(x) / Cos(x)).DeepSelect(SinCosToTanFunc)

            .AssertEqTo(Tan(x));

        (y * Sin(x) / Cos(x)).DeepSelect(SinCosToTanFunc)

            .AssertEqTo(Tan(x) * y);

        (Sin(x) * Sin(y) / Cos(x) / Cos(y))
            .DeepSelect(SinCosToTanFunc)
            .DeepSelect(SinCosToTanFunc)

            .AssertEqTo(Tan(x) * Tan(y));
    }

    [Fact]
    public void PSE_5E_P4_11()
    {
        // One strategy in a snowball fight is to throw a first snowball
        // at a high angle over level ground. While your opponent is watching
        // the first one, you throw a second one at a low angle and timed
        // to arrive at your opponent before or at the same time as the first one.

        // Assume both snowballs are thrown with a speed of 25.0 m/s.

        // The first one is thrown at an angle of 70.0° with respect to the horizontal. 

        // (a) At what angle should the second (lowangle) 
        // snowball be thrown if it is to land at the same
        // point as the first?

        // (b) How many seconds later should the second snowball 
        // be thrown if it is to land at the same time as the first?

        var xA = new Symbol("xA");
        var yA = new Symbol("yA");

        var vxA = new Symbol("vxA");
        var vyA = new Symbol("vyA");

        var vA = new Symbol("vA");

        var thA = new Symbol("thA");

        var xB = new Symbol("xB");
        var yB = new Symbol("yB");

        var vxB = new Symbol("vxB");
        var vyB = new Symbol("vyB");

        var tAB = new Symbol("tAB");
        var tAC = new Symbol("tAC");

        var ax = new Symbol("ax");
        var ay = new Symbol("ay");

        var Pi = new Symbol("Pi");

        var eqs = new And(

            vxA == vA * Cos(thA),
            vyA == vA * Sin(thA),

            vxB == vxA + ax * tAB,
            vyB == vyA + ay * tAB,

            xB == xA + vxA * tAB + ax * (tAB ^ 2) / 2,
            yB == yA + vyA * tAB + ay * (tAB ^ 2) / 2
        );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        {
            var vals = new List<Equation>() { xA == 0, yA == 0, /* vxA vyA */ vA == 25.0, /* thA == 70.0, */ /* xB == 20.497, */ /* yB */ /* vxB */ vyB == 0, /* tAB */ ax == 0, ay == -9.8, Pi == Math.PI };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            {
                // thA = ... || thA = ...

                var expr = eqs
                    .SubstituteEqLs(zeros)
                    .EliminateVariables(yB, vxA, vyA, vxB, tAB)
                    .DeepSelect(DoubleAngleFormulaFunc)
                    .IsolateVariable(thA);

                // th_delta = ...

                var th1 = ((expr as Or).args[0] as Equation).b;
                var th2 = ((expr as Or).args[1] as Equation).b;

                var th_delta = new Symbol("th_delta");

                eqs
                    .Add(th_delta == (th1 - th2).AlgebraicExpand())
                    .SubstituteEqLs(zeros)

                    .EliminateVariables(yB, vxA, vyA, vxB, tAB)

                    .DeepSelect(DoubleAngleFormulaFunc)
                    .EliminateVariable(xB)

                    .AssertEqTo(th_delta == Asin(Sin(2 * thA)) - Pi / 2)

                    .SubstituteEq(thA == (70).ToRadians())
                    .SubstituteEq(Pi == Math.PI)

                    .AssertEqTo(th_delta == -0.87266462599716454)
                    ;
            }

            {
                // tAB = ...

                var tAB_eq = eqs
                    .SubstituteEqLs(zeros)
                    .EliminateVariables(yB, vxA, vyA, vxB, xB)
                    .IsolateVariable(tAB);

                And(
                    Or(thA == (20).ToRadians(), thA == (70).ToRadians()),
                    tAB_eq,
                    tAC == tAB * 2)
                    .LogicalExpand()
                    .EliminateVariables(thA, tAB)

                    .AssertEqTo(Or(
                        tAC == -2 * Sin(Pi / 9) * vA / ay,
                        tAC == -2 * Sin(7 * Pi / 18) * vA / ay))

                    .SubstituteEqLs(vals)
                    .AssertEqTo(
                        Or(
                            tAC == 1.7450007312534115,
                            tAC == 4.794350106050552));
            }
        }

        DoubleFloat.DoubleFloatTolerance = null;
    }

    [Fact]
    public void PSE_5E_P4_13()
    {
        // An artillery shell is fired with an initial velocity of 
        // 300 m/s at 55.0° above the horizontal. It explodes on a
        // mountainside 42.0 s after firing. What are the x and y
        // coordinates of the shell where it explodes, relative to its
        // firing point?

        var xA = new Symbol("xA");
        var yA = new Symbol("yA");

        var vxA = new Symbol("vxA");
        var vyA = new Symbol("vyA");

        var vA = new Symbol("vA");
        var thA = new Symbol("thA");

        var xB = new Symbol("xB");
        var yB = new Symbol("yB");

        var vxB = new Symbol("vxB");
        var vyB = new Symbol("vyB");

        var tAB = new Symbol("tAB");

        var ax = new Symbol("ax");
        var ay = new Symbol("ay");

        var Pi = new Symbol("Pi");

        var eqs = And(
            vxA == vA * Cos(thA),
            vyA == vA * Sin(thA),

            vxB == vxA + ax * tAB,
            vyB == vyA + ay * tAB,

            xB == xA + vxA * tAB + ax * (tAB ^ 2) / 2,
            yB == yA + vyA * tAB + ay * (tAB ^ 2) / 2
        );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        {
            var vals = new List<Equation>() { xA == 0, yA == 0, /* vxA vyA */ vA == 300.0, thA == (55).ToRadians(), /* xB yB vxB vyB */ tAB == 42, ax == 0, ay == -9.8, Pi == Math.PI };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            {
                eqs
                    .SubstituteEqLs(zeros)
                    .EliminateVariable(vxA)
                    .EliminateVariable(vyA)

                    .AssertEqTo(
                        And(
                            vxB == Cos(thA) * vA,
                            vyB == ay * tAB + Sin(thA) * vA,
                            xB == Cos(thA) * tAB * vA,
                            yB == ay * (tAB ^ 2) / 2 + Sin(thA) * tAB * vA))

                    .SubstituteEqLs(vals)

                    .AssertEqTo(
                        And(
                            vxB == 172.07293090531385,
                            vyB == -165.85438671330249,
                            xB == 7227.0630980231817,
                            yB == 1677.7157580412968))

                    ;
            }
        }

        DoubleFloat.DoubleFloatTolerance = null;
    }

    [Fact]
    public void PSE_5E_P4_15()
    {
        // A projectile is fired in such a way that its horizontal
        // range is equal to three times its maximum height.
        //
        // What is the angle of projection? 
        // Give your answer to three significant figures.

        var xA = new Symbol("xA");
        var yA = new Symbol("yA");

        var vxA = new Symbol("vxA");
        var vyA = new Symbol("vyA");

        var vA = new Symbol("vA");
        var thA = new Symbol("thA");


        var xB = new Symbol("xB");
        var yB = new Symbol("yB");

        var vxB = new Symbol("vxB");
        var vyB = new Symbol("vyB");


        var xC = new Symbol("xC");
        var yC = new Symbol("yC");

        var vxC = new Symbol("vxC");
        var vyC = new Symbol("vyC");


        var tAB = new Symbol("tAB");
        var tBC = new Symbol("tBC");

        var ax = new Symbol("ax");
        var ay = new Symbol("ay");

        var Pi = new Symbol("Pi");

        var eqs = And(

            xC - xA == 3 * yB,

            tAB == tBC,


            vxA == vA * Cos(thA),
            vyA == vA * Sin(thA),

            vxB == vxA + ax * tAB,
            vyB == vyA + ay * tAB,

            xB == xA + vxA * tAB + ax * (tAB ^ 2) / 2,
            yB == yA + vyA * tAB + ay * (tAB ^ 2) / 2,


            vxC == vxB + ax * tBC,
            vyC == vyB + ay * tBC,

            xC == xB + vxB * tBC + ax * (tBC ^ 2) / 2,
            yC == yB + vyB * tBC + ay * (tBC ^ 2) / 2

        );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        {
            var vals = new List<Equation>()
                {
                    xA == 0, yA == 0, /* vxA vyA vA thA */ /* xB yB vxB */ vyB == 0, /* tAB tBC */ 
                    /* xC */ yC == 0,

                    ax == 0, ay == -9.8, Pi == Math.PI
                };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            eqs
                .SubstituteEqLs(zeros)
                .EliminateVariables(xC, tAB, vxA, vyA, vxB, xB, yB, vxC, vyC, tBC)
                .IsolateVariable(thA)
                .AssertEqTo(thA == new Atan(new Integer(4) / 3));
        }

        DoubleFloat.DoubleFloatTolerance = null;
    }

    [Fact]
    public void PSE_5E_P4_17()
    {
        // A cannon with a muzzle speed of 1000 m/s is used to
        // start an avalanche on a mountain slope. The target is 
        // 2000 m from the cannon horizontally and 800 m above
        // the cannon.
        //
        // At what angle, above the horizontal, should the cannon be fired?

        var xA = new Symbol("xA");
        var yA = new Symbol("yA");

        var vxA = new Symbol("vxA");
        var vyA = new Symbol("vyA");

        var vA = new Symbol("vA");
        var thA = new Symbol("thA");


        var xB = new Symbol("xB");
        var yB = new Symbol("yB");

        var vxB = new Symbol("vxB");
        var vyB = new Symbol("vyB");


        var xC = new Symbol("xC");
        var yC = new Symbol("yC");

        var vxC = new Symbol("vxC");
        var vyC = new Symbol("vyC");


        var tAB = new Symbol("tAB");
        var tBC = new Symbol("tBC");

        var ax = new Symbol("ax");
        var ay = new Symbol("ay");

        var Pi = new Symbol("Pi");

        var phi = new Symbol("phi");

        var eqs = And(

            vxA == vA * Cos(thA),
            vyA == vA * Sin(thA),

            vxB == vxA + ax * tAB,
            vyB == vyA + ay * tAB,

            xB == xA + vxA * tAB + ax * (tAB ^ 2) / 2,
            yB == yA + vyA * tAB + ay * (tAB ^ 2) / 2
        );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        {
            var vals = new List<Equation>()
                {
                    xA ==    0, yA ==   0, /* vxA vyA */ vA == 1000, /* thA */ 
                    xB == 2000, yB == 800.0, /* vxB vyB */ 
                    /* tAB */ ax == 0, ay == -9.8, Pi == Math.PI
                };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            {
                eqs
                    .SubstituteEqLs(zeros)
                    .EliminateVariables(vxA, vyA, vxB, vyB, tAB)

                    .MultiplyBothSidesBy(Cos(thA) ^ 2).AlgebraicExpand()
                    .Substitute(Cos(thA) ^ 2, (1 + Cos(2 * thA)) / 2)
                    .DeepSelect(DoubleAngleFormulaFunc).AlgebraicExpand()
                    .AddToBothSides(-Sin(2 * thA) * xB / 2)
                    .AddToBothSides(-yB / 2)
                    .MultiplyBothSidesBy(2 / xB).AlgebraicExpand()

                    // yB / xB = Tan(phi)
                    // yB / xB = Sin(phi) / Cos(phi)

                    // phi = Atan(yB / xB)

                    .Substitute(Cos(2 * thA) * yB / xB, Cos(2 * thA) * Sin(phi) / Cos(phi))
                    .MultiplyBothSidesBy(Cos(phi)).AlgebraicExpand()
                    .DeepSelect(SumDifferenceFormulaFunc)
                    .IsolateVariable(thA)

                    .Substitute(phi, new Atan(yB / xB).Simplify())

                    .AssertEqTo(
                        Or(
                            thA == -(Asin(ay * Cos(Atan(yB / xB)) * (vA ^ -2) * xB + -1 * Cos(Atan(yB / xB)) * yB / xB) - Atan(yB / xB)) / 2,
                            thA == -(-Asin(ay * Cos(Atan(yB / xB)) * (vA ^ -2) * xB - Cos(Atan(yB / xB)) * yB / xB) - Atan(yB / xB) + Pi) / 2))

                    .SubstituteEqLs(vals)

                    .AssertEqTo(
                        Or(
                            thA == 0.39034573609628065,
                            thA == -1.5806356857788124))
                    ;
            }
        }

        DoubleFloat.DoubleFloatTolerance = null;
    }

    [Fact]
    public void PSE_5E_P4_19()
    {
        // A placekicker must kick a football from a point 36.0 m
        // (about 40 yards) from the goal, and half the crowd
        // hopes the ball will clear the crossbar, which is 3.05 m
        // high. When kicked, the ball leaves the ground with a
        // speed of 20.0 m/s at an angle of 53.0° to the horizontal.
        //
        // (a) By how much does the ball clear or fall short of
        //     clearing the crossbar ?
        //
        // (b) Does the ball approach the crossbar while still 
        //     rising or while falling ?

        //MathObject sqrt(MathObject obj)
        //{
        //    return obj ^ (new Integer(1) / 2);
        //}

        var xA = new Symbol("xA");
        var yA = new Symbol("yA");

        var vxA = new Symbol("vxA");
        var vyA = new Symbol("vyA");

        var vA = new Symbol("vA");
        var thA = new Symbol("thA");


        var xB = new Symbol("xB");
        var yB = new Symbol("yB");

        var vxB = new Symbol("vxB");
        var vyB = new Symbol("vyB");

        var tAB = new Symbol("tAB");

        var ax = new Symbol("ax");
        var ay = new Symbol("ay");

        var Pi = new Symbol("Pi");

        var cleared_by = new Symbol("cleared_by");

        var goal_height = new Symbol("goal_height");

        var eqs = And(

            vxA == vA * Cos(thA),
            vyA == vA * Sin(thA),

            vxB == vxA + ax * tAB,
            vyB == vyA + ay * tAB,

            xB == xA + vxA * tAB + ax * (tAB ^ 2) / 2,
            yB == yA + vyA * tAB + ay * (tAB ^ 2) / 2,

            cleared_by == yB - goal_height
        );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        {
            var vals = new List<Equation>()
                {
                    xA == 0, yA == 0, /* vxA vyA */ vA == 20, thA == (53).ToRadians(),
                    xB == 36, /* yB */ /* vxB vyB */ 
                    /* tAB */ ax == 0, ay == -9.8, Pi == Math.PI,

                    goal_height == 3.05
                };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            {
                eqs
                    .SubstituteEqLs(zeros)
                    .EliminateVariables(vxA, vyA, vxB, vyB, tAB, yB)

                    .AssertEqTo(
                        cleared_by == -goal_height + Sin(thA) / Cos(thA) * xB + ay / 2 * (Cos(thA) ^ -2) * (vA ^ -2) * (xB ^ 2)
                        )

                    .SubstituteEqLs(vals)

                    .AssertEqTo(cleared_by == 0.88921618776713007);
            }

            {
                eqs
                    .SubstituteEqLs(zeros)

                    .EliminateVariables(cleared_by, vxA, vyA, vxB, tAB, yB)
                    .IsolateVariable(vyB)

                    .AssertEqTo(vyB == Sin(thA) * vA + ay / Cos(thA) / vA * xB)

                    .SubstituteEqLs(vals)

                    .AssertEqTo(vyB == -13.338621888454744);
            }
        }

        DoubleFloat.DoubleFloatTolerance = null;
    }

    [Fact]
    public void PSE_5E_P4_21()
    {
        // A firefighter a distance d from a burning building directs 
        // a stream of water from a fire hose at angle θi above
        // the horizontal as in Figure P4.20.If the initial speed of
        // the stream is vi, at what height h does the water strike
        // the building?

        var xA = new Symbol("xA");
        var yA = new Symbol("yA");

        var vxA = new Symbol("vxA");
        var vyA = new Symbol("vyA");

        var vA = new Symbol("vA");
        var thA = new Symbol("thA");


        var xB = new Symbol("xB");
        var yB = new Symbol("yB");

        var vxB = new Symbol("vxB");
        var vyB = new Symbol("vyB");

        var tAB = new Symbol("tAB");

        var ax = new Symbol("ax");
        var ay = new Symbol("ay");

        var Pi = new Symbol("Pi");

        var d = new Symbol("d");
        var thi = new Symbol("thi");
        var vi = new Symbol("vi");
        var h = new Symbol("h");


        var eqs = And(

            vxA == vA * Cos(thA),
            vyA == vA * Sin(thA),

            vxB == vxA + ax * tAB,
            vyB == vyA + ay * tAB,

            xB == xA + vxA * tAB + ax * (tAB ^ 2) / 2,
            yB == yA + vyA * tAB + ay * (tAB ^ 2) / 2

        );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        {
            var vals = new List<Equation>()
                {
                    xA == 0, yA == 0, /* vxA vyA */ vA == vi, thA == thi,
                    xB == d, yB == h, /* vxB vyB */ 
                    /* tAB */ ax == 0, ay == -9.8, Pi == Math.PI
                };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            {
                eqs
                    .SubstituteEqLs(zeros)
                    .EliminateVariables(vxA, vyA, vxB, vyB, tAB)

                    .SubstituteEqLs(vals.Where(eq => eq.b is Symbol).ToList())

                    .AssertEqTo(

                        h == d * Sin(thi) / Cos(thi) + ay * (d ^ 2) / (Cos(thi) ^ 2) / (vi ^ 2) / 2

                        );
            }
        }

        DoubleFloat.DoubleFloatTolerance = null;
    }

    [Fact]
    public void PSE_5E_P4_23()
    {
        // A basketball star covers 2.80 m horizontally in a jump to
        // dunk the ball. His motion through space can be modeled as 
        // that of a particle at a point called his center of mass. 
        // His center of mass is at elevation 1.02 m when he leaves 
        // the floor. It reaches a maximum height of 1.85 m above 
        // the floor and is at elevation 0.900 m when he touches down
        // again.

        // Determine:

        // (a) his time of flight (his “hang time”)

        // (b) his horizontal and (c) vertical velocity components at the instant of takeoff

        // (d) his takeoff angle. 

        // (e) For comparison, determine the hang time of a
        // whitetail deer making a jump with center-of-mass elevations
        // y_i = 1.20 m
        // y_max = 2.50 m
        // y_f = 0.700 m

        var xA = new Symbol("xA");
        var yA = new Symbol("yA");

        var vxA = new Symbol("vxA");
        var vyA = new Symbol("vyA");

        var vA = new Symbol("vA");
        var thA = new Symbol("thA");


        var xB = new Symbol("xB");
        var yB = new Symbol("yB");

        var vxB = new Symbol("vxB");
        var vyB = new Symbol("vyB");


        var tAB = new Symbol("tAB");


        var xC = new Symbol("xC");
        var yC = new Symbol("yC");

        var vxC = new Symbol("vxC");
        var vyC = new Symbol("vyC");


        var tBC = new Symbol("tBC");

        var tAC = new Symbol("tAC");

        var ax = new Symbol("ax");
        var ay = new Symbol("ay");

        var Pi = new Symbol("Pi");

        var eqs = And(

            //vxA == vA * Cos(thA),
            //vyA == vA * Sin(thA),

            vxB == vxA + ax * tAB,
            vyB == vyA + ay * tAB,

            xB == xA + vxA * tAB + ax * (tAB ^ 2) / 2,
            yB == yA + vyA * tAB + ay * (tAB ^ 2) / 2,


            vxC == vxB + ax * tBC,
            vyC == vyB + ay * tBC,

            xC == xB + vxB * tBC + ax * (tBC ^ 2) / 2,
            yC == yB + vyB * tBC + ay * (tBC ^ 2) / 2,

            tAC == tAB + tBC,

            // vyA / vxA == Tan(thA),

            Tan(thA) == vyA / vxA,

            ay != 0

        );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        {
            var vals = new List<Equation>()
                {
                    xA == 0,    yA == 1.02, /* vxA vyA vA thA */
                    /* xB */    yB == 1.85, /* vxB            */ vyB == 0,
                    xC == 2.80, yC == 0.9,  /* vxC vyC        */

                    /* tAB tBC */ ax == 0, ay == -9.8, Pi == Math.PI
                };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            {
                eqs
                    .SubstituteEqLs(zeros)

                    .EliminateVariables(thA, vxB, xB, vxC, vyC, vxA, vyA, tAB)

                    .CheckVariable(ay).SimplifyEquation().SimplifyLogical()

                    .EliminateVariable(tBC)

                    .LogicalExpand().SimplifyEquation().CheckVariable(ay).SimplifyLogical()

                    .AssertEqTo(

                        Or(
                            And(ay != 0, tAC == (ay ^ -1) * Sqrt(-2 * ay * (-1 * yA + yB)) + -1 * (ay ^ -1) * Sqrt(2 * ay * (-1 * yB + yC))),
                            And(ay != 0, tAC == (ay ^ -1) * Sqrt(-2 * ay * (-1 * yA + yB)) + (ay ^ -1) * Sqrt(2 * ay * (-1 * yB + yC))),
                            And(ay != 0, tAC == -1 * (ay ^ -1) * Sqrt(-2 * ay * (-1 * yA + yB)) + -1 * (ay ^ -1) * Sqrt(2 * ay * (-1 * yB + yC))),
                            And(ay != 0, tAC == -1 * (ay ^ -1) * Sqrt(-2 * ay * (-1 * yA + yB)) + (ay ^ -1) * Sqrt(2 * ay * (-1 * yB + yC)))))

                    .SubstituteEqLs(vals)

                    .AssertEqTo(

                        Or(
                            tAC == 0.028747849043843032,
                            tAC == -0.85188272280886768,
                            tAC == 0.85188272280886768,
                            tAC == -0.028747849043843032));
            }

            {
                eqs
                    .SubstituteEqLs(zeros)

                    .EliminateVariables(thA, vxB, vxC, xB)

                    .IsolateVariable(vxA)

                    .EliminateVariables(tAC, vyC, tAB, vyA)

                    .SimplifyEquation().CheckVariable(ay)

                    .EliminateVariable(tBC)

                    .LogicalExpand().SimplifyEquation().CheckVariable(ay).SimplifyLogical()

                    .AssertEqTo(

                        Or(
                            And(ay != 0, vxA == xC * ((-1 * Sqrt(-2 * (ay ^ -1) * (-1 * yA + yB)) + -1 * (ay ^ -1) * Sqrt(2 * ay * (-1 * yB + yC))) ^ -1)),
                            And(ay != 0, vxA == xC * ((-1 * Sqrt(-2 * (ay ^ -1) * (-1 * yA + yB)) + (ay ^ -1) * Sqrt(2 * ay * (-1 * yB + yC))) ^ -1)),
                            And(ay != 0, vxA == xC * ((Sqrt(-2 * (ay ^ -1) * (-1 * yA + yB)) + -1 * (ay ^ -1) * Sqrt(2 * ay * (-1 * yB + yC))) ^ -1)),
                            And(ay != 0, vxA == xC * ((Sqrt(-2 * (ay ^ -1) * (-1 * yA + yB)) + (ay ^ -1) * Sqrt(2 * ay * (-1 * yB + yC))) ^ -1))))

                    .SubstituteEqLs(vals)

                    .AssertEqTo(

                        Or(
                            vxA == 97.398591307814215,
                            vxA == -3.286837407346058,
                            vxA == 3.286837407346058,
                            vxA == -97.398591307814215));
            }

            {
                eqs
                    .SubstituteEqLs(zeros)

                    .EliminateVariables(thA, vxA, vxC, vyC, vxB, xB, tAB, tAC, tBC)

                    .SimplifyEquation().CheckVariable(ay).SimplifyLogical()

                    .IsolateVariable(vyA)

                    .LogicalExpand().SimplifyEquation().CheckVariable(ay)

                    .AssertEqTo(
                        Or(
                            And(ay != 0, vyA == ay * Sqrt(-2 * (ay ^ -1) * (-1 * yA + yB))),
                            And(ay != 0, vyA == -1 * ay * Sqrt(-2 * (ay ^ -1) * (-1 * yA + yB)))))

                    .SubstituteEqLs(vals)

                    .AssertEqTo(
                        Or(
                            vyA == -4.0333608814486217,
                            vyA == 4.0333608814486217));
            }

            {
                eqs
                    .SubstituteEqLs(zeros)

                    .EliminateVariables(vxA, vyA, vxB, xB, vxC, tBC, tAB, vyC, tAC)

                    .LogicalExpand()
                    .SimplifyEquation()
                    .SimplifyLogical()
                    .CheckVariable(ay)

                    .AssertEqTo(

                        Or(
                            And(ay != 0, Tan(thA) == -1 * (xC ^ -1) * Sqrt(-2 * ay * (-1 * yA + yB)) * ((ay ^ -1) * Sqrt(-2 * ay * (-1 * yA + yB)) + -1 * Sqrt(2 * (ay ^ -1) * (-1 * yB + yC)))),
                            And(ay != 0, Tan(thA) == -1 * (xC ^ -1) * Sqrt(-2 * ay * (-1 * yA + yB)) * ((ay ^ -1) * Sqrt(-2 * ay * (-1 * yA + yB)) + Sqrt(2 * (ay ^ -1) * (-1 * yB + yC)))),
                            And(ay != 0, Tan(thA) == (xC ^ -1) * Sqrt(-2 * ay * (-1 * yA + yB)) * (-1 * (ay ^ -1) * Sqrt(-2 * ay * (-1 * yA + yB)) + -1 * Sqrt(2 * (ay ^ -1) * (-1 * yB + yC)))),
                            And(ay != 0, Tan(thA) == (xC ^ -1) * Sqrt(-2 * ay * (-1 * yA + yB)) * (-1 * (ay ^ -1) * Sqrt(-2 * ay * (-1 * yA + yB)) + Sqrt(2 * (ay ^ -1) * (-1 * yB + yC))))

                            ))

                    .IsolateVariable(thA)

                    .SubstituteEqLs(vals)

                    .AssertEqTo(
                        Or(
                            thA == 0.88702813023277882,
                            thA == -0.041387227947930878,
                            thA == -0.041387227947930878,
                            thA == 0.88702813023277882));

            }
        }

        DoubleFloat.DoubleFloatTolerance = null;
    }

    [Fact]
    public void PSE_5E_E5_1()
    {
        // A hockey puck having a mass of 0.30 kg slides on the horizontal, 
        // frictionless surface of an ice rink. Two forces act on
        // the puck, as shown in Figure 5.5.The force F1 has a magnitude 
        // of 5.0 N, and the force F2 has a magnitude of 8.0 N.

        // Determine both the magnitude and the direction of the puck’s acceleration.

        // Determine the components of a third force that,
        // when applied to the puck, causes it to have zero acceleration.

        var F = new Symbol("F");
        var th = new Symbol("th");

        var Fx = new Symbol("Fx");
        var Fy = new Symbol("Fy");


        var F1 = new Symbol("F1");
        var th1 = new Symbol("th1");

        var F1x = new Symbol("F1x");
        var F1y = new Symbol("F1y");


        var F2 = new Symbol("F2");
        var th2 = new Symbol("th2");

        var F2x = new Symbol("F2x");
        var F2y = new Symbol("F2y");


        var F3 = new Symbol("F3");
        var th3 = new Symbol("th3");

        var F3x = new Symbol("F3x");
        var F3y = new Symbol("F3y");


        var a = new Symbol("a");

        var ax = new Symbol("ax");
        var ay = new Symbol("ay");

        var m = new Symbol("m");

        var Pi = new Symbol("Pi");

        var eqs = And(

            Fx == F * Cos(th),
            Fy == F * Sin(th),

            Fx == ax * m,
            Fy == ay * m,

            Fx == F1x + F2x + F3x,
            Fy == F1y + F2y + F3y,

            F1x == F1 * Cos(th1), F1y == F1 * Sin(th1),

            F2x == F2 * Cos(th2), F2y == F2 * Sin(th2),

            F3x == F3 * Cos(th3), F3y == F3 * Sin(th3),

            a == Sqrt((ax ^ 2) + (ay ^ 2))

            );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        {
            var vals = new List<Equation>()
                {

                    m == 0.3,

                    F1 == 5.0, th1 == (-20).ToRadians(),
                    F2 == 8.0, th2 == (60).ToRadians(),

                    F3 == 0,

                    Pi == Math.PI
                };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            // a 
            {
                eqs
                    .SubstituteEqLs(zeros)
                    .EliminateVariables(ax, ay, Fx, Fy, F, F1x, F1y, F2x, F2y, F3x, F3y)
                    .DeepSelect(SinCosToTanFunc)
                    .EliminateVariable(th)

                    .AssertEqTo(

                        a ==
                            Sqrt(
                                ((Cos(th1) * F1 + Cos(th2) * F2) ^ 2) * (m ^ -2) +
                                (Cos(Atan(((Cos(th1) * F1 + Cos(th2) * F2) ^ -1) * (F1 * Sin(th1) + F2 * Sin(th2)))) ^ -2) *
                                ((Cos(th1) * F1 + Cos(th2) * F2) ^ 2) *
                                (m ^ -2) *
                                (Sin(Atan(((Cos(th1) * F1 + Cos(th2) * F2) ^ -1) * (F1 * Sin(th1) + F2 * Sin(th2)))) ^ 2))

                    )

                    .SubstituteEqLs(vals)

                    .Substitute(3, 3.0)

                    //.DispLong()

                    .AssertEqTo(a == 33.811874017759315);
            }

            // th
            {
                eqs
                    .SubstituteEqLs(zeros)
                    .EliminateVariables(a, F, Fx, Fy, ax, ay, F1x, F1y, F2x, F2y, F3x, F3y)
                    .DeepSelect(SinCosToTanFunc)
                    .IsolateVariable(th)

                    .AssertEqTo(

                        th == Atan((F1 * Sin(th1) + F2 * Sin(th2)) / (Cos(th1) * F1 + Cos(th2) * F2))

                        )

                    .SubstituteEqLs(vals)

                    .Substitute(3, 3.0)

                    .AssertEqTo(th == 0.54033704850428876);
            }
        }

        {
            var vals = new List<Equation>()
                {

                    m == 0.3,

                    F1 == 5.0, th1 == (-20).ToRadians(),
                    F2 == 8.0, th2 == (60).ToRadians(),

                    ax == 0, ay == 0,

                    Pi == Math.PI
                };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            // F3x
            {
                eqs
                    .SubstituteEqLs(zeros)
                    .EliminateVariables(F3, th3, F3y, F1x, F2x, Fx, F, Fy, F1y, F2y, a)
                    .IsolateVariable(F3x)

                    .AssertEqTo(F3x == -1 * Cos(th1) * F1 + -1 * Cos(th2) * F2)

                    .SubstituteEqLs(vals)

                    .AssertEqTo(F3x == -8.6984631039295444);
            }


            // F3y
            {
                eqs
                    .SubstituteEqLs(zeros)
                    .EliminateVariables(F3, th3, F3x, F1x, F2x, Fx, F, Fy, F1y, F2y, a)
                    .IsolateVariable(F3y)

                    .AssertEqTo(F3y == -1 * F1 * Sin(th1) + -1 * F2 * Sin(th2))

                    .SubstituteEqLs(vals)

                    // .DispLong()

                    .Substitute(3, 3.0)

                    .AssertEqTo(F3y == -5.2181025136471657);
            }
        }
    }

    [Fact]
    public void PSE_5E_E5_4()
    {
        // A traffic light weighing 125 N hangs from a cable tied to two
        // other cables fastened to a support. The upper cables make
        // angles of 37.0° and 53.0° with the horizontal. Find the tension
        // in the three cables.

        var F = new Symbol("F");    // total force magnitude
        var th = new Symbol("th");  // total force direction

        var Fx = new Symbol("Fx");  // total force x-component
        var Fy = new Symbol("Fy");  // total force y-component


        var F1 = new Symbol("F1");
        var th1 = new Symbol("th1");

        var F1x = new Symbol("F1x");
        var F1y = new Symbol("F1y");


        var F2 = new Symbol("F2");
        var th2 = new Symbol("th2");

        var F2x = new Symbol("F2x");
        var F2y = new Symbol("F2y");


        var F3 = new Symbol("F3");
        var th3 = new Symbol("th3");

        var F3x = new Symbol("F3x");
        var F3y = new Symbol("F3y");


        var a = new Symbol("a");

        var ax = new Symbol("ax");
        var ay = new Symbol("ay");

        var m = new Symbol("m");

        var Pi = new Symbol("Pi");

        var eqs = And(

            Fx == F * Cos(th),
            Fy == F * Sin(th),

            Fx == ax * m,
            Fy == ay * m,

            Fx == F1x + F2x + F3x,
            Fy == F1y + F2y + F3y,

            F1x == F1 * Cos(th1), F1y == F1 * Sin(th1),
            F2x == F2 * Cos(th2), F2y == F2 * Sin(th2),
            F3x == F3 * Cos(th3), F3y == F3 * Sin(th3),

            a == Sqrt((ax ^ 2) + (ay ^ 2))

            );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        {
            var vals = new List<Equation>()
                {

                    // m 

                    /* F1 */    th1 == (180 - 37).ToRadians(),  // F1x F1y
                    /* F2 */    th2 == (53).ToRadians(),        // F2x F2y
                    F3 == 125,  th3 == (270).ToRadians(),       // F3x F3y
                    
                    ax == 0,    ay == 0,

                    Pi == Math.PI
                };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            // F1
            {
                eqs
                    .SubstituteEqLs(zeros)
                    .EliminateVariables(Fx, Fy, F, F1x, F1y, F2x, F2y, F2, F3x, F3y, a)
                    .IsolateVariable(F1)

                    .AssertEqTo(F1 == (F3 * Sin(th3) - Cos(th3) * F3 * Sin(th2) / Cos(th2)) / (Cos(th1) * Sin(th2) / Cos(th2) - Sin(th1)))

                    .SubstituteEqLs(vals)

                    .AssertEqTo(F1 == 75.226877894006023);
            }

            // F2
            {
                eqs
                    .SubstituteEqLs(zeros)
                    .EliminateVariables(Fx, Fy, F, F1x, F1y, F2x, F2y, F1, F3x, F3y, a)
                    .IsolateVariable(F2)

                    .AssertEqTo(F2 == (Cos(th3) * F3 * Sin(th1) / Cos(th1) - F3 * Sin(th3)) / (Sin(th2) - Cos(th2) * Sin(th1) / Cos(th1)))

                    .SubstituteEqLs(vals)

                    .AssertEqTo(F2 == 99.829438755911582);
            }

        }
    }

    [Fact]
    public void PSE_5E_E5_6()
    {
        // A crate of mass m is placed on a frictionless inclined plane of
        // angle θ. (a) Determine the acceleration of the crate after it is
        // released.

        // (b) Suppose the crate is released from rest at the top of
        // the incline, and the distance from the front edge of the crate
        // to the bottom is d. How long does it take the front edge to
        // reach the bottom, and what is its speed just as it gets there?

        var F = new Symbol("F");    // total force magnitude
        var th = new Symbol("th");  // total force direction

        var Fx = new Symbol("Fx");  // total force x-component
        var Fy = new Symbol("Fy");  // total force y-component


        var F1 = new Symbol("F1");
        var th1 = new Symbol("th1");

        var F1x = new Symbol("F1x");
        var F1y = new Symbol("F1y");


        var F2 = new Symbol("F2");
        var th2 = new Symbol("th2");

        var F2x = new Symbol("F2x");
        var F2y = new Symbol("F2y");


        //var F3 = new Symbol("F3");
        //var th3 = new Symbol("th3");

        //var F3x = new Symbol("F3x");
        //var F3y = new Symbol("F3y");


        var a = new Symbol("a");

        var ax = new Symbol("ax");
        var ay = new Symbol("ay");

        var m = new Symbol("m");

        var n = new Symbol("n");
        var g = new Symbol("g");

        var incline = new Symbol("incline");

        var Pi = new Symbol("Pi");

        var xA = new Symbol("xA");
        var yA = new Symbol("yA");

        var vxA = new Symbol("vxA");
        var vyA = new Symbol("vyA");

        var vA = new Symbol("vA");
        var thA = new Symbol("thA");


        var xB = new Symbol("xB");
        var yB = new Symbol("yB");

        var vxB = new Symbol("vxB");
        var vyB = new Symbol("vyB");


        var tAB = new Symbol("tAB");

        var d = new Symbol("d");

        var eqs = And(

            Fx == F * Cos(th),
            Fy == F * Sin(th),

            Fx == ax * m,
            Fy == ay * m,

            Fx == F1x + F2x, //+ F3x,
            Fy == F1y + F2y, //+ F3y,

            F1x == F1 * Cos(th1), F1y == F1 * Sin(th1),
            F2x == F2 * Cos(th2), F2y == F2 * Sin(th2),
            //F3x == F3 * Cos(th3), F3y == F3 * Sin(th3),

            a == Sqrt((ax ^ 2) + (ay ^ 2)),

            xB == xA + vxA * tAB + ax * (tAB ^ 2) / 2,

            vxB == vxA + ax * tAB,

            d != 0

            );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        {
            var vals = new List<Equation>()
                {

                    // m 
                    
                    F1 == n,        th1 == 90 * Pi / 180,            // F1x F1y
                    F2 == m * g,    th2 == 270 * Pi / 180 + incline, // F2x F2y
                    //F3 == 125,    th3 == (270).ToRadians(),        // F3x F3y
                    
                    /* ax */  ay == 0,

                    // Pi == Math.PI

                    xA == 0, xB == d, vxA == 0
                };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            // ax
            {
                eqs
                    .SubstituteEqLs(zeros)
                    .EliminateVariables(a, F)
                    .DeepSelect(SinCosToTanFunc)
                    .EliminateVariables(th, Fx, F1x, F2x, Fy, F1y, F2y, vxB, xB)
                    .SubstituteEqLs(vals)
                    .EliminateVariable(n)
                    .IsolateVariable(ax)

                    .AssertEqTo(
                        And(
                            ax == g * Sin(incline),
                            d != 0));
            }

            // tAB
            {
                eqs
                    .SubstituteEqLs(zeros)
                    .EliminateVariables(a, F)
                    .DeepSelect(SinCosToTanFunc)
                    .EliminateVariables(th, Fx, F1x, F2x, Fy, F1y, F2y, ax, vxB)
                    .SubstituteEqLs(vals)
                    .EliminateVariable(n)
                    .IsolateVariable(tAB).LogicalExpand().CheckVariable(d)

                    .AssertEqTo(
                        Or(
                            And(
                                tAB == -Sqrt(2 * d * g * Sin(incline)) / Sin(incline) / g,
                                -g * Sin(incline) / 2 != 0,
                                d != 0),
                            And(
                                tAB == Sqrt(2 * d * g * Sin(incline)) / Sin(incline) / g,
                                -g * Sin(incline) / 2 != 0,
                                d != 0))
                    );
            }

            // vxB
            {
                eqs
                    .SubstituteEqLs(zeros)
                    .EliminateVariables(a, F)
                    .DeepSelect(SinCosToTanFunc)
                    .EliminateVariables(th, Fx, F1x, F2x, Fy, F1y, F2y, ax, tAB)
                    .SubstituteEqLs(vals)
                    .CheckVariable(d)
                    .EliminateVariable(n)

                    .AssertEqTo(
                        Or(
                            And(
                                -g * Sin(incline) / 2 != 0,
                                vxB == -Sqrt(2 * d * g * Sin(incline)),
                                d != 0
                            ),
                            And(
                                -g * Sin(incline) / 2 != 0,
                                vxB == Sqrt(2 * d * g * Sin(incline)),
                                d != 0))
                    );
            }
        }
    }

    [Fact]
    public void PSE_5E_E5_9()
    {
        // When two objects of unequal mass are hung vertically over a
        // frictionless pulley of negligible mass, as shown in Figure
        // 5.15a, the arrangement is called an Atwood machine. The device 
        // is sometimes used in the laboratory to measure the freefall
        // acceleration.
        //
        // Determine the magnitude of the acceleration of the two 
        // objects and the tension in the lightweight cord.

        var F_m1 = new Symbol("F_m1");      // total force on mass 1
        var F_m2 = new Symbol("F_m2");      // total force on mass 2

        var F1_m1 = new Symbol("F1_m1");    // force 1 on mass 1
        var F2_m1 = new Symbol("F2_m1");    // force 2 on mass 1

        var F1_m2 = new Symbol("F1_m2");    // force 1 on mass 2
        var F2_m2 = new Symbol("F2_m2");    // force 2 on mass 2

        var m1 = new Symbol("m1");
        var m2 = new Symbol("m2");

        var a = new Symbol("a");

        var T = new Symbol("T");

        var g = new Symbol("g");


        var eqs = And(

            F_m1 == F1_m1 - F2_m1,
            F_m2 == F2_m2 - F1_m2,

            F_m1 == m1 * a,
            F_m2 == m2 * a,

            F1_m1 == T,
            F2_m1 == m1 * g,

            F1_m2 == T,
            F2_m2 == m2 * g

            );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        {
            var vals = new List<Equation>()
                {
                    m1 == 2.0, m2 == 4.0, g == 9.8
                };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            // a
            {
                eqs
                    .EliminateVariables(F_m1, F_m2, F2_m1, F2_m2, F1_m1, F1_m2, T)
                    .IsolateVariable(a)

                    .AssertEqTo(
                        a == (-1 * g * m1 + g * m2) / (m1 + m2)
                    )

                    .SubstituteEqLs(vals)

                    .AssertEqTo(a == 3.2666666666666666);
            }

            // T
            {
                eqs
                    .EliminateVariables(F_m1, F_m2, F2_m1, F2_m2, F1_m1, F1_m2, a)
                    .IsolateVariable(T)

                    .AssertEqTo(
                        T == 2 * g * m2 / (1 + m2 / m1)
                    )

                    .SubstituteEqLs(vals)

                    .AssertEqTo(
                        T == 26.133333333333333
                    );
            }

        }
    }

    [Fact]
    public void PSE_5E_E5_10()
    {
        // Acceleration of Two Objects Connected by a Cord
        //
        // A ball of mass m1 and a block of mass m2 are attached by a
        // lightweight cord that passes over a frictionless pulley of 
        // negligible mass, as shown in Figure 5.16a. The block lies 
        // on a frictionless incline of angle th. Find the magnitude 
        // of the acceleration of the two objects and the tension in the cord.

        ////////////////////////////////////////////////////////////////////////////////

        var F1_m1 = new Symbol("F1_m1");        // force 1 on mass 1
        var F2_m1 = new Symbol("F2_m1");        // force 2 on mass 1
        var F3_m1 = new Symbol("F3_m1");        // force 3 on mass 1

        var th1_m1 = new Symbol("th1_m1");      // direction of force 1 on mass 1
        var th2_m1 = new Symbol("th2_m1");      // direction of force 2 on mass 1
        var th3_m1 = new Symbol("th3_m1");      // direction of force 3 on mass 1

        var F1x_m1 = new Symbol("F1x_m1");      // x-component of force 1 on mass 1
        var F2x_m1 = new Symbol("F2x_m1");      // x-component of force 2 on mass 1
        var F3x_m1 = new Symbol("F3x_m1");      // x-component of force 3 on mass 1

        var F1y_m1 = new Symbol("F1y_m1");      // y-component of force 1 on mass 1
        var F2y_m1 = new Symbol("F2y_m1");      // y-component of force 2 on mass 1
        var F3y_m1 = new Symbol("F3y_m1");      // y-component of force 3 on mass 1

        var Fx_m1 = new Symbol("Fx_m1");        // x-component of total force on mass 1
        var Fy_m1 = new Symbol("Fy_m1");        // y-component of total force on mass 1

        var ax_m1 = new Symbol("ax_m1");        // x-component of acceleration of mass 1
        var ay_m1 = new Symbol("ay_m1");        // y-component of acceleration of mass 1

        var m1 = new Symbol("m1");

        ////////////////////////////////////////////////////////////////////////////////

        var F1_m2 = new Symbol("F1_m2");        // force 1 on mass 2
        var F2_m2 = new Symbol("F2_m2");        // force 2 on mass 2
        var F3_m2 = new Symbol("F3_m2");        // force 3 on mass 2

        var th1_m2 = new Symbol("th1_m2");      // direction of force 1 on mass 2
        var th2_m2 = new Symbol("th2_m2");      // direction of force 2 on mass 2
        var th3_m2 = new Symbol("th3_m2");      // direction of force 3 on mass 2

        var F1x_m2 = new Symbol("F1x_m2");      // x-component of force 1 on mass 2
        var F2x_m2 = new Symbol("F2x_m2");      // x-component of force 2 on mass 2
        var F3x_m2 = new Symbol("F3x_m2");      // x-component of force 3 on mass 2

        var F1y_m2 = new Symbol("F1y_m2");      // y-component of force 1 on mass 2
        var F2y_m2 = new Symbol("F2y_m2");      // y-component of force 2 on mass 2
        var F3y_m2 = new Symbol("F3y_m2");      // y-component of force 3 on mass 2

        var Fx_m2 = new Symbol("Fx_m2");        // x-component of total force on mass 2
        var Fy_m2 = new Symbol("Fy_m2");        // y-component of total force on mass 2

        var ax_m2 = new Symbol("ax_m2");        // x-component of acceleration of mass 2
        var ay_m2 = new Symbol("ay_m2");        // y-component of acceleration of mass 2

        var m2 = new Symbol("m2");

        ////////////////////////////////////////////////////////////////////////////////

        var incline = new Symbol("incline");

        var T = new Symbol("T");                // tension in cable

        var g = new Symbol("g");                // gravity

        var n = new Symbol("n");                // normal force on block

        var a = new Symbol("a");

        var Pi = new Symbol("Pi");

        var eqs = And(

            ax_m2 == ay_m1,                     // the block moves right as the ball moves up

            ////////////////////////////////////////////////////////////////////////////////

            F1x_m1 == F1_m1 * Cos(th1_m1),
            F2x_m1 == F2_m1 * Cos(th2_m1),
            F3x_m1 == F3_m1 * Cos(th3_m1),

            F1y_m1 == F1_m1 * Sin(th1_m1),
            F2y_m1 == F2_m1 * Sin(th2_m1),
            F3y_m1 == F3_m1 * Sin(th3_m1),

            Fx_m1 == F1x_m1 + F2x_m1 + F3x_m1,
            Fy_m1 == F1y_m1 + F2y_m1 + F3y_m1,

            Fx_m1 == m1 * ax_m1,
            Fy_m1 == m1 * ay_m1,

            ////////////////////////////////////////////////////////////////////////////////

            F1x_m2 == F1_m2 * Cos(th1_m2),
            F2x_m2 == F2_m2 * Cos(th2_m2),
            F3x_m2 == F3_m2 * Cos(th3_m2),

            F1y_m2 == F1_m2 * Sin(th1_m2),
            F2y_m2 == F2_m2 * Sin(th2_m2),
            F3y_m2 == F3_m2 * Sin(th3_m2),

            Fx_m2 == F1x_m2 + F2x_m2 + F3x_m2,
            Fy_m2 == F1y_m2 + F2y_m2 + F3y_m2,

            Fx_m2 == m2 * ax_m2,
            Fy_m2 == m2 * ay_m2,

            ////////////////////////////////////////////////////////////////////////////////

            a == ax_m2

            );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        {
            var vals = new List<Equation>()
                {
                    ax_m1 == 0,                         // ball  moves vertically
                    ay_m2 == 0,                         // block moves horizontally

                    F1_m1 == T,
                    F2_m1 == m1 * g,
                    F3_m1 == 0,

                    th1_m1 == 90 * Pi / 180,            // force 1 is straight up
                    th2_m1 == 270 * Pi / 180,           // force 2 is straight down

                    F1_m2 == n,
                    F2_m2 == T,
                    F3_m2 == m2 * g,

                    th1_m2 == 90 * Pi / 180,            // force 1 is straight up
                    th2_m2 == 180 * Pi / 180,           // force 2 is straight down
                    th3_m2 == 270 * Pi / 180 + incline  // force 3 direction

                };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            // a
            {
                eqs
                    .SubstituteEqLs(vals)

                    .EliminateVariables(
                        F1x_m1, F2x_m1, F3x_m1,
                        F1y_m1, F2y_m1, F3y_m1,

                        Fx_m1, Fy_m1,

                        F1x_m2, F2x_m2, F3x_m2,
                        F1y_m2, F2y_m2, F3y_m2,

                        Fx_m2, Fy_m2,

                        ax_m2, n, T, ay_m1
                    )

                    .AssertEqTo(

                        a == (g * m1 - g * m2 * Sin(incline)) / (-m1 - m2)

                    )

                    .SubstituteEq(m1 == 10.0)
                    .SubstituteEq(m2 == 5.0)
                    .SubstituteEq(incline == 45 * Math.PI / 180)
                    .SubstituteEq(g == 9.8)

                    .AssertEqTo(a == -4.2234511814572784);
            }

            // T
            {
                eqs
                    .SubstituteEqLs(vals)

                    .EliminateVariables(
                        F1x_m1, F2x_m1, F3x_m1,
                        F1y_m1, F2y_m1, F3y_m1,

                        Fx_m1, Fy_m1,

                        F1x_m2, F2x_m2, F3x_m2,
                        F1y_m2, F2y_m2, F3y_m2,

                        Fx_m2, Fy_m2,

                        ax_m2, n, a, ay_m1
                    )

                    .IsolateVariable(T)
                    .RationalizeExpression()

                    .AssertEqTo(

                        T == m1 * (-g * m2 - g * m2 * Sin(incline)) / (-m1 - m2)

                    );
            }
        }
    }

    [Fact]
    public void PSE_5E_E5_10_Obj3()
    {
        // Acceleration of Two Objects Connected by a Cord - Obj3
        //
        // A ball of mass m1 and a block of mass m2 are attached by a
        // lightweight cord that passes over a frictionless pulley of 
        // negligible mass, as shown in: 
        //
        //      http://i.imgur.com/XMHM6On.png
        //
        // The block lies on a frictionless incline of angle th.
        //
        // Find the magnitude of the acceleration of the two objects
        // and the tension in the cord.

        var bal = new Obj2("bal");
        var blk = new Obj3("blk");

        var th = new Symbol("th");

        var T = new Symbol("T");                // tension in cable
        var g = new Symbol("g");                // gravity
        var n = new Symbol("n");                // normal force on block
        var a = new Symbol("a");

        var m1 = new Symbol("m1");
        var m2 = new Symbol("m2");
        _ = new Symbol("Pi");

        var eqs = And(

            blk.ax == bal.ay,                   // the block moves right as the ball moves up

            a == blk.ax,

            bal.Equations(),
            blk.Equations()

            );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        var vals = new List<Equation>
            {
                bal.ax == 0,

                bal.m == m1,

                bal.F1 == T,            bal.th1 == (90).ToRadians(),                // force 1 is straight up
                bal.F2 == m1 * g,       bal.th2 == (270).ToRadians(),               // force 2 is straight down

                blk.ay == 0,

                blk.m == m2,

                blk.F1 == n,            blk.th1 == (90).ToRadians(),                // force 1 is straight up
                blk.F2 == T,            blk.th2 == (180).ToRadians(),               // force 2 is straight down
                blk.F3 == m2 * g,       blk.th3 == (270).ToRadians() + th           // force 3 direction
            };

        // a
        eqs
            .SubstituteEqLs(vals)

            .EliminateVariables(

                bal.ΣFx, bal.F1x, bal.F2x,
                bal.ΣFy, bal.F1y, bal.F2y,

                blk.ΣFx, blk.F1x, blk.F2x, blk.F3x,
                blk.ΣFy, blk.F1y, blk.F2y, blk.F3y,

                blk.ax, bal.ay,

                T, n
            )

            .IsolateVariable(a)

            .AssertEqTo(

                a == (g * m1 - g * m2 * Sin(th)) / (-m1 - m2)

            );

        // T
        eqs
            .SubstituteEqLs(vals)

            .EliminateVariables(

                bal.ΣFx, bal.F1x, bal.F2x,
                bal.ΣFy, bal.F1y, bal.F2y,

                blk.ΣFx, blk.F1x, blk.F2x, blk.F3x,
                blk.ΣFy, blk.F1y, blk.F2y, blk.F3y,

                blk.ax, bal.ay,

                a, n
            )

        .IsolateVariable(T)

        .RationalizeExpression()

        .AssertEqTo(

            T == m1 * (-g * m2 - g * m2 * Sin(th)) / (-m1 - m2)

        );
    }

    [Fact]
    public void PSE_5E_E5_12()
    {
        // Experimental Determination of μs and μk
        //
        // The following is a simple method of measuring coefficients of
        // friction: Suppose a block is placed on a rough surface
        // inclined relative to the horizontal, as shown in Figure 5.19. 
        // The incline angle is increased until the block starts to move. 
        // Let us show that by measuring the critical angle θ_c at which this
        // slipping just occurs, we can obtain μs.

        ////////////////////////////////////////////////////////////////////////////////

        var F1_m1 = new Symbol("F1_m1");        // force 1 on mass 1
        var F2_m1 = new Symbol("F2_m1");        // force 2 on mass 1
        var F3_m1 = new Symbol("F3_m1");        // force 3 on mass 1

        var th1_m1 = new Symbol("th1_m1");      // direction of force 1 on mass 1
        var th2_m1 = new Symbol("th2_m1");      // direction of force 2 on mass 1
        var th3_m1 = new Symbol("th3_m1");      // direction of force 3 on mass 1

        var F1x_m1 = new Symbol("F1x_m1");      // x-component of force 1 on mass 1
        var F2x_m1 = new Symbol("F2x_m1");      // x-component of force 2 on mass 1
        var F3x_m1 = new Symbol("F3x_m1");      // x-component of force 3 on mass 1

        var F1y_m1 = new Symbol("F1y_m1");      // y-component of force 1 on mass 1
        var F2y_m1 = new Symbol("F2y_m1");      // y-component of force 2 on mass 1
        var F3y_m1 = new Symbol("F3y_m1");      // y-component of force 3 on mass 1

        var Fx_m1 = new Symbol("Fx_m1");        // x-component of total force on mass 1
        var Fy_m1 = new Symbol("Fy_m1");        // y-component of total force on mass 1

        var ax_m1 = new Symbol("ax_m1");        // x-component of acceleration of mass 1
        var ay_m1 = new Symbol("ay_m1");        // y-component of acceleration of mass 1

        var m1 = new Symbol("m1");

        ////////////////////////////////////////////////////////////////////////////////

        var incline = new Symbol("incline");

        var f_s = new Symbol("f_s");            // force due to static friction

        var g = new Symbol("g");                // gravity

        var n = new Symbol("n");                // normal force on block

        var a = new Symbol("a");

        var Pi = new Symbol("Pi");

        var mu_s = new Symbol("mu_s");          // coefficient of static friction

        var eqs = And(

            F1x_m1 == F1_m1 * Cos(th1_m1),
            F2x_m1 == F2_m1 * Cos(th2_m1),
            F3x_m1 == F3_m1 * Cos(th3_m1),

            F1y_m1 == F1_m1 * Sin(th1_m1),
            F2y_m1 == F2_m1 * Sin(th2_m1),
            F3y_m1 == F3_m1 * Sin(th3_m1),

            Fx_m1 == F1x_m1 + F2x_m1 + F3x_m1,
            Fy_m1 == F1y_m1 + F2y_m1 + F3y_m1,

            Fx_m1 == m1 * ax_m1,
            Fy_m1 == m1 * ay_m1,

            f_s == mu_s * n

            );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        {
            var vals = new List<Equation>()
                {
                    ax_m1 == 0,
                    ay_m1 == 0,

                    F1_m1 == n,
                    F2_m1 == f_s,
                    F3_m1 == m1 * g,

                    th1_m1 == 90 * Pi / 180,            // force 1 is straight up
                    th2_m1 == 180 * Pi / 180,           // force 2 is straight down
                    th3_m1 == 270 * Pi / 180 + incline  // force 3 direction 
                };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            // mu_s
            {
                eqs
                    .SubstituteEqLs(vals)

                    .EliminateVariables(
                        F1x_m1, F2x_m1, F3x_m1,
                        F1y_m1, F2y_m1, F3y_m1,
                        Fx_m1, Fy_m1,
                        f_s, n
                    )
                    .IsolateVariable(mu_s)

                    .DeepSelect(SinCosToTanFunc)

                    .AssertEqTo(mu_s == Tan(incline));
            }
        }
    }

    [Fact]
    public void PSE_5E_E5_13()
    {
        // The Sliding Hockey Puck
        //
        // A hockey puck on a frozen pond is given an initial speed of
        // 20.0  m/s. If the puck always remains on the ice and slides
        // 115 m before coming to rest, determine the coefficient of
        // kinetic friction between the puck and ice.

        ////////////////////////////////////////////////////////////////////////////////

        var s = new Symbol("s");                // displacement
        var u = new Symbol("u");                // initial velocity
        var v = new Symbol("v");                // final velocity
        var a = new Symbol("a");                // acceleration
        var t = new Symbol("t");                // time elapsed

        var F1_m1 = new Symbol("F1_m1");        // force 1 on mass 1
        var F2_m1 = new Symbol("F2_m1");        // force 2 on mass 1
        var F3_m1 = new Symbol("F3_m1");        // force 3 on mass 1

        var th1_m1 = new Symbol("th1_m1");      // direction of force 1 on mass 1
        var th2_m1 = new Symbol("th2_m1");      // direction of force 2 on mass 1
        var th3_m1 = new Symbol("th3_m1");      // direction of force 3 on mass 1

        var F1x_m1 = new Symbol("F1x_m1");      // x-component of force 1 on mass 1
        var F2x_m1 = new Symbol("F2x_m1");      // x-component of force 2 on mass 1
        var F3x_m1 = new Symbol("F3x_m1");      // x-component of force 3 on mass 1

        var F1y_m1 = new Symbol("F1y_m1");      // y-component of force 1 on mass 1
        var F2y_m1 = new Symbol("F2y_m1");      // y-component of force 2 on mass 1
        var F3y_m1 = new Symbol("F3y_m1");      // y-component of force 3 on mass 1

        var Fx_m1 = new Symbol("Fx_m1");        // x-component of total force on mass 1
        var Fy_m1 = new Symbol("Fy_m1");        // y-component of total force on mass 1

        var ax_m1 = new Symbol("ax_m1");        // x-component of acceleration of mass 1
        var ay_m1 = new Symbol("ay_m1");        // y-component of acceleration of mass 1

        var m1 = new Symbol("m1");

        ////////////////////////////////////////////////////////////////////////////////

        // var incline = new Symbol("incline");

        var f_s = new Symbol("f_s");            // force due to static friction

        var f_k = new Symbol("f_k");            // force due to kinetic friction

        var g = new Symbol("g");                // gravity

        var n = new Symbol("n");                // normal force on block

        // var a = new Symbol("a");

        var Pi = new Symbol("Pi");

        var mu_s = new Symbol("mu_s");          // coefficient of static friction

        var mu_k = new Symbol("mu_k");          // coefficient of kinetic friction

        var eqs = And(

            a == ax_m1,

            v == u + a * t,
            s == (u + v) * t / 2,

            F1x_m1 == F1_m1 * Cos(th1_m1),
            F2x_m1 == F2_m1 * Cos(th2_m1),
            F3x_m1 == F3_m1 * Cos(th3_m1),

            F1y_m1 == F1_m1 * Sin(th1_m1),
            F2y_m1 == F2_m1 * Sin(th2_m1),
            F3y_m1 == F3_m1 * Sin(th3_m1),

            Fx_m1 == F1x_m1 + F2x_m1 + F3x_m1,
            Fy_m1 == F1y_m1 + F2y_m1 + F3y_m1,

            Fx_m1 == m1 * ax_m1,
            Fy_m1 == m1 * ay_m1,

            f_s == mu_s * n,
            f_k == mu_k * n

            );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        {
            var symbolic_vals = new List<Equation>()
                {
                    F1_m1 == n,
                    F2_m1 == f_k,
                    F3_m1 == m1 * g,

                    th1_m1 == 90 * Pi / 180,            // force 1 is straight up
                    th2_m1 == 180 * Pi / 180,           // force 2 is left
                    th3_m1 == 270 * Pi / 180            // force 3 is straight down
                };

            var vals = new List<Equation>()
                {
                    //ax_m1 == 0,
                    ay_m1 == 0,

                    s == 115,
                    u == 20,
                    v == 0,

                    g == 9.8
                };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            // mu_k
            {
                eqs
                    .SubstituteEqLs(zeros)
                    .SubstituteEqLs(symbolic_vals)

                    .EliminateVariables(
                        t,
                        F1x_m1, F2x_m1, F3x_m1,
                        F1y_m1, F2y_m1, F3y_m1,

                        Fx_m1, Fy_m1,

                        f_s, f_k,

                        n,

                        ax_m1, a

                        )

                    .IsolateVariable(mu_k)

                    .AssertEqTo(mu_k == (u ^ 2) / s / g / 2)

                    .SubstituteEqLs(vals)

                    .AssertEqTo(mu_k == 0.17746228926353147);
            }
        }
    }

    [Fact]
    public void PSE_5E_E5_14()
    {
        // Acceleration of Two Connected Objects When Friction Is Present
        //
        // A block of mass m1 on a rough, horizontal surface is connected
        // to a ball of mass m2 by a lightweight cord over a lightweight,
        // frictionless pulley, as shown:
        //
        // http://i.imgur.com/0fHOmGJ.png
        //
        // A force of magnitude F at an angle th with the horizontal is
        // applied to the block as shown. The coefficient of kinetic
        // friction between the block and surface is mu_k.
        // 
        // Determine the magnitude of the acceleration of the two objects.

        ////////////////////////////////////////////////////////////////////////////////

        var F1_m1 = new Symbol("F1_m1");        // force 1 on mass 1
        var F2_m1 = new Symbol("F2_m1");        // force 2 on mass 1
        var F3_m1 = new Symbol("F3_m1");        // force 3 on mass 1
        var F4_m1 = new Symbol("F4_m1");        // force 4 on mass 1
        var F5_m1 = new Symbol("F5_m1");        // force 5 on mass 1

        var th1_m1 = new Symbol("th1_m1");      // direction of force 1 on mass 1
        var th2_m1 = new Symbol("th2_m1");      // direction of force 2 on mass 1
        var th3_m1 = new Symbol("th3_m1");      // direction of force 3 on mass 1
        var th4_m1 = new Symbol("th4_m1");      // direction of force 4 on mass 1
        var th5_m1 = new Symbol("th5_m1");      // direction of force 5 on mass 1

        var F1x_m1 = new Symbol("F1x_m1");      // x-component of force 1 on mass 1
        var F2x_m1 = new Symbol("F2x_m1");      // x-component of force 2 on mass 1
        var F3x_m1 = new Symbol("F3x_m1");      // x-component of force 3 on mass 1
        var F4x_m1 = new Symbol("F4x_m1");      // x-component of force 4 on mass 1
        var F5x_m1 = new Symbol("F5x_m1");      // x-component of force 5 on mass 1

        var F1y_m1 = new Symbol("F1y_m1");      // y-component of force 1 on mass 1
        var F2y_m1 = new Symbol("F2y_m1");      // y-component of force 2 on mass 1
        var F3y_m1 = new Symbol("F3y_m1");      // y-component of force 3 on mass 1
        var F4y_m1 = new Symbol("F4y_m1");      // y-component of force 4 on mass 1
        var F5y_m1 = new Symbol("F5y_m1");      // y-component of force 5 on mass 1

        var Fx_m1 = new Symbol("Fx_m1");        // x-component of total force on mass 1
        var Fy_m1 = new Symbol("Fy_m1");        // y-component of total force on mass 1

        var ax_m1 = new Symbol("ax_m1");        // x-component of acceleration of mass 1
        var ay_m1 = new Symbol("ay_m1");        // y-component of acceleration of mass 1

        var m1 = new Symbol("m1");

        ////////////////////////////////////////////////////////////////////////////////

        var F1_m2 = new Symbol("F1_m2");        // force 1 on mass 2
        var F2_m2 = new Symbol("F2_m2");        // force 2 on mass 2

        var th1_m2 = new Symbol("th1_m2");      // direction of force 1 on mass 2
        var th2_m2 = new Symbol("th2_m2");      // direction of force 2 on mass 2

        var F1x_m2 = new Symbol("F1x_m2");      // x-component of force 1 on mass 2
        var F2x_m2 = new Symbol("F2x_m2");      // x-component of force 2 on mass 2

        var F1y_m2 = new Symbol("F1y_m2");      // y-component of force 1 on mass 2
        var F2y_m2 = new Symbol("F2y_m2");      // y-component of force 2 on mass 2

        var Fx_m2 = new Symbol("Fx_m2");        // x-component of total force on mass 2
        var Fy_m2 = new Symbol("Fy_m2");        // y-component of total force on mass 2

        var ax_m2 = new Symbol("ax_m2");        // x-component of acceleration of mass 2
        var ay_m2 = new Symbol("ay_m2");        // y-component of acceleration of mass 2

        var m2 = new Symbol("m2");

        ////////////////////////////////////////////////////////////////////////////////

        var F = new Symbol("F");                // force applied at angle on block
        var th = new Symbol("th");              // angle of force applied on block
        var T = new Symbol("T");                // tension in cable
        var g = new Symbol("g");                // gravity
        var n = new Symbol("n");                // normal force on block

        var a = new Symbol("a");

        var Pi = new Symbol("Pi");

        var f_k = new Symbol("f_k");            // force due to kinetic friction

        var mu_k = new Symbol("mu_k");          // coefficient of kinetic friction

        var eqs = And(

            ax_m1 == ay_m2,                     // the block moves right as the ball moves up

            ////////////////////////////////////////////////////////////////////////////////

            F1x_m1 == F1_m1 * Cos(th1_m1),
            F2x_m1 == F2_m1 * Cos(th2_m1),
            F3x_m1 == F3_m1 * Cos(th3_m1),
            F4x_m1 == F4_m1 * Cos(th4_m1),
            F5x_m1 == F5_m1 * Cos(th5_m1),

            F1y_m1 == F1_m1 * Sin(th1_m1),
            F2y_m1 == F2_m1 * Sin(th2_m1),
            F3y_m1 == F3_m1 * Sin(th3_m1),
            F4y_m1 == F4_m1 * Sin(th4_m1),
            F5y_m1 == F5_m1 * Sin(th5_m1),

            Fx_m1 == F1x_m1 + F2x_m1 + F3x_m1 + F4x_m1 + F5x_m1,
            Fy_m1 == F1y_m1 + F2y_m1 + F3y_m1 + F4y_m1 + F5y_m1,

            Fx_m1 == m1 * ax_m1,
            Fy_m1 == m1 * ay_m1,

            ////////////////////////////////////////////////////////////////////////////////

            F1x_m2 == F1_m2 * Cos(th1_m2),
            F2x_m2 == F2_m2 * Cos(th2_m2),

            F1y_m2 == F1_m2 * Sin(th1_m2),
            F2y_m2 == F2_m2 * Sin(th2_m2),

            Fx_m2 == F1x_m2 + F2x_m2,
            Fy_m2 == F1y_m2 + F2y_m2,

            Fx_m2 == m2 * ax_m2,
            Fy_m2 == m2 * ay_m2,

            ////////////////////////////////////////////////////////////////////////////////

            f_k == mu_k * n,

            a == ax_m1

            );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        {
            var vals = new List<Equation>()
                {
                    ay_m1 == 0,                                     // block moves horizontally
                    ax_m2 == 0,                                     // ball moves vertically
                    
                    F1_m1 == F,         th1_m1 == th,               // force applied at angle
                    F2_m1 == n,         th2_m1 == 90 * Pi / 180,    // normal force is straight up
                    F3_m1 == T,         th3_m1 == 180 * Pi / 180,   // force due to cord is left
                    F4_m1 == f_k,       th4_m1 == 180 * Pi / 180,   // force due to friction is left
                    F5_m1 == m1 * g,    th5_m1 == 270 * Pi / 180,   // force due to gravity is down
                    
                    F1_m2 == T,         th1_m2 == 90 * Pi / 180,    // force due to cord is up
                    F2_m2 == m2 * g,    th2_m2 == 270 * Pi / 180    // force due to gravity is down                                
                };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            // a
            {
                eqs
                    .SubstituteEqLs(vals)

                    .EliminateVariables(
                        ax_m1,

                        Fx_m1, Fy_m1,
                        Fx_m2, Fy_m2,

                        F1x_m1, F2x_m1, F3x_m1, F4x_m1, F5x_m1,
                        F1y_m1, F2y_m1, F3y_m1, F4y_m1, F5y_m1,

                        F1x_m2, F2x_m2,
                        F1y_m2, F2y_m2,

                        T, f_k, n,

                        ay_m2
                    )

                    .AssertEqTo(

                        a == (g * m2 + g * m1 * mu_k - F * mu_k * Sin(th) - Cos(th) * F) / (-m1 - m2)

                    );
            }
        }
    }

    [Fact]
    public void PSE_5E_E5_14_Obj5()
    {
        // Acceleration of Two Connected Objects When Friction Is Present - Obj5
        //
        // A block of mass m1 on a rough, horizontal surface is connected
        // to a ball of mass m2 by a lightweight cord over a lightweight,
        // frictionless pulley, as shown:
        //
        // http://i.imgur.com/0fHOmGJ.png
        //
        // A force of magnitude F at an angle th with the horizontal is
        // applied to the block as shown. The coefficient of kinetic
        // friction between the block and surface is mu_k.
        // 
        // Determine the magnitude of the acceleration of the two objects.

        var blk = new Obj5("blk");
        var bal = new Obj3("bal");

        var F = new Symbol("F");                // force applied at angle on block
        var th = new Symbol("th");              // angle of force applied on block
        var T = new Symbol("T");                // tension in cable
        var g = new Symbol("g");                // gravity
        var n = new Symbol("n");                // normal force on block

        var a = new Symbol("a");

        var Pi = new Symbol("Pi");

        var f_k = new Symbol("f_k");            // force due to kinetic friction

        var mu_k = new Symbol("mu_k");          // coefficient of kinetic friction

        var m1 = new Symbol("m1");
        var m2 = new Symbol("m2");

        var eqs = And(

            blk.ax == bal.ay,                   // the block moves right as the ball moves up

            blk.Equations(),
            bal.Equations(),

            f_k == mu_k * n,

            a == blk.ax

            );

        var vals = new List<Equation>()
            {
                blk.ay == 0,                                        // block moves horizontally
                    
                blk.F1 == F,            blk.th1 == th,              // block moves horizontally
                blk.F2 == n,            blk.th2 == 90 * Pi / 180,   // normal force is straight up
                blk.F3 == T,            blk.th3 == 180 * Pi / 180,  // force due to cord is left
                blk.F4 == f_k,          blk.th4 == 180 * Pi / 180,  // force due to friction is left
                blk.F5 == blk.m * g,    blk.th5 == 270 * Pi / 180,  // force due to gravity is down

                bal.ax == 0,                                        // ball moves vertically

                bal.F1 == T,            bal.th1 == 90 * Pi / 180,   // force due to cord is up
                bal.F2 == bal.m * g,    bal.th2 == 270 * Pi / 180,  // force due to gravity is down
                bal.F3 == 0,

                blk.m == m1,
                bal.m == m2
            };

        // a

        eqs
            .SubstituteEqLs(vals)

            .EliminateVariables(

                blk.ax,

                blk.ΣFx, blk.ΣFy,
                bal.ΣFx, bal.ΣFy,

                blk.F1x, blk.F2x, blk.F3x, blk.F4x, blk.F5x,
                blk.F1y, blk.F2y, blk.F3y, blk.F4y, blk.F5y,

                bal.F1x, bal.F2x, bal.F3x,
                bal.F1y, bal.F2y, bal.F3y,

                T, f_k, n,

                bal.ay
            )

            .AssertEqTo(

                a == (g * m2 + g * m1 * mu_k - F * mu_k * Sin(th) - Cos(th) * F) / (-m1 - m2)

            );

    }

    [Fact]
    public void PSE_5E_P5_25()
    {
        // A bag of cement of weight F_g hangs from three wires as
        // shown in http://i.imgur.com/f5JpLjY.png
        //  
        // Two of the wires make angles th1 and th2 with the horizontal.
        // If the system is in equilibrium, show that the tension in the
        // left -hand wire is:
        //
        //          T1 == F_g Cos(th2) / Sin(th1 + th2)

        ////////////////////////////////////////////////////////////////////////////////

        var F1_m1 = new Symbol("F1_m1");        // force 1 on mass 1
        var F2_m1 = new Symbol("F2_m1");        // force 2 on mass 1
        var F3_m1 = new Symbol("F3_m1");        // force 3 on mass 1

        var th1_m1 = new Symbol("th1_m1");      // direction of force 1 on mass 1
        var th2_m1 = new Symbol("th2_m1");      // direction of force 2 on mass 1
        var th3_m1 = new Symbol("th3_m1");      // direction of force 3 on mass 1

        var F1x_m1 = new Symbol("F1x_m1");      // x-component of force 1 on mass 1
        var F2x_m1 = new Symbol("F2x_m1");      // x-component of force 2 on mass 1
        var F3x_m1 = new Symbol("F3x_m1");      // x-component of force 3 on mass 1

        var F1y_m1 = new Symbol("F1y_m1");      // y-component of force 1 on mass 1
        var F2y_m1 = new Symbol("F2y_m1");      // y-component of force 2 on mass 1
        var F3y_m1 = new Symbol("F3y_m1");      // y-component of force 3 on mass 1

        var Fx_m1 = new Symbol("Fx_m1");        // x-component of total force on mass 1
        var Fy_m1 = new Symbol("Fy_m1");        // y-component of total force on mass 1

        var ax_m1 = new Symbol("ax_m1");        // x-component of acceleration of mass 1
        var ay_m1 = new Symbol("ay_m1");        // y-component of acceleration of mass 1

        var m1 = new Symbol("m1");

        ////////////////////////////////////////////////////////////////////////////////

        var g = new Symbol("g");                // gravity

        var a = new Symbol("a");

        var Pi = new Symbol("Pi");

        var T1 = new Symbol("T1");
        var T2 = new Symbol("T2");
        var T3 = new Symbol("T3");

        var th1 = new Symbol("th1");
        var th2 = new Symbol("th2");

        var eqs = And(

            F1x_m1 == F1_m1 * Cos(th1_m1),
            F2x_m1 == F2_m1 * Cos(th2_m1),
            F3x_m1 == F3_m1 * Cos(th3_m1),

            F1y_m1 == F1_m1 * Sin(th1_m1),
            F2y_m1 == F2_m1 * Sin(th2_m1),
            F3y_m1 == F3_m1 * Sin(th3_m1),

            Fx_m1 == F1x_m1 + F2x_m1 + F3x_m1,
            Fy_m1 == F1y_m1 + F2y_m1 + F3y_m1,

            Fx_m1 == m1 * ax_m1,
            Fy_m1 == m1 * ay_m1

            );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        {
            var vals = new List<Equation>()
                {
                    ax_m1 == 0,
                    ay_m1 == 0,

                    F1_m1 == T2,
                    F2_m1 == T1,
                    F3_m1 == m1 * g,

                    th1_m1 == th2,
                    th2_m1 == 180 * Pi / 180 - th1,
                    th3_m1 == 270 * Pi / 180
                };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            // T1
            {
                eqs
                    .SubstituteEqLs(vals)

                    .EliminateVariables(

                        F1x_m1, F2x_m1, F3x_m1,
                        F1y_m1, F2y_m1, F3y_m1,

                        Fx_m1, Fy_m1,

                        T2
                    )

                    .IsolateVariable(T1)

                    .RationalizeExpression()

                    .DeepSelect(SumDifferenceFormulaAFunc)

                    .AssertEqTo(
                        T1 == Cos(th2) * g * m1 / Sin(th1 + th2)
                    );
            }
        }
    }

    [Fact]
    public void PSE_5E_P5_25_Obj()
    {
        // A bag of cement of weight F_g hangs from three wires as
        // shown in http://i.imgur.com/f5JpLjY.png
        //  
        // Two of the wires make angles th1 and th2 with the horizontal.
        // If the system is in equilibrium, show that the tension in the
        // left -hand wire is:
        //
        //          T1 == F_g Cos(th2) / Sin(th1 + th2)

        var bag = new Obj3("bag");

        var T1 = new Symbol("T1");
        var T2 = new Symbol("T2");
        var T3 = new Symbol("T3");

        var F_g = new Symbol("F_g");

        var th1 = new Symbol("th1");
        var th2 = new Symbol("th2");

        var eqs = bag.Equations();

        var vals = new List<Equation>()
            {
                bag.ax == 0,
                bag.ay == 0,

                bag.F1 == T1,       bag.th1 == (180).ToRadians() - th1,
                bag.F2 == T2,       bag.th2 == th2,
                bag.F3 == F_g,      bag.th3 == (270).ToRadians()
            };

        eqs
            .SubstituteEqLs(vals)

            .EliminateVariables(

                bag.ΣFx, bag.F1x, bag.F2x, bag.F3x,
                bag.ΣFy, bag.F1y, bag.F2y, bag.F3y,

                T2

            )

            .IsolateVariable(T1)

            .RationalizeExpression()

            .DeepSelect(SumDifferenceFormulaAFunc)

            .AssertEqTo(T1 == Cos(th2) * F_g / Sin(th1 + th2));

    }

    [Fact]
    public void PSE_5E_P5_31()
    {
        // Two people pull as hard as they can on ropes attached
        // to a boat that has a mass of 200 kg. If they pull in the
        // same direction, the boat has an acceleration of
        // 1.52 m/s^2 to the right. If they pull in opposite directions,
        // the boat has an acceleration of 0.518 m/s^2 to the
        // left.
        // 
        // What is the force exerted by each person on the
        // boat? (Disregard any other forces on the boat.)

        ////////////////////////////////////////////////////////////////////////////////

        var F1_m1 = new Symbol("F1_m1");        // force 1 on mass 1
        var F2_m1 = new Symbol("F2_m1");        // force 2 on mass 1

        var th1_m1 = new Symbol("th1_m1");      // direction of force 1 on mass 1
        var th2_m1 = new Symbol("th2_m1");      // direction of force 2 on mass 1

        var F1x_m1 = new Symbol("F1x_m1");      // x-component of force 1 on mass 1
        var F2x_m1 = new Symbol("F2x_m1");      // x-component of force 2 on mass 1

        var F1y_m1 = new Symbol("F1y_m1");      // y-component of force 1 on mass 1
        var F2y_m1 = new Symbol("F2y_m1");      // y-component of force 2 on mass 1

        var Fx_m1 = new Symbol("Fx_m1");        // x-component of total force on mass 1
        var Fy_m1 = new Symbol("Fy_m1");        // y-component of total force on mass 1


        var ax_m1 = new Symbol("ax_m1");        // x-component of acceleration of mass 1
        var ay_m1 = new Symbol("ay_m1");        // y-component of acceleration of mass 1

        var m1 = new Symbol("m1");

        ////////////////////////////////////////////////////////////////////////////////

        var F1_m2 = new Symbol("F1_m2");        // force 1 on mass 2
        var F2_m2 = new Symbol("F2_m2");        // force 2 on mass 2

        var th1_m2 = new Symbol("th1_m2");      // direction of force 1 on mass 2
        var th2_m2 = new Symbol("th2_m2");      // direction of force 2 on mass 2

        var F1x_m2 = new Symbol("F1x_m2");      // x-component of force 1 on mass 2
        var F2x_m2 = new Symbol("F2x_m2");      // x-component of force 2 on mass 2

        var F1y_m2 = new Symbol("F1y_m2");      // y-component of force 1 on mass 2
        var F2y_m2 = new Symbol("F2y_m2");      // y-component of force 2 on mass 2

        var Fx_m2 = new Symbol("Fx_m2");        // x-component of total force on mass 2
        var Fy_m2 = new Symbol("Fy_m2");        // y-component of total force on mass 2

        var ax_m2 = new Symbol("ax_m2");        // x-component of acceleration of mass 2
        var ay_m2 = new Symbol("ay_m2");        // y-component of acceleration of mass 2

        var m2 = new Symbol("m2");

        ////////////////////////////////////////////////////////////////////////////////

        var Pi = new Symbol("Pi");

        var T1 = new Symbol("T1");
        var T2 = new Symbol("T2");

        var eqs = And(

            m1 == m2,

            F1x_m1 == F1_m1 * Cos(th1_m1),
            F2x_m1 == F2_m1 * Cos(th2_m1),

            F1y_m1 == F1_m1 * Sin(th1_m1),
            F2y_m1 == F2_m1 * Sin(th2_m1),

            Fx_m1 == F1x_m1 + F2x_m1,
            Fy_m1 == F1y_m1 + F2y_m1,

            Fx_m1 == m1 * ax_m1,
            Fy_m1 == m1 * ay_m1,


            F1x_m2 == F1_m2 * Cos(th1_m2),
            F2x_m2 == F2_m2 * Cos(th2_m2),

            F1y_m2 == F1_m2 * Sin(th1_m2),
            F2y_m2 == F2_m2 * Sin(th2_m2),

            Fx_m2 == F1x_m2 + F2x_m2,
            Fy_m2 == F1y_m2 + F2y_m2,

            Fx_m2 == m2 * ax_m2,
            Fy_m2 == m2 * ay_m2
            );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        {
            var vals = new List<Equation>()
                {
                    ay_m1 == 0,

                    F1_m1 == T1,    th1_m1 == 0,
                    F2_m1 == T2,    th2_m1 == 0,

                    ay_m2 == 0,

                    F1_m2 == T1,    th1_m2 == 180 * Pi / 180,
                    F2_m2 == T2,    th2_m2 == 0
                };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            var numerical_vals = new List<Equation>()
                {
                    m1 == 200,

                    ax_m1 == 1.52,
                    ax_m2 == -0.518
                };

            // T1
            {
                eqs
                    .SubstituteEqLs(vals)

                    .EliminateVariables(

                        m2,

                        F1x_m1, F2x_m1,
                        F1y_m1, F2y_m1,

                        F1x_m2, F2x_m2,
                        F1y_m2, F2y_m2,

                        Fx_m1, Fy_m1,

                        Fx_m2, Fy_m2,

                        T2

                        )

                    .IsolateVariable(T1)

                    .AssertEqTo(

                        T1 == -(ax_m2 * m1 - ax_m1 * m1) / 2

                    )

                    .SubstituteEqLs(numerical_vals)

                    .AssertEqTo(T1 == 203.8);
            }

            // T2
            {
                eqs
                    .SubstituteEqLs(vals)

                    .EliminateVariables(

                        m2,

                        F1x_m1, F2x_m1,
                        F1y_m1, F2y_m1,

                        F1x_m2, F2x_m2,
                        F1y_m2, F2y_m2,

                        Fx_m1, Fy_m1,

                        Fx_m2, Fy_m2,

                        T1

                        )

                    .IsolateVariable(T2)

                    .AssertEqTo(

                        T2 == (ax_m1 * m1 + ax_m2 * m1) / 2

                    )

                    .SubstituteEqLs(numerical_vals)

                    .AssertEqTo(T2 == 100.19999999999999);
            }
        }
    }

    [Fact]
    public void PSE_5E_P5_31_Obj()
    {
        // Two people pull as hard as they can on ropes attached
        // to a boat that has a mass of 200 kg. If they pull in the
        // same direction, the boat has an acceleration of
        // 1.52 m/s^2 to the right. If they pull in opposite directions,
        // the boat has an acceleration of 0.518 m/s^2 to the
        // left.
        // 
        // What is the force exerted by each person on the
        // boat? (Disregard any other forces on the boat.)

        ////////////////////////////////////////////////////////////////////////////////

        var b1 = new Obj2("b1");            // boat in scenario 1 (same direction)
        var b2 = new Obj2("b2");            // boat in scenario 2 (opposite directions)

        var m = new Symbol("m");

        ////////////////////////////////////////////////////////////////////////////////

        var Pi = new Symbol("Pi");

        var T1 = new Symbol("T1");
        var T2 = new Symbol("T2");

        var eqs = And(

            b1.Equations(),
            b2.Equations()

            );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        var vals = new List<Equation>()
            {
                b1.m == m,

                b1.ay == 0,

                b1.F1 == T1, b1.th1 == 0,
                b1.F2 == T2, b1.th2 == 0,

                b2.m == m,

                b2.ay == 0,

                b2.F1 == T1, b2.th1 == (180).ToRadians(),
                b2.F2 == T2, b2.th2 == 0

            };

        var zeros = vals.Where(eq => eq.b == 0).ToList();

        var numerical_vals = new List<Equation>()
            {
                m == 200,

                b1.ax == 1.52,
                b2.ax == -0.518
            };

        // T1
        eqs
            .SubstituteEqLs(vals)

            .EliminateVariables(

                b1.ΣFx, b1.F1x, b1.F2x,
                b1.ΣFy, b1.F1y, b1.F2y,

                b2.ΣFx, b2.F1x, b2.F2x,
                b2.ΣFy, b2.F1y, b2.F2y,

                T2
            )

            .IsolateVariable(T1)

            .AssertEqTo(

                T1 == -(b2.ax * m - b1.ax * m) / 2

            )

            .SubstituteEqLs(numerical_vals)

            .AssertEqTo(T1 == 203.8);

        // T2
        eqs
            .SubstituteEqLs(vals)

            .EliminateVariables(

                b1.ΣFx, b1.F1x, b1.F2x,
                b1.ΣFy, b1.F1y, b1.F2y,

                b2.ΣFx, b2.F1x, b2.F2x,
                b2.ΣFy, b2.F1y, b2.F2y,

                T1
            )

            .IsolateVariable(T2)

            .AssertEqTo(

                T2 == (b1.ax * m + b2.ax * m) / 2

            )

            .SubstituteEqLs(numerical_vals)

            .AssertEqTo(T2 == 100.19999999999999);

    }

    [Fact]
    public void PSE_5E_P5_55()
    {
        // An inventive child named Pat wants to reach an apple
        // in a tree without climbing the tree. Sitting in a chair
        // connected to a rope that passes over a frictionless pulley
        // Pat pulls on the loose end of the rope with such a force
        // that the spring scale reads 250 N. Pat’s weight is 320 N,
        // and the chair weighs 160 N.
        //
        // http://i.imgur.com/wwlypzB.png
        //
        // (a) Draw free - body diagrams for Pat and the chair considered as
        // separate systems, and draw another diagram for Pat and
        // the chair considered as one system.
        //
        // (b) Show that the acceleration of the system is upward and
        // find its magnitude.
        //
        // (c) Find the force Pat exerts on the chair.

        var b = new Obj3("b");          // boy
        var c = new Obj3("c");          // chair
        var s = new Obj3("s");          // system

        var T = new Symbol("T");        // rope tension
        var n = new Symbol("n");        // normal force

        var Fg_b = new Symbol("Fg_b");  // force due to gravity of the boy
        var Fg_c = new Symbol("Fg_c");  // force due to gravity of the chair
        var Fg_s = new Symbol("Fg_s");  // force due to gravity of the system

        var a = new Symbol("a");        // acceleration

        var Pi = new Symbol("Pi");
        var g = new Symbol("g");

        var eqs = And(

            Fg_b == b.m * g,
            Fg_c == c.m * g,
            Fg_s == s.m * g,

            Fg_s == Fg_c + Fg_b,

            s.Equations(),
            c.Equations()

            );

        var vals = new List<Equation>()
            {
                //b.ax == 0,
                c.ax == 0,
                s.ax == 0,

                //b.F1 == T,          b.th1 == 90 * Pi / 180,
                //b.F2 == n,          b.th2 == 90 * Pi / 180,
                //b.F3 == b.m * g,    b.th3 == 270 * Pi / 180,

                c.F1 == T,          c.th1 == 90 * Pi / 180,
                c.F2 == n,          c.th2 == 270 * Pi / 180,
                c.F3 == Fg_c,       c.th3 == 270 * Pi / 180,

                s.F1 == T,          s.th1 == 90 * Pi / 180,
                s.F2 == T,          s.th2 == 90 * Pi / 180,
                s.F3 == Fg_s,       s.th3 == 270 * Pi / 180,

                //b.ay == a,
                c.ay == a,
                s.ay == a
            };

        var numerical_vals = new List<Equation>()
            {
                T == 250.0,
                Fg_b == 320,
                Fg_c == 160,
                g == 9.8
            };

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        // a
        eqs
            .SubstituteEqLs(vals)

            .EliminateVariables(

                s.ΣFx, s.F1x, s.F2x, s.F3x,
                s.ΣFy, s.F1y, s.F2y, s.F3y,

                c.ΣFx, c.F1x, c.F2x, c.F3x,
                c.ΣFy, c.F1y, c.F2y, c.F3y,

                n,

                s.m,

                Fg_s,

                b.m, c.m

            )

            .IsolateVariable(a)

            .AssertEqTo(

                a == -g * (Fg_b + Fg_c - 2 * T) / (Fg_b + Fg_c)

            )

            .SubstituteEqLs(numerical_vals)

            .AssertEqTo(a == 0.40833333333333333);

        // n
        eqs
            .SubstituteEqLs(vals)

            .EliminateVariables(

                s.ΣFx, s.F1x, s.F2x, s.F3x,
                s.ΣFy, s.F1y, s.F2y, s.F3y,

                c.ΣFx, c.F1x, c.F2x, c.F3x,
                c.ΣFy, c.F1y, c.F2y, c.F3y,

                c.m, s.m,

                Fg_s,

                b.m,

                a

            )

            .IsolateVariable(n)

            .AssertEqTo(

                n == -1 * (Fg_c - T - Fg_c * (Fg_b + Fg_c - 2 * T) / (Fg_b + Fg_c))

            )

            .SubstituteEqLs(numerical_vals);

        DoubleFloat.DoubleFloatTolerance = null;

    }

    [Fact]
    public void PSE_5E_P5_59()
    {
        // A mass M is held in place by an applied force F and a
        // pulley system: 
        //
        //                 http://i.imgur.com/TPAHTlW.png
        //
        // The pulleys are massless and frictionless. Find 
        //                     
        // (a) the tension in each section of rope, T1, T2, T3, T4, and T5
        //                     
        // (b) the magnitude of F. 
        //                     
        // (Hint: Draw a free - body diagram for each pulley.)

        var pul1_F = new Symbol("pul1_F");      // magnitude of total force on pully 1
        var pul1_m = new Symbol("pul1_m");      // mass of pully 1
        var pul1_a = new Symbol("pul1_a");      // acceleration of pully 1

        var pul2_F = new Symbol("pul2_F");      // magnitude of total force on pully 2
        var pul2_m = new Symbol("pul2_m");      // mass of pully 2
        var pul2_a = new Symbol("pul2_a");      // acceleration of pully 2

        var T1 = new Symbol("T1");
        var T2 = new Symbol("T2");
        var T3 = new Symbol("T3");
        var T4 = new Symbol("T4");
        var T5 = new Symbol("T5");

        var F = new Symbol("F");

        var M = new Symbol("M");

        var g = new Symbol("g");

        var eqs = And(

             T1 == F,
             T2 == T3,
             T1 == T3,
             T5 == M * g,

             pul1_a == 0,
             pul1_m == 0,

             pul1_F == T4 - T1 - T2 - T3,
             pul1_F == pul1_m * pul1_a,

             pul2_m == 0,

             pul2_F == T2 + T3 - T5,
             pul2_F == pul2_m * pul2_a

            );

        DoubleFloat.DoubleFloatTolerance = 0.00001;

        // T1
        {
            eqs
                .EliminateVariables(pul1_F, pul2_F, pul1_m, pul2_m, pul1_a, T2, T3, T4, T5, F)
                .IsolateVariable(T1)
                .AssertEqTo(T1 == g * M / 2);
        }

        // T2
        {
            eqs
                .EliminateVariables(pul1_F, pul2_F, pul1_m, pul2_m, pul1_a, T1, T3, T4, T5, F)
                .IsolateVariable(T2)
                .AssertEqTo(T2 == g * M / 2);
        }

        // T3
        {
            eqs
                .EliminateVariables(pul1_F, pul2_F, pul1_m, pul2_m, pul1_a, T1, T2, T4, T5, F)
                .IsolateVariable(T3)
                .AssertEqTo(T3 == g * M / 2);
        }

        // T4
        {
            eqs
                .EliminateVariables(pul1_F, pul2_F, pul1_m, pul2_m, pul1_a, T1, T2, T3, T5, F)
                .IsolateVariable(T4)
                .AssertEqTo(T4 == g * M * 3 / 2);
        }

        // T5
        {
            eqs
                .EliminateVariables(pul1_F, pul2_F, pul1_m, pul2_m, pul1_a, T1, T2, T3, T4, F)
                .AssertEqTo(T5 == g * M);
        }

        // F
        {
            eqs
                .EliminateVariables(pul1_F, pul2_F, pul1_m, pul2_m, pul1_a, T1, T2, T3, T4, T5)
                .IsolateVariable(F)
                .AssertEqTo(F == g * M / 2);
        }
    }

    [Fact]
    public void PSE_5E_P5_69()
    {
        // What horizontal force must be applied to the cart shown:

        // http://i.imgur.com/fpkzsYI.png

        // so that the blocks remain stationary relative to the cart?
        // Assume all surfaces, wheels, and pulley are frictionless.
        // (Hint:Note that the force exerted by the string accelerates m1.)

        var blk1 = new Obj3("blk1");
        var blk2 = new Obj3("blk2");

        var sys = new Obj3("sys");

        var m1 = new Symbol("m1");
        var m2 = new Symbol("m2");

        var T = new Symbol("T");
        var F = new Symbol("F");
        var M = new Symbol("M");
        var g = new Symbol("g");
        var a = new Symbol("a");

        var eqs = And(

            blk1.Equations(),
            blk2.Equations(),

            sys.Equations()

            );

        var vals = new List<Equation>()
            {
                blk1.ax == a,
                blk1.ay == 0,

                blk1.m == m1,

                blk1.F1 == T,   blk1.th1 == 0,

                blk1.th2 == (90).ToRadians(),
                blk1.th3 == (270).ToRadians(),


                blk2.ax == a,
                blk2.ay == 0,

                blk2.m == m2,

                blk2.th1 == 0,

                blk2.F2 == T,       blk2.th2 == (90).ToRadians(),
                blk2.F3 == m2 * g,  blk2.th3 == (270).ToRadians(),


                sys.ax == a,
                sys.ay == 0,

                sys.m == M + m1 + m2,

                sys.F1 == F,        sys.th1 == 0,

                sys.th2 == (90).ToRadians(),
                sys.th3 == (270).ToRadians()

            };

        eqs
            .SubstituteEqLs(vals)

            .EliminateVariables(

                blk1.ΣFx, blk1.F1x, blk1.F2x, blk1.F3x,
                blk1.ΣFy, blk1.F1y, blk1.F2y, blk1.F3y,

                blk1.F2,

                blk2.ΣFx, blk2.F1x, blk2.F2x, blk2.F3x,
                blk2.ΣFy, blk2.F1y, blk2.F2y, blk2.F3y,

                blk2.F1,

                sys.ΣFx, sys.F1x, sys.F2x, sys.F3x,
                sys.ΣFy, sys.F1y, sys.F2y, sys.F3y,

                sys.F2,

                T, a

            )

            .AssertEqTo(F == g * m2 / m1 * (M + m1 + m2));
    }

    [Fact]
    public void PSE_5E_E7_7()
    {
        // A  6.0-kg block initially at rest is pulled to the right along a
        // horizontal, frictionless surface by a constant horizontal force
        // of 12 N. Find the speed of the block after it has moved 3.0 m.

        var W = new Symbol("W");
        var F = new Symbol("F");
        var d = new Symbol("d");

        var Kf = new Symbol("Kf");
        var Ki = new Symbol("Ki");

        var m = new Symbol("m");

        var vf = new Symbol("vf");
        var vi = new Symbol("vi");

        var eqs = And(

            W == F * d,

            W == Kf - Ki,

            Kf == m * (vf ^ 2) / 2,
            Ki == m * (vi ^ 2) / 2,

            m != 0

            );

        var vals = new List<Equation>() { m == 6.0, vi == 0, F == 12, d == 3 };

        // vf
        eqs
            .EliminateVariables(Kf, Ki, W)
            .IsolateVariable(vf)
            .LogicalExpand().CheckVariable(m).SimplifyEquation().SimplifyLogical()

            .AssertEqTo(

                Or(
                    And(
                        vf == Sqrt(-2 * m * (-d * F - m * (vi ^ 2) / 2)) / m,
                        m != 0),
                    And(
                        vf == -Sqrt(-2 * m * (-d * F - m * (vi ^ 2) / 2)) / m,
                        m != 0)))

            .SubstituteEq(vi == 0)

            .AssertEqTo(

                Or(
                    And(
                        vf == Sqrt(2 * d * F * m) / m,
                        m != 0),
                    And(
                        vf == -Sqrt(2 * d * F * m) / m,
                        m != 0)))

            .SubstituteEqLs(vals)

            .AssertEqTo(
                Or(
                    vf == 3.4641016151377544,
                    vf == -3.4641016151377544));

    }

    [Fact]
    public void PSE_5E_E7_8()
    {
        // Find the final speed of the block described in Example 7.7 if
        // the surface is not frictionless but instead has a coefficient of
        // kinetic friction of 0.15.

        var W = new Symbol("W");
        var F = new Symbol("F");
        var d = new Symbol("d");
        var n = new Symbol("n");

        var g = new Symbol("g");

        var Kf = new Symbol("Kf");
        var Ki = new Symbol("Ki");

        var m = new Symbol("m");

        var vf = new Symbol("vf");
        var vi = new Symbol("vi");

        var fk = new Symbol("fk");

        var μk = new Symbol("μk");

        var eqs = And(

            Kf == m * (vf ^ 2) / 2,
            Ki == m * (vi ^ 2) / 2,

            W == F * d,

            n == m * g,

            fk == n * μk,

            W - fk * d == Kf - Ki,

            m != 0

            );

        var vals = new List<Equation>()
            {
                vi == 0,
                F == 12.0,
                d == 3.0,

                m == 6.0,

                μk == 0.15,

                g == 9.8,
            };

        // vf
        eqs
            .EliminateVariables(Kf, Ki, W, n, fk)
            .IsolateVariable(vf)
            .LogicalExpand().SimplifyEquation().SimplifyLogical().CheckVariable(m)
            .SubstituteEq(vi == 0)
            .AssertEqTo(
                Or(
                    And(
                        vf == -Sqrt(2 * m * (d * F - d * g * m * μk)) / m,
                        m != 0),
                    And(
                        vf == Sqrt(2 * m * (d * F - d * g * m * μk)) / m,
                        m != 0)))

            .SubstituteEqLs(vals)

            .AssertEqTo(Or(vf == -1.7832554500127007, vf == 1.7832554500127007));

    }

    [Fact]
    public void PSE_5E_E7_11()
    {
        // A block of mass 1.6 kg is attached to a horizontal spring that
        // has a force constant of 1.0 x 10^3 N/m, as shown in Figure
        // 7.10. The spring is compressed 2.0 cm and is then released
        // from  rest.

        // (a) Calculate the  speed of  the block  as it  passes
        // through the equilibrium position x = 0 if the surface is frictionless.

        // (b) Calculate the speed of the block as it passes through
        // the equilibrium position if a constant frictional force of 4.0 N
        // retards its motion from the moment it is released.

        var ΣW = new Symbol("ΣW");

        var Kf = new Symbol("Kf");
        var Ki = new Symbol("Ki");

        var m = new Symbol("m");
        var d = new Symbol("d");
        var k = new Symbol("k");

        var vf = new Symbol("vf");
        var vi = new Symbol("vi");

        var fk = new Symbol("fk");

        var W_s = new Symbol("W_s");
        var W_f = new Symbol("W_f");

        var x_max = new Symbol("x_max");

        var eqs = And(

            W_s == k * (x_max ^ 2) / 2,

            Kf == m * (vf ^ 2) / 2,
            Ki == m * (vi ^ 2) / 2,

            W_f == -fk * d,

            ΣW == Kf - Ki,

            ΣW == W_s + W_f,

            m != 0

            );

        // vf
        {
            var vals = new List<Equation>() { m == 1.6, vi == 0, fk == 0, k == 1000, x_max == -0.02 };

            eqs
                .EliminateVariables(ΣW, Kf, Ki, W_f, W_s)
                .IsolateVariable(vf)
                .LogicalExpand().SimplifyEquation().SimplifyLogical().CheckVariable(m)

                .AssertEqTo(
                    Or(
                        And(
                            vf == Sqrt(-2 * m * (d * fk - m * (vi ^ 2) / 2 - k * (x_max ^ 2) / 2)) / m,
                            m != 0),
                        And(
                            vf == -Sqrt(-2 * m * (d * fk - m * (vi ^ 2) / 2 - k * (x_max ^ 2) / 2)) / m,
                            m != 0)))

                .SubstituteEqLs(vals)

                .AssertEqTo(Or(vf == 0.5, vf == -0.5));
        }

        // vf
        {
            var vals = new List<Equation>() { m == 1.6, vi == 0, fk == 4, k == 1000, x_max == -0.02, d == 0.02 };

            eqs
                .EliminateVariables(ΣW, Kf, Ki, W_f, W_s)
                .IsolateVariable(vf)
                .LogicalExpand().SimplifyEquation().SimplifyLogical().CheckVariable(m)

                .SubstituteEqLs(vals)

                .AssertEqTo(Or(vf == 0.3872983346207417, vf == -0.3872983346207417));
        }

    }

    [Fact]
    public void PSE_6E_P7_3()
    {
        // Batman, whose mass is 80.0kg, is dangling on the free end
        // of a 12.0m rope, the other end of which is fixed to a tree
        // limb above. He is able to get the rope in motion as only
        // Batman knows how, eventually getting it to swing enough
        // that he can reach a ledge when the rope makes a 60.0°
        // angle with the vertical. How much work was done by the
        // gravitational force on Batman in this maneuver?

        var m = new Symbol("m");
        var a = new Symbol("a");

        var W = new Symbol("W");
        var F = new Symbol("F");
        var d = new Symbol("d");

        var yA = new Symbol("yA");
        var yB = new Symbol("yB");

        var th = new Symbol("th");

        var len = new Symbol("len");

        var eqs = And(

            yA == -len,

            yB == -len * Cos(th),

            d == yB - yA,

            F == m * a,

            W == F * d

            );

        var vals = new List<Equation>()
            { m == 80, len == 12, th == (60).ToRadians(), a == -9.8 };

        eqs
            .EliminateVariables(F, d, yA, yB)

            .AssertEqTo(W == a * (len - Cos(th) * len) * m)

            .SubstituteEqLs(vals)

            .AssertEqTo(W == -4704.0);
    }

    [Fact]
    public void PSE_5E_P7_23()
    {
        // If it takes 4.00J of work to stretch a Hooke’s-law spring
        // 10.0cm from its unstressed length, determine the extra
        // work required to stretch it an additional 10.0cm.

        var WsAB = new Symbol("WsAB");

        var WsA = new Symbol("WsA");
        var WsB = new Symbol("WsB");

        var k = new Symbol("k");

        var xA = new Symbol("xA");
        var xB = new Symbol("xB");

        var eqs = And(

            WsA == k * (xA ^ 2) / 2,
            WsB == k * (xB ^ 2) / 2,

            WsAB == WsB - WsA

            );

        var vals = new List<Equation>() { xA == 0.1, xB == 0.2, WsA == 4 };

        eqs

            .EliminateVariables(WsB, k)

            .AssertEqTo(

                WsAB == WsA * (xB ^ 2) / (xA ^ 2) - WsA

                )

            .SubstituteEqLs(vals)

            .AssertEqTo(WsAB == 12.0);

    }

    [Fact]
    public void PSE_5E_P7_33()
    {
        // A 40.0-kg box initially at rest is pushed 5.00m along a
        // rough, horizontal floor with a constant applied horizontal
        // force of 130 N. If the coefficient of friction between
        // the box and the floor is 0.300, find

        // (a) the work done by the applied force
        // (b) the energy loss due to friction
        // (c) the work done by the normal force
        // (d) the work done by gravity
        // (e) the change in kinetic energy of the box
        // (f) the final speed of the box

        var ΣW = new Symbol("ΣW");

        var Kf = new Symbol("Kf");
        var Ki = new Symbol("Ki");

        var F = new Symbol("F");

        var m = new Symbol("m");
        var d = new Symbol("d");

        var n = new Symbol("n");
        var g = new Symbol("g");

        var vf = new Symbol("vf");
        var vi = new Symbol("vi");

        var fk = new Symbol("fk");

        var W_F = new Symbol("W_F");
        var W_f = new Symbol("W_f");

        var μk = new Symbol("μk");

        var eqs = And(

            n == m * g,

            fk == μk * n,

            Kf == m * (vf ^ 2) / 2,
            Ki == m * (vi ^ 2) / 2,

            W_F == F * d,

            W_f == -fk * d,

            ΣW == Kf - Ki,

            ΣW == W_F + W_f,

            m != 0

            );

        var vals = new List<Equation>()
            { m == 40, vi == 0, d == 5, F == 130, μk == 0.3, g == 9.8 };

        // W_F, W_f
        eqs
            .EliminateVariables(fk, n, Kf, Ki, ΣW, vf)
            .LogicalExpand().SimplifyEquation().SimplifyLogical().CheckVariable(m)

            .AssertEqTo(
                And(
                    m != 0,
                    W_F == d * F,
                    W_f == -d * g * m * μk))

            .SubstituteEqLs(vals)

            .AssertEqTo(And(W_F == 650, W_f == -588.0));

        // ΣW
        eqs
            .EliminateVariables(W_F, W_f, fk, n, Ki, Kf)

            .AssertEqTo(
                And(
                    ΣW == m * (vf ^ 2) / 2 - m * (vi ^ 2) / 2,
                    ΣW == d * F - d * g * m * μk,
                    m != 0))

            .SubstituteEqLs(vals)

            .AssertEqTo(And(ΣW == 20 * (vf ^ 2), ΣW == 62.0));

        // vf
        eqs
            .EliminateVariables(Kf, Ki, ΣW, W_F, W_f, fk, n)
            .IsolateVariable(vf)
            .LogicalExpand().SimplifyEquation().SimplifyLogical().CheckVariable(m)

            .AssertEqTo(
                Or(
                    And(
                        vf == Sqrt(-2 * m * (-d * F - m * (vi ^ 2) / 2 + d * g * m * μk)) / m,
                        m != 0),
                    And(
                        vf == -Sqrt(-2 * m * (-d * F - m * (vi ^ 2) / 2 + d * g * m * μk)) / m,
                        m != 0)))

            .SubstituteEqLs(vals)

            .AssertEqTo(Or(vf == 1.7606816861659009, vf == -1.7606816861659009));
    }

    [Fact]
    public void PSE_5E_P7_35()
    {
        // A crate of mass 10.0kg is pulled up a rough incline with
        // an initial speed of 1.50 m/s.The pulling force is 100 N
        // parallel to the incline, which makes an angle of 20.0°
        // with the horizontal. The coefficient of kinetic friction is
        // 0.400, and the crate is pulled 5.00 m.

        // (a) How much work is done by gravity?
        // (b) How much energy is lost because of friction?
        // (c) How much work is done by the 100-N force?
        // (d) What is the change in kinetic energy of the crate?
        // (e) What is the speed of the crate after it has been pulled 5.00 m?

        var ΣW = new Symbol("ΣW");

        var Kf = new Symbol("Kf");
        var Ki = new Symbol("Ki");

        var F = new Symbol("F");

        var m = new Symbol("m");
        var d = new Symbol("d");

        var n = new Symbol("n");
        var g = new Symbol("g");

        var vf = new Symbol("vf");
        var vi = new Symbol("vi");

        var fk = new Symbol("fk");

        var W_F = new Symbol("W_F");
        var W_f = new Symbol("W_f");
        var W_g = new Symbol("W_g");

        var μk = new Symbol("μk");

        var th = new Symbol("th");

        var F_g = new Symbol("F_g");

        var Pi = new Symbol("Pi");

        var eqs = And(

            F_g == m * g,

            n == F_g * Cos(th),

            fk == μk * n,

            Kf == m * (vf ^ 2) / 2,
            Ki == m * (vi ^ 2) / 2,

            W_F == F * d,

            W_f == -fk * d,

            W_g == -F_g * Sin(th) * d,

            ΣW == Kf - Ki,

            ΣW == W_F + W_f + W_g,

            m != 0

            );

        var vals = new List<Equation>()
            {
                m == 10.0, g == 9.8, d == 5.0, th == (20).ToRadians(), μk == 0.4, F == 100.0,
                vi == 1.5, Pi == Math.PI
            };

        // W_g, W_f, W_F
        eqs
            .EliminateVariables(F_g, fk, n)

            .AssertEqTo(
                And(
                    Kf == m * (vf ^ 2) / 2,
                    Ki == m * (vi ^ 2) / 2,
                    W_F == d * F,
                    W_f == -Cos(th) * d * g * m * μk,
                    W_g == -d * g * m * Sin(th),
                    ΣW == Kf - Ki,
                    ΣW == W_f + W_F + W_g,
                    m != 0
                ))

            .SubstituteEqLs(vals)

            .AssertEqTo(
                And(
                    Kf == 5.0 * (vf ^ 2),
                    Ki == 11.25,
                    W_F == 500.0,
                    W_f == -184.17975367403804,
                    W_g == -167.58987022957766,
                    ΣW == Kf - Ki,
                    ΣW == W_f + W_F + W_g
                ));

        // ΣW
        eqs
            .EliminateVariables(F_g, fk, n, W_F, W_f, W_g)

            .SubstituteEqLs(vals)

            .AssertEqTo(
                And(
                    Kf == 5.0 * (vf ^ 2),
                    Ki == 11.25,
                    ΣW == Kf - Ki,
                    ΣW == 148.23037609638431
                ));

        // vf
        eqs
            .EliminateVariables(F_g, fk, n, W_F, W_f, W_g, ΣW, Kf, Ki)

            .IsolateVariable(vf)

            .LogicalExpand().SimplifyEquation().SimplifyLogical().CheckVariable(m)

            .SubstituteEqLs(vals)

            .AssertEqTo(Or(vf == 5.6476610396939435, vf == -5.6476610396939435));

    }

    [Fact]
    public void PSE_5E_P7_39()
    {
        // A bullet with a mass of 5.00 g and a speed of 600 m/s
        // penetrates a tree to a depth of 4.00 cm.

        // (a) Use work and energy considerations to find the average
        // frictional force that stops the bullet.

        // (b) Assuming that the frictional force is constant,
        // determine how much time elapsed between the moment
        // the bullet entered the tree and the moment it stopped.

        var ΣW = new Symbol("ΣW");

        var Kf = new Symbol("Kf");
        var Ki = new Symbol("Ki");

        var m = new Symbol("m");
        var d = new Symbol("d");

        var vf = new Symbol("vf");
        var vi = new Symbol("vi");

        var fk = new Symbol("fk");

        var W_f = new Symbol("W_f");

        var t = new Symbol("t");

        var eqs = And(

            Kf == m * (vf ^ 2) / 2,
            Ki == m * (vi ^ 2) / 2,

            W_f == -fk * d,

            ΣW == Kf - Ki,

            ΣW == W_f

            );

        var vals = new List<Equation>() { m == 0.005, vi == 600.0, vf == 0.0, d == 0.04 };

        // fk
        eqs
            .EliminateVariables(W_f, ΣW, Ki, Kf)
            .IsolateVariable(fk)
            .AssertEqTo(

                fk == (m * (vi ^ 2) / 2 - m * (vf ^ 2) / 2) / d

                )

            .SubstituteEqLs(vals)

            .AssertEqTo(fk == 22500.0);

        // t
        (d == (vi + vf) * t / 2)
            .IsolateVariable(t)
            .AssertEqTo(t == 2 * d / (vf + vi))
            .SubstituteEqLs(vals)
            .AssertEqTo(t == 1.3333333333333334e-4);

    }

    [Fact]
    public void PSE_5E_P7_41()
    {
        // A 2.00-kg block is attached to a spring of force constant
        // 500 N/m, as shown in Figure 7.10. The block is pulled
        // 5.00 cm to the right of equilibrium and is then released
        // from rest. Find the speed of the block as it passes
        // through equilibrium if

        // (a) the horizontal surface is frictionless

        // (b) the coefficient of friction between the block and the surface is 0.350.

        var ΣW = new Symbol("ΣW");

        var Kf = new Symbol("Kf");
        var Ki = new Symbol("Ki");

        var m = new Symbol("m");
        var d = new Symbol("d");

        var n = new Symbol("n");
        var g = new Symbol("g");
        var k = new Symbol("k");

        var vf = new Symbol("vf");
        var vi = new Symbol("vi");

        var fk = new Symbol("fk");

        var W_f = new Symbol("W_f");
        var W_s = new Symbol("W_s");

        var μk = new Symbol("μk");

        var xi = new Symbol("xi");
        var xf = new Symbol("xf");

        var eqs = And(

            n == m * g,

            fk == μk * n,

            Kf == m * (vf ^ 2) / 2,
            Ki == m * (vi ^ 2) / 2,

            W_f == -fk * d,

            W_s == k * (xi ^ 2) / 2 - k * (xf ^ 2) / 2,

            ΣW == Kf - Ki,

            ΣW == W_f + W_s,

            m != 0

            );

        var vals = new List<Equation>()
            { m == 2.0, k == 500, xi == 0.05, xf == 0.0, vi == 0, d == 0.05, g == 9.8 };

        eqs
            .EliminateVariables(Kf, Ki, ΣW, W_f, W_s, n, fk)
            .IsolateVariable(vf)
            .LogicalExpand().SimplifyEquation().SimplifyLogical().CheckVariable(m)

            .AssertEqTo(
                Or(
                    And(
                        vf == Sqrt(-2 * m * (-m * (vi ^ 2) / 2 + k * (xf ^ 2) / 2 - k * (xi ^ 2) / 2 + d * g * m * μk)) / m,
                        m != 0
                    ),
                    And(
                        vf == -Sqrt(-2 * m * (-m * (vi ^ 2) / 2 + k * (xf ^ 2) / 2 - k * (xi ^ 2) / 2 + d * g * m * μk)) / m,
                        m != 0)))

            .SubstituteEqLs(vals).SubstituteEq(μk == 0)

            .AssertEqTo(Or(vf == 0.79056941504209488, vf == -0.79056941504209488));

        eqs
            .EliminateVariables(Kf, Ki, ΣW, W_f, W_s, n, fk)
            .IsolateVariable(vf)
            .LogicalExpand().SimplifyEquation().SimplifyLogical().CheckVariable(m)
            .SubstituteEqLs(vals).SubstituteEq(μk == 0.35)
            .AssertEqTo(Or(vf == 0.53103672189407025, vf == -0.53103672189407025));
    }

    [Fact]
    public void PSE_5E_P7_55()
    {
        // A baseball outfielder throws a 0.150-kg baseball at a
        // speed of 40.0 m/s and an initial angle of 30.0°. What is
        // the kinetic energy of the baseball at the highest point of
        // the trajectory?

        var vx = new Symbol("vx");
        var vi = new Symbol("vi");
        var th = new Symbol("th");

        var m = new Symbol("m");
        var K = new Symbol("K");

        var vals = new List<Equation>() { m == 0.15, vi == 40.0, th == (30).ToRadians() };

        var eqs = And(

            vx == vi * Cos(th),

            K == m * (vx ^ 2) / 2

            );

        eqs
            .EliminateVariables(vx)

            .AssertEqTo(K == (Cos(th) ^ 2) * m * (vi ^ 2) / 2)

            .SubstituteEqLs(vals)

            .AssertEqTo(K == 90.0);
    }

    [Fact]
    public void PSE_5E_E8_2()
    {
        // A ball  of mass m is dropped from a height h above the
        // ground, as shown in Figure 8.6.

        // (a) Neglecting air resistance, determine the speed of
        // the ball when it is at a height ya bove the ground.

        // (b) Determine the speed of the ball at y if at the instant of
        // release it already has an initial speed vi at the initial altitude h.

        var m = new Symbol("m");

        var yi = new Symbol("yi");
        var yf = new Symbol("yf");

        var vi = new Symbol("vi");
        var vf = new Symbol("vf");

        var Ki = new Symbol("Ki");
        var Kf = new Symbol("Kf");

        var Ugi = new Symbol("Ugi");
        var Ugf = new Symbol("Ugf");

        var ΣUi = new Symbol("ΣUi");
        var ΣUf = new Symbol("ΣUf");

        var Ei = new Symbol("Ei");
        var Ef = new Symbol("Ef");

        var g = new Symbol("g");

        var h = new Symbol("h");
        var y = new Symbol("y");

        var eqs = And(
            Ki == m * (vi ^ 2) / 2,
            Kf == m * (vf ^ 2) / 2,

            Ugi == m * g * yi,
            Ugf == m * g * yf,

            ΣUi == Ugi,
            ΣUf == Ugf,

            Ei == Ki + ΣUi,
            Ef == Kf + ΣUf,

            Ei == Ef
        );

        var vals = new List<Equation>() { yi == h, yf == y };

        // vf, vi == 0
        eqs
            .EliminateVariables(Ugi, Ugf, ΣUi, ΣUf, Ki, Kf, Ei, Ef)
            .MultiplyBothSidesBy(1 / m)
            .AlgebraicExpand()
            .IsolateVariable(vf)
            .SubstituteEqLs(vals)
            .SubstituteEq(vi == 0)

            .AssertEqTo(
                Or(
                    vf == -Sqrt(2 * (g * h - g * y)),
                    vf == Sqrt(2 * (g * h - g * y))
                ));

        // vf
        eqs
            .EliminateVariables(Ugi, Ugf, ΣUi, ΣUf, Ki, Kf, Ei, Ef)
            .MultiplyBothSidesBy(1 / m)
            .AlgebraicExpand()
            .IsolateVariable(vf)
            .SubstituteEqLs(vals)

            .AssertEqTo(
                Or(
                    vf == -Sqrt(2 * (g * h + (vi ^ 2) / 2 - g * y)),
                    vf == Sqrt(2 * (g * h + (vi ^ 2) / 2 - g * y))
                ));

    }

    [Fact]
    public void PSE_5E_E8_3()
    {
        // A pendulum consists of a sphere of mass mattached to a light
        // cord of length L, as shown in Figure 8.7. The sphere is released
        // from rest when the cord makes an angle thA with the vertical,
        // and the pivot at P is frictionless.

        // (a) Find the speed of the sphere when it is at the lowest point B.

        // (b) What is the tension T_B in the cord at B?

        // (c) A pendulum of length 2.00 m and mass 0.500 kg
        // is released from rest when the cord makes an angle of 30.0°
        // with the vertical. Find the speed of the sphere and the tension
        // in the cord when the sphere is at its lowest point.

        var m = new Symbol("m");

        var yi = new Symbol("yi");
        var yf = new Symbol("yf");

        var vi = new Symbol("vi");
        var vf = new Symbol("vf");

        var Ki = new Symbol("Ki");
        var Kf = new Symbol("Kf");

        var Ugi = new Symbol("Ugi");
        var Ugf = new Symbol("Ugf");

        var ΣUi = new Symbol("ΣUi");
        var ΣUf = new Symbol("ΣUf");

        var Ei = new Symbol("Ei");
        var Ef = new Symbol("Ef");

        var g = new Symbol("g");

        var L = new Symbol("L");

        var thA = new Symbol("thA");

        var ar_f = new Symbol("ar_f");

        var r = new Symbol("r");

        var ΣFr = new Symbol("ΣFr");

        var T_f = new Symbol("T_f");

        var vf_sq = new Symbol("vf_sq");

        var eqs = And(

            Ki == m * (vi ^ 2) / 2,
            Kf == m * (vf ^ 2) / 2,

            Ugi == m * g * yi,
            Ugf == m * g * yf,

            ΣUi == Ugi,
            ΣUf == Ugf,

            Ei == Ki + ΣUi,
            Ef == Kf + ΣUf,

            Ei == Ef,

            ar_f == (vf ^ 2) / r,

            ΣFr == T_f - m * g,

            ΣFr == m * ar_f

            );

        var vals = new List<Equation>()
            {
                yi == -L * Cos(thA),
                yf == -L,
                vi == 0,

                r == L
            };

        var numerical_vals = new List<Equation>() { L == 2.0, m == 0.5, thA == (30).ToRadians(), g == 9.8 };

        // vf
        eqs
            .SubstituteEqLs(vals)
            .EliminateVariables(ar_f, ΣFr, T_f, Ki, Kf, Ugi, Ugf, ΣUi, ΣUf, Ei, Ef)
            .MultiplyBothSidesBy(1 / m)
            .AlgebraicExpand()
            .IsolateVariable(vf)

            .AssertEqTo(

                Or(
                    vf == -Sqrt(2 * (g * L - Cos(thA) * g * L)),
                    vf == Sqrt(2 * (g * L - Cos(thA) * g * L))
                )

            )

            .SubstituteEqLs(numerical_vals).Substitute(3, 3.0)

            .AssertEqTo(
                Or(
                    vf == -2.2916815161906787,
                    vf == 2.2916815161906787
                )
            );

        // T_f
        eqs
            .SubstituteEqLs(vals)
            .Substitute(vf ^ 2, vf_sq)
            .EliminateVariables(Ki, Kf, Ugi, Ugf, ΣUi, ΣUf, Ei, Ef, ar_f, ΣFr, vf_sq)
            .MultiplyBothSidesBy(1 / m)
            .AlgebraicExpand()
            .IsolateVariable(T_f)

            .AssertEqTo(

                T_f == (3 * g - 2 * Cos(thA) * g) * m

            );

    }

    [Fact]
    public void PSE_5E_E8_4()
    {
        // A 3.00-kg crate slides down a ramp. The ramp is 1.00 m in
        // length and inclined at an angle of 30.0°, as shown in Figure
        // 8.8. The crate starts from rest at the top, experiences a
        // constant frictional force of magnitude 5.00 N, and continues to
        // move a short distance on the flat floor after it leaves the
        // ramp. Use energy methods to determine the speed of the
        // crate at the bottom of the ramp.

        var m = new Symbol("m");

        var yi = new Symbol("yi");
        var yf = new Symbol("yf");

        var vi = new Symbol("vi");
        var vf = new Symbol("vf");

        var Ki = new Symbol("Ki");
        var Kf = new Symbol("Kf");

        var Ugi = new Symbol("Ugi");
        var Ugf = new Symbol("Ugf");

        var ΣUi = new Symbol("ΣUi");
        var ΣUf = new Symbol("ΣUf");

        var Ei = new Symbol("Ei");
        var Ef = new Symbol("Ef");

        var fk = new Symbol("fk");

        var W_f = new Symbol("W_f");

        var ΔE = new Symbol("ΔE");

        var g = new Symbol("g");

        var d = new Symbol("d");

        var θ = new Symbol("θ");

        var eqs = And(

            yi == d * Sin(θ),

            Ki == m * (vi ^ 2) / 2,
            Kf == m * (vf ^ 2) / 2,

            Ugi == m * g * yi,
            Ugf == m * g * yf,

            ΣUi == Ugi,
            ΣUf == Ugf,

            W_f == -fk * d,

            ΔE == W_f,

            Ei == Ki + ΣUi,
            Ef == Kf + ΣUf,

            Ei + ΔE == Ef,

            m != 0

            );

        var vals = new List<Equation>()
            { m == 3.0, d == 1.0, θ == (30).ToRadians(), fk == 5.0, vi == 0.0, g == 9.8, yf == 0.0 };

        eqs
            .EliminateVariables(Ei, Ef, ΔE, Ki, Kf, ΣUi, ΣUf, W_f, Ugi, Ugf, yi)
            .IsolateVariable(vf)
            .LogicalExpand().SimplifyEquation().SimplifyLogical().CheckVariable(m)

            .AssertEqTo(
                Or(
                    And(
                        vf == -Sqrt(2 * m * (-d * fk + m * (vi ^ 2) / 2 - g * m * yf + g * m * d * Sin(θ))) / m,
                        m != 0
                    ),
                    And(
                        vf == Sqrt(2 * m * (-d * fk + m * (vi ^ 2) / 2 - g * m * yf + g * m * d * Sin(θ))) / m,
                        m != 0
                    )))

            .SubstituteEqLs(vals)

            .AssertEqTo(Or(vf == -2.54296414970142, vf == 2.54296414970142));
    }

    [Fact]
    public void PSE_5E_Example_8_5()
    {
        // A child of mass mrides on an irregularly curved slide of
        // height as shown in  Figure 8.9.The child starts
        // from rest at the top.

        // (a) Determine his speed at the bottom, assuming no friction is present.

        // (b) If a force of kinetic friction acts on the child, how
        // much mechanical energy does the system lose? Assume that
        // vf = 3.0 m/s and m = 20.0 kg.

        var m = new Symbol("m");

        var yi = new Symbol("yi");
        var yf = new Symbol("yf");

        var vi = new Symbol("vi");
        var vf = new Symbol("vf");

        var Ki = new Symbol("Ki");
        var Kf = new Symbol("Kf");

        var Ugi = new Symbol("Ugi");
        var Ugf = new Symbol("Ugf");

        var ΣUi = new Symbol("ΣUi");
        var ΣUf = new Symbol("ΣUf");

        var Ei = new Symbol("Ei");
        var Ef = new Symbol("Ef");

        var fk = new Symbol("fk");

        var W_f = new Symbol("W_f");

        var ΔE = new Symbol("ΔE");

        var g = new Symbol("g");

        var d = new Symbol("d");

        var eqs = And(

            Ki == m * (vi ^ 2) / 2,
            Kf == m * (vf ^ 2) / 2,

            Ugi == m * g * yi,
            Ugf == m * g * yf,

            ΣUi == Ugi,
            ΣUf == Ugf,

            W_f == -fk * d,

            ΔE == W_f,

            Ei == Ki + ΣUi,
            Ef == Kf + ΣUf,

            Ei + ΔE == Ef);

        {
            var vals = new List<Equation>()
                { yi == 2.0, yf == 0, vi == 0, fk == 0, g == 9.8 };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            // vf
            eqs
                .SubstituteEqLs(zeros)
                .EliminateVariables(Ei, Ef, ΔE, Ki, Kf, ΣUi, ΣUf, W_f, Ugi, Ugf)
                .MultiplyBothSidesBy(1 / m)
                .IsolateVariable(vf)

                .AssertEqTo(
                    Or(
                        vf == -Sqrt(2 * g * yi),
                        vf == Sqrt(2 * g * yi)))

                .SubstituteEqLs(vals)

                .AssertEqTo(
                    Or(
                        vf == -6.2609903369994111,
                        vf == 6.2609903369994111));
        }

        {
            var vals = new List<Equation>()
                { m == 20.0, yi == 2.0, yf == 0, vi == 0, vf == 3.0, g == 9.8 };

            var zeros = vals.Where(eq => eq.b == 0).ToList();

            // ΔE
            eqs
                .SubstituteEqLs(zeros)
                .EliminateVariables(fk, Ei, Ef, Ki, Kf, ΣUi, ΣUf, Ugi, Ugf, W_f)
                .IsolateVariable(ΔE)

                .AssertEqTo(ΔE == m * (vf ^ 2) / 2 - g * m * yi)

                .SubstituteEqLs(vals)

                .AssertEqTo(ΔE == -302.0);
        }
    }
}
