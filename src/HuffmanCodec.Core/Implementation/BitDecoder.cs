using HuffmanCodec.Core.Abstractions;
using HuffmanCodec.Core.Models;

namespace HuffmanCodec.Core.Implementation;

public sealed class BitDecoder : IBitDecoder
{
    public byte[] Decode(
        ReadOnlySpan<byte> payload,
        byte padBits,
        HuffmanNode? root,
        long originalLength,
        ReadOnlySpan<uint> frequencies)
    {
        _ = frequencies;
        if (originalLength < 0)
            throw new ArgumentOutOfRangeException(nameof(originalLength));
        if (originalLength == 0)
            return Array.Empty<byte>();

        if (root is null)
            throw new InvalidOperationException("Дерево отсутствует при ненулевой длине данных.");

        var buf = payload.ToArray();
        var reader = new HuffmanBitReader(buf, padBits);
        var treeRoot = root;
        var result = new byte[originalLength];

        for (var i = 0; i < originalLength; i++)
        {
            var node = treeRoot;
            while (!node!.IsLeaf)
                node = reader.ReadBit() ? node.Right! : node.Left!;
            result[i] = node.Symbol!.Value;
        }

        return result;
    }
}
