using HuffmanCodec.Core.Abstractions;
using HuffmanCodec.Core.Models;

namespace HuffmanCodec.Core.Implementation;

public sealed class HuffmanCodecService : IHuffmanCodec
{
    private readonly IFrequencyAnalyzer _frequencyAnalyzer;
    private readonly IHuffmanTreeBuilder _treeBuilder;
    private readonly ICodeTableBuilder _codeTableBuilder;
    private readonly IHuffmanArchiveWriter _archiveWriter;
    private readonly IHuffmanArchiveReader _archiveReader;

    public HuffmanCodecService(
        IFrequencyAnalyzer frequencyAnalyzer,
        IHuffmanTreeBuilder treeBuilder,
        ICodeTableBuilder codeTableBuilder,
        IHuffmanArchiveWriter archiveWriter,
        IHuffmanArchiveReader archiveReader)
    {
        _frequencyAnalyzer = frequencyAnalyzer;
        _treeBuilder = treeBuilder;
        _codeTableBuilder = codeTableBuilder;
        _archiveWriter = archiveWriter;
        _archiveReader = archiveReader;
    }

    public async Task CompressFileAsync(
        string inputPath,
        string outputPath,
        CancellationToken cancellationToken = default,
        IProgress<CodecProgress>? progress = null)
    {
        CodecProgressReporter.Report(progress, 0, "prepare");
        await using var input = File.OpenRead(inputPath);
        CodecProgressReporter.Report(progress, 2, "open_input");
        await using var output = File.Create(outputPath);
        CodecProgressReporter.Report(progress, 4, "open_output");
        await _archiveWriter.WriteCompressedAsync(input, output, cancellationToken, progress).ConfigureAwait(false);
        CodecProgressReporter.Report(progress, 100, "done");
    }

    public async Task DecompressFileAsync(
        string inputPath,
        string outputPath,
        CancellationToken cancellationToken = default,
        IProgress<CodecProgress>? progress = null)
    {
        CodecProgressReporter.Report(progress, 0, "prepare");
        await using var input = File.OpenRead(inputPath);
        CodecProgressReporter.Report(progress, 2, "open_input");
        var data = await _archiveReader.ReadDecompressedAsync(input, cancellationToken, progress).ConfigureAwait(false);
        CodecProgressReporter.Report(progress, 92, "write_output");
        await File.WriteAllBytesAsync(outputPath, data, cancellationToken).ConfigureAwait(false);
        CodecProgressReporter.Report(progress, 100, "done");
    }

    public async Task<CodecPreviewResult> PreviewAsync(string path, CancellationToken cancellationToken = default)
    {
        var bytes = await File.ReadAllBytesAsync(path, cancellationToken).ConfigureAwait(false);
        var freq = _frequencyAnalyzer.Analyze(bytes);
        var tree = _treeBuilder.BuildTree(freq);
        var codes = _codeTableBuilder.BuildCodes(tree, freq);
        return new CodecPreviewResult(freq, tree, codes, bytes.LongLength);
    }
}
