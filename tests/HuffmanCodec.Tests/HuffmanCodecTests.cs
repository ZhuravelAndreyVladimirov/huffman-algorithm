using System.Text;
using HuffmanCodec.Core.Implementation;

namespace HuffmanCodec.Tests;

public class HuffmanCodecTests
{
    private static readonly HuffmanCodecService Codec = CreateCodec();

    private static HuffmanCodecService CreateCodec()
    {
        var fa = new FrequencyAnalyzer();
        var tb = new HuffmanTreeBuilder();
        var ct = new CodeTableBuilder();
        var enc = new BitEncoder();
        var dec = new BitDecoder();
        var w = new HuffmanArchiveWriter(fa, tb, ct, enc);
        var r = new HuffmanArchiveReader(tb, dec);
        return new HuffmanCodecService(fa, tb, ct, w, r);
    }

    [Fact]
    public void RoundTrip_RandomPayload()
    {
        var rnd = new Random(42);
        var data = new byte[10_000];
        rnd.NextBytes(data);
        using var msIn = new MemoryStream(data);
        using var msOut = new MemoryStream();
        var writer = new HuffmanArchiveWriter(new FrequencyAnalyzer(), new HuffmanTreeBuilder(), new CodeTableBuilder(), new BitEncoder());
        writer.WriteCompressed(msIn, msOut);
        msOut.Position = 0;
        var reader = new HuffmanArchiveReader(new HuffmanTreeBuilder(), new BitDecoder());
        var restored = reader.ReadDecompressed(msOut);
        Assert.Equal(data, restored);
    }

    [Fact]
    public void RoundTrip_EmptyFile()
    {
        using var msIn = new MemoryStream(Array.Empty<byte>());
        using var msOut = new MemoryStream();
        new HuffmanArchiveWriter(new FrequencyAnalyzer(), new HuffmanTreeBuilder(), new CodeTableBuilder(), new BitEncoder()).WriteCompressed(msIn, msOut);
        msOut.Position = 0;
        var restored = new HuffmanArchiveReader(new HuffmanTreeBuilder(), new BitDecoder()).ReadDecompressed(msOut);
        Assert.Empty(restored);
    }

    [Fact]
    public void RoundTrip_SingleDistinctByte()
    {
        var data = new byte[500];
        Array.Fill(data, (byte)7);
        using var msIn = new MemoryStream(data);
        using var msOut = new MemoryStream();
        new HuffmanArchiveWriter(new FrequencyAnalyzer(), new HuffmanTreeBuilder(), new CodeTableBuilder(), new BitEncoder()).WriteCompressed(msIn, msOut);
        msOut.Position = 0;
        var restored = new HuffmanArchiveReader(new HuffmanTreeBuilder(), new BitDecoder()).ReadDecompressed(msOut);
        Assert.Equal(data, restored);
    }

    /// <summary>Контент с весами как на слайде (a1..a6 → байты 0..5): круговой проход архива.</summary>
    [Fact]
    public void Slide_Weights_A1_to_A6_RoundTrip()
    {
        var payload = new List<byte>();
        void AddMany(byte symbol, int count)
        {
            for (var i = 0; i < count; i++)
                payload.Add(symbol);
        }

        AddMany(0, 25);
        AddMany(1, 5);
        AddMany(2, 25);
        AddMany(3, 10);
        AddMany(4, 25);
        AddMany(5, 10);
        var data = payload.ToArray();

        using var msIn = new MemoryStream(data);
        using var msOut = new MemoryStream();
        new HuffmanArchiveWriter(new FrequencyAnalyzer(), new HuffmanTreeBuilder(), new CodeTableBuilder(), new BitEncoder()).WriteCompressed(msIn, msOut);
        msOut.Position = 0;
        var restored = new HuffmanArchiveReader(new HuffmanTreeBuilder(), new BitDecoder()).ReadDecompressed(msOut);
        Assert.Equal(data, restored);
    }

    /// <summary>Длины кодов для оптимального префиксного кода с данными весами (мультимножество совпадает с оптимумом Хаффмана).</summary>
    [Fact]
    public void Slide_Weights_A1_to_A6_CodeLengths_AreOptimalMultiset()
    {
        var freq = new uint[256];
        freq[0] = 25;
        freq[1] = 5;
        freq[2] = 25;
        freq[3] = 10;
        freq[4] = 25;
        freq[5] = 10;

        var tree = new HuffmanTreeBuilder().BuildTree(freq);
        var codes = new CodeTableBuilder().BuildCodes(tree, freq);
        var lengths = codes.Values.Select(c => c.BitCount).OrderBy(x => x).ToArray();
        // Оптимальные длины для этих весов (как на учебном слайде): 2,2,2,3,4,4.
        Assert.Equal(new[] { 2, 2, 2, 3, 4, 4 }, lengths);
    }

    [Fact]
    public void RussianPhrase_Windows1251_FrequenciesMatchSlide()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var enc1251 = Encoding.GetEncoding(1251);
        const string phrase = "на дворе трава, на траве дрова";
        var bytes = enc1251.GetBytes(phrase);

        var freq = new FrequencyAnalyzer().Analyze(bytes);
        Assert.Equal(30, bytes.Length);
        Assert.Equal(6u, freq[enc1251.GetBytes("а")[0]]);
        Assert.Equal(4u, freq[enc1251.GetBytes("в")[0]]);
        Assert.Equal(2u, freq[enc1251.GetBytes("д")[0]]);
        Assert.Equal(1u, freq[enc1251.GetBytes(",")[0]]);
        Assert.Equal(2u, freq[enc1251.GetBytes("е")[0]]);
        Assert.Equal(2u, freq[enc1251.GetBytes("н")[0]]);
        Assert.Equal(4u, freq[enc1251.GetBytes("р")[0]]);
        Assert.Equal(2u, freq[enc1251.GetBytes("о")[0]]);
        Assert.Equal(2u, freq[enc1251.GetBytes("т")[0]]);
        Assert.Equal(5u, freq[(byte)' ']);
    }

    [Fact]
    public async Task CompressFileAsync_RoundTrip()
    {
        var dir = Path.Combine(Path.GetTempPath(), "HuffmanCodecTests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        try
        {
            var input = Path.Combine(dir, "in.bin");
            var arch = Path.Combine(dir, "out.huff");
            var output = Path.Combine(dir, "out.bin");
            await File.WriteAllBytesAsync(input, new byte[] { 1, 2, 3, 1, 2, 3, 1, 2, 2, 2 });
            await Codec.CompressFileAsync(input, arch);
            await Codec.DecompressFileAsync(arch, output);
            var back = await File.ReadAllBytesAsync(output);
            Assert.Equal(await File.ReadAllBytesAsync(input), back);
        }
        finally
        {
            try
            {
                Directory.Delete(dir, true);
            }
            catch
            {
                // ignored
            }
        }
    }
}
