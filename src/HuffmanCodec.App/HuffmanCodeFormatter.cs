using System.Text;
using HuffmanCodec.Core.Models;

namespace HuffmanCodec.App;

internal static class HuffmanCodeFormatter
{
    public static string ToBitString(HuffmanCode code)
    {
        if (code.BitCount <= 0)
            return "";
        var sb = new StringBuilder(code.BitCount);
        for (var i = code.BitCount - 1; i >= 0; i--)
            sb.Append(((code.Bits >> i) & 1) != 0 ? '1' : '0');
        return sb.ToString();
    }
}
