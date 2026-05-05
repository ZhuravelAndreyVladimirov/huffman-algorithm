using System.Diagnostics;
using HuffmanCodec.Core;
using HuffmanCodec.Core.Abstractions;
using HuffmanCodec.Core.Models;

namespace HuffmanCodec.App;

public partial class MainForm : Form
{
    private const long MaxAnalysisFileBytes = 50L * 1024 * 1024;

    private readonly IHuffmanCodec _codec;
    private string? _selectedPath;
    private CancellationTokenSource? _operationCts;
    private string? _lastOutputPath;
    private string _lastSummaryText = "";

    public MainForm(IHuffmanCodec codec)
    {
        _codec = codec;
        InitializeComponent();
        TryApplySystemTheme();
        ApplyButtonIcons();

        try
        {
            var path = Environment.ProcessPath;
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
                Text += $"  [сборка {File.GetLastWriteTime(path):dd.MM.yyyy HH:mm:ss}]";
        }
        catch
        {
            // ignored
        }
    }

    private void TryApplySystemTheme()
    {
        if (!ThemeHelper.IsWindowsDarkMode())
            return;
        ThemeHelper.ApplyDarkThemeToForm(this);
    }

    private void ApplyButtonIcons()
    {
        try
        {
            SetButtonIcon(btnPick, SystemIcons.Application);
            SetButtonIcon(btnCompress, SystemIcons.WinLogo);
            SetButtonIcon(btnDecompress, SystemIcons.Information);
        }
        catch
        {
            // ignored
        }
    }

    private static void SetButtonIcon(Button button, Icon systemIcon)
    {
        using var sized = new Icon(systemIcon, 16, 16);
        button.Image = sized.ToBitmap();
        button.ImageAlign = ContentAlignment.MiddleLeft;
        button.TextImageRelation = TextImageRelation.ImageBeforeText;
    }

    private IProgress<CodecProgress> CreateProgressReporter() =>
        new Progress<CodecProgress>(p =>
        {
            void apply()
            {
                if (IsDisposed)
                    return;
                _progressBar.Value = Math.Clamp(p.Percent, _progressBar.Minimum, _progressBar.Maximum);
                lblProgressDetail.Text = ProgressStageLabels.FormatLine(p);
            }

            if (InvokeRequired)
                BeginInvoke(apply);
            else
                apply();
        });

    private void BeginBusy()
    {
        _operationCts?.Dispose();
        _operationCts = new CancellationTokenSource();
        _progressBar.Style = ProgressBarStyle.Blocks;
        _progressBar.Minimum = 0;
        _progressBar.Maximum = 100;
        _progressBar.Value = 0;
        _progressBar.Visible = true;
        lblProgressDetail.Text = "";
        btnCancel.Visible = true;
        btnPick.Enabled = btnCompress.Enabled = btnDecompress.Enabled = false;
        btnRefreshAnalysis.Enabled = false;
    }

    private void EndBusy()
    {
        btnCancel.Visible = false;
        _operationCts?.Dispose();
        _operationCts = null;
        _progressBar.Value = 0;
        _progressBar.Visible = false;
        lblProgressDetail.Text = "";
        btnPick.Enabled = btnCompress.Enabled = btnDecompress.Enabled = true;
        btnRefreshAnalysis.Enabled = true;
    }

    private void BtnCancel_Click(object? sender, EventArgs e) => _operationCts?.Cancel();

