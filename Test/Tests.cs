using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NUnit.Framework;
using IterableDictionary;
using System.Collections.Generic;

namespace Test
{
    [TestClass]
    public class Tests
    {
        IterableDict<int, int> _dict;
        int _length = 10;

        [TestInitialize]
        public void Setup()
        {
            _dict = new IterableDict<int, int>();
            for (int i = 0; i < _length; i++)
                _dict.AddOrUpdate(i, i * 10);
        }

        [TestMethod]
        public void ModifyNextNode()
        {
            var cursor1 = _dict.GetCursor();

            var cursor1Read = new Dictionary<int, int>();
            IterableLinkedListNode<int> curr;

            // read
            Assert.IsTrue(cursor1.MoveNext(), "Couldn't read next");
            curr = cursor1.GetCurrentNode();
            Assert.IsNotNull(curr?.Value);
            cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);

            // read
            Assert.IsTrue(cursor1.MoveNext(), "Couldn't read next");
            curr = cursor1.GetCurrentNode();
            Assert.IsNotNull(curr?.Value);
            cursor1Read.Add(curr.Value, _dict[curr.Value]);


            // update next node
            _dict.AddOrUpdate(2, _dict[2] * 2);

            // read the rest
            while (cursor1.MoveNext())
            {
                curr = cursor1.GetCurrentNode();
                Assert.IsNotNull(curr?.Value);
                cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);
            }

            // read from beginning with a new cursor
            var cursor2 = _dict.GetCursor();
            var cursor2Read = new Dictionary<int, int>();
            while (cursor2.MoveNext())
            {
                curr = cursor2.GetCurrentNode();
                Assert.IsNotNull(curr?.Value);
                cursor2Read.AddOrUpdate(curr.Value, _dict[curr.Value]);
            }

