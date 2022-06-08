using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class QueueTests
    {
        [TestMethod]
        public void IsSynchronizedIsFalse()
        {
            var llq = new LLQueue<int>();
            Assert.IsFalse(llq.IsSynchronized);
        }

        [TestMethod]
        public void TwoQueuesHaveDifferentSynchRoots()
        {
            var llq1 = new LLQueue<int>();
            var llq2 = new LLQueue<int>();
            Assert.AreNotEqual(llq1.SyncRoot, llq2.SyncRoot);
        }

        [TestMethod]
        public void EnsureCapacityAlwaysGivesBackInputValue()
        {
            var llq = new LLQueue<int>();
            var count = new Random().Next(10, 100);
            for (int i = 0; i < count; i++)
            {
                var input = new Random(DateTime.Now.Millisecond).Next();
                Assert.AreEqual(input, llq.EnsureCapacity(input));
            }

        }

        [TestMethod]
        public void EnqueueAddsAnItem()
        {
            var llq = new LLQueue<int>();
            Assert.AreEqual(0, llq.Count);

            llq.Enqueue(1);

            Assert.AreEqual(1, llq.Count);
        }

        [TestMethod]
        public void DequeueSubtractsAnItem()
        {
            var llq = new LLQueue<int>(new int[] { 1 });
            Assert.AreEqual(1, llq.Count);
            _ = llq.Dequeue();

            Assert.AreEqual(0, llq.Count);

        }

        [TestMethod]
        public void DequeuedItemIsAlwaysTheFirst()
        {
            var llq = new LLQueue<int>(new int[] { 1, 2, 3 });
            Assert.AreEqual(3, llq.Count);
            int item = 0;

            item = llq.Dequeue();
            Assert.AreEqual(2, llq.Count);
            Assert.AreEqual(1, item);

            item = llq.Dequeue();
            Assert.AreEqual(1, llq.Count);
            Assert.AreEqual(2, item);

            item = llq.Dequeue();
            Assert.AreEqual(0, llq.Count);
            Assert.AreEqual(3, item);
        }

        [TestMethod]
        public void PeekDoesNotChangeTheQueue()
        {
            var llq = new LLQueue<int>(new int[] { 10 });
            Assert.AreEqual(1, llq.Count);
            var item = llq.Peek();

            Assert.AreEqual(10, item);
            Assert.AreEqual(1, llq.Count);

            item = llq.Peek();

            Assert.AreEqual(10, item);
            Assert.AreEqual(1, llq.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EmptyQueueThrowsOnDequeue()
        {
            var llq = new LLQueue<int>();
            _ = llq.Dequeue();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EmptyQueueThrowsOnPeek()
        {
            var llq = new LLQueue<int>();
            _ = llq.Peek();
        }

        [TestMethod]
        public void ContainsFindsItem()
        {
            var llq = new LLQueue<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.IsTrue(llq.Contains(5));

            var toFind = new object();
            var ollq = new LLQueue<object>(new object[] { toFind });
            Assert.IsTrue(ollq.Contains(toFind));

        }

        [TestMethod]
        public void ClearEmptiesTheQueue()
        {
            var llq = new LLQueue<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.IsTrue(llq.Contains(5));
            llq.Clear();

            Assert.AreEqual(0, llq.Count);

        }

        [TestMethod]
        public void ForeachDoesNotChangeToQueue()
        {
            var reference = new int[] { 1, 2, 3, 4, 5, 6 };
            var llq = new LLQueue<int>(reference);

            int refCount = 0;
            foreach (var item in llq)
            {
                Assert.AreEqual(reference[refCount], item);
                refCount++;
            }

            Assert.AreEqual(6, llq.Count);

        }

        [TestMethod]
        public void CopyToGenericArray()
        {
            var reference = new int[] { 1, 2, 3, 4, 5, 6 };
            var llq = new LLQueue<int>(reference);

            var arr = new int[6];

            llq.CopyTo(arr, 0);

            int i = 0;
            foreach (var item in llq)
            {
                Assert.AreEqual(arr[i], item);
                i++;
            }
        }

        [TestMethod]
        public void CopyToArray()
        {
            var reference = new int[] { 1, 2, 3, 4, 5, 6 };
            var llq = new LLQueue<int>(reference);

            var arr = new int[6] as Array;
            (llq as System.Collections.ICollection).CopyTo(arr, 0);

            int i = 0;
            foreach (var item in (llq as System.Collections.IEnumerable))
            {
                Assert.AreEqual(arr.GetValue(i), item);
                i++;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyToArrayThrowsOnNullArray()
        {
            var reference = new int[] { 1, 2, 3, 4, 5, 6 };
            var llq = new LLQueue<int>(reference);

            var arr = null as Array;
            (llq as System.Collections.ICollection).CopyTo(arr, 0);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CopyToArrayThrowsOnRankGreater1()
        {
            var reference = new int[] { 1, 2, 3, 4, 5, 6 };
            var llq = new LLQueue<int>(reference);

            var arr = new int[0, 0] as Array;
            (llq as System.Collections.ICollection).CopyTo(arr, 0);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CopyToArrayThrowsOnIndexLessThan0()
        {
            var reference = new int[] { 1, 2, 3, 4, 5, 6 };
            var llq = new LLQueue<int>(reference);

            var arr = new int[1] as Array;
            (llq as System.Collections.ICollection).CopyTo(arr, -1);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CopyToArrayThrowsOnIndexGreaterThanArrayLength()
        {
            var reference = new int[] { 1, 2, 3, 4, 5, 6 };
            var llq = new LLQueue<int>(reference);

            var arr = new int[1] as Array;
            (llq as System.Collections.ICollection).CopyTo(arr, 2);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CopyToArrayThrowsOnWrongType()
        {
            var reference = new int[] { 1, 2, 3, 4, 5, 6 };
            var llq = new LLQueue<int>(reference);

            var arr = new string[1] as Array;
            (llq as System.Collections.ICollection).CopyTo(arr, 0);

        }

        [TestMethod]
        public void AsForeachConsumesQueue()
        {
            var reference = new int[] { 1, 2 };
            var llq = new LLQueue<int>(reference.ToArray());

            int refCount = 0;
            var llqc = llq.AsForeachDequeue();
            foreach (var item in llqc)
            {
                Assert.AreEqual(reference[refCount], item);
                refCount++;
            }

            Assert.AreEqual(0, llq.Count);

        }
    }
}