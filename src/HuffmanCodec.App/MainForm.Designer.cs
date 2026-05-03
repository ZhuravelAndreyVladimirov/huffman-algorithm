#nullable enable
namespace HuffmanCodec.App;

partial class MainForm
{
    private System.ComponentModel.IContainer? components;
    private Button btnPick = null!;
    private Button btnCompress = null!;
    private Button btnDecompress = null!;
    private Label lblPath = null!;
    private Label lblStatus = null!;
    private Label lblHint = null!;
    private TableLayoutPanel panelTop = null!;
    private FlowLayoutPanel flowButtons = null!;
    private Panel panelContent = null!;
    private ProgressBar _progressBar = null!;
    private ToolTip toolTip = null!;

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        toolTip = new ToolTip(components);
        btnPick = new Button();
        btnCompress = new Button();
        btnDecompress = new Button();
        lblPath = new Label();
        lblStatus = new Label();
        lblHint = new Label();
        panelTop = new TableLayoutPanel();
        flowButtons = new FlowLayoutPanel();
        panelContent = new Panel();
        _progressBar = new ProgressBar();
        panelContent.SuspendLayout();
        panelTop.SuspendLayout();
        flowButtons.SuspendLayout();
        SuspendLayout();

        btnPick.Text = "1. Выбрать файл";
        btnPick.AutoSize = true;
        btnPick.MinimumSize = new Size(140, 32);
        btnPick.Margin = new Padding(4);
        btnPick.UseVisualStyleBackColor = true;
        btnPick.Click += BtnPick_Click;
        toolTip.SetToolTip(btnPick, "Выбор исходного файла для сжатия (путь отображается выше).");

        btnCompress.Text = "2. Закодировать (сжать)…";
        btnCompress.AutoSize = true;
        btnCompress.MinimumSize = new Size(200, 32);
        btnCompress.Margin = new Padding(4);
        btnCompress.UseVisualStyleBackColor = true;
        btnCompress.Click += BtnCompress_Click;
        toolTip.SetToolTip(btnCompress, "Сжать выбранный файл в архив .huff");

        btnDecompress.Text = "3. Декодировать (распаковать)…";
        btnDecompress.AutoSize = true;
        btnDecompress.MinimumSize = new Size(260, 32);
        btnDecompress.Margin = new Padding(4);
        btnDecompress.UseVisualStyleBackColor = true;
        btnDecompress.Click += BtnDecompress_Click;
        toolTip.SetToolTip(btnDecompress, "Обратное декодирование: выберите архив .huff и куда сохранить восстановленный файл");

        lblHint.AutoSize = true;
        lblHint.Margin = new Padding(8, 4, 8, 4);
        lblHint.ForeColor = Color.DimGray;
        lblHint.Text =
            "Сжатие: шаг 1 → шаг 2.   Распаковка: шаг 3 (отдельный диалог выбора .huff — исходный файл для шага 2 не обязателен).";

        lblPath.AutoEllipsis = true;
        lblPath.Dock = DockStyle.Fill;
        lblPath.TextAlign = ContentAlignment.MiddleLeft;
        lblPath.Margin = new Padding(8, 2, 8, 2);
        lblPath.Text = "Файл не выбран";

        lblStatus.AutoEllipsis = true;
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.TextAlign = ContentAlignment.MiddleLeft;
        lblStatus.Margin = new Padding(8, 2, 8, 4);
        lblStatus.Text = "";

        flowButtons.AutoSize = true;
        flowButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        flowButtons.Dock = DockStyle.Fill;
        flowButtons.FlowDirection = FlowDirection.LeftToRight;
        flowButtons.WrapContents = true;
        flowButtons.Padding = new Padding(4);
        flowButtons.Controls.Add(btnPick);
        flowButtons.Controls.Add(btnCompress);
        flowButtons.Controls.Add(btnDecompress);

        panelTop.ColumnCount = 1;
        panelTop.RowCount = 4;
        panelTop.Dock = DockStyle.Top;
        panelTop.AutoSize = true;
        panelTop.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        panelTop.Padding = new Padding(4);
        panelTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        panelTop.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panelTop.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panelTop.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panelTop.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panelTop.Controls.Add(flowButtons, 0, 0);
        panelTop.Controls.Add(lblHint, 0, 1);
        panelTop.Controls.Add(lblPath, 0, 2);
        panelTop.Controls.Add(lblStatus, 0, 3);

        panelContent.Dock = DockStyle.Fill;
        panelContent.Padding = new Padding(4, 0, 4, 4);
        panelContent.BackColor = SystemColors.Control;

        _progressBar.Dock = DockStyle.Bottom;
        _progressBar.Height = 26;
        _progressBar.Visible = false;
        _progressBar.Style = ProgressBarStyle.Continuous;
        _progressBar.Minimum = 0;
        _progressBar.Maximum = 100;
        _progressBar.Value = 0;

        panelContent.Controls.Add(_progressBar);

        MinimumSize = new Size(800, 400);
        ClientSize = new Size(900, 520);
        Controls.Add(panelTop);
        Controls.Add(panelContent);

        Text = "Кодирование Хаффмана — HuffmanCodec";

        panelContent.ResumeLayout(false);
        panelTop.ResumeLayout(false);
        panelTop.PerformLayout();
        flowButtons.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }
}
