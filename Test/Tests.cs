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
        public void TestModifyNextNode()
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
        public void TestModifyCurrentNode()
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
        public void TestModifyAlreadyReadNodes()
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
        public void TestModifyNextCurrentAndPreviousNodes()
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
        public void TestModifyAll()
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
        public void TestModifyLastNodeThenCursorShouldReReadIt()
        {
            var cursor1 = _dict.GetCursor();

            var cursor1Read = new Dictionary<int, int>();
            IterableLinkedListNode<int> curr;

            // read the rest
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
