using Xunit;
using VisualVid.Core.Helpers;

namespace VisualVid.Tests.Core;

public class FileHelperTests
{
    private readonly string _tempDir;

    public FileHelperTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), $"VisualVidTests_{Guid.NewGuid():N}");
        Directory.CreateDirectory(_tempDir);
    }

    [Fact]
    public void WriteFile_CreatesDirectoryAndFile()
    {
        var path = Path.Combine(_tempDir, "sub", "test.txt");
        FileHelper.WriteFile(path, "Hello World");

        Assert.True(File.Exists(path));
        Assert.Equal("Hello World", File.ReadAllText(path));
    }

    [Fact]
    public void ReadFile_ReturnsFileContents()
    {
        var path = Path.Combine(_tempDir, "read.txt");
        File.WriteAllText(path, "Test Content");

        var result = FileHelper.ReadFile(path);
        Assert.Equal("Test Content", result);
    }

    [Fact]
    public void WriteFile_OverwritesExistingFile()
    {
        var path = Path.Combine(_tempDir, "overwrite.txt");
        FileHelper.WriteFile(path, "First");
        FileHelper.WriteFile(path, "Second");

        Assert.Equal("Second", File.ReadAllText(path));
    }

    [Fact]
    public void ReadFile_NonExistentFile_ThrowsException()
    {
        var path = Path.Combine(_tempDir, "nonexistent.txt");
        Assert.Throws<FileNotFoundException>(() => FileHelper.ReadFile(path));
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, true);
    }
}
