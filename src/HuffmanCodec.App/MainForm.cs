using HuffmanCodec.Core;
using HuffmanCodec.Core.Abstractions;

namespace HuffmanCodec.App;

public partial class MainForm : Form
{
    private readonly IHuffmanCodec _codec;
    private string? _selectedPath;

    public MainForm(IHuffmanCodec codec)
    {
        _codec = codec;
        InitializeComponent();

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

    /// <summary>Прогресс 0–100 с потока фона на UI-поток.</summary>
    private IProgress<int> CreateProgressReporter() =>
        new Progress<int>(v =>
        {
            void apply()
            {
                if (IsDisposed)
                    return;
                _progressBar.Value = Math.Clamp(v, _progressBar.Minimum, _progressBar.Maximum);
            }

            if (InvokeRequired)
                BeginInvoke(apply);
            else
                apply();
        });

    private void BeginBusy()
    {
        _progressBar.Style = ProgressBarStyle.Continuous;
        _progressBar.Minimum = 0;
        _progressBar.Maximum = 100;
        _progressBar.Value = 0;
        _progressBar.Visible = true;
        btnPick.Enabled = btnCompress.Enabled = btnDecompress.Enabled = false;
    }

    private void EndBusy()
    {
        _progressBar.Value = 0;
        _progressBar.Visible = false;
        btnPick.Enabled = btnCompress.Enabled = btnDecompress.Enabled = true;
    }

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
        _selectedPath = dlg.FileName;
        lblPath.Text = _selectedPath;
        lblStatus.Text = "Файл выбран. Нажмите «Закодировать» для сжатия.";
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
            Title = "Сохранить сжатый файл — выберите формат (расширение)",
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
            UseWaitCursor = true;
            var sw = System.Diagnostics.Stopwatch.StartNew();
            await _codec.CompressFileAsync(_selectedPath, dlg.FileName, progress: progress).ConfigureAwait(true);
            sw.Stop();
            var inLen = new FileInfo(_selectedPath).Length;
            var outLen = new FileInfo(dlg.FileName).Length;
            lblStatus.Text = $"Готово за {sw.ElapsedMilliseconds} мс. Исходный: {inLen} Б → архив: {outLen} Б.";
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            Title = "Сохранить распакованный файл — выберите формат",
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
            UseWaitCursor = true;
            await _codec.DecompressFileAsync(dlgOpen.FileName, dlgSave.FileName, progress: progress).ConfigureAwait(true);
            lblStatus.Text = $"Распаковано в: {dlgSave.FileName}";
            _selectedPath = dlgSave.FileName;
            lblPath.Text = _selectedPath;
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblStatus.Text = "Ошибка распаковки.";
        }
        finally
        {
            UseWaitCursor = false;
            EndBusy();
        }
    }
}
