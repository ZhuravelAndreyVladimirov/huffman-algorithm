namespace HuffmanCodec.Core;

/// <summary>
/// Константы бинарного контейнера .huff (little-endian для чисел).
/// </summary>
public static class HuffmanArchiveFormat
{
    public static ReadOnlySpan<byte> Magic => "HUFF"u8;

    public const byte Version = 1;

    /// <summary>4 magic + 1 version + 8 original length + 256×4 frequencies.</summary>
    public const int HeaderSize = 4 + 1 + 8 + 256 * sizeof(uint);

    public const string DefaultExtension = ".huff";
}
