using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IterableDictionary
{
    public class IterableDict<TKey, TValue>
    {
        public int Size => _dict.Count;

        private ISimpleDictionary<TKey, TValue> _dict;
        private IterableLinkedList<TKey> _list;

        /// <summary>
        /// This will use a normal C# dictionary
        /// </summary>
        public IterableDict() : this(new MemorySimpleDictionary<TKey, TValue>()) { }

        public IterableDict(ISimpleDictionary<TKey, TValue> sourceDict)
        {
            if (sourceDict == null)
                throw new ArgumentNullException(nameof(sourceDict));

            _dict = sourceDict;
            _list = new IterableLinkedList<TKey>();
        }

        public void AddOrUpdate(TKey key, TValue value)
        {
            if (_dict.ContainsKey(key))
                _dict[key] = value;
            else
                _dict.Add(key, value);

            _list.AddOrPromote(key);
        }

        public TValue this[TKey key]
        {
            get
            {
                return _dict[key];
            }
        }

        public IterableLinkedListCursor<TKey> GetCursor()
        {
            return _list.GetCursor();
        }
    }
}
