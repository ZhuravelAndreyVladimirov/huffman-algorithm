using System.Buffers.Binary;
using HuffmanCodec.Core.Abstractions;

namespace HuffmanCodec.Core.Implementation;

public sealed class HuffmanArchiveReader : IHuffmanArchiveReader
{
    private readonly IHuffmanTreeBuilder _treeBuilder;
    private readonly IBitDecoder _bitDecoder;

    public HuffmanArchiveReader(IHuffmanTreeBuilder treeBuilder, IBitDecoder bitDecoder)
    {
        _treeBuilder = treeBuilder;
        _bitDecoder = bitDecoder;
    }

    public byte[] ReadDecompressed(Stream source)
    {
        using var ms = new MemoryStream();
        source.CopyTo(ms);
        return ReadDecompressed(ms.ToArray().AsSpan(), null);
    }

    public async Task<byte[]> ReadDecompressedAsync(
        Stream source,
        CancellationToken cancellationToken = default,
        IProgress<int>? progress = null)
    {
        progress?.Report(8);
        using var ms = new MemoryStream();
        await source.CopyToAsync(ms, cancellationToken).ConfigureAwait(false);
        progress?.Report(22);
        return ReadDecompressed(ms.ToArray().AsSpan(), progress);
    }

    private byte[] ReadDecompressed(ReadOnlySpan<byte> file, IProgress<int>? progress)
    {
        progress?.Report(28);
        if (file.Length < HuffmanArchiveFormat.HeaderSize + 1)
            throw new InvalidDataException("Файл слишком короткий для заголовка и маркера выравнивания.");

        if (!file.StartsWith(HuffmanArchiveFormat.Magic))
            throw new InvalidDataException("Неверная сигнатура архива (ожидался HUFF).");

        if (file[4] != HuffmanArchiveFormat.Version)
            throw new InvalidDataException($"Неподдерживаемая версия формата: {file[4]}.");

        progress?.Report(38);
        var originalLength = BinaryPrimitives.ReadInt64LittleEndian(file.Slice(5, 8));
        if (originalLength < 0)
            throw new InvalidDataException("Отрицательная длина исходных данных.");

        var freq = new uint[256];
        var off = 13;
        for (var i = 0; i < 256; i++)
        {
            freq[i] = BinaryPrimitives.ReadUInt32LittleEndian(file.Slice(off, 4));
            off += 4;
        }

        progress?.Report(48);
        var padBits = file[^1];
        if (padBits > 7)
            throw new InvalidDataException("Некорректное значение padBits.");

        var payloadLen = file.Length - off - 1;
        if (payloadLen < 0)
            throw new InvalidDataException("Некорректная структура файла.");

        var payload = file.Slice(off, payloadLen).ToArray();

        var tree = _treeBuilder.BuildTree(freq);
        progress?.Report(58);
        var decoded = _bitDecoder.Decode(payload, padBits, tree, originalLength, freq);
        progress?.Report(88);
        return decoded;
    }
}
