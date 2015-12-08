using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IterableDictionary
{
    public class MemorySimpleDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISimpleDictionary<TKey, TValue>
    {
    }
}
