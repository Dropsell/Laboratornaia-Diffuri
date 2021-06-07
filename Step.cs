using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratornaia
{
    struct Coordinat
    {
        public double Xvalue;
        public double YAvalue;
        public double YRKvalue;
        public double deltaY()
        {
            return Math.Abs(YAvalue - YRKvalue);
        }
    }
    
    class Step
    {
        private double X;
        private double Xn;
        private double Y;
        private double h;
        private double exp;

        private List<Coordinat> TableOfSolution = new List<Coordinat>();

        public Step (double newX, double newXn, double newY, double newH, double newExp)
        {
            X = newX;
            Xn = newXn;
            Y = newY;
            h = newH;
            exp = newExp;
            Start();
        }

        public List<Coordinat> GetTableOfSolution
        {
            get
            {
                return TableOfSolution;
            }
        }

        private void Start()
        {
            int k = 1;  //уточнение шага
            while ((((X * k) % 1) != 0) || (((h * k) % 1) != 0))
            {
                k *= 10;
            }
            double intX = Convert.ToInt32(X * k);
            int intH = Convert.ToInt32(h * k);

            Coordinat newRow;
            newRow.Xvalue = X;
            newRow.YAvalue = analiticYFunc(X);
            newRow.YRKvalue = Y;
            TableOfSolution.Add(newRow);
            intX += intH;

            while (intX <= (Xn * k))
            {
                newRow.Xvalue = 1.0 * intX / k;
                newRow.YAvalue = analiticYFunc(1.0 * intX / k);
                Y = RK(1.0 * intX / k, Y);
                newRow.YRKvalue = Y; 
                TableOfSolution.Add(newRow);
                intX += intH;
            }
        }

        private double RK(double thisX, double thisY)
        {
            double k1 = h * F(thisX, thisY);
            double k2 = h * F(thisX + 0.5 * h, thisY + 0.5 * k1);
            double k3 = h * F(thisX + 0.5 * h, thisY + 0.5 * k2);
            double k4 = h * F(thisX + h, thisY + k3);
            double deltY = (k1 + 2 * k2 + 2 * k3 + k4) / 6;
            return thisY + deltY;
        }

        private double analiticYFunc(double thisX)
        {
            return 0.999999 * Math.Pow(exp, 1.0 * -1/ thisX) + Math.Pow(exp, thisX - 1.0 * 1 / thisX);
        }

        private double F(double thisX, double thisY)
        {
            return -20 * (thisY - Math.Sin(thisX));
        }
    }
}
