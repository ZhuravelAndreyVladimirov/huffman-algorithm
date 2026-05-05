#nullable enable
namespace HuffmanCodec.App;

partial class MainForm
{
    private System.ComponentModel.IContainer? components;
    private TabControl mainTabs = null!;
    private TabPage tabWork = null!;
    private TabPage tabAnalysis = null!;
    private Panel panelWorkRoot = null!;
    private TableLayoutPanel panelTop = null!;
    private FlowLayoutPanel flowButtons = null!;
    private FlowLayoutPanel flowPostActions = null!;
    private TableLayoutPanel tlpProgress = null!;
    private Panel panelProgressHost = null!;
    private Label lblPath = null!;
    private Label lblStatus = null!;
    private Label lblHint = null!;
    private Label lblProgressDetail = null!;
    private Button btnPick = null!;
    private Button btnCompress = null!;
    private Button btnDecompress = null!;
    private Button btnCopySummary = null!;
    private Button btnOpenExplorer = null!;
    private Button btnCancel = null!;
    private ProgressBar _progressBar = null!;
    private ToolTip toolTip = null!;
    private TableLayoutPanel panelAnalysisRoot = null!;
    private Label lblAnalysisHint = null!;
    private FlowLayoutPanel flowAnalysisBar = null!;
    private Button btnRefreshAnalysis = null!;
    private Label lblAnalysisStatus = null!;
    private FrequencyHistogramControl histogramControl = null!;
    private DataGridView analysisGrid = null!;

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        toolTip = new ToolTip(components);
        mainTabs = new TabControl();
        tabWork = new TabPage();
        tabAnalysis = new TabPage();
        panelWorkRoot = new Panel();
        panelTop = new TableLayoutPanel();
        flowButtons = new FlowLayoutPanel();
        flowPostActions = new FlowLayoutPanel();
        panelProgressHost = new Panel();
        tlpProgress = new TableLayoutPanel();
        lblProgressDetail = new Label();
        btnCancel = new Button();
        _progressBar = new ProgressBar();
        btnPick = new Button();
        btnCompress = new Button();
        btnDecompress = new Button();
        lblPath = new Label();
        lblStatus = new Label();
        lblHint = new Label();
        btnCopySummary = new Button();
        btnOpenExplorer = new Button();
        panelAnalysisRoot = new TableLayoutPanel();
        lblAnalysisHint = new Label();
        flowAnalysisBar = new FlowLayoutPanel();
        btnRefreshAnalysis = new Button();
        lblAnalysisStatus = new Label();
        histogramControl = new FrequencyHistogramControl();
        analysisGrid = new DataGridView();
        panelWorkRoot.SuspendLayout();
        panelTop.SuspendLayout();
        flowButtons.SuspendLayout();
        flowPostActions.SuspendLayout();
        panelProgressHost.SuspendLayout();
        tlpProgress.SuspendLayout();
        mainTabs.SuspendLayout();
        tabWork.SuspendLayout();
        tabAnalysis.SuspendLayout();
        panelAnalysisRoot.SuspendLayout();
        flowAnalysisBar.SuspendLayout();
        analysisGrid.SuspendLayout();
        SuspendLayout();

        btnPick.Text = "1. Выбрать файл";
        btnPick.AutoSize = true;
        btnPick.MinimumSize = new Size(140, 32);
        btnPick.Margin = new Padding(4);
        btnPick.UseVisualStyleBackColor = true;
        btnPick.Click += BtnPick_Click;
        toolTip.SetToolTip(btnPick, "Выбор исходного файла для сжатия.");

        btnCompress.Text = "2. Сжать…";
        btnCompress.AutoSize = true;
        btnCompress.MinimumSize = new Size(160, 32);
        btnCompress.Margin = new Padding(4);
        btnCompress.UseVisualStyleBackColor = true;
        btnCompress.Click += BtnCompress_Click;
        toolTip.SetToolTip(btnCompress, "Сжать выбранный файл в архив Huffman (.huff / .hfc / .hfm).");

        btnDecompress.Text = "3. Распаковать…";
        btnDecompress.AutoSize = true;
        btnDecompress.MinimumSize = new Size(180, 32);
        btnDecompress.Margin = new Padding(4);
        btnDecompress.UseVisualStyleBackColor = true;
        btnDecompress.Click += BtnDecompress_Click;
        toolTip.SetToolTip(btnDecompress, "Выберите архив и путь для восстановленного файла.");

        lblHint.AutoSize = true;
        lblHint.Margin = new Padding(8, 4, 8, 4);
        lblHint.ForeColor = Color.DimGray;
        lblHint.Tag = "Dim";
        lblHint.Text =
            "Сжатие: шаг 1 → шаг 2. Распаковка: шаг 3. Файл можно перетащить на окно. Архивы .huff / .hfc / .hfm можно распаковать шагом 3.";

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

