using HuffmanCodec.Core.Abstractions;
using HuffmanCodec.Core.Implementation;

namespace HuffmanCodec.App;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var frequencyAnalyzer = new FrequencyAnalyzer();
        var treeBuilder = new HuffmanTreeBuilder();
        var codeTableBuilder = new CodeTableBuilder();
        var bitEncoder = new BitEncoder();
        var bitDecoder = new BitDecoder();
        var archiveWriter = new HuffmanArchiveWriter(frequencyAnalyzer, treeBuilder, codeTableBuilder, bitEncoder);
        var archiveReader = new HuffmanArchiveReader(treeBuilder, bitDecoder);
        var codec = new HuffmanCodecService(frequencyAnalyzer, treeBuilder, codeTableBuilder, archiveWriter, archiveReader);

        Application.Run(new MainForm(codec));
    }
}
