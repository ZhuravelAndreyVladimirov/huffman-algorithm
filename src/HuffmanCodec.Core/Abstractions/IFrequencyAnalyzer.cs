namespace HuffmanCodec.Core.Abstractions;

public interface IFrequencyAnalyzer
{
    uint[] Analyze(ReadOnlySpan<byte> data);

    uint[] Analyze(Stream input, CancellationToken cancellationToken = default);
}