        btnCopySummary.Text = "Копировать сводку";
        btnCopySummary.AutoSize = true;
        btnCopySummary.Margin = new Padding(4);
        btnCopySummary.Enabled = false;
        btnCopySummary.UseVisualStyleBackColor = true;
        btnCopySummary.Click += BtnCopySummary_Click;
        toolTip.SetToolTip(btnCopySummary, "Копирует в буфер обмена данные последней успешной операции.");

        btnOpenExplorer.Text = "Показать в проводнике";
        btnOpenExplorer.AutoSize = true;
        btnOpenExplorer.Margin = new Padding(4);
        btnOpenExplorer.Enabled = false;
        btnOpenExplorer.UseVisualStyleBackColor = true;
        btnOpenExplorer.Click += BtnOpenExplorer_Click;
        toolTip.SetToolTip(btnOpenExplorer, "Открывает папку и выделяет последний выходной файл.");

        flowPostActions.AutoSize = true;
        flowPostActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        flowPostActions.Dock = DockStyle.Top;
        flowPostActions.FlowDirection = FlowDirection.LeftToRight;
        flowPostActions.WrapContents = false;
        flowPostActions.Padding = new Padding(8, 2, 8, 2);
        flowPostActions.Controls.Add(btnCopySummary);
        flowPostActions.Controls.Add(btnOpenExplorer);

        lblProgressDetail.AutoEllipsis = true;
        lblProgressDetail.Dock = DockStyle.Fill;
        lblProgressDetail.TextAlign = ContentAlignment.MiddleLeft;
        lblProgressDetail.Margin = new Padding(4, 6, 4, 2);
        lblProgressDetail.Text = "";
        lblProgressDetail.MinimumSize = new Size(100, 22);

        btnCancel.Text = "Отмена";
        btnCancel.AutoSize = true;
        btnCancel.Margin = new Padding(4, 4, 8, 4);
        btnCancel.Visible = false;
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += BtnCancel_Click;

        tlpProgress.ColumnCount = 2;
        tlpProgress.RowCount = 2;
        tlpProgress.Dock = DockStyle.Fill;
        tlpProgress.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpProgress.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tlpProgress.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpProgress.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        tlpProgress.Controls.Add(lblProgressDetail, 0, 0);
        tlpProgress.SetColumnSpan(lblProgressDetail, 1);
        tlpProgress.Controls.Add(btnCancel, 1, 0);
        tlpProgress.Controls.Add(_progressBar, 0, 1);
        tlpProgress.SetColumnSpan(_progressBar, 2);

        _progressBar.Dock = DockStyle.Fill;
        _progressBar.Margin = new Padding(4, 2, 4, 6);
        _progressBar.Visible = false;
        _progressBar.Style = ProgressBarStyle.Blocks;
        _progressBar.Minimum = 0;
        _progressBar.Maximum = 100;
        _progressBar.Value = 0;

        panelProgressHost.Controls.Add(tlpProgress);
        panelProgressHost.Dock = DockStyle.Bottom;
        panelProgressHost.Height = 72;

        panelWorkRoot.Controls.Add(panelProgressHost);
        panelWorkRoot.Controls.Add(flowPostActions);
        panelWorkRoot.Controls.Add(panelTop);
        panelWorkRoot.Dock = DockStyle.Fill;
        panelWorkRoot.Padding = new Padding(0, 0, 0, 4);

        tabWork.Text = "Работа";
        tabWork.Padding = new Padding(8);
        tabWork.Controls.Add(panelWorkRoot);

        lblAnalysisHint.AutoSize = true;
        lblAnalysisHint.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblAnalysisHint.Margin = new Padding(4);
        lblAnalysisHint.MaximumSize = new Size(2000, 0);
        lblAnalysisHint.Text =
            "Гистограмма частот байтов и таблица кодов Хаффмана для файла, выбранного на вкладке «Работа» (без записи архива). Файлы > 50 МиБ не анализируются.";

        btnRefreshAnalysis.Text = "Обновить анализ";
        btnRefreshAnalysis.AutoSize = true;
        btnRefreshAnalysis.Margin = new Padding(4);
        btnRefreshAnalysis.UseVisualStyleBackColor = true;
        btnRefreshAnalysis.Click += BtnRefreshAnalysis_Click;

        lblAnalysisStatus.AutoEllipsis = true;
        lblAnalysisStatus.AutoSize = true;
        lblAnalysisStatus.Margin = new Padding(4);
        lblAnalysisStatus.Text = "";

