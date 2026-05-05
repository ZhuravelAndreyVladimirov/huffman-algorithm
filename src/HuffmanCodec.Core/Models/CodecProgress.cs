namespace HuffmanCodec.Core.Models;

/// <summary>Отчёт о ходе сжатия/распаковки: процент 0–100 и стабильный идентификатор этапа для локализации в UI.</summary>
public readonly record struct CodecProgress(int Percent, string StageId);
