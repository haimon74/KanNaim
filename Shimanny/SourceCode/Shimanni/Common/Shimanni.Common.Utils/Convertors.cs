using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shimanni.Common.Utils
{
    public class EnumToList
    {
        #region Members
        private string _EnumString;
        private int _EnumValue;
        #endregion

        #region Ctors
        public EnumToList(string EnumString, int EnumValue)
        {
            this._EnumString = EnumString;
            this._EnumValue = EnumValue;
        }
        #endregion

        #region Properties
        public string EnumString
        {
            get
            {
                return _EnumString;
            }
            set
            {
                _EnumString = value;
            }
        }
        public int EnumValue
        {
            get
            {
                return _EnumValue;
            }
            set
            {
                _EnumValue = value;
            }
        }
        #endregion

        #region CommonUtils
        public static List<EnumToList> List<T>()
        {
            List<EnumToList> _List = new List<EnumToList>();
            int[] x = (int[])Enum.GetValues(typeof(T));

            foreach (int Const in x)
            {
                _List.Add(new EnumToList(Enum.GetName(typeof(T), Const), (int)Const));

            }
            return _List;
        }
        #endregion
    }
}