        flowAnalysisBar.AutoSize = true;
        flowAnalysisBar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        flowAnalysisBar.Dock = DockStyle.Fill;
        flowAnalysisBar.FlowDirection = FlowDirection.LeftToRight;
        flowAnalysisBar.WrapContents = false;
        flowAnalysisBar.Controls.Add(btnRefreshAnalysis);
        flowAnalysisBar.Controls.Add(lblAnalysisStatus);

        histogramControl.Dock = DockStyle.Fill;
        histogramControl.Margin = new Padding(0);
        histogramControl.Font = new Font("Segoe UI", 9F);
        toolTip.SetToolTip(histogramControl, "Распределение частот байтов 0…255 (логарифмическая шкала по высоте).");

        analysisGrid.Dock = DockStyle.Fill;
        analysisGrid.Margin = new Padding(0);
        analysisGrid.Font = new Font("Segoe UI", 9F);
        analysisGrid.MinimumSize = new Size(200, 200);
        analysisGrid.ReadOnly = true;
        analysisGrid.AllowUserToAddRows = false;
        analysisGrid.AllowUserToDeleteRows = false;
        analysisGrid.AllowUserToResizeRows = false;
        analysisGrid.RowHeadersVisible = false;
        analysisGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        analysisGrid.MultiSelect = false;
        analysisGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        analysisGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        analysisGrid.EnableHeadersVisualStyles = true;
        analysisGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 252);
        analysisGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colByte",
            HeaderText = "Байт",
            FillWeight = 10f,
            MinimumWidth = 52,
            SortMode = DataGridViewColumnSortMode.Automatic
        });
        analysisGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colHex",
            HeaderText = "Hex",
            FillWeight = 10f,
            MinimumWidth = 52,
            SortMode = DataGridViewColumnSortMode.Automatic
        });
        analysisGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colSym",
            HeaderText = "Символ",
            FillWeight = 12f,
            MinimumWidth = 56,
            SortMode = DataGridViewColumnSortMode.Automatic
        });
        analysisGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colFreq",
            HeaderText = "Частота",
            FillWeight = 14f,
            MinimumWidth = 72,
            SortMode = DataGridViewColumnSortMode.Automatic
        });
        analysisGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colCode",
            HeaderText = "Код Хаффмана",
            FillWeight = 42f,
            MinimumWidth = 120,
            SortMode = DataGridViewColumnSortMode.Automatic
        });
        analysisGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colBits",
            HeaderText = "Бит",
            FillWeight = 12f,
            MinimumWidth = 44,
            SortMode = DataGridViewColumnSortMode.Automatic
        });
        toolTip.SetToolTip(analysisGrid,
            "Только байты с ненулевой частотой: десятичное значение, hex, символ (если печатаемый ASCII), частота и префиксный код.");

        panelAnalysisRoot.ColumnCount = 1;
        panelAnalysisRoot.RowCount = 4;
        panelAnalysisRoot.Dock = DockStyle.Fill;
        panelAnalysisRoot.Padding = new Padding(4);
        panelAnalysisRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        panelAnalysisRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panelAnalysisRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panelAnalysisRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));
        panelAnalysisRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        panelAnalysisRoot.Controls.Add(lblAnalysisHint, 0, 0);
        panelAnalysisRoot.Controls.Add(flowAnalysisBar, 0, 1);
        panelAnalysisRoot.Controls.Add(histogramControl, 0, 2);
        panelAnalysisRoot.Controls.Add(analysisGrid, 0, 3);

        tabAnalysis.Text = "Анализ";
        tabAnalysis.Controls.Add(panelAnalysisRoot);

        mainTabs.Dock = DockStyle.Fill;
        mainTabs.Padding = new Point(8, 4);
        mainTabs.Controls.Add(tabWork);
        mainTabs.Controls.Add(tabAnalysis);

        MinimumSize = new Size(800, 480);
        ClientSize = new Size(920, 620);
        Controls.Add(mainTabs);
        Font = new Font("Segoe UI", 9F);
        Text = "Сжатие Хаффмана — HuffmanCodec";
        AllowDrop = true;
        DragEnter += MainForm_DragEnter;
        DragDrop += MainForm_DragDrop;

        analysisGrid.ResumeLayout(false);
        flowAnalysisBar.ResumeLayout(false);
        flowAnalysisBar.PerformLayout();
        panelAnalysisRoot.ResumeLayout(false);
        panelAnalysisRoot.PerformLayout();
        tabAnalysis.ResumeLayout(false);
        tabWork.ResumeLayout(false);
        mainTabs.ResumeLayout(false);
        tlpProgress.ResumeLayout(false);
        tlpProgress.PerformLayout();
        panelProgressHost.ResumeLayout(false);
        flowPostActions.ResumeLayout(false);
        flowPostActions.PerformLayout();
        panelTop.ResumeLayout(false);
        panelTop.PerformLayout();
        flowButtons.ResumeLayout(false);
        panelWorkRoot.ResumeLayout(false);
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
