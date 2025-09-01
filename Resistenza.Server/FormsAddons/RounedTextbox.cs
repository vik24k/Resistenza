using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Resistenza.Server.FormsAddons
{

    public class RoundedTextBox : TextBox
    {
        private const int WM_PAINT = 0xF;

        public RoundedTextBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            ResizeRedraw = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                int radius = 10; // Imposta il raggio per gli angoli arrotondati

                path.AddArc(0, 0, radius * 2, radius * 2, 180, 90); // Angolo in alto a sinistra
                path.AddArc(Width - (radius * 2), 0, radius * 2, radius * 2, 270, 90); // Angolo in alto a destra
                path.AddArc(Width - (radius * 2), Height - (radius * 2), radius * 2, radius * 2, 0, 90); // Angolo in basso a destra
                path.AddArc(0, Height - (radius * 2), radius * 2, radius * 2, 90, 90); // Angolo in basso a sinistra
                path.CloseFigure();

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Riempimento del colore dello sfondo all'interno dei bordi
                using (SolidBrush backgroundBrush = new SolidBrush(BackColor))
                {
                    e.Graphics.FillPath(backgroundBrush, path);
                }

                // Disegna il bordo
                using (Pen borderPen = new Pen(BorderColor, BorderThickness))
                {
                    e.Graphics.DrawPath(borderPen, path);
                }
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

        private Color borderColor = Color.Gray;
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                Invalidate();
            }
        }

        private int borderThickness = 1;
        public int BorderThickness
        {
            get { return borderThickness; }
            set
            {
                borderThickness = value;
                Invalidate();
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_PAINT)
            {
                using (Graphics g = Graphics.FromHwnd(Handle))
                {
                    using (Pen borderPen = new Pen(BorderColor, BorderThickness))
                    {
                        g.DrawRectangle(borderPen, 0, 0, Width - 1, Height - 1);
                    }
                }
            }
        }
    }
}




