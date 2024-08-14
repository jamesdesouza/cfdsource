using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCX.CFD.Controls;

public class CircularProgressBar : Control
{
	private const int BarMargin = 5;

	private const float lineWidth = 3f;

	private const float barWidth = 3f;

	private bool keepAnimating;

	private int startAngle;

	private int sweepAngle;

	public CircularProgressBar()
	{
		SetStyle(ControlStyles.SupportsTransparentBackColor, value: true);
		SetStyle(ControlStyles.Opaque, value: true);
		base.Size = new Size(30, 30);
		DoubleBuffered = true;
	}

	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);
		SetStandardSize();
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		base.OnSizeChanged(e);
		SetStandardSize();
	}

	protected override void OnPaintBackground(PaintEventArgs p)
	{
		base.OnPaintBackground(p);
	}

	private void SetStandardSize()
	{
		int num = Math.Max(base.Width, base.Height);
		base.Size = new Size(num, num);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		using Bitmap image = new Bitmap(base.Width, base.Height);
		using Graphics graphics = Graphics.FromImage(image);
		graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
		graphics.CompositingQuality = CompositingQuality.HighQuality;
		graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
		graphics.SmoothingMode = SmoothingMode.AntiAlias;
		PaintTransparentBackground(this, e);
		using (Brush brush = new SolidBrush(BackColor))
		{
			graphics.FillEllipse(brush, 5, 5, base.Width - 10, base.Height - 10);
		}
		using (Pen pen = new Pen(Color.DimGray, 3f))
		{
			graphics.DrawEllipse(pen, 5, 5, base.Width - 10, base.Height - 10);
		}
		using (SolidBrush brush2 = new SolidBrush(Color.FromArgb(5, 151, 212)))
		{
			using Pen pen2 = new Pen(brush2, 3f);
			pen2.StartCap = LineCap.Round;
			pen2.EndCap = LineCap.Round;
			graphics.DrawArc(pen2, 5, 5, base.Width - 10, base.Height - 10, startAngle, sweepAngle);
		}
		e.Graphics.DrawImage(image, 0, 0);
	}

	private static void PaintTransparentBackground(Control c, PaintEventArgs e)
	{
		if (c.Parent != null && Application.RenderWithVisualStyles)
		{
			ButtonRenderer.DrawParentBackground(e.Graphics, c.ClientRectangle, c);
		}
	}

	private async Task AnimationLoop()
	{
		while (keepAnimating)
		{
			startAngle += 6;
			sweepAngle += 10;
			while (startAngle > 360)
			{
				startAngle -= 360;
			}
			while (sweepAngle > 360)
			{
				sweepAngle -= 360;
			}
			Invalidate();
			await Task.Delay(40);
		}
	}

	public void StartAnimation()
	{
		keepAnimating = true;
		Task.Run(() => AnimationLoop());
	}

	public void StopAnimation()
	{
		keepAnimating = false;
	}
}
