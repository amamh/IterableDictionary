﻿using System;
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
        private IterableLinkedList<TKey> _list;

        public IterableDict()
        {
            _dict = new Dictionary<TKey, TValue>();
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

    public class IterableLinkedList<T>
    {
        private IterableLinkedListNode<T> _start;
        private IterableLinkedListNode<T> _end;
        private Dictionary<T, IterableLinkedListNode<T>> _lookup;

        private HashSet<IterableLinkedListCursor<T>> _cursorsWaiting;

        public IterableLinkedList()
        {
            _lookup = new Dictionary<T, IterableLinkedListNode<T>>();
            _start = null;
            _end = null;
            _cursorsWaiting = new HashSet<IterableLinkedListCursor<T>>();
        }

        public IterableLinkedListCursor<T> GetCursor()
        {
            var c = new IterableLinkedListCursor<T>(_start, _cursorsWaiting);
            return c;
        }

        public void Add(T value)
        {
            Add(new IterableLinkedListNode<T>(value));
        }

        public void Add(IterableLinkedListNode<T> node)
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

        /// <summary>
        /// Move to the end of the list
        /// </summary>
        /// <param name="value"></param>
        public void Promote(T value)
        {
            if (_lookup.ContainsKey(value))
                Promote(_lookup[value]);
            else
                throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Move to the end of the list
        /// </summary>
        public void Promote(IterableLinkedListNode<T> node)
        {
            if (node == null)
                throw new ArgumentNullException();
            var value = node.Value;
            if (!_lookup.ContainsKey(value))
                throw new ArgumentOutOfRangeException();

            if (_end == node) // tell all cursors to read this again
            {
                foreach (var curosr in _cursorsWaiting)
                {
                    node.CursorsIncoming.Add(curosr);
                    curosr._next = node;
                }
                return;
            }

            Debug.Assert(node.Next != null); // since we exclude last nodes

            var prev = node.Prev;
            var next = node.Next;
            var last = _end;

            // fix cursors
            foreach (var cursor in node.CursorsIncoming)
            {
                node.Next.CursorsIncoming.Add(cursor);
                cursor._next = node.Next;
            }
            node.CursorsIncoming.Clear();

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

        /// <summary>
        /// Add or move to the end of the list
        /// </summary>
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
    public class IterableLinkedListNode<T>
    {
        public T Value { get; private set; }
        public IterableLinkedListNode<T> Prev { get; set; }

        public IterableLinkedListNode<T> Next
        {
            get; set;
        }

        private HashSet<IterableLinkedListCursor<T>> _cursorsIncoming;

        //public List<Cursor<T>> Cursors { get; set; }
        public HashSet<IterableLinkedListCursor<T>> CursorsIncoming
        {
            get
            {
                return _cursorsIncoming;
            }
        }


        public IterableLinkedListNode(T value)
        {
            Value = value;
            _cursorsIncoming = new HashSet<IterableLinkedListCursor<T>>();
        }
    }

    public class IterableLinkedListCursor<T>
    {
        IterableLinkedListNode<T> _node = null;
        internal IterableLinkedListNode<T> _next = null;
        HashSet<IterableLinkedListCursor<T>> _waitingList;

        public IterableLinkedListCursor(IterableLinkedListNode<T> node, HashSet<IterableLinkedListCursor<T>> waitingList)
        {
            _node = null;
            _waitingList = waitingList;

            SetNext(node);
        }

        internal void SetNext(IterableLinkedListNode<T> next)
        {
            // remove from old CursorsIncoming
            if (_next != null)
            {
                Debug.Assert(_next.CursorsIncoming.Contains(this));
                _next.CursorsIncoming.Remove(this);
            }

            // Add to new CursorsIncoming
            if (next != null)
            {
                next.CursorsIncoming.Add(this);
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

        public IterableLinkedListNode<T> GetCurrent()
        {
            return _node;
        }
    }
}
