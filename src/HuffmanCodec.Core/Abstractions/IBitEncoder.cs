using HuffmanCodec.Core.Models;

namespace HuffmanCodec.Core.Abstractions;

public interface IBitEncoder
{
    /// <summary>Возвращает полезную нагрузку и число «лишних» битов в последнем байте (0..7).</summary>
    (byte[] Payload, byte PadBits) Encode(ReadOnlySpan<byte> data, IReadOnlyDictionary<byte, HuffmanCode> codes);
}
