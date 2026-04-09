using System.Text.RegularExpressions;

namespace VisualVid.Core.Helpers;

public static class Validator
{
    private static readonly Regex EmailRegex = new(
        @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static bool IsValidEmail(string email)
    {
        return !string.IsNullOrWhiteSpace(email) && EmailRegex.IsMatch(email);
    }
}
