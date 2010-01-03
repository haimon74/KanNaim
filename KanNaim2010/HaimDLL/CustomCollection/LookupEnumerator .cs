using System;
using System.Collections;

namespace HaimDLL.CustomCollection
{
    public class LookupEnumerator : IDictionaryEnumerator
    {
        private int _index = -1;
        private ArrayList _items;

        //---------------------------------------------------------------------

        // Constructor

        //---------------------------------------------------------------------

        public LookupEnumerator(ArrayList list)
        {
            _items = list;
        }

        //---------------------------------------------------------------------

        // MoveNext

        //---------------------------------------------------------------------

        public bool MoveNext()
        {
            _index++;
            if (_index >= _items.Count)
                return false;

            return true;
        }

        //=====================================================================
        // PROPERTIES
        //=====================================================================

        //---------------------------------------------------------------------
        // Reset
        //---------------------------------------------------------------------
        public void Reset()
        {
            _index = -1;
        }

        //---------------------------------------------------------------------
        // Current
        //---------------------------------------------------------------------
        public object Current
        {
            get
            {
                if (_index < 0 || _index >= _items.Count)
                    throw new InvalidOperationException();

                return _items[_index];
            }
        }

        //---------------------------------------------------------------------
        // Entry
        //---------------------------------------------------------------------
        public DictionaryEntry Entry
        {
            get
            {
                return ((Lookup)Current).ToDictionaryEntry();
            }
        }

        //---------------------------------------------------------------------
        // Key
        //---------------------------------------------------------------------
        public object Key
        {
            get
            {
                return Entry.Key;
            }
        }

        //---------------------------------------------------------------------
        // Value
        //---------------------------------------------------------------------
        public object Value
        {
            get
            {
                return Entry.Value;
            }
        }
    }

}
