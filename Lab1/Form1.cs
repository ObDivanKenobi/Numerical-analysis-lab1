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
            int len = dataGridViewSpline.Rows.Count;
            List<double> x = new List<double>(),
                      y = new List<double>();

            foreach (DataGridViewRow r in dataGridViewSpline.Rows)
            {
                try
                {
                    double tx = double.Parse((string)r.Cells[0].Value),
                           ty = double.Parse((string)r.Cells[1].Value);
                }
                catch
                {
                    MessageBox.Show("Введены неверные данные.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            double d;
            try
            {
                d = double.Parse(textBoxDelta.Text);
            }
            catch
            {
                MessageBox.Show("Введены неверные данные.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            chart.Series.Clear();
            chart.Series.Add(new Series());
            chart.Series[0].XValueMember = "X";
            chart.Series[0].YValueMembers = "Y";
            CubicSpline mySpline = new CubicSpline();
            mySpline.BuildSpline(x, y);
            for (double i = x.Min(); i < x.Max(); i += d)
                chart.Series[0].Points.AddXY(i, mySpline.Interpolate(i));

            chart.Series.Add(new Series());
            chart.Series[1].XValueMember = "X";
            chart.Series[1].YValueMembers = "Y";
            for (int i = 0; i < len; ++i)
                chart.Series[1].Points.AddXY(x[i], y[i]);
            chart.Series[1].Color = Color.Green;

            ChartArea area = chart.ChartAreas[0];
            area.AxisX.Minimum = chart.ChartAreas[0].AxisX2.Minimum = x.Min();
            area.AxisX.Maximum = chart.ChartAreas[0].AxisX2.Maximum = x.Max();
            area.AxisY.Minimum = area.AxisY2.Minimum = y.Min();
            area.AxisY.Maximum = area.AxisY2.Maximum = y.Max();

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
