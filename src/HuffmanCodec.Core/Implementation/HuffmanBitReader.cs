namespace HuffmanCodec.Core.Implementation;

/// <summary>Чтение битов из буфера; последние padBits битов потока игнорируются.</summary>
internal sealed class HuffmanBitReader
{
    private readonly byte[] _data;
    private readonly int _totalBits;
    private int _position;

    public HuffmanBitReader(byte[] data, byte padBits)
    {
        _data = data;
        _totalBits = data.Length * 8 - padBits;
        _position = 0;
    }

    public bool ReadBit()
    {
        if (_position >= _totalBits)
            throw new InvalidOperationException("Недостаточно битов в потоке.");

        var byteIndex = _position / 8;
        var bitInByte = 7 - (_position % 8);
        _position++;
        return ((_data[byteIndex] >> bitInByte) & 1) != 0;
    }

    public bool HasMoreBits => _position < _totalBits;
}
