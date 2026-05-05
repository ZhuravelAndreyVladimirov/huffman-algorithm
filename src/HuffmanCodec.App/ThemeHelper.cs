using Microsoft.Win32;

namespace HuffmanCodec.App;

internal static class ThemeHelper
{
    public static bool IsWindowsDarkMode()
    {
        try
        {
            using var k = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                writable: false);
            if (k?.GetValue("AppsUseLightTheme") is int i)
                return i == 0;
        }
        catch
        {
            // ignored
        }

        return false;
    }

    public static void ApplyDarkThemeToForm(Form form)
    {
        var bg = Color.FromArgb(45, 45, 48);
        var fg = Color.FromArgb(220, 220, 220);
        var panelBg = Color.FromArgb(37, 37, 38);

        form.BackColor = bg;
        form.ForeColor = fg;
        ApplyRecursive(form, bg, panelBg, fg);
    }

    private static void ApplyRecursive(Control parent, Color formBg, Color panelBg, Color fg)
    {
        foreach (Control c in parent.Controls)
        {
            switch (c)
            {
                case TabControl tc:
                    tc.BackColor = panelBg;
                    foreach (TabPage tp in tc.TabPages)
                    {
                        tp.BackColor = panelBg;
                        tp.ForeColor = fg;
                        ApplyRecursive(tp, formBg, panelBg, fg);
                    }

                    break;
                case DataGridView dgv:
                    dgv.BackgroundColor = panelBg;
                    dgv.GridColor = Color.FromArgb(63, 63, 70);
                    dgv.EnableHeadersVisualStyles = false;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(55, 55, 60);
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = fg;
                    dgv.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
                    dgv.DefaultCellStyle.ForeColor = fg;
                    dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(75, 90, 120);
                    dgv.DefaultCellStyle.SelectionForeColor = Color.White;
                    dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 43);
                    dgv.AlternatingRowsDefaultCellStyle.ForeColor = fg;
                    dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                        dgv.DefaultCellStyle.SelectionBackColor;
                    dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                        dgv.DefaultCellStyle.SelectionForeColor;
                    break;
                case FrequencyHistogramControl fh:
                    fh.BackColor = Color.FromArgb(30, 30, 32);
                    fh.ForeColor = fg;
                    break;
                case Panel or TableLayoutPanel or FlowLayoutPanel:
                    c.BackColor = panelBg;
                    c.ForeColor = fg;
                    ApplyRecursive(c, formBg, panelBg, fg);
                    break;
                case Label lb:
                    lb.ForeColor = fg;
                    if (lb.Tag is string s && s == "Dim")
                        lb.ForeColor = Color.FromArgb(180, 180, 180);
                    break;
                case Button btn:
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(63, 63, 70);
                    btn.BackColor = Color.FromArgb(63, 63, 70);
                    btn.ForeColor = fg;
                    btn.UseVisualStyleBackColor = false;
                    break;
                case ProgressBar:
                    c.BackColor = panelBg;
                    break;
                default:
                    ApplyRecursive(c, formBg, panelBg, fg);
                    break;
            }
        }
    }
}
