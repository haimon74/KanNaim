using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HaimDLL
{
    public class Conversions
    {
        public static int NullInt2Int(int? nint)
        {
            if (nint == null)
                return 0;
            else
                return nint.Value;
        }

        public static long NullLong2Long(long? nlong)
        {
            if (nlong == null)
                return 0;
            else
                return nlong.Value;
        }
        public static decimal NullDec2Dec(decimal? ndec)
        {
            if (ndec == null)
                return 0;
            else
                return ndec.Value;
        }
        public static double NullDouble2Double(double? nlong)
        {
            if (nlong == null)
                return 0;
            else
                return nlong.Value;
        }
        public static string NullString2String(string nstr)
        {
            if (nstr == null)
                return "";
            else
                return nstr;
        }
        
    }

    public class Is
    {
        public static bool StringEmpty(string str)
        {
            return (str == String.Empty);
        }
        public static bool StringNull(string str)
        {
            return (str == null);
        }
        public static bool IntNull(int? nint)
        {
            return (nint == null);
        }
        public static bool DecimalNull(decimal? ndec)
        {
            return (ndec == null);
        }
        public static bool LongNull(long? nlong)
        {
            return (nlong == null);
        }
        public static bool DateTimeNull(DateTime? ndate)
        {
            return (ndate == null);
        }
        public static bool ObjectNull(object obj)
        {
            return (obj == null);
        }

    }
}
