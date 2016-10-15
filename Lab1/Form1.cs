using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Lab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonBuild_Click(object sender, EventArgs e)
        {
            int min = int.Parse(textBoxMinX.Text),
                max = int.Parse(textBoxMaxX.Text),
                a, b, c, d;
            if (textBoxA.Text == "") a = 0;
            else a = int.Parse(textBoxA.Text);
            if (textBoxB.Text == "") b = 0;
            else b = int.Parse(textBoxB.Text);
            if (textBoxC.Text == "") c = 0;
            else c = int.Parse(textBoxC.Text);
            if (textBoxD.Text == "") d = 0;
            else d = int.Parse(textBoxD.Text);

            Series s = new Series("Default");
            s.ChartType = SeriesChartType.Spline;
            List<PointF> source = new List<PointF>();
            for (int i = min; i <= max; ++i)
            {
                float y = (float)(a * Math.Pow(i, 3) + b * Math.Pow(i, 2) + c * i + d);
                source.Add(new PointF(i, y));
            }

            chart.Series.Clear();
            chart.Series.Add(s);
            chart.ChartAreas[0].AxisX.Minimum = chart.ChartAreas[0].AxisX2.Minimum = min;
            chart.ChartAreas[0].AxisX.Maximum = chart.ChartAreas[0].AxisX2.Maximum = max;

            chart.DataSource = source;
            chart.Series[0].XValueMember = "X";
            chart.Series[0].YValueMembers = "Y";
            Invalidate();
        }
    }
}
