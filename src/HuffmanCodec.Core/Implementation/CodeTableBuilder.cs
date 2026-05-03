using HuffmanCodec.Core.Abstractions;
using HuffmanCodec.Core.Models;

namespace HuffmanCodec.Core.Implementation;

public sealed class CodeTableBuilder : ICodeTableBuilder
{
    public IReadOnlyDictionary<byte, HuffmanCode> BuildCodes(HuffmanNode? root, ReadOnlySpan<uint> frequencies)
    {
        var map = new Dictionary<byte, HuffmanCode>();
        if (root is null)
            return map;

        var freqCopy = frequencies.ToArray();

        void Visit(HuffmanNode node, uint prefix, int depth)
        {
            if (node.IsLeaf)
            {
                var sym = node.Symbol!.Value;
                // Фиктивный лист с весом 0 не кодируется в данных.
                if (freqCopy[sym] == 0)
                    return;
                map[sym] = depth == 0 ? new HuffmanCode(0, 1) : new HuffmanCode(prefix, depth);
                return;
            }

            Visit(node.Left!, (prefix << 1) | 0u, depth + 1);
            Visit(node.Right!, (prefix << 1) | 1u, depth + 1);
        }

        Visit(root, 0, 0);
        return map;
    }
}
