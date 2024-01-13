using Symbolism.CoefficientGpe;
using Symbolism.DegreeGpe;

namespace Symbolism.LeadingCoefficientGpe;

public static class Extensions
{
    public static MathObject LeadingCoefficientGpe(this MathObject u, MathObject x) =>
        u.CoefficientGpe(x, u.DegreeGpe([x]));
}

