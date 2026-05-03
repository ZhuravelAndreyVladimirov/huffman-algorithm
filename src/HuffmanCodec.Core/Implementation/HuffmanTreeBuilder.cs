using HuffmanCodec.Core.Abstractions;
using HuffmanCodec.Core.Models;

namespace HuffmanCodec.Core.Implementation;

public sealed class HuffmanTreeBuilder : IHuffmanTreeBuilder
{
    public HuffmanNode? BuildTree(ReadOnlySpan<uint> frequencies)
    {
        var leaves = new List<HuffmanNode>();
        for (var b = 0; b < 256; b++)
        {
            var w = frequencies[b];
            if (w > 0)
                leaves.Add(HuffmanNode.Leaf(w, (byte)b));
        }

        if (leaves.Count == 0)
            return null;

        // Один уникальный символ: классическое дерево из двух листьев (второй — фиктивный с весом 0, не встречается в данных).
        if (leaves.Count == 1)
        {
            var only = leaves[0];
            var sym = only.Symbol!.Value;
            var dummyByte = sym == 0 ? (byte)1 : (byte)0;
            var dummy = HuffmanNode.Leaf(0, dummyByte);
            // Порядок детей: меньший TieBreakKey слева (ветка 0).
            return sym < dummyByte
                ? HuffmanNode.Internal(only, dummy)
                : HuffmanNode.Internal(dummy, only);
        }

        // Детерминизм: при равных весах — меньший индекс байта (TieBreakKey) «раньше» в приоритете очереди.
        var pq = new PriorityQueue<HuffmanNode, (uint Weight, int Tie)>();
        foreach (var n in leaves)
            pq.Enqueue(n, (n.Weight, n.TieBreakKey));

        while (pq.Count > 1)
        {
            pq.TryDequeue(out var first, out _);
            pq.TryDequeue(out var second, out _);
            // first — минимум по (Weight, Tie); second — следующий. Слева меньший по тем же ключам.
            if (Compare(first!, second!) > 0)
                (first, second) = (second, first);

            var parent = HuffmanNode.Internal(first!, second!);
            pq.Enqueue(parent, (parent.Weight, parent.TieBreakKey));
        }

        pq.TryDequeue(out var root, out _);
        return root;
    }

    private static int Compare(HuffmanNode a, HuffmanNode b)
    {
        var cw = a.Weight.CompareTo(b.Weight);
        if (cw != 0)
            return cw;
        return a.TieBreakKey.CompareTo(b.TieBreakKey);
    }
}
