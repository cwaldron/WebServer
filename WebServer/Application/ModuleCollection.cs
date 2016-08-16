using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace WebServer.Application
{
    internal class ModuleCollection : ICollection<ApplicationModule>
    {
        private readonly ConcurrentDictionary<Type, ApplicationModule> _moduleEntries;

        public int Count => _moduleEntries.Count;

        public bool IsReadOnly => false;

        public ModuleCollection()
        {
            _moduleEntries = new ConcurrentDictionary<Type, ApplicationModule>();
        }

        public IEnumerator<ApplicationModule> GetEnumerator()
        {
            return _moduleEntries.Select(pair => pair.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ApplicationModule item)
        {
            _moduleEntries.AddOrUpdate(item.GetType(), item, (k, v) => item);
        }

        public void Clear()
        {
            _moduleEntries.Clear();
        }

        public bool Contains(ApplicationModule item)
        {
            return _moduleEntries.ContainsKey(item.GetType());
        }

        public void CopyTo(ApplicationModule[] array, int arrayIndex)
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

            foreach (var item in this)
            {
                array[arrayIndex++] = item;
            }
        }

        public bool Remove(ApplicationModule item)
        {
            ApplicationModule removedItem;
            return _moduleEntries.TryRemove(item.GetType(), out removedItem);
        }
    }
}
