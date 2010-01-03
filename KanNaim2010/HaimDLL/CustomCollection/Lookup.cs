using System;
using System.Collections;

namespace HaimDLL.CustomCollection
{
    [Serializable]
    public class Lookup : IComparable
    {
        private object _mKey;
        private object _mValue;

        //---------------------------------------------------------------------

        // Default Constructor

        //---------------------------------------------------------------------

        public Lookup()
            : this(null, null)
        {
        }

        //---------------------------------------------------------------------

        // Overloaded Constructor

        //---------------------------------------------------------------------

        public Lookup(object key, object value)
        {
            Key = key;
            Value = value;
        }

        //---------------------------------------------------------------------

        // CompareTo

        //---------------------------------------------------------------------

        public int CompareTo(object obj)
        {
            int result = 0;

            if (obj is Lookup)
            {
                result =
                 ((IComparable)Value).CompareTo((IComparable)
                                            (((Lookup)obj).Value));
            }

            return result;
        }

        //---------------------------------------------------------------------

        // ToDictionaryEntry

        //---------------------------------------------------------------------

        public DictionaryEntry ToDictionaryEntry()
        {
            return new DictionaryEntry(Key, Value);
        }

        //=====================================================================

        // PROPERTIES

        //=====================================================================

        public object Key
        {
            get
            {
                return _mKey;
            }
            set
            {
                if ((_mKey != null) && (_mKey != value))
                    {
                        _mKey = value;
                    }
            }
        }

        public object Value
        {
            get
            {
                return _mValue;
            }
            set
            {
                if ((_mValue != null) && (_mValue != value))
                    {
                        _mValue = value;
                    }
            }
        }
    }

}
