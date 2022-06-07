using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void EnqueueAddsAnItem()
        {
            var llq = new LLQueue<int>();
            Assert.AreEqual(0, llq.Count);
            
            llq.Enqueue(1);

            Assert.AreEqual( 1, llq.Count); 
        }

        [TestMethod]
        public void DequeueSubtractsAnItem()
        {
            var llq = new LLQueue<int>(new int[] {1});
            Assert.AreEqual(1, llq.Count);
            _ = llq.Dequeue();

            Assert.AreEqual(0, llq.Count);

        }

        [TestMethod]
        public void DequeuedItemIsAlwaysTheFirst()
        {
            var llq = new LLQueue<int>(new int[] { 1,2,3 });
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
            var llq = new LLQueue<int>(new int[]{1,2,3,4,5,6 });
            Assert.IsTrue(llq.Contains(5));

            var toFind = new object();
            var ollq = new LLQueue<object>(new object[] {toFind});
            Assert.IsTrue(ollq.Contains(toFind));

        }
    }
}