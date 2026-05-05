using System.Diagnostics;

namespace HuffmanCodec.App;

internal static class ExplorerHelper
{
    public static void TrySelectInExplorer(string filePath)
    {
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            return;
        var full = Path.GetFullPath(filePath);
        Process.Start(new ProcessStartInfo
        {
            FileName = "explorer.exe",
            Arguments = $"/select,\"{full}\"",
            UseShellExecute = true
        });
    }
}
