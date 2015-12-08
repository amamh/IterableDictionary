using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IterableDictionary
{
    public interface ISimpleDictionary<TKey, TValue>
    {
        int Count { get; }

        void Add(TKey key, TValue value);

        bool ContainsKey(TKey key);

        TValue this[TKey key] { get; set; }
    }
}
