using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Shimanni.Common.Utils
{
    public class CommonUtils
    {
        #region CommonUtils
        public static string SeperateStringByUperCase(string strIn)
        {
            StringBuilder sb = new StringBuilder(strIn.Length);

            for (int i = 0; i < strIn.Length; i++)
            {
                if (char.IsUpper(strIn[i]) && i != 0 && char.IsWhiteSpace(strIn[i]) == false )
                {
                    sb.Append(" ");
                    sb.Append(strIn[i]);
                }
                else
                {
                    sb.Append(strIn[i]);
                }
            }
            return sb.ToString();
        }
        public static void SeprateGridHeaderTextByUperCase(DataGridView pGrid)
        {
            for (int i = 0; i < pGrid.ColumnCount; i++)
            {
                pGrid.Columns[i].HeaderText = CommonUtils.SeperateStringByUperCase(pGrid.Columns[i].Name);
            }
        }

        #region Min & Max Fanction
        public static double Min(double num1, double num2, double num3, bool MinButGreaterorEqualToZero)
        {
            List<double> List = new List<double>();
            List.Add(num1);
            List.Add(num2);
            List.Add(num3);
            List.Sort();
            if (MinButGreaterorEqualToZero==true)
                return Math.Max(List[0],0);
            else return List[0];
        }
        public static int Min(int num1, int num2, int num3, bool MinButGreaterorEqualToZero)
        {
            List<int> List = new List<int>();
            List.Add(num1);
            List.Add(num2);
            List.Add(num3);
            List.Sort();
            if (MinButGreaterorEqualToZero == true)
                return Math.Max(List[0], 0);
            else return List[0];
        }
        public static int Min(int num1, int num2, int num3, int num4, bool MinButGreaterorEqualToZero)
        {
            List<int> List = new List<int>();
            List.Add(num1);
            List.Add(num2);
            List.Add(num3);
            List.Add(num4);
            List.Sort();
            if (MinButGreaterorEqualToZero == true)
                return Math.Max(List[0], 0);
            else return List[0];
        }
        public static int Min(int num1, int num2, int num3, int num4, int num5, bool MinButGreaterorEqualToZero)
        {
            List<int> List = new List<int>();
            List.Add(num1);
            List.Add(num2);
            List.Add(num3);
            List.Add(num4);
            List.Add(num5);
            List.Sort();
            if (MinButGreaterorEqualToZero == true)
                return Math.Max(List[0], 0);
            else return List[0];
        }
        public static double Min(double num1, double num2, double num3, double num4, bool MinButGreaterorEqualToZero)
        {
            List<double> List = new List<double>();
            List.Add(num1);
            List.Add(num2);
            List.Add(num3);
            List.Add(num4);
            List.Sort(); 
            if (MinButGreaterorEqualToZero == true)
                return Math.Max(List[0], 0);
            else return List[0];
        }
        public static double Min(double num1, double num2, double num3, double num4,double num5,  bool MinButGreaterorEqualToZero)
        {
            List<double> List = new List<double>();
            List.Add(num1);
            List.Add(num2);
            List.Add(num3);
            List.Add(num4);
            List.Add(num5);
            List.Sort();
            if (MinButGreaterorEqualToZero == true)
                return Math.Max(List[0], 0);
            else return List[0];
        }
        public static double Max(double num1, double num2, double num3)
        {
            List<double> List = new List<double>();
            List.Add(num1);
            List.Add(num2);
            List.Add(num3);
            List.Sort();
            return List[2];
        }
        public static double Max(double num1, double num2, double num3, double num4)
        {
            List<double> List = new List<double>();
            List.Add(num1);
            List.Add(num2);
            List.Add(num3);
            List.Add(num4);
            List.Sort();
            return List[3];
        }
#endregion

        
        public static int DoubleToIntGTOrETZero(double size)
        {
            return Math.Max(0,(int) Math.Round(size, 0));
        }

        public static int MinGTZero(List<double> pList)
        {
            pList.Sort();
            return Math.Max(0, (int)Math.Round(pList[0], 0));
        }
        #endregion
    }
}


