using SixLabors.ImageSharp.Formats.Png;

namespace ChatApp.BlazorServer.Helpers;

public class ImageHelper
{
    public static async Task<string> StreamToAvaImageBase64(Stream stream)
    {
        using var image = await Image.LoadAsync(stream);
        using var memoryStream = new MemoryStream();
        await image.SaveAsync(memoryStream, new PngEncoder
        {
            CompressionLevel = PngCompressionLevel.BestCompression,
        });
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(500, 500),
            Mode = ResizeMode.Crop,
        }));
        var buffer = memoryStream.ToArray();
        var imageBase64 = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";
        return imageBase64;
    }
}