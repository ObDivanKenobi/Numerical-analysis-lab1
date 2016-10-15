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
            chart.Series.Clear();
        }

        internal void OnIdle(object sender, EventArgs e)
        {
            buttonColor.Enabled = chart.Series.Count > 0;
        }

        private void buttonBuild_Click(object sender, EventArgs e)
        {
            //ChartArea area = chart.ChartAreas[0];
            //chart.Series.Clear();
            //chart.Series.Add(s);
            //area.AxisX.Minimum = chart.ChartAreas[0].AxisX2.Minimum = min;
            //area.AxisX.Maximum = chart.ChartAreas[0].AxisX2.Maximum = max;
            //area.AxisY.Minimum = area.AxisY2.Minimum = source.Min(point => point.Y);
            //area.AxisY.Maximum = area.AxisY2.Maximum = source.Max(point => point.Y);

            //chart.DataSource = source;
            chart.Series[0].XValueMember = "X";
            chart.Series[0].YValueMembers = "Y";
            Invalidate();
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            if (colorDialogGrapicColor.ShowDialog() == DialogResult.OK && chart.Series.Count > 0)
                chart.Series[0].Color = colorDialogGrapicColor.Color;
        }

        private void buttonErase_Click(object sender, EventArgs e)
        {
            chart.Series.Clear();
        }
    }
}
