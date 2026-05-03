namespace HuffmanCodec.Core.Implementation;

/// <summary>Побитовая запись: внутри байта биты идут от старшего к младшему.</summary>
internal sealed class HuffmanBitWriter
{
    private readonly List<byte> _bytes = new();
    private int _totalBits;

    public void WriteCode(uint bits, int bitCount)
    {
        for (var i = bitCount - 1; i >= 0; i--)
            WriteBit((int)((bits >> i) & 1u));
    }

    private void WriteBit(int bit)
    {
        var byteIndex = _totalBits / 8;
        var bitInByte = 7 - (_totalBits % 8);
        while (byteIndex >= _bytes.Count)
            _bytes.Add(0);
        if (bit != 0)
            _bytes[byteIndex] |= (byte)(1 << bitInByte);
        _totalBits++;
    }

    /// <summary>Завершает буфер и возвращает число незначащих битов в последнем байте (0..7).</summary>
    public (byte[] Bytes, byte PadBits) Finish()
    {
        if (_totalBits == 0)
            return (Array.Empty<byte>(), 0);
        var pad = (byte)((8 - (_totalBits % 8)) % 8);
        return (_bytes.ToArray(), pad);
    }
}
