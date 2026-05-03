using HuffmanCodec.Core.Models;

namespace HuffmanCodec.Core.Abstractions;

/// <summary>Фасад сжатия/распаковки для UI и тестов.</summary>
public interface IHuffmanCodec
{
    /// <param name="progress">0–100, этапы чтения/построения дерева/кодирования/записи.</param>
    Task CompressFileAsync(
        string inputPath,
        string outputPath,
        CancellationToken cancellationToken = default,
        IProgress<int>? progress = null);

    Task DecompressFileAsync(
        string inputPath,
        string outputPath,
        CancellationToken cancellationToken = default,
        IProgress<int>? progress = null);

    /// <summary>Читает файл, строит дерево и таблицу для визуализации (без записи архива).</summary>
    Task<CodecPreviewResult> PreviewAsync(string path, CancellationToken cancellationToken = default);
}

public sealed record CodecPreviewResult(
    uint[] Frequencies,
    HuffmanNode? Tree,
    IReadOnlyDictionary<byte, HuffmanCode> Codes,
    long FileLength);
