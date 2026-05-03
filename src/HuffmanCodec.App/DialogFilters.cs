namespace HuffmanCodec.App;

/// <summary>Строки фильтров для диалогов: на выходе можно выбрать тип файла (расширение). Содержимое архива Huffman одно и то же для .huff/.hfc/.hfm.</summary>
internal static class DialogFilters
{
    /// <summary>Исходный файл для сжатия — любой тип.</summary>
    public const string InputFileOpen =
        "Все файлы (*.*)|*.*|Текст (*.txt;*.log;*.md)|*.txt;*.log;*.md|Двоичные (*.bin;*.dat)|*.bin;*.dat|Изображения (*.png;*.jpg;*.bmp)|*.png;*.jpg;*.bmp";

    /// <summary>Сохранение сжатого файла — несколько расширений одного формата HUFF.</summary>
    public const string CompressSave =
        "Архив Huffman (*.huff)|*.huff|Архив (*.hfc)|*.hfc|Сжатый (*.hfm)|*.hfm|Все файлы (*.*)|*.*";

    /// <summary>Открытие архива для распаковки.</summary>
    public const string DecompressOpen =
        "Архив Huffman (*.huff;*.hfc;*.hfm)|*.huff;*.hfc;*.hfm|Все файлы (*.*)|*.*";

    /// <summary>Сохранение распакованных данных — тип «на выходе».</summary>
    public const string DecompressSave =
        "Текст (*.txt)|*.txt|Двоичные (*.bin)|*.bin|Данные (*.dat)|*.dat|JSON (*.json)|*.json|XML (*.xml)|*.xml|Любой тип (*.*)|*.*";
}
