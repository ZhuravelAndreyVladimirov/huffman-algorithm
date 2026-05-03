using HuffmanCodec.Core.Models;

namespace HuffmanCodec.Core.Abstractions;

public interface ICodeTableBuilder
{
    IReadOnlyDictionary<byte, HuffmanCode> BuildCodes(HuffmanNode? root, ReadOnlySpan<uint> frequencies);
}
