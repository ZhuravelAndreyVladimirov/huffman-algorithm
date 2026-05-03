namespace HuffmanCodec.Core.Models;

/// <summary>
/// Узел дерева Хаффмана. Лист: Symbol задан, Left/Right null. Внутренний: Symbol null.
/// </summary>
public sealed class HuffmanNode
{
    private HuffmanNode(uint weight, byte? symbol, HuffmanNode? left, HuffmanNode? right, int tieBreakKey)
    {
        Weight = weight;
        Symbol = symbol;
        Left = left;
        Right = right;
        TieBreakKey = tieBreakKey;
    }

    public uint Weight { get; }
    public byte? Symbol { get; }
    public HuffmanNode? Left { get; }
    public HuffmanNode? Right { get; }

    /// <summary>Детерминированный tie-break при равных весах: минимальный индекс байта в поддереве.</summary>
    public int TieBreakKey { get; }

    public bool IsLeaf => Left is null && Right is null;

    public static HuffmanNode Leaf(uint weight, byte symbol) =>
        new(weight, symbol, null, null, symbol);

    /// <summary>Внутренний узел: левое ребро = 0, правое = 1 (как на учебных слайдах).</summary>
    public static HuffmanNode Internal(HuffmanNode left, HuffmanNode right) =>
        new(left.Weight + right.Weight, null, left, right, Math.Min(left.TieBreakKey, right.TieBreakKey));
}
