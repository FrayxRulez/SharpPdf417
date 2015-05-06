using SharpPdf417;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int aspectRatio = 2;
            int paddingLeftRight = 2;
            int paddingTopBottom = 2;

            Pdf417Generator generator = new Pdf417Generator("Hello World", ErrorCorrectionLevel.LevelZero, aspectRatio, paddingLeftRight, paddingTopBottom);
            Barcode barcode = generator.Encode();

            int bw = 3;
            int bh = 3;

            int y = 0;
            for (int r = 0; r < barcode.Rows; ++r)
            {
                int x = 0;
                for (int c = 0; c < barcode.Columns; ++c)
                {
                    if (barcode.RawData[r][c] == 1)
                    {
                        e.Graphics.FillRectangle(Brushes.Black, x, y, bw, bh);
                    }
                    x += bw;
                }
                y += bh;
            } 
        }
    }
}
