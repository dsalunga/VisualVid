using SkiaSharp;

namespace VisualVid.Core.Helpers;

public static class ImageUtil
{
    public static (int Width, int Height) GetImageSize(string path)
    {
        using var stream = File.OpenRead(path);
        using var codec = SKCodec.Create(stream);
        return (codec.Info.Width, codec.Info.Height);
    }

    public static void GenerateThumbnail(string sourcePath, string destPath, int width, int height)
    {
        var directory = Path.GetDirectoryName(destPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        using var sourceStream = File.OpenRead(sourcePath);
        using var original = SKBitmap.Decode(sourceStream);

        using var resized = original.Resize(new SKImageInfo(width, height), new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));
        using var image = SKImage.FromBitmap(resized);
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, 90);

        using var outStream = File.OpenWrite(destPath);
        data.SaveTo(outStream);
    }
}
