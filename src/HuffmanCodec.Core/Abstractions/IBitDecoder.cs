using HuffmanCodec.Core.Models;

namespace HuffmanCodec.Core.Abstractions;

public interface IBitDecoder
{
    byte[] Decode(ReadOnlySpan<byte> payload, byte padBits, HuffmanNode? root, long originalLength, ReadOnlySpan<uint> frequencies);
}
