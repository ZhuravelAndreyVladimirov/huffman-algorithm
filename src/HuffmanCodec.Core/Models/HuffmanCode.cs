namespace HuffmanCodec.Core.Models;

/// <summary>
/// Битовый код символа: биты в младших bitCount разрядах, старший значащий бит — ближе к корню дерева.
/// </summary>
public readonly record struct HuffmanCode(uint Bits, int BitCount);
