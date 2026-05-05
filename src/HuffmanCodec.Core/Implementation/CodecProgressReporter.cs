using HuffmanCodec.Core.Models;

namespace HuffmanCodec.Core.Implementation;

internal static class CodecProgressReporter
{
    public static void Report(IProgress<CodecProgress>? progress, int percent, string stageId) =>
        progress?.Report(new CodecProgress(percent, stageId));
}
