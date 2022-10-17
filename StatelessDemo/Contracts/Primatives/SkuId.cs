// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Text.RegularExpressions;
using Vogen;

namespace PdnNuGetTalk.StatelessDemo.Contracts;

[ValueObject(typeof(string))]
public partial struct SkuId
{
    private static readonly Regex validator = GetValidator();

    private static Validation Validate(string value)
    {
        return IsValue(value) ? Validation.Ok : Validation.Invalid(
            "SkuIds must conform to the following format: \"XXX-00000-00\".");
    }

    public static bool IsValue(string value) => validator.IsMatch(value);

    [GeneratedRegex("^[A-Z][A-Z0-9]{2}-\\d{5}-\\d{2}$")]
    private static partial Regex GetValidator();
}