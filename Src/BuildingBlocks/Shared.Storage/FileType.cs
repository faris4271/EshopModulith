namespace Shared.Storage;

public enum FileType
{
    Image,
    Document,
    Pdf
}

public class FileValidationRules
{
    public IReadOnlyList<string> AllowedExtensions { get; init; } = Array.Empty<string>();
    public int MaxSizeInMB { get; init; } = 5;
}

public static class FileTypeMetadata
{
    public static FileValidationRules GetRules(FileType type) =>
        type switch
        {
            FileType.Image => new()
            {
                AllowedExtensions = [
         ".jpg", ".jpeg", ".png",   // Standard formats
        ".gif", ".bmp",            // Legacy/Common formats
        ".webp",                   // Modern web format
        ".tiff", ".tif",           // High-quality/Print formats
        ".ico",                    // Icon format
        ".svg",                    // Vector format (Commonly used)
        ".heic", ".heif",          // Apple/Mobile modern formats
        ".jfif"                    // JPEG File Interchange Format
     ],
                MaxSizeInMB = 5
            },
            FileType.Pdf => new() { AllowedExtensions = [".pdf"], MaxSizeInMB = 10 },
            _ => throw new NotSupportedException($"Unsupported file type: {type}")
        };
}