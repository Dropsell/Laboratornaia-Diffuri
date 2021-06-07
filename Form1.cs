using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratornaia
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //label7.Text = "Идёт расчёт... Круг расчёта: ";
            dataGridView1.Rows.Clear();
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();

            //var exp = double.Parse(textBoxEXP.Text);
            var exp = Math.Exp(1);
            var h = double.Parse(textBoxH.Text);
            var X0 = double.Parse(textBoxX1.Text);
            var Xn = double.Parse(textBoxX2.Text);
            var Y0 = double.Parse(textBoxY0.Text);

            var precision = double.Parse(textBoxEXP.Text);
            //var precision = 0.0001;
            var R = precision + 1;
            var h2 = h;


            string[] tokens = h.ToString("G", CultureInfo.InvariantCulture).Split('.');
            int countK = tokens.Length > 1 ? tokens[1].Length : 0;

            string countL = "{0:0.";
            for (int i = 0; i < countK; i++)
            {
                countL += "0";
            }
            countL += "}";
            //countL = "{0:0.0}";

            var Controller0 = new Step(X0, Xn, Y0, h, exp);
            var TableOfSolutions = Controller0.GetTableOfSolution;

            List<Coordinat> Table1 = null;
            List<Coordinat> Table2 = null;

            //int count = 0;

            while (R > precision)
            {
                var Controller1 = new Step(X0, Xn, Y0, h2, exp);
                var Controller2 = new Step(X0, Xn, Y0, h2*0.5, exp);
                Table1 = Controller1.GetTableOfSolution;
                Table2 = Controller2.GetTableOfSolution;
                //label7.Text = $"Идёт расчёт... Круг расчёта: {count}";
                //count++;

                for (int i=0; i < Table1.Count; i++)
                {
                    for (int j = 0; j < Table2.Count; j++)
                    {
                        if (Table1[i].Xvalue == Table2[j].Xvalue)
                        {
                            R = 1.0 * 1 / 15 * Math.Abs(Table1[i].YRKvalue - Table2[j].YRKvalue);
                            if (R > precision)
                            {
                                break;
                            }
                        }
                    }
                }
                h2 *= 0.5;
            }

            for (int i = 0; i < TableOfSolutions.Count; i++)
            {
                for (int j = 0; j < Table2.Count; j++)
                {
                    if (TableOfSolutions[i].Xvalue == Table2[j].Xvalue)
                    {
                        Coordinat newPoint;
                        newPoint.Xvalue = TableOfSolutions[i].Xvalue;
                        newPoint.YAvalue = TableOfSolutions[i].YAvalue;
                        newPoint.YRKvalue = Table2[j].YRKvalue;

                        TableOfSolutions[i] = newPoint;
                    }
                }
            }



            for (int i = 0; i < TableOfSolutions.Count; i++)
            {
                var thisX = TableOfSolutions[i].Xvalue;
                var thisYanakitik = TableOfSolutions[i].YAvalue;
                var thisYRK = TableOfSolutions[i].YRKvalue;
                dataGridView1.Rows.Add(String.Format(countL, thisX), String.Format("{0:0.000000}", thisYanakitik), String.Format("{0:0.000000}", thisYRK), String.Format("{0:0.000000}", TableOfSolutions[i].deltaY()));
                chart1.Series[0].Points.AddXY(thisX, thisYanakitik);
                chart1.Series[1].Points.AddXY(thisX, thisYRK);
            }
            label7.Text = "Расчёт окончен";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label7.Visible = false;
            dataGridView1.Columns.Add("column-x", "X");
            dataGridView1.Columns.Add("column-yRK", "Точное решение");
            dataGridView1.Columns.Add("column-yA", "Приближённое решение");
            dataGridView1.Columns.Add("column-delt", "Погрешность");
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 125;
            dataGridView1.Columns[2].Width = 125;
            dataGridView1.Columns[3].Width = 125;
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
