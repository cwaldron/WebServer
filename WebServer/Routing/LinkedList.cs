using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebServer.Routing
{
    internal class Node<T>
    {
        protected Node()
        {
        }

        public Node(T item, LinkedList<T> parent)
        {
            Value = item;
            Parent = parent;
        }

        public LinkedList<T> Parent { get; protected set; }

        public T Value { get; protected set; }

        public Node<T> Next { get; protected set; }
    }

    internal class LinkedList<T> : Node<T>, ICollection<T>
    {
        private Node<T> _head;
        private Node<T> _tail;

        public int Count { get; protected set; }

        public bool IsReadOnly { get; }


        public LinkedList()
        {
            _head = _tail = null;
            Count = 0;
            IsReadOnly = false;
        }

        public LinkedList(IEnumerable<T> collection)
            : this()
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var item in collection)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Iterate through the list.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (var node = _head; node != null; node = node.Next)
            {
                yield return node.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Add item to the end of the list.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            ((LinkedList<T>)_tail).Next = new Node<T>(item, this);
            _tail = _tail.Next;
            _head = _head ?? _tail;
            ++Count;
        }

        public void Clear()
        {
            _head = _tail = null;
            Count = 0;
        }

        public bool Contains(T item)
        {
            return this.Any(x => x.Equals(item));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Rank != 1)
            {
                throw new ArgumentException("Invalid rank");
            }

            if (array.GetLowerBound(0) != 0)
            {
                throw new ArgumentException("Invalid lower bound");
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), @"Index is out of range");
            }

            if (array.Length - arrayIndex < Count)
            {
                throw new ArgumentException("Insufficient space");
            }

            var objectArray = array as object[];
            if (objectArray != null)
            {
                foreach (var item in this)
                {
                    objectArray[arrayIndex++] = item;
                }
            }
        }

        public bool Remove(T item)
        {
            // Find item.
            var tuple = Find(item);

            // Check if the item is found.
            if (tuple.Item1 == null) return false;

            // Item found; adjust nodes.
            var node = tuple.Item1;
            var prev = tuple.Item2;
            if (prev != null)
            {
                ((LinkedList<T>)prev).Next = node.Next;
            }
            else
            {
                _head = node.Next;
            }

            --Count;
            return true;
        }

        #region Helpers

        private Tuple<Node<T>, Node<T>> Find(T item)
        {
            Node<T> prev = null;
            for (var node = _head; node != null; node = node.Next)
            {
                if (node.Value.Equals(item))
                {
                    return new Tuple<Node<T>, Node<T>>(node, prev);
                }
                prev = node;
            }

            return new Tuple<Node<T>, Node<T>>(null, null);
        }

        #endregion
    }
}