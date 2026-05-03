using System.Buffers.Binary;
using HuffmanCodec.Core.Abstractions;

namespace HuffmanCodec.Core.Implementation;

public sealed class HuffmanArchiveWriter : IHuffmanArchiveWriter
{
    private readonly IFrequencyAnalyzer _frequencies;
    private readonly IHuffmanTreeBuilder _treeBuilder;
    private readonly ICodeTableBuilder _codeTable;
    private readonly IBitEncoder _bitEncoder;

    public HuffmanArchiveWriter(
        IFrequencyAnalyzer frequencies,
        IHuffmanTreeBuilder treeBuilder,
        ICodeTableBuilder codeTable,
        IBitEncoder bitEncoder)
    {
        _frequencies = frequencies;
        _treeBuilder = treeBuilder;
        _codeTable = codeTable;
        _bitEncoder = bitEncoder;
    }

    public void WriteCompressed(Stream source, Stream destination)
    {
        using var ms = new MemoryStream();
        source.CopyTo(ms);
        var data = ms.ToArray();
        var freq = _frequencies.Analyze(data);
        var tree = _treeBuilder.BuildTree(freq);
        var codes = _codeTable.BuildCodes(tree, freq);
        var (payload, padBits) = _bitEncoder.Encode(data, codes);

        destination.Write(HuffmanArchiveFormat.Magic);
        destination.WriteByte(HuffmanArchiveFormat.Version);
        var lenBuf = new byte[8];
        BinaryPrimitives.WriteInt64LittleEndian(lenBuf, data.LongLength);
        destination.Write(lenBuf);
        for (var i = 0; i < 256; i++)
        {
            var b = new byte[4];
            BinaryPrimitives.WriteUInt32LittleEndian(b, freq[i]);
            destination.Write(b);
        }

        destination.Write(payload);
        destination.WriteByte(padBits);
    }

    public async Task WriteCompressedAsync(
        Stream source,
        Stream destination,
        CancellationToken cancellationToken = default,
        IProgress<int>? progress = null)
    {
        progress?.Report(5);
        using var ms = new MemoryStream();
        await source.CopyToAsync(ms, cancellationToken).ConfigureAwait(false);
        progress?.Report(18);
        var data = ms.ToArray();
        var freq = _frequencies.Analyze(data);
        progress?.Report(30);
        var tree = _treeBuilder.BuildTree(freq);
        progress?.Report(40);
        var codes = _codeTable.BuildCodes(tree, freq);
        progress?.Report(48);
        var (payload, padBits) = _bitEncoder.Encode(data, codes);
        progress?.Report(58);

        await destination.WriteAsync(HuffmanArchiveFormat.Magic.ToArray(), cancellationToken).ConfigureAwait(false);
        await destination.WriteAsync(new[] { HuffmanArchiveFormat.Version }, cancellationToken).ConfigureAwait(false);
        progress?.Report(62);
        var lenBuf = new byte[8];
        BinaryPrimitives.WriteInt64LittleEndian(lenBuf, data.LongLength);
        await destination.WriteAsync(lenBuf, cancellationToken).ConfigureAwait(false);
        progress?.Report(65);
        for (var i = 0; i < 256; i++)
        {
            var b = new byte[4];
            BinaryPrimitives.WriteUInt32LittleEndian(b, freq[i]);
            await destination.WriteAsync(b, cancellationToken).ConfigureAwait(false);
            if ((i & 31) == 31)
                progress?.Report(65 + 22 * (i + 1) / 256);
        }

        progress?.Report(88);
        if (payload.Length > 0)
            await destination.WriteAsync(payload, cancellationToken).ConfigureAwait(false);
        await destination.WriteAsync(new[] { padBits }, cancellationToken).ConfigureAwait(false);
        progress?.Report(96);
    }
}
