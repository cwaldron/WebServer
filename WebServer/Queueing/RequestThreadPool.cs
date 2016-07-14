using System;
using System.Collections.Generic;
using System.Threading;

namespace WebServer.Queueing
{
    /// <summary>
    /// Used to queue requests.
    /// </summary>
    internal class RequestThreadPool<T> : IDisposable
    {
        #region Private Members

        private readonly IList<RequestQueue<T>> _requestWorkers;
        private const int DefaultWorkerThreads = 25;
        private readonly int _threadCount;
        private int _threadIndex;
        private bool _disposed;

        #endregion

        #region Automatic Properties

        public int Count => _requestWorkers.Count; 

        #endregion

        #region Constructors

        /// <summary>
        /// RequestQueue constructors.
        /// </summary>
        public RequestThreadPool(int threadCount = DefaultWorkerThreads)
        {
            _requestWorkers = new List<RequestQueue<T>>();
            _threadCount = threadCount;
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Start the treadpool with common action for all thread.
        /// </summary>
        /// <param name="action">common action</param>
        public void Start(Action<RequestQueue<T>> action)
        {
            var operation = new ParameterizedThreadStart(obj => action((RequestQueue<T>)obj));

            for (int ii = 0; ii < _threadCount; ++ii)
            {
                var thread = new Thread(operation) { IsBackground = true };
                var requestQueue = new RequestQueue<T>();
                _requestWorkers.Add(requestQueue);
                thread.Start(requestQueue);
            }
        }

        /// <summary>
        /// Post request into the thread pool.
        /// </summary>
        /// <param name="request">request</param>
        public void PostRequest(T request)
        {
            // Queue request in simple round-round fashion.
            _requestWorkers[_threadIndex++].Enqueue(request);
            _threadIndex %= _threadCount;
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
            foreach (var worker in _requestWorkers)
            {
                worker.Dispose();
            }

            _requestWorkers.Clear();
        }

        #endregion
    }
}
