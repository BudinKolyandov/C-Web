using SIS.HTTP.Common;
using SIS.HTTP.Cookies;
using SIS.HTTP.Cookies.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Extensions;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;
using SIS.HTTP.Responses.Contracts;
using System.Text;

namespace SIS.HTTP.Responses
{
    public class HttpResponse : IHttpResponse
    {
        public HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();
            this.Content = new byte[0];
        }

        public HttpResponse(HttpResponseStatusCode statusCode)
            : this()
        {
            CoreValidator.ThrowIfNull(statusCode, nameof(statusCode));
            this.StatusCode = statusCode;
        }


        public HttpResponseStatusCode StatusCode { get; set; }

        public IHttpHeaderCollection Headers { get; }

        public byte[] Content { get; set; }

        public IHttpCookieCollection Cookies { get; }

        public void AddCookie(HttpCookie cookie)
        {
            CoreValidator.ThrowIfNull(cookie, nameof(cookie));
            this.Cookies.AddCookie(cookie);
        }

        public void AddHeader(HttpHeader header)
        {
            CoreValidator.ThrowIfNull(header, nameof(header));
            this.Headers.AddHeader(header);
        }

        public byte[] GetBytes()
        {
            byte[] httpResponceWithoutBody = Encoding.UTF8.GetBytes(this.ToString());
            byte[] httpResponceWithBody = new byte[httpResponceWithoutBody.Length + this.Content.Length];

            for (int i = 0; i < httpResponceWithoutBody.Length; i++)
            {
                httpResponceWithBody[i] = httpResponceWithoutBody[i];
            }
            for (int i = 0; i < httpResponceWithBody.Length - httpResponceWithoutBody.Length; i++)
            {
                httpResponceWithBody[i + httpResponceWithoutBody.Length] = Content[i];
            }
            return httpResponceWithBody;
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.Append($"{GlobalConstants.HttpOneProtocolFragment}{this.StatusCode.GetStatusLine()}")
                .Append(GlobalConstants.HttpNewLine)
                .Append(this.Headers)
                .Append(GlobalConstants.HttpNewLine);

            if (this.Cookies.HasCookie())
            {
                result.Append(this.Cookies)
                .Append(GlobalConstants.HttpNewLine);
            }
            
                result.Append(GlobalConstants.HttpNewLine);

            return result.ToString();
        }
    }
}
