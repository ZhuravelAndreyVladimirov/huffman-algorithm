using HuffmanCodec.Core.Models;

namespace HuffmanCodec.App;

internal static class ProgressStageLabels
{
    private static readonly IReadOnlyDictionary<string, string> Ru = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["prepare"] = "Подготовка",
        ["open_input"] = "Открытие входного файла",
        ["open_output"] = "Создание выходного файла",
        ["read_input"] = "Чтение данных",
        ["frequencies"] = "Подсчёт частот",
        ["build_tree"] = "Построение дерева Хаффмана",
        ["code_table"] = "Таблица кодов",
        ["encode"] = "Кодирование",
        ["write_header"] = "Запись заголовка",
        ["write_payload"] = "Запись сжатых данных",
        ["parse_header"] = "Разбор заголовка",
        ["read_frequencies"] = "Чтение частот",
        ["decode"] = "Декодирование",
        ["write_output"] = "Запись восстановленного файла",
        ["done"] = "Готово"
    };

    public static string ToRussian(CodecProgress p) =>
        Ru.TryGetValue(p.StageId, out var s) ? s : p.StageId;

    public static string FormatLine(CodecProgress p) => $"{ToRussian(p)} — {p.Percent} %";
}
