using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace parkingManagementSystem
{
    public class DonutChart : Control
    {
        private int available = 100;
        private int occupied = 0;
        private Color availableColor = Color.FromArgb(34, 197, 94);
        private Color occupiedColor = Color.FromArgb(239, 68, 68);
        private Color textColor = Color.FromArgb(40, 42, 58);

        [DefaultValue(100)]
        [Category("Data")]
        public int Available
        {
            get { return available; }
            set { available = value; Invalidate(); }
        }

        [DefaultValue(0)]
        [Category("Data")]
        public int Occupied
        {
            get { return occupied; }
            set { occupied = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color AvailableColor
        {
            get { return availableColor; }
            set { availableColor = value; Invalidate(); }
        }
        private bool ShouldSerializeAvailableColor() => availableColor != Color.FromArgb(34, 197, 94);
        private void ResetAvailableColor() => availableColor = Color.FromArgb(34, 197, 94);

        [Category("Appearance")]
        public Color OccupiedColor
        {
            get { return occupiedColor; }
            set { occupiedColor = value; Invalidate(); }
        }
        private bool ShouldSerializeOccupiedColor() => occupiedColor != Color.FromArgb(239, 68, 68);
        private void ResetOccupiedColor() => occupiedColor = Color.FromArgb(239, 68, 68);

        [Category("Appearance")]
        public Color TextColor
        {
            get { return textColor; }
            set { textColor = value; Invalidate(); }
        }
        private bool ShouldSerializeTextColor() => textColor != Color.FromArgb(40, 42, 58);
        private void ResetTextColor() => textColor = Color.FromArgb(40, 42, 58);

        public DonutChart()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
            BackColor = Color.White;
        }

        public void SetValues(int avail, int occ)
        {
            available = avail;
            occupied = occ;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int size = Math.Min(Width, Height) - 20;
            if (size <= 0) return;

            int x = (Width - size) / 2;
            int y = (Height - size) / 2;
            Rectangle rect = new Rectangle(x, y, size, size);

            int total = available + occupied;
            if (total == 0)
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(230, 230, 235)))
                    e.Graphics.FillEllipse(brush, rect);
            }
            else
            {
                float availableAngle = (float)available / total * 360f;
                float occupiedAngle = (float)occupied / total * 360f;

                if (available > 0)
                {
                    using (SolidBrush brush = new SolidBrush(availableColor))
                        e.Graphics.FillPie(brush, rect, -90, availableAngle);
                }
                if (occupied > 0)
                {
                    using (SolidBrush brush = new SolidBrush(occupiedColor))
                        e.Graphics.FillPie(brush, rect, -90 + availableAngle, occupiedAngle);
                }
            }

            // Donut hole
            int holeSize = (int)(size * 0.6);
            int holeX = x + (size - holeSize) / 2;
            int holeY = y + (size - holeSize) / 2;
            using (SolidBrush bgBrush = new SolidBrush(BackColor))
                e.Graphics.FillEllipse(bgBrush, holeX, holeY, holeSize, holeSize);

            // Percentage text
            double rate = total == 0 ? 0 : (double)occupied / total * 100;
            string percentText = $"{rate:F0}%";
            string labelText = "Occupied";

            using (Font bigFont = new Font("Segoe UI Semibold", 22F, FontStyle.Bold))
            using (Font smallFont = new Font("Segoe UI", 9F))
            using (SolidBrush textBrush = new SolidBrush(textColor))
            using (SolidBrush subBrush = new SolidBrush(Color.FromArgb(120, 125, 140)))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                Rectangle textRect = new Rectangle(holeX, holeY - 10, holeSize, holeSize);
                e.Graphics.DrawString(percentText, bigFont, textBrush, textRect, sf);

                Rectangle labelRect = new Rectangle(holeX, holeY + 25, holeSize, holeSize);
                e.Graphics.DrawString(labelText, smallFont, subBrush, labelRect, sf);
            }
        }
    }
}