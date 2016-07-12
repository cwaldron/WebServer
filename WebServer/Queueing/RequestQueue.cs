using System;
using System.Collections.Concurrent;
using System.Threading;

namespace WebServer.Queueing
{
    /// <summary>
    /// Used to queue requests.
    /// </summary>
    internal class RequestQueue<T> : IDisposable
    {
        #region Private Members

        private readonly Semaphore _producer;
        private readonly Semaphore _consumer;
        private readonly ConcurrentQueue<T> _requests;
        private bool _disposed;

        #endregion

        #region Automatic Properties

        /// <summary>
        /// Count of requests in queue.
        /// </summary>
        public int Count => _requests.Count;

        #endregion

        #region Constructors

        /// <summary>
        /// RequestQueue constructors.
        /// </summary>
        public RequestQueue()
        {
            _producer = new Semaphore(0, int.MaxValue);
            _consumer = new Semaphore(int.MaxValue, int.MaxValue);
            _requests = new ConcurrentQueue<T>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Enqueue a request context and release the semaphore that
        /// a thread is waiting on.
        /// </summary>
        public void Enqueue(T item)
        {
            _requests.Enqueue(item);
            _consumer.WaitOne();
            _producer.Release();
        }

        /// <summary>
        /// Dequeue a request.
        /// </summary>
        public T Dequeue()
        {
            _producer.WaitOne();
            _consumer.Release();
            T item;
            _requests.TryDequeue(out item);
            return item;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Disposes the concurrent queue.
        /// </summary>
        public void Dispose()
        {
            Dispose(!_disposed);
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose concurrent queue.
        /// </summary>
        /// <param name="disposing">indicate whether the queue is disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            _producer.Dispose();
            _consumer.Dispose();
        }

        #endregion
    }
}
