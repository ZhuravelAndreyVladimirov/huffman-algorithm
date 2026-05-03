using HuffmanCodec.Core.Abstractions;
using HuffmanCodec.Core.Models;

namespace HuffmanCodec.Core.Implementation;

/// <summary>Иерархическая раскладка: глубина по Y, ширина пропорциональна весу поддерева.</summary>
public sealed class TreeLayoutCalculator : ITreeLayoutCalculator
{
    private readonly List<LayoutNode> _nodes = new();
    private readonly List<LayoutEdge> _edges = new();

    public TreeLayoutResult Layout(HuffmanNode? root, float width, float height)
    {
        _nodes.Clear();
        _edges.Clear();
        if (root is null || width <= 0 || height <= 0)
            return new TreeLayoutResult(_nodes, _edges);

        const float margin = 16f;
        const float top = 24f;
        var bottom = height - margin;
        var usableW = width - 2 * margin;
        LayoutNodeRecursive(root, margin, margin + usableW, top, bottom, 0);
        return new TreeLayoutResult(_nodes.ToArray(), _edges.ToArray());
    }

    private void LayoutNodeRecursive(HuffmanNode node, float x0, float x1, float y0, float y1, int depth)
    {
        var cx = (x0 + x1) * 0.5f;
        var cy = y0;
        var radius = Math.Min(22f, (x1 - x0) * 0.25f);
        if (node.IsLeaf)
        {
            _nodes.Add(new LayoutNode(cx, cy, radius, node.Weight, node.Symbol, null, true));
            return;
        }

        _nodes.Add(new LayoutNode(cx, cy, radius, node.Weight, null, null, false));
        var total = (float)(node.Left!.Weight + node.Right!.Weight);
        var split = x0 + (x1 - x0) * (node.Left.Weight / total);

        var yNext = y0 + Math.Max(48f, (y1 - y0) * 0.18f);
        _edges.Add(new LayoutEdge(cx, cy + radius, (x0 + split) * 0.5f, yNext - radius, 0));
        _edges.Add(new LayoutEdge(cx, cy + radius, (split + x1) * 0.5f, yNext - radius, 1));

        LayoutNodeRecursive(node.Left, x0, split, yNext, y1, depth + 1);
        LayoutNodeRecursive(node.Right, split, x1, yNext, y1, depth + 1);
    }
}
