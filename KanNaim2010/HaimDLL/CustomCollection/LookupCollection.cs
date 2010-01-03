using System;
using System.Collections;


namespace HaimDLL.CustomCollection
{

    [Serializable]
    public class LookupCollection : ICollection, IDictionary, IEnumerable
    {
        private readonly ArrayList _mItems = new ArrayList();

        //---------------------------------------------------------------------
        // Add
        //---------------------------------------------------------------------
        public void Add(object key, object value)
        {
            // do some validation

            if (key == null)
                throw new ArgumentNullException("key", "key is a null reference");
            
            if (Contains(key))
                throw new ArgumentException("An element with the same key already exists");

            // add the new item

            var newItem = new Lookup();

            newItem.Key = key;
            newItem.Value = value;

            _mItems.Add(newItem);
            _mItems.Sort();
        }

        //---------------------------------------------------------------------

        // Clear

        //---------------------------------------------------------------------

        public void Clear()
        {
            _mItems.Clear();
        }

        //---------------------------------------------------------------------

        // Contains

        //---------------------------------------------------------------------

        public bool Contains(object key)
        {
            return (GetByKey(key) != null);
        }

        //---------------------------------------------------------------------

        // CopyTo

        //---------------------------------------------------------------------

        public void CopyTo(Array array, int index)
        {
            _mItems.CopyTo(array, index);
        }

        //---------------------------------------------------------------------

        // GetEnumerator (1)

        //---------------------------------------------------------------------

        public IDictionaryEnumerator GetEnumerator()
        {
            return new LookupEnumerator(_mItems);
        }

        //---------------------------------------------------------------------

        // GetEnumerator (2)

        //---------------------------------------------------------------------

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new LookupEnumerator(_mItems);
        }

        //---------------------------------------------------------------------

        // Remove

        //---------------------------------------------------------------------

        public void Remove(object key)
        {
            if (key == null)
                throw new ArgumentNullException("key", "key is a null reference");

            Lookup deleteItem = GetByKey(key);
            if (deleteItem != null)
            {
                _mItems.Remove(deleteItem);
                _mItems.Sort();
            }
        }

        //=====================================================================

        // PRIVATE

        //=====================================================================

        private Lookup GetByKey(object key)
        {
            Lookup result = null;
            int keyIndex = -1;
            ArrayList keys = (ArrayList)Keys;

            if (_mItems.Count > 0)
            {
                keyIndex = keys.IndexOf(key);

                if (keyIndex >= 0)
                {
                    result = (Lookup)_mItems[keyIndex];
                }
            }

            return result;
        }

        //=====================================================================

        // PROPERTIES

        //=====================================================================

        public int Count
        {
            get
            {
                return _mItems.Count;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public object this[object key]
        {
            get
            {

                if (key == null)
                    throw new ArgumentNullException("key", "key is a null reference");

                object result = null;

                Lookup findItem = GetByKey(key);
                if (findItem != null)
                {
                    result = findItem.Value;
                }

                return result;
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key", "key is a null reference");
                }

                Lookup findItem = GetByKey(key);
                if (findItem != null)
                {
                    findItem.Value = value;
                }
            }
        }
         
        public ICollection Keys
        {
            get
            {
                var result = new ArrayList();

                _mItems.Sort();

                foreach (Lookup curItem in _mItems)
                {
                    result.Add(curItem.Key);
                }

                return result;
            }
        }

        public ICollection Values
        {
            get
            {
                var result = new ArrayList();

                foreach (Lookup curItem in _mItems)
                {
                    result.Add(curItem.Value);
                }

                return result;
            }
        }
    }

}
