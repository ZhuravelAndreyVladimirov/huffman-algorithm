namespace HuffmanCodec.App;

internal sealed class FrequencyHistogramControl : Control
{
    private uint[] _frequencies = Array.Empty<uint>();

    public FrequencyHistogramControl()
    {
        DoubleBuffered = true;
        SetStyle(ControlStyles.ResizeRedraw, true);
    }

    public void SetFrequencies(uint[] frequencies)
    {
        _frequencies = frequencies ?? Array.Empty<uint>();
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.Clear(BackColor);
        var w = ClientSize.Width;
        var h = ClientSize.Height;
        if (w <= 1 || h <= 20 || _frequencies.Length == 0)
            return;

        var max = 1u;
        foreach (var f in _frequencies)
        {
            if (f > max)
                max = f;
        }

        var logMax = Math.Log10(max);
        var padL = 4;
        var padR = 4;
        var padT = 4;
        var padB = 12;
        var chartW = w - padL - padR;
        var chartH = h - padT - padB;
        if (chartW <= 0 || chartH <= 0)
            return;

        var barW = Math.Max(1f, chartW / 256f);
        using var barBrush = new SolidBrush(Color.FromArgb(90, 120, 180));
        for (var i = 0; i < 256; i++)
        {
            var f = _frequencies[i];
            if (f == 0)
                continue;
            var ratio = logMax > 0 ? Math.Log10(f) / logMax : 0;
            var bh = Math.Max(1, (int)(ratio * chartH));
            var x = padL + i * barW;
            var y = padT + chartH - bh;
            g.FillRectangle(barBrush, x, y, Math.Max(1f, barW - 0.5f), bh);
        }

        using var axisPen = new Pen(ForeColor, 1);
        g.DrawLine(axisPen, padL, padT + chartH, padL + chartW, padT + chartH);
        using var ft = new Font(Font.FontFamily, 7f);
        g.DrawString("0", ft, SystemBrushes.GrayText, padL, padT + chartH + 1);
        g.DrawString("255", ft, SystemBrushes.GrayText, padL + chartW - 18, padT + chartH + 1);
    }
}
