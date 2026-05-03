namespace HuffmanCodec.Core.Abstractions;

public interface IHuffmanArchiveReader
{
    /// <param name="progress">0–100 по этапам чтения архива и декодирования.</param>
    Task<byte[]> ReadDecompressedAsync(
        Stream source,
        CancellationToken cancellationToken = default,
        IProgress<int>? progress = null);

    byte[] ReadDecompressed(Stream source);
}
