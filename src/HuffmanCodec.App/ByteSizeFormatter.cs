namespace HuffmanCodec.App;

internal static class ByteSizeFormatter
{
    private static readonly string[] Units = ["Б", "КиБ", "МиБ", "ГиБ"];

    public static string Format(long bytes)
    {
        if (bytes < 0)
            bytes = 0;
        double v = bytes;
        var u = 0;
        while (v >= 1024 && u < Units.Length - 1)
        {
            v /= 1024;
            u++;
        }

        return u == 0 ? $"{bytes} {Units[0]}" : $"{v:0.##} {Units[u]}";
    }

    /// <summary>Размер архива относительно исходника, % (100 % = без выигрыша).</summary>
    public static double ArchivePercentOfOriginal(long originalBytes, long archiveBytes)
    {
        if (originalBytes <= 0)
            return archiveBytes == 0 ? 100 : double.PositiveInfinity;
        return 100.0 * archiveBytes / originalBytes;
    }
}