    private void BtnPick_Click(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Выберите файл для сжатия",
            Filter = DialogFilters.InputFileOpen,
            FilterIndex = 1
        };
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;
        SetSelectedPath(dlg.FileName, "Файл выбран. Нажмите «Сжать» для архивации.");
    }

    private void SetSelectedPath(string path, string statusMessage)
    {
        _selectedPath = path;
        lblPath.Text = _selectedPath;
        lblStatus.Text = statusMessage;
    }

    private async void BtnCompress_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_selectedPath))
        {
            MessageBox.Show(this, "Сначала выберите файл.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dlg = new SaveFileDialog
        {
            Title = "Сохранить сжатый файл — выберите расширение",
            Filter = DialogFilters.CompressSave,
            FilterIndex = 1,
            DefaultExt = "huff",
            AddExtension = true,
            FileName = Path.GetFileNameWithoutExtension(_selectedPath) + HuffmanArchiveFormat.DefaultExtension
        };
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        var progress = CreateProgressReporter();
        try
        {
            lblStatus.Text = "Сжатие…";
            BeginBusy();
            var token = _operationCts!.Token;
            UseWaitCursor = true;
            var sw = Stopwatch.StartNew();
            await _codec.CompressFileAsync(_selectedPath, dlg.FileName, token, progress).ConfigureAwait(true);
            sw.Stop();
            var inLen = new FileInfo(_selectedPath).Length;
            var outLen = new FileInfo(dlg.FileName).Length;
            var ratio = ByteSizeFormatter.ArchivePercentOfOriginal(inLen, outLen);
            lblStatus.Text =
                $"Готово за {sw.ElapsedMilliseconds} мс. {ByteSizeFormatter.Format(inLen)} → {ByteSizeFormatter.Format(outLen)} (архив {ratio:0.##} % от исходника).";
            _lastOutputPath = dlg.FileName;
            _lastSummaryText =
                $"Операция: сжатие{Environment.NewLine}" +
                $"Вход: {_selectedPath}{Environment.NewLine}" +
                $"Выход: {dlg.FileName}{Environment.NewLine}" +
                $"Время: {sw.ElapsedMilliseconds} мс{Environment.NewLine}" +
                $"Исходный размер: {ByteSizeFormatter.Format(inLen)} ({inLen} Б){Environment.NewLine}" +
                $"Размер архива: {ByteSizeFormatter.Format(outLen)} ({outLen} Б){Environment.NewLine}" +
                $"Архив от исходника: {ratio:0.##} %";
            btnCopySummary.Enabled = true;
            btnOpenExplorer.Enabled = true;
        }
        catch (OperationCanceledException)
        {
            lblStatus.Text = "Сжатие отменено.";
        }
        catch (Exception ex)
        {
            ShowError(this, ex);
            lblStatus.Text = "Ошибка сжатия.";
        }
        finally
        {
            UseWaitCursor = false;
            EndBusy();
        }
    }

    private async void BtnDecompress_Click(object? sender, EventArgs e)
    {
        using var dlgOpen = new OpenFileDialog
        {
            Title = "Выберите архив Huffman",
            Filter = DialogFilters.DecompressOpen,
            FilterIndex = 1
        };
        if (dlgOpen.ShowDialog(this) != DialogResult.OK)
            return;

        var baseName = Path.GetFileNameWithoutExtension(dlgOpen.FileName);
        using var dlgSave = new SaveFileDialog
        {
            Title = "Сохранить распакованный файл — выберите тип",
            Filter = DialogFilters.DecompressSave,
            FilterIndex = 1,
            DefaultExt = "txt",
            AddExtension = true,
            FileName = string.IsNullOrEmpty(baseName) ? "output" : baseName
        };
        if (dlgSave.ShowDialog(this) != DialogResult.OK)
            return;

        var progress = CreateProgressReporter();
        try
        {
            lblStatus.Text = "Распаковка…";
            BeginBusy();
            var token = _operationCts!.Token;
            UseWaitCursor = true;
            var sw = Stopwatch.StartNew();
            await _codec.DecompressFileAsync(dlgOpen.FileName, dlgSave.FileName, token, progress).ConfigureAwait(true);
            sw.Stop();
            var archiveLen = new FileInfo(dlgOpen.FileName).Length;
            var outLen = new FileInfo(dlgSave.FileName).Length;
            lblStatus.Text =
                $"Готово за {sw.ElapsedMilliseconds} мс. Распаковано в {ByteSizeFormatter.Format(outLen)} (архив был {ByteSizeFormatter.Format(archiveLen)}).";
            SetSelectedPath(dlgSave.FileName, lblStatus.Text);
            _lastOutputPath = dlgSave.FileName;
            _lastSummaryText =
                $"Операция: распаковка{Environment.NewLine}" +
                $"Архив: {dlgOpen.FileName}{Environment.NewLine}" +
                $"Выход: {dlgSave.FileName}{Environment.NewLine}" +
                $"Время: {sw.ElapsedMilliseconds} мс{Environment.NewLine}" +
                $"Размер архива: {ByteSizeFormatter.Format(archiveLen)} ({archiveLen} Б){Environment.NewLine}" +
                $"Восстановлено: {ByteSizeFormatter.Format(outLen)} ({outLen} Б)";
            btnCopySummary.Enabled = true;
            btnOpenExplorer.Enabled = true;
        }
        catch (OperationCanceledException)
        {
            lblStatus.Text = "Распаковка отменена.";
        }
        catch (Exception ex)
        {
            ShowError(this, ex);
            lblStatus.Text = "Ошибка распаковки.";
        }
        finally
        {
            UseWaitCursor = false;
            EndBusy();
        }
    }

    private void BtnCopySummary_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_lastSummaryText))
            return;
        try
        {
            Clipboard.SetText(_lastSummaryText);
            lblStatus.Text = "Сводка скопирована в буфер обмена.";
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Буфер обмена", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnOpenExplorer_Click(object? sender, EventArgs e) =>
        ExplorerHelper.TrySelectInExplorer(_lastOutputPath ?? "");

    private async void BtnRefreshAnalysis_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_selectedPath))
        {
            MessageBox.Show(this, "Сначала выберите файл на вкладке «Работа».", Text, MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        var len = new FileInfo(_selectedPath).Length;
        if (len > MaxAnalysisFileBytes)
        {
            MessageBox.Show(this,
                $"Файл больше {ByteSizeFormatter.Format(MaxAnalysisFileBytes)} — анализ отключён, чтобы не загружать всё в память.",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        lblAnalysisStatus.Text = "Загрузка…";
        UseWaitCursor = true;
        btnRefreshAnalysis.Enabled = false;
        try
        {
            var preview = await _codec.PreviewAsync(_selectedPath).ConfigureAwait(true);
            histogramControl.SetFrequencies(preview.Frequencies);
            CodecAnalysisGridBinder.Bind(analysisGrid, preview.Frequencies, preview.Codes);
            lblAnalysisStatus.Text =
                $"{Path.GetFileName(_selectedPath)} — {ByteSizeFormatter.Format(len)}, уникальных байт с ненулевой частотой: {CountNonZero(preview.Frequencies)}";
        }
        catch (Exception ex)
        {
            ShowError(this, ex);
            lblAnalysisStatus.Text = "Ошибка анализа.";
            histogramControl.SetFrequencies(new uint[256]);
            analysisGrid.Rows.Clear();
        }
        finally
        {
            UseWaitCursor = false;
            btnRefreshAnalysis.Enabled = true;
        }
    }

    private static int CountNonZero(uint[] frequencies)
    {
        var n = 0;
        foreach (var f in frequencies)
        {
            if (f != 0)
                n++;
        }

        return n;
    }

    private void MainForm_DragEnter(object? sender, DragEventArgs e)
    {
        e.Effect = e.Data?.GetDataPresent(DataFormats.FileDrop) == true
            ? DragDropEffects.Copy
            : DragDropEffects.None;
    }

    private void MainForm_DragDrop(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetData(DataFormats.FileDrop) is not string[] paths || paths.Length == 0)
            return;
        var p = paths[0];
        if (!File.Exists(p))
            return;
        var ext = Path.GetExtension(p).ToLowerInvariant();
        var hint = ext is ".huff" or ".hfc" or ".hfm"
            ? "Файл выбран. Для архива Huffman используйте «Распаковать»."
            : "Файл выбран. Нажмите «Сжать» для архивации.";
        SetSelectedPath(p, hint);
    }

    private static void ShowError(IWin32Window owner, Exception ex)
    {
        var r = MessageBox.Show(owner,
            ex.Message + Environment.NewLine + Environment.NewLine +
            "Скопировать подробности в буфер обмена?",
            "Ошибка",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Error);
        if (r == DialogResult.Yes)
        {
            try
            {
                Clipboard.SetText(ex.ToString());
            }
            catch
            {
                // ignored
            }
        }
    }
}
