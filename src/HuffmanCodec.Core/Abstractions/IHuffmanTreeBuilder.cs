using HuffmanCodec.Core.Models;

namespace HuffmanCodec.Core.Abstractions;

public interface IHuffmanTreeBuilder
{
    /// <summary>Null если все частоты нулевые (пустые данные).</summary>
    HuffmanNode? BuildTree(ReadOnlySpan<uint> frequencies);
}
