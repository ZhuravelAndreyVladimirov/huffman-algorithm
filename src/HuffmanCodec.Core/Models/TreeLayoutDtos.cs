namespace HuffmanCodec.Core.Models;

/// <summary>Узел для отрисовки дерева (без ссылки на HuffmanNode — только данные).</summary>
public sealed record LayoutNode(
    float CenterX,
    float CenterY,
    float Radius,
    uint Weight,
    byte? Symbol,
    string? CodeLabel,
    bool IsLeaf);

/// <summary>Ребро с меткой бита (0 — влево, 1 — вправо).</summary>
public sealed record LayoutEdge(float X1, float Y1, float X2, float Y2, int Bit);
