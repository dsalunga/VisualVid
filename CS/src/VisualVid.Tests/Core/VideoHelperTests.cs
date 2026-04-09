using Xunit;
using VisualVid.Core.Helpers;

namespace VisualVid.Tests.Core;

public class VideoHelperTests
{
    [Fact]
    public void FormatTags_NullInput_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, VideoHelper.FormatTags(null));
    }

    [Fact]
    public void FormatTags_EmptyInput_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, VideoHelper.FormatTags(""));
    }

    [Fact]
    public void FormatTags_WhitespaceOnly_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, VideoHelper.FormatTags("   "));
    }

    [Fact]
    public void FormatTags_SingleTag_ReturnsLink()
    {
        var result = VideoHelper.FormatTags("music");
        Assert.Contains("href=\"/Search?q=music\"", result);
        Assert.Contains(">music</a>", result);
    }

    [Fact]
    public void FormatTags_MultipleTags_SpaceSeparated()
    {
        var result = VideoHelper.FormatTags("music dance");
        Assert.Contains(">music</a>", result);
        Assert.Contains(">dance</a>", result);
    }

    [Fact]
    public void FormatTags_CommaSeparated_ParsesCorrectly()
    {
        var result = VideoHelper.FormatTags("rock,pop,jazz");
        Assert.Contains(">rock</a>", result);
        Assert.Contains(">pop</a>", result);
        Assert.Contains(">jazz</a>", result);
    }

    [Fact]
    public void FormatTags_SpecialCharacters_AreEncoded()
    {
        var result = VideoHelper.FormatTags("c#");
        // HTML encoding should encode special characters
        Assert.Contains("c#", result); // # is html-safe
        // URL encoding should encode the #
        Assert.Contains("q=c%23", result);
    }
}
