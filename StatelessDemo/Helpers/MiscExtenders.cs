// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Text;
using System.Text.RegularExpressions;

namespace PdnNuGetTalk.StatelessDemo.Helpers;

public static partial class MiscExtenders
{
    private static readonly Regex emailValidator = GetValidator();

    public static string ToDelimitedString<T>(this List<T> items,
        Func<T, string>? getValue = null,
        string delimiter = ", ", string finalDelimiter = " or ")
    {
        ArgumentNullException.ThrowIfNull(items, nameof(delimiter));

        if (!items.Any() || items.Any(i => i == null))
            throw new ArgumentOutOfRangeException(nameof(items));

        ArgumentNullException.ThrowIfNull(delimiter, nameof(delimiter));

        ArgumentNullException.ThrowIfNull(finalDelimiter, nameof(finalDelimiter));

        var sb = new StringBuilder();

        for (int i = 0; i < items.Count - 1; i++)
        {
            if (i > 0)
                sb.Append(delimiter);

            if (getValue == null)
                sb.Append(items[i]);
            else
                sb.Append(getValue(items[i]));
        }

        if (sb.Length > 0)
            sb.Append(finalDelimiter);

        sb.Append(items.Last());

        return sb.ToString();
    }

    public static bool IsEmailAddress(this string value) =>
        emailValidator.IsMatch(value);

    [GeneratedRegex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")]
    private static partial Regex GetValidator();
}