            // Check we got the same
            foreach (var pair in cursor1Read)
            {
                Assert.IsTrue(cursor2Read.ContainsKey(pair.Key));
                Assert.AreEqual(cursor2Read[pair.Key], pair.Value);
            }
        }

        [TestMethod]
        public void ModifyCurrentNode()
        {
            var cursor1 = _dict.GetCursor();

            var cursor1Read = new Dictionary<int, int>();
            IterableLinkedListNode<int> curr;

            // read
            Assert.IsTrue(cursor1.MoveNext(), "Couldn't read next");
            curr = cursor1.GetCurrentNode();
            Assert.IsNotNull(curr?.Value);
            cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);

            // read
            Assert.IsTrue(cursor1.MoveNext(), "Couldn't read next");
            curr = cursor1.GetCurrentNode();
            Assert.IsNotNull(curr?.Value);
            cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);


            // update last read node
            _dict.AddOrUpdate(1, _dict[2] * 2);

            // read the rest
            while (cursor1.MoveNext())
            {
                curr = cursor1.GetCurrentNode();
                Assert.IsNotNull(curr?.Value);
                cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);
            }

            // read from beginning with a new cursor
            var cursor2 = _dict.GetCursor();
            var cursor2Read = new Dictionary<int, int>();
            while (cursor2.MoveNext())
            {
                curr = cursor2.GetCurrentNode();
                Assert.IsNotNull(curr?.Value);
                cursor2Read.AddOrUpdate(curr.Value, _dict[curr.Value]);
            }

            // Check we got the same
            foreach (var pair in cursor1Read)
            {
                Assert.IsTrue(cursor2Read.ContainsKey(pair.Key));
                Assert.AreEqual(cursor2Read[pair.Key], pair.Value);
            }
        }

        [TestMethod]
        public void ModifyAlreadyReadNodes()
        {
            var cursor1 = _dict.GetCursor();

            var cursor1Read = new Dictionary<int, int>();
            IterableLinkedListNode<int> curr;

            // read
            Assert.IsTrue(cursor1.MoveNext(), "Couldn't read next");
            curr = cursor1.GetCurrentNode();
            Assert.IsNotNull(curr?.Value);
            cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);

            // read
            Assert.IsTrue(cursor1.MoveNext(), "Couldn't read next");
            curr = cursor1.GetCurrentNode();
            Assert.IsNotNull(curr?.Value);
            cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);

            // read
            Assert.IsTrue(cursor1.MoveNext(), "Couldn't read next");
            curr = cursor1.GetCurrentNode();
            Assert.IsNotNull(curr?.Value);
            cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);

            // update previously read nodes
            _dict.AddOrUpdate(0, _dict[0] * 2);
            _dict.AddOrUpdate(1, _dict[1] * 2);

            // read the rest
            while (cursor1.MoveNext())
            {
                curr = cursor1.GetCurrentNode();
                Assert.IsNotNull(curr?.Value);
                cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);
            }

            // read from beginning with a new cursor
            var cursor2 = _dict.GetCursor();
            var cursor2Read = new Dictionary<int, int>();
            while (cursor2.MoveNext())
            {
                curr = cursor2.GetCurrentNode();
                Assert.IsNotNull(curr?.Value);
                cursor2Read.AddOrUpdate(curr.Value, _dict[curr.Value]);
            }

            // Check we got the same
            foreach (var pair in cursor1Read)
            {
                Assert.IsTrue(cursor2Read.ContainsKey(pair.Key));
                Assert.AreEqual(cursor2Read[pair.Key], pair.Value);
            }
        }

        [TestMethod]
        public void ModifyNextCurrentAndPreviousNodes()
        {
            var cursor1 = _dict.GetCursor();

            var cursor1Read = new Dictionary<int, int>();
            IterableLinkedListNode<int> curr;

            // read
            Assert.IsTrue(cursor1.MoveNext(), "Couldn't read next");
            curr = cursor1.GetCurrentNode();
            Assert.IsNotNull(curr?.Value);
            cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);

            // read
            Assert.IsTrue(cursor1.MoveNext(), "Couldn't read next");
            curr = cursor1.GetCurrentNode();
            Assert.IsNotNull(curr?.Value);
            cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);

            // read
            Assert.IsTrue(cursor1.MoveNext(), "Couldn't read next");
            curr = cursor1.GetCurrentNode();
            Assert.IsNotNull(curr?.Value);
            cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);

            // update previously read nodes
            _dict.AddOrUpdate(0, _dict[0] * 2);
            _dict.AddOrUpdate(1, _dict[1] * 2);
            // update current node
            _dict.AddOrUpdate(2, _dict[2] * 2);
            // update next node
            _dict.AddOrUpdate(3, _dict[3] * 2);

            // read the rest
            while (cursor1.MoveNext())
            {
                curr = cursor1.GetCurrentNode();
                Assert.IsNotNull(curr?.Value);
                cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);
            }

            // read from beginning with a new cursor
            var cursor2 = _dict.GetCursor();
            var cursor2Read = new Dictionary<int, int>();
            while (cursor2.MoveNext())
            {
                curr = cursor2.GetCurrentNode();
                Assert.IsNotNull(curr?.Value);
                cursor2Read.AddOrUpdate(curr.Value, _dict[curr.Value]);
            }

            // Check we got the same
            foreach (var pair in cursor1Read)
            {
                Assert.IsTrue(cursor2Read.ContainsKey(pair.Key));
                Assert.AreEqual(cursor2Read[pair.Key], pair.Value);
            }
        }

        [TestMethod]
        public void ModifyAll()
        {
            var cursor1 = _dict.GetCursor();

            var cursor1Read = new Dictionary<int, int>();
            IterableLinkedListNode<int> curr;

            // read
            Assert.IsTrue(cursor1.MoveNext(), "Couldn't read next");
            curr = cursor1.GetCurrentNode();
            Assert.IsNotNull(curr?.Value);
            cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);

            // read
            Assert.IsTrue(cursor1.MoveNext(), "Couldn't read next");
            curr = cursor1.GetCurrentNode();
            Assert.IsNotNull(curr?.Value);
            cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);

            // read
            Assert.IsTrue(cursor1.MoveNext(), "Couldn't read next");
            curr = cursor1.GetCurrentNode();
            Assert.IsNotNull(curr?.Value);
            cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);

            // update everything
            for (int i = 0; i < _length; i++)
            {
                _dict.AddOrUpdate(i, _dict[i] * 2);
            }

            // read the rest
            while (cursor1.MoveNext())
            {
                curr = cursor1.GetCurrentNode();
                Assert.IsNotNull(curr?.Value);
                cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);
            }

            // read from beginning with a new cursor
            var cursor2 = _dict.GetCursor();
            var cursor2Read = new Dictionary<int, int>();
            while (cursor2.MoveNext())
            {
                curr = cursor2.GetCurrentNode();
                Assert.IsNotNull(curr?.Value);
                cursor2Read.AddOrUpdate(curr.Value, _dict[curr.Value]);
            }

            // Check we got the same
            foreach (var pair in cursor1Read)
            {
                Assert.IsTrue(cursor2Read.ContainsKey(pair.Key));
                Assert.AreEqual(cursor2Read[pair.Key], pair.Value);
            }
        }

        [TestMethod]
        public void ReadAllThenModifyLastNode()
        {
            var cursor1 = _dict.GetCursor();

            var cursor1Read = new Dictionary<int, int>();
            IterableLinkedListNode<int> curr;

            // read all
            while (cursor1.MoveNext())
            {
                curr = cursor1.GetCurrentNode();
                Assert.IsNotNull(curr?.Value);
                cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);
            }

            // update last node
            _dict.AddOrUpdate(9, _dict[2] * 2);

            // read again:
            Assert.IsTrue(cursor1.MoveNext(), "Couldn't read next");
            curr = cursor1.GetCurrentNode();
            Assert.IsNotNull(curr?.Value);
            cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);

            // read from beginning with a new cursor
            var cursor2 = _dict.GetCursor();
            var cursor2Read = new Dictionary<int, int>();
            while (cursor2.MoveNext())
            {
                curr = cursor2.GetCurrentNode();
                Assert.IsNotNull(curr?.Value);
                cursor2Read.AddOrUpdate(curr.Value, _dict[curr.Value]);
            }

            // Check we got the same
            foreach (var pair in cursor1Read)
            {
                Assert.IsTrue(cursor2Read.ContainsKey(pair.Key));
                Assert.AreEqual(cursor2Read[pair.Key], pair.Value);
            }
        }

        [TestMethod]
        public void ReadAddRead()
        {
            var cursor1 = _dict.GetCursor();

            var cursor1Read = new Dictionary<int, int>();
            IterableLinkedListNode<int> curr;

            // read all
            while (cursor1.MoveNext())
            {
                curr = cursor1.GetCurrentNode();
                Assert.IsNotNull(curr?.Value);
                cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);
            }

            // Add more
            for (int i = 0; i < 10; i++)
            {
                var index = 10 + i;
                _dict.AddOrUpdate(index, index * 10);
            }

            // read rest
            while (cursor1.MoveNext())
            {
                curr = cursor1.GetCurrentNode();
                Assert.IsNotNull(curr?.Value);
                cursor1Read.AddOrUpdate(curr.Value, _dict[curr.Value]);
            }

            // read from beginning with a new cursor
            var cursor2 = _dict.GetCursor();
            var cursor2Read = new Dictionary<int, int>();
            while (cursor2.MoveNext())
            {
                curr = cursor2.GetCurrentNode();
                Assert.IsNotNull(curr?.Value);
                cursor2Read.AddOrUpdate(curr.Value, _dict[curr.Value]);
            }

            // Check we got the same
            foreach (var pair in cursor1Read)
            {
                Assert.IsTrue(cursor2Read.ContainsKey(pair.Key));
                Assert.AreEqual(cursor2Read[pair.Key], pair.Value);
            }
        }


        [TestMethod]
        public void CreateFromExistingDict()
        {
            // create new normal dict
            var internalDict = new Dictionary<int, int>();
            // copy the iterable one into it
            var cursor = _dict.GetCursor();
            int i = 0;
            while (cursor.MoveNext())
            {
                var v = cursor.GetCurrent();
                internalDict.AddOrUpdate(i, v);
                i++;
            }

            // create a new iterable from the normal dict
            var dict = new IterableDict<int, int>(internalDict);

            // read all from the new iterable
            var cursor1 = dict.GetCursor();
            var cursor1Read = new Dictionary<int, int>();
            IterableLinkedListNode<int> curr;
            while (cursor1.MoveNext())
            {
                curr = cursor1.GetCurrentNode();
                Assert.IsNotNull(curr?.Value);
                cursor1Read.AddOrUpdate(curr.Value, dict[curr.Value]);
            }

            // Check that we read the same from the new iterable as the normal dict
            foreach (var pair in internalDict)
            {
                Assert.IsTrue(cursor1Read.ContainsKey(pair.Key));
                Assert.AreEqual(cursor1Read[pair.Key], pair.Value);
            }
        }
    }

    static class Extend
    {
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey k, TValue v)
        {
            if (dict.ContainsKey(k))
                dict[k] = v;
            else
                dict.Add(k, v);
        }
    }
}
