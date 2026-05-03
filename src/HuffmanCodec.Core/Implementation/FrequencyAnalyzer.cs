using HuffmanCodec.Core.Abstractions;

namespace HuffmanCodec.Core.Implementation;

public sealed class FrequencyAnalyzer : IFrequencyAnalyzer
{
    public uint[] Analyze(ReadOnlySpan<byte> data)
    {
        var freq = new uint[256];
        foreach (var b in data)
            freq[b]++;
        return freq;
    }

    public uint[] Analyze(Stream input, CancellationToken cancellationToken = default)
    {
        var freq = new uint[256];
        var buffer = new byte[64 * 1024];
        int read;
        while ((read = input.Read(buffer)) > 0)
        {
            cancellationToken.ThrowIfCancellationRequested();
            for (var i = 0; i < read; i++)
                freq[buffer[i]]++;
        }

        return freq;
    }
}
