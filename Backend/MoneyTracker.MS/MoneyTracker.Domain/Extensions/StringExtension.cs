namespace MoneyTracker.Domain.Extensions;

using System.Globalization;

public static class StringExtension
{
    public static string ToUpperFirstLetter(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        var a = str.ToCharArray();
        a[0] = char.ToUpper(a[0], CultureInfo.CurrentCulture);
        return new string(a);
    }

    public static string ToLowerFirstLetter(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        var a = str.ToCharArray();
        a[0] = char.ToLower(a[0], CultureInfo.CurrentCulture);
        return new string(a);
    }
}