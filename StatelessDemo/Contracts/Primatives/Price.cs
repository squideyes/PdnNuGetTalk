// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using Vogen;

namespace PdnNuGetTalk.StatelessDemo.Contracts;

[ValueObject(typeof(double))]
public partial struct Price
{
    private static Validation Validate(double value)
    {
        return IsValue(value) ? Validation.Ok : Validation.Invalid(
            "A price must be in the 0.01 to 999.99 range and have 0 to 2 decimal digits.");
    }

    public static bool IsValue(double value) => 
        value >= 0.01 && value < 999.99 && NoExcessDigits(value);

    private static bool NoExcessDigits(double value) => value == Math.Round(value, 2);
}