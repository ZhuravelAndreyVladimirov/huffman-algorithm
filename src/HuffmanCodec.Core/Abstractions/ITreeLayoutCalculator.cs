using HuffmanCodec.Core.Models;

namespace HuffmanCodec.Core.Abstractions;

public interface ITreeLayoutCalculator
{
    /// <summary>Рассчитывает координаты для области с заданными размерами (логика без GDI).</summary>
    TreeLayoutResult Layout(HuffmanNode? root, float width, float height);
}

public sealed record TreeLayoutResult(IReadOnlyList<LayoutNode> Nodes, IReadOnlyList<LayoutEdge> Edges);
