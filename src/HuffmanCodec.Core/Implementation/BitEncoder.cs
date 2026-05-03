using HuffmanCodec.Core.Abstractions;
using HuffmanCodec.Core.Models;

namespace HuffmanCodec.Core.Implementation;

public sealed class BitEncoder : IBitEncoder
{
    public (byte[] Payload, byte PadBits) Encode(ReadOnlySpan<byte> data, IReadOnlyDictionary<byte, HuffmanCode> codes)
    {
        if (data.Length == 0)
            return (Array.Empty<byte>(), 0);

        var writer = new HuffmanBitWriter();
        foreach (var b in data)
        {
            var code = codes[b];
            writer.WriteCode(code.Bits, code.BitCount);
        }

        return writer.Finish();
    }
}
