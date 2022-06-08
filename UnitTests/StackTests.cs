using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class StackTests
    {
        [TestMethod]
        public void IsSynchronizedIsFalse()
        {
            var lls = new LLStack<int>();
            Assert.IsFalse(lls.IsSynchronized);
        }

        [TestMethod]
        public void TwoStacksHaveDifferentSynchRoots()
        {
            var lls1 = new LLStack<int>();
            var lls2 = new LLStack<int>();
            Assert.AreNotEqual(lls1.SyncRoot,lls2.SyncRoot);
        }

        [TestMethod]
        public void EnsureCapacityAlwaysGivesBackInputValue()
        {
            var lls = new LLStack<int>();
            var count = new Random().Next(10, 100);
            for (int i = 0; i < count; i++)
            {
                var input = new Random(DateTime.Now.Millisecond).Next();
                Assert.AreEqual(input, lls.EnsureCapacity(input));
            }

        }

        [TestMethod]
        public void PushAddsAnItem()
        {
            var lls = new LLStack<int>();
            Assert.AreEqual(0, lls.Count);
            
            lls.Push(1);

            Assert.AreEqual( 1, lls.Count); 
        }

        [TestMethod]
        public void PopSubtractsAnItem()
        {
            var lls = new LLStack<int>(new int[] {1});
            Assert.AreEqual(1, lls.Count);
            _ = lls.Pop();

            Assert.AreEqual(0, lls.Count);

        }

        [TestMethod]
        public void PopdItemIsAlwaysTheFirst()
        {
            var lls = new LLStack<int>(new int[] { 1,2,3 });
            Assert.AreEqual(3, lls.Count);
            int item = 0;

            item = lls.Pop();
            Assert.AreEqual(2, lls.Count);
            Assert.AreEqual(3, item);

            item = lls.Pop();
            Assert.AreEqual(1, lls.Count);
            Assert.AreEqual(2, item);

            item = lls.Pop();
            Assert.AreEqual(0, lls.Count);
            Assert.AreEqual(1, item);
        }

        [TestMethod]
        public void PeekDoesNotChangeTheStack()
        {
            var lls = new LLStack<int>(new int[] { 10 });
            Assert.AreEqual(1, lls.Count);
            var item = lls.Peek();

            Assert.AreEqual(10, item);
            Assert.AreEqual(1, lls.Count);

            item = lls.Peek();

            Assert.AreEqual(10, item);
            Assert.AreEqual(1, lls.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EmptyStackThrowsOnPop()
        {
            var lls = new LLStack<int>();
            _ = lls.Pop();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EmptyStackThrowsOnPeek()
        {
            var lls = new LLStack<int>();
            _ = lls.Peek();
        }

        [TestMethod]
        public void ContainsFindsItem()
        {
            var lls = new LLStack<int>(new int[]{1,2,3,4,5,6 });
            Assert.IsTrue(lls.Contains(5));

            var toFind = new object();
            var olls = new LLStack<object>(new object[] {toFind});
            Assert.IsTrue(olls.Contains(toFind));

        }

        [TestMethod]
        public void ClearEmptiesTheQueue()
        {
            var lls = new LLStack<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.IsTrue(lls.Contains(5));
            lls.Clear();

            Assert.AreEqual(0, lls.Count);

        }

        [TestMethod]
        public void ForeachDoesNotChangeToQueue()
        {
            var reference = new int[] { 1, 2, 3, 4, 5, 6 };
            var lls = new LLStack<int>(reference.Reverse());
            Assert.IsTrue(lls.Contains(5));

            int refCount = 0;
            foreach (var item in lls)
            {
                Assert.AreEqual(reference[refCount], item);
                refCount++;
            }

            Assert.AreEqual(6, lls.Count);

        }

        [TestMethod]
        public void CopyToGenericArray()
        {
            var reference = new int[] { 1, 2, 3, 4, 5, 6 };
            var lls = new LLStack<int>(reference.Reverse());
            Assert.IsTrue(lls.Contains(5));


            reference = new int[6];

            lls.CopyTo(reference, 0);

            int refCount = 0;
            foreach (var item in lls)
            {
                Assert.AreEqual(reference[refCount], item);
                refCount++;
            }

            Assert.AreEqual(6, lls.Count);

        }

        [TestMethod]
        public void CopyToArray()
        {
            var reference = new int[] { 1, 2, 3, 4, 5, 6 };
            var lls = new LLStack<int>(reference.Reverse());
            Assert.IsTrue(lls.Contains(5));

            var arr = new int[6] as Array;
            (lls as System.Collections.ICollection).CopyTo(arr, 0);

            int refCount = 0;
            foreach (var item in (lls as System.Collections.IEnumerable))
            {
                Assert.AreEqual(arr.GetValue(refCount), item);
                refCount++;
            }

            Assert.AreEqual(6, lls.Count);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyToArrayThrowsOnNullArray()
        {
            var reference = new int[] { 1, 2, 3, 4, 5, 6 };
            var llq = new LLStack<int>(reference.Reverse());

            var arr = null as Array;
            (llq as System.Collections.ICollection).CopyTo(arr, 0);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CopyToArrayThrowsOnRankGreater1()
        {
            var reference = new int[] { 1, 2, 3, 4, 5, 6 };
            var llq = new LLStack<int>(reference.Reverse());

            var arr = new int[0, 0] as Array;
            (llq as System.Collections.ICollection).CopyTo(arr, 0);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CopyToArrayThrowsOnIndexLessThan0()
        {
            var reference = new int[] { 1, 2, 3, 4, 5, 6 };
            var llq = new LLStack<int>(reference.Reverse());

            var arr = new int[1] as Array;
            (llq as System.Collections.ICollection).CopyTo(arr, -1);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CopyToArrayThrowsOnIndexGreaterThanArrayLength()
        {
            var reference = new int[] { 1, 2, 3, 4, 5, 6 };
            var llq = new LLStack<int>(reference.Reverse());

            var arr = new int[1] as Array;
            (llq as System.Collections.ICollection).CopyTo(arr, 2);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CopyToArrayThrowsOnWrongType()
        {
            var reference = new int[] { 1, 2, 3, 4, 5, 6 };
            var llq = new LLStack<int>(reference.Reverse());

            var arr = new string[1] as Array;
            (llq as System.Collections.ICollection).CopyTo(arr, 0);

        }
        [TestMethod]
        public void AsForeachConsumesStack()
        {
            var reference = new int[] { 1, 2 };
            var llq = new LLStack<int>(reference.Reverse());

            int refCount = 0;
            var llqc = llq.AsForeachPop();
            foreach (var item in llqc)
            {
                Assert.AreEqual(reference[refCount], item);
                refCount++;
            }

            Assert.AreEqual(0, llq.Count);

        }
    }
}