namespace Audit.Services;

public static class FileGuard
{
    public static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf", ".png", ".jpg", ".jpeg", ".docx", ".zip", ".txt",
        ".cs", ".csproj", ".razor", ".json", ".xml", ".yml", ".yaml",
        ".js", ".jsx", ".ts", ".tsx", ".mjs", ".cjs",
        ".py", ".rb", ".go", ".rs", ".java", ".kt", ".swift",
        ".c", ".h", ".cpp", ".hpp", ".cc",
        ".php", ".sh", ".bash", ".zsh", ".sql",
        ".html", ".css", ".scss", ".sass", ".less",
        ".vue", ".svelte", ".astro", ".dart", ".lua", ".r", ".pl", ".ex", ".exs",
        ".md", ".toml", ".ini", ".env"
    };

    public const long MaxBytes = 25 * 1024 * 1024;

    public static bool IsAllowed(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName)) return false;
        var ext = Path.GetExtension(fileName);
        return !string.IsNullOrEmpty(ext) && AllowedExtensions.Contains(ext);
    }
}
