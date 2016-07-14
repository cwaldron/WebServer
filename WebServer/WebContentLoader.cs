using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

using WebServer.Content;

namespace WebServer
{
    public class WebContentLoader
    {
        #region Private Members

        private Dictionary<string, Tuple<string, IContentLoader>> _contentLoaders;

        #endregion

        #region Constructors

        /// <summary>
        /// WebContentLoader constructor.
        /// </summary>
        public WebContentLoader()
        {
            _contentLoaders = new Dictionary<string, Tuple<string, IContentLoader>>
            {
                {string.Empty, new Tuple<string, IContentLoader>("text/html", new PageLoader())},
                {"html", new Tuple<string, IContentLoader>("text/html", new PageLoader())},
                {"js", new Tuple<string, IContentLoader>("text/javascript", new FileLoader())},
                {"css", new Tuple<string, IContentLoader>("text/css", new FileLoader())},
                {"json", new Tuple<string, IContentLoader>("text/json", new FileLoader())},
                {"png", new Tuple<string, IContentLoader>("image/png", new ImageLoader())},
                {"jpg", new Tuple<string, IContentLoader>("image/jpg", new ImageLoader())},
                {"gif", new Tuple<string, IContentLoader>("image/gif", new ImageLoader())},
                {"ico", new Tuple<string, IContentLoader>("image/ico", new ImageLoader())},
                {"bmp", new Tuple<string, IContentLoader>("image/bmp", new ImageLoader())},
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add or update a loader to the registry.
        /// </summary>
        /// <param name="contentKey">content key (content file extension)</param>
        /// <param name="contentType"></param>
        /// <param name="loader"></param>
        public void AddLoader(string contentKey, string contentType, IContentLoader loader)
        {
            if (string.IsNullOrWhiteSpace(contentKey))
            {
                throw new ArgumentNullException(nameof(contentKey));
            }

            if (string.IsNullOrWhiteSpace(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (loader == null)
            {
                throw new ArgumentNullException(nameof(loader));
            }

            if (_contentLoaders.ContainsKey(contentKey))
            {
                _contentLoaders[contentKey] = new Tuple<string, IContentLoader>(contentType, loader);
            }
            else
            {
                _contentLoaders.Add(contentKey, new Tuple<string, IContentLoader>(contentType, loader));
            }
        }

        /// <summary>
        /// Obtains the content type for the content key.
        /// </summary>
        /// <param name="contentKey">content key (content file extension)</param>
        /// <returns></returns>
        public string FindContentType(string contentKey)
        {
            Tuple<string, IContentLoader> contentPair;
            return _contentLoaders.TryGetValue(contentKey, out contentPair) ? contentPair.Item1 : null;
        }

        /// <summary>
        /// Load the content.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="pathName"></param>
        /// <returns>content</returns>
        public Stream Load(HttpListenerContext content, string pathName)
        {
            var contentKey = Path.GetExtension(pathName);
            Tuple<string, IContentLoader> contentPair;
            if (contentKey != null && _contentLoaders.TryGetValue(contentKey, out contentPair))
            {
                return contentPair.Item2.LoadContent(content, pathName, contentKey);
            }

            return null;
        }

        #endregion
    }
}
