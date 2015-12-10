using System;
using System.Collections.Generic;

namespace IterableDictionary
{
    public class IterableDict<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _dict;
        private readonly IterableLinkedList<TKey> _list;

        /// <summary>
        ///     This will use a new normal C# dictionary
        /// </summary>
        public IterableDict() : this(new Dictionary<TKey, TValue>())
        {
        }

        /// <summary>
        ///     The given IDictionary must implement methods/props: Add, [], Count, Keys, ContainsKey
        /// </summary>
        /// <param name="sourceDict"></param>
        public IterableDict(IDictionary<TKey, TValue> sourceDict)
        {
            if (sourceDict == null)
                throw new ArgumentNullException(nameof(sourceDict));

            _dict = sourceDict;
            _list = new IterableLinkedList<TKey>();

            foreach (var key in _dict.Keys)
                _list.Add(key);
        }

        public int Size => _dict.Count;

        public TValue this[TKey key] => _dict[key];

        public void AddOrUpdate(TKey key, TValue value)
        {
            if (_dict.ContainsKey(key))
                _dict[key] = value;
            else
                _dict.Add(key, value);

            _list.AddOrPromote(key);
        }

        public IterableLinkedListCursor<TKey> GetCursor()
        {
            return _list.GetCursor();
        }
    }
}