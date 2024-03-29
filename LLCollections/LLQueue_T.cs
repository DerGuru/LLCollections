﻿namespace System.Collections.Generic
{
    public class LLQueue<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
    {
        private readonly LinkedList<T> _storage;
        private readonly object _syncRoot = new object();

        public LLQueue()
        {
            _storage = new LinkedList<T>();
        }

        public LLQueue(IEnumerable<T> collection)
        {
            _storage = new LinkedList<T>(collection);
        }

        protected LLQueue(LLQueue<T> queue)
        {
            _storage = queue._storage;
            _syncRoot = queue._syncRoot;
        }

        [Obsolete("For compatibility purposes only! Use the empty constructor instead")]
        public LLQueue(int capacity) : this()
        {

        }

        public int Count => _storage.Count;

        public bool IsSynchronized { get; } = false;

        public object SyncRoot => _syncRoot;

        public void CopyTo(T[] array, int arrayIndex) => _storage.CopyTo(array, arrayIndex);

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Rank != 1)
            {
                throw new ArgumentException("Ramk of array is > 1");
            }

            if (array.GetLowerBound(0) != 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            int arrayLen = array.Length;
            if (index < 0 || index > arrayLen)
            {
                throw new ArgumentOutOfRangeException(paramName: nameof(index), index, "Öut of Range");
            }


            int numToCopy = (arrayLen - index < _storage.Count) ? arrayLen - index : _storage.Count;
            if (numToCopy == 0) return;

            try
            {
                Array.Copy(_storage.ToArray(), 0, array, index, numToCopy);
            }
            catch (ArrayTypeMismatchException ex)
            {
                throw new ArgumentException($"Types of {typeof(T).FullName} and {array.GetType().FullName} do not match", ex);
            }
        }

        public void Clear() => _storage.Clear();
        //
        // Summary:
        //     Determines whether an element is in the System.Collections.Generic.Queue`1.
        //
        // Parameters:
        //   item:
        //     The object to locate in the System.Collections.Generic.Queue`1. The value can
        //     be null for reference types.
        //
        // Returns:
        //     true if item is found in the System.Collections.Generic.Queue`1; otherwise, false.
        public bool Contains(T item) => _storage.Contains(item);
        //
        // Summary:
        //     Removes and returns the object at the beginning of the System.Collections.Generic.Queue`1.
        //
        // Returns:
        //     The object that is removed from the beginning of the System.Collections.Generic.Queue`1.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     The System.Collections.Generic.Queue`1 is empty.
        public T Dequeue()
        {
            if (!_storage.Any())
            {
                throw new InvalidOperationException("Queue is empty.");
            }

            var item = _storage.First();
            _storage.RemoveFirst();
            return item;
        }

        //
        // Summary:
        //     Adds an object to the end of the System.Collections.Generic.Queue`1.
        //
        // Parameters:
        //   item:
        //     The object to add to the System.Collections.Generic.Queue`1. The value can be
        //     null for reference types.
        public void Enqueue(T item) => _storage.AddLast(item);

        //
        // Summary:
        //     Ensures that the capacity of this queue is at least the specified capacity. If
        //     the current capacity is less than capacity, it is successively increased to twice
        //     the current capacity until it is at least the specified capacity.
        //
        // Parameters:
        //   capacity:
        //     The minimum capacity to ensure.
        //
        // Returns:
        //     The new capacity of this queue.
        public int EnsureCapacity(int capacity) => capacity;

        //
        // Summary:
        //     Returns the object at the beginning of the System.Collections.Generic.Queue`1
        //     without removing it.
        //
        // Returns:
        //     The object at the beginning of the System.Collections.Generic.Queue`1.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     The System.Collections.Generic.Queue`1 is empty.
        public T Peek()
        {
            if (!_storage.Any())
                throw new InvalidOperationException("Queue is empty.");

            return _storage.First();
        }

        //
        // Summary:
        //     Sets the capacity to the actual number of elements in the System.Collections.Generic.Queue`1,
        //     if that number is less than 90 percent of current capacity.
        public void TrimExcess() { }

        //
        // Summary:
        //     Removes the object at the beginning of the System.Collections.Generic.Queue`1,
        //     and copies it to the result parameter.
        //
        // Parameters:
        //   result:
        //     The removed object.
        //
        // Returns:
        //     true if the object is successfully removed; false if the System.Collections.Generic.Queue`1
        //     is empty.
        public bool TryDequeue([Diagnostics.CodeAnalysis.MaybeNullWhen(false)] out T result)
        {
            try
            {
                result = Dequeue();
                return true;
            }
            catch (InvalidOperationException)
            {
                result = default(T);
                return false;
            }
        }

        //
        // Summary:
        //     Returns a value that indicates whether there is an object at the beginning of
        //     the System.Collections.Generic.Queue`1, and if one is present, copies it to the
        //     result parameter. The object is not removed from the System.Collections.Generic.Queue`1.
        //
        // Parameters:
        //   result:
        //     If present, the object at the beginning of the System.Collections.Generic.Queue`1;
        //     otherwise, the default value of T.
        //
        // Returns:
        //     true if there is an object at the beginning of the System.Collections.Generic.Queue`1;
        //     false if the System.Collections.Generic.Queue`1 is empty.
        public bool TryPeek([Diagnostics.CodeAnalysis.MaybeNullWhen(false)] out T result)
        {
            try
            {
                result = Peek();
                return true;
            }
            catch (InvalidOperationException)
            {
                result = default(T);
                return false;
            }
        }

        //
        // Summary:
        //     Returns an enumerator that iterates through the System.Collections.Generic.Queue`1.
        //
        // Returns:
        //     An System.Collections.Generic.Queue`1.Enumerator for the System.Collections.Generic.Queue`1.
        public virtual IEnumerator<T> GetEnumerator() => _storage.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public LLQueueForEachDequeue<T> AsForeachDequeue() => new LLQueueForEachDequeue<T>(this);


    }

    public class LLQueueForEachDequeue<T> : LLQueue<T>
    {
        public LLQueueForEachDequeue(LLQueue<T> queue) : base(queue)
        {
        }


        public class LLQueueForEachDequeueEnumerator : IEnumerator<T>
        {
            private readonly LLQueue<T> queue;
            
            public LLQueueForEachDequeueEnumerator(LLQueue<T> queue)
            {
                this.queue = queue;
            }


            private T _current = default(T);
            public T Current => _current;


            object IEnumerator.Current => _current;


            public void Dispose()
            {
            }

            public bool MoveNext() => queue.TryDequeue(out _current);


            public void Reset()
            {

            }
        }
        public override IEnumerator<T> GetEnumerator() => new LLQueueForEachDequeueEnumerator(this);

    }
}