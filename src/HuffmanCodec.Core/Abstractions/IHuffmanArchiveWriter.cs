using HuffmanCodec.Core.Models;

namespace HuffmanCodec.Core.Abstractions;

public interface IHuffmanArchiveWriter
{
    /// <param name="progress">0–100 по этапам алгоритма (чтение, частоты, дерево, кодирование, запись).</param>
    Task WriteCompressedAsync(
        Stream source,
        Stream destination,
        CancellationToken cancellationToken = default,
        IProgress<CodecProgress>? progress = null);

    void WriteCompressed(Stream source, Stream destination);
}
