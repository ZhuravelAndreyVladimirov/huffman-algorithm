using HuffmanCodec.Core.Models;

namespace HuffmanCodec.App;

internal static class CodecAnalysisGridBinder
{
    public static void Bind(DataGridView grid, uint[] frequencies, IReadOnlyDictionary<byte, HuffmanCode> codes)
    {
        grid.Rows.Clear();
        for (var i = 0; i < 256; i++)
        {
            var f = frequencies[i];
            if (f == 0)
                continue;
            var b = (byte)i;
            if (!codes.TryGetValue(b, out var hc))
                continue;
            grid.Rows.Add(i, $"0x{i:X2}", PrintableSymbol(b), f, HuffmanCodeFormatter.ToBitString(hc), hc.BitCount);
        }
    }

    private static string PrintableSymbol(byte b) =>
        b is >= 32 and < 127 ? $"{(char)b}" : "—";
}
