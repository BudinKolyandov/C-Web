using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using System.Text;

namespace SIS.MvcFramework.Result
{
    public class TextResult : ActionResult
    {
        public TextResult(string content, HttpResponseStatusCode responseStatusCode, string contentType = "text/plain; charset=utf-8")
            : base(responseStatusCode)
        {
            Headers.AddHeader(new HttpHeader("Content-Type", contentType));
            Content = Encoding.UTF8.GetBytes(content);
        }

        public TextResult(byte[] content, HttpResponseStatusCode responseStatusCode, string contentType = "text/plain; charset=utf-8")
            : base(responseStatusCode)
        {
            Content = content;
            Headers.AddHeader(new HttpHeader("Content-Type", contentType));
        }
    }
}
