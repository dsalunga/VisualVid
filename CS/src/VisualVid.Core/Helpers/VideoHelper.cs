using System.Web;

namespace VisualVid.Core.Helpers;

public static class VideoHelper
{
    public static string FormatTags(string? tags)
    {
        if (string.IsNullOrWhiteSpace(tags))
            return string.Empty;

        var tagList = tags.Split([' ', ','], StringSplitOptions.RemoveEmptyEntries);
        var formatted = tagList.Select(tag =>
            $"<a href=\"/Search?q={Uri.EscapeDataString(tag.Trim())}\">{HttpUtility.HtmlEncode(tag.Trim())}</a>");

        return string.Join(" ", formatted);
    }
}
