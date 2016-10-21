using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ZedGraph;

namespace Lab1
{
    public partial class Form1 : Form
    {
        PointPairList list = null;
        Color color;

        public Form1()
        {
            InitializeComponent();
            LoadDefault();
            comboBox1.SelectedItem = comboBox1.Items[0];
            color = Color.Blue;
            chart.GraphPane.Title = "График интерполированной функции";
        }

        public void OnIdle(object sender, EventArgs e)
        {
            buttonColor.Enabled = list != null;
        }

        private void buttonBuild_Click(object sender, EventArgs e)
        {
            SortedDictionary<double, double> table = new SortedDictionary<double, double>();
            for(int i = 0; i < dataGridViewSpline.Rows.Count - 1; ++i) 
            { 
                DataGridViewRow r = dataGridViewSpline.Rows[i];
                try { table.Add(double.Parse((string)r.Cells[0].Value), double.Parse((string)r.Cells[1].Value)); }
                catch(FormatException)
                {
                    MessageBox.Show("Проверьте правильность введённых данных.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch(ArgumentException)
                {
                    MessageBox.Show("Проверьте отсутствие повторяющихся данных в таблице.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            double min_x = table.Keys.Min(),
                   max_x = table.Keys.Max();

            CubicSpline spline = new CubicSpline();
            spline.BuildSpline(table.Keys.ToList(), table.Values.ToList());

            list = new PointPairList();
            string ds = d.ToString();
            int index = ds.IndexOf(',');
            int signs = index < 0 ? 0 : ds.Substring(index + 1).Length;
            for (double i = min_x; Math.Round(max_x - i, signs) >= 0; i = Math.Round(i + d, signs))
                list.Add(i, spline.Interpolate(i));

            BuildGraph(list);
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            if (colorDialogGrapicColor.ShowDialog() == DialogResult.OK)
            {
                color = colorDialogGrapicColor.Color;
                BuildGraph(list);
            }
        }

        private void buttonErase_Click(object sender, EventArgs e)
        {
            chart.GraphPane.CurveList.Clear();
            chart.Invalidate();
            list = null;
        }

        private void buttonSaveAsDefault_Click(object sender, EventArgs e)
        {
            StreamWriter write = new StreamWriter("default.txt");
            for (int i = 0; i < dataGridViewSpline.Rows.Count - 1; ++i)
                write.WriteLine(string.Format("{0} {1}", dataGridViewSpline[0, i].Value, dataGridViewSpline[1, i].Value));
            write.Close();
        }

        void LoadDefault()
        {
            StreamReader read = null;
            try
            {
                read = new StreamReader("default.txt");
                int len = int.Parse(read.ReadLine());
                for (int i = 0; i < len; ++i)
                    dataGridViewSpline.Rows.Add(new DataGridViewRow());
                int j = 0;
                while (!read.EndOfStream)
                {
                    string[] values = read.ReadLine().Split(' ');
                    dataGridViewSpline[0, j].Value = values[0];
                    dataGridViewSpline[1, j].Value = values[1];
                    ++j;
                }
            }
            catch(Exception e) { return; }
            finally { read?.Close(); }
        }

        void BuildGraph(PointPairList list)
        {
            chart.GraphPane.CurveList.Clear();
            SymbolType point;
            switch(comboBox1.SelectedIndex)
            {
                case 0: { point = SymbolType.None; break; }
                case 1: { point = SymbolType.Circle; break; }
                case 2: { point = SymbolType.Square; break; }
                case 3: { point = SymbolType.Triangle; break; }
                default: { point = SymbolType.Diamond; break; }
            }

            LineItem graphic = chart.GraphPane.AddCurve("", list, color, point);
            chart.AxisChange();
            chart.Invalidate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (list != null)
                BuildGraph(list);
        }
    }
}
