namespace VisualVid.Core.Helpers;

public static class FileHelper
{
    public static string ReadFile(string path)
    {
        using var reader = new StreamReader(path);
        return reader.ReadToEnd();
    }

    public static void WriteFile(string path, string content)
    {
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        using var writer = new StreamWriter(path);
        writer.Write(content);
    }
}
