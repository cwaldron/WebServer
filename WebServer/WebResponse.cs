using System.Text;

namespace WebServer
{
    public class WebResponse
    {
        private byte[] _data;
        private string _text;

        public WebResponse()
        {
            ContentType = "text/html";
            ContentEncoding = Encoding.UTF8;
            Status = 200;
            StatusText = "OK";
        }

        public WebResponse(string text)
            : this()
        {
            ResponseText = text;
        }

        public Encoding ContentEncoding { get; set; }

        public string ContentType { get; set; }

        public int Status { get; set; }

        public string StatusText { get; set; }

        public string ResponseText
        {
            get { return _text; }
            set
            {
                _text = value;
                _data = ContentEncoding.GetBytes(_text);
            }
        }

        public byte[] ResponseData
        {
            get { return _data; }
        }
    }
}
