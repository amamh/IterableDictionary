using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IterableDict
{

    public class IterableDict<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _dict;
        private CustomLList<TKey> _list;

        public IterableDict()
        {
            _dict = new Dictionary<TKey, TValue>();
            _list = new CustomLList<TKey>();
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

        public Cursor<TKey> GetCursor()
        {
            return _list.GetCursor();
        }
    }

    public class CustomLList<T>
    {
        private CustomNode<T> _start;
        private CustomNode<T> _end;
        private Dictionary<T, CustomNode<T>> _lookup;

        private HashSet<Cursor<T>> _cursorsWaiting;

        public CustomLList()
        {
            _lookup = new Dictionary<T, CustomNode<T>>();
            _start = null;
            _end = null;
            _cursorsWaiting = new HashSet<Cursor<T>>();
        }

        public Cursor<T> GetCursor()
        {
            var c = new Cursor<T>(_start, _cursorsWaiting);
            return c;
        }

        public void Add(T value)
        {
            Add(new CustomNode<T>(value));
        }


        public void Add(CustomNode<T> node)
        {
            if (_start == null)
            {
                Debug.Assert(_end == null);
                _start = node;
                _end = _start;
            }
            else
            {
                foreach (var cursor in _cursorsWaiting)
                {
                    cursor.SetNext(node);
                }

                _end.Next = node;
                node.Prev = _end;
                _end = node;
            }
            if (!_lookup.ContainsKey(node.Value))
                _lookup.Add(node.Value, node);
        }

        public void Promote(T value)
        {
            if (_lookup.ContainsKey(value))
                Promote(_lookup[value]);
            else
                throw new ArgumentOutOfRangeException();
        }

        public void Promote(CustomNode<T> node)
        {
            if (node == null)
                throw new ArgumentNullException();
            var value = node.Value;
            if (!_lookup.ContainsKey(value))
                throw new ArgumentOutOfRangeException();

            if (_end == node) // nothing to do
                return;

            Debug.Assert(node.Next != null); // since we exclude last nodes

            var prev = node.Prev;
            var next = node.Next;
            var last = _end;

            lock (node.CursorsIncoming)
            {
                lock (node.Next.CursorsIncoming)
                {
                    // fix cursors
                    foreach (var cursor in node.CursorsIncoming)
                    {
                        node.Next.CursorsIncoming.Add(cursor);
                        cursor._next = node.Next;
                    }
                }
                node.CursorsIncoming.Clear();
            }

            // take out
            node.Next = null;
            node.Prev = null;
            if (prev != null)
                prev.Next = next;
            else // this must be start node !
            {
                Debug.Assert(node == _start);
                _start = next;
            }
            next.Prev = prev;

            // attach to end
            Add(node);
        }

        public void AddOrPromote(T value)
        {
            if (_lookup.ContainsKey(value))
                Promote(_lookup[value]);
            else
                Add(value);
        }
    }

    /// <summary>
    /// A linked list node that maintains a list of cursors and makes sure their next node to read will
    /// be the current next node even if the current next gets changed to something else
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomNode<T>
    {
        public T Value { get; private set; }
        public CustomNode<T> Prev { get; set; }

        public CustomNode<T> Next
        {
            get; set;
        }

        private HashSet<Cursor<T>> _cursorsIncoming;

        //public List<Cursor<T>> Cursors { get; set; }
        public HashSet<Cursor<T>> CursorsIncoming
        {
            get
            {
                return _cursorsIncoming;
            }
        }


        public CustomNode(T value)
        {
            Value = value;
            _cursorsIncoming = new HashSet<Cursor<T>>();
        }
    }

    public class Cursor<T>
    {
        CustomNode<T> _node = null;
        internal CustomNode<T> _next = null;
        HashSet<Cursor<T>> _waitingList;

        public Cursor(CustomNode<T> node, HashSet<Cursor<T>> waitingList)
        {
            _node = null;
            _waitingList = waitingList;

            SetNext(node);
        }

        internal void SetNext(CustomNode<T> next)
        {
            // remove from old CursorsIncoming
            if (_next != null)
            {
                lock (_next.CursorsIncoming)
                {
                    Debug.Assert(_next.CursorsIncoming.Contains(this));
                    _next.CursorsIncoming.Remove(this);
                }
            }

            // Add to new CursorsIncoming
            if (next != null)
            {
                lock (next.CursorsIncoming)
                {
                    next.CursorsIncoming.Add(this);
                }
            }

            // Cursor is now waiting for more
            if (next == null && !_waitingList.Contains(this))
                _waitingList.Add(this);

            // Cursor was waiting and now has something to read
            if (next != null && _waitingList.Contains(this))
                _waitingList.Remove(this);

            _next = next;
        }

        /// <summary>
        /// Must call this first before GetCurrent
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (_next == null)
                return false;
            _node = _next;

            SetNext(_node.Next);
            return true;
        }

        public CustomNode<T> GetCurrent()
        {
            return _node;
        }
    }
}
