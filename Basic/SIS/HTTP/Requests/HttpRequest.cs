using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Exceptions;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;
using SIS.HTTP.Requests.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIS.HTTP.Requests
{
    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));

            this.FormData = new Dictionary<string, List<object>>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();

            this.ParseRequest(requestString);
        }


        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, List<object>> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        private void ParseRequestFormDataParameters(string requestBody)
        {
            var parsedQueryParameterKVPs = requestBody
                .Split('&')
                .Select(queryParameter => queryParameter.Split('='))
                .ToList();

            foreach (var kvp in parsedQueryParameterKVPs)
            {
                if (!this.FormData.ContainsKey(kvp[0]))
                {
                    this.FormData.Add(kvp[0], new List<object>());
                    
                }
                this.FormData[kvp[0]].Add(kvp[1]);
            }
        }

        private void ParseQueryParameters()
        {
            this.Url.Split(new[] { '?', '#' })[1]
                .Split('&')
                .Select(queryParameter => queryParameter.Split('='))
                .ToList()
                .ForEach(parsedQueryParameterKVP => 
                this.QueryData.Add(parsedQueryParameterKVP[0], parsedQueryParameterKVP[1]));
        }

        private void ParseRequestParameters(string requestBody)
        {
            this.ParseQueryParameters();
            this.ParseRequestFormDataParameters(requestBody);
        }

        private void ParseRequestHeaders(string[] headersParams)
        {
            headersParams.Select(headersParam => headersParam.Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                .ToList()
                .ForEach(headerkvp => this.Headers.AddHeader(new HttpHeader(headerkvp[0], headerkvp[1])));
        }

        private void ParseCookies()
        {
            throw new NotImplementedException();
        }

        private void ParseRequestPath()
        {
            this.Path = this.Url.Split('?')[0];
        }

        private void ParseRequestUrl(string[] requestLine)
        {
            this.Url = requestLine[1];
        }

        private void ParseRequestMethod(string[] requestLine)
        {
            var parseResult = HttpRequestMethod
                .TryParse(requestLine[0], true, out HttpRequestMethod method);
            if (!parseResult)
            {
                throw new BadRequestException(string.Format(GlobalConstants.UnsupportedHttpMethodExceptionMessage, requestLine[0]));
            }

            this.RequestMethod = method;
        }

        private bool IsValidRequestQueryString(string[] requestLine)
        {
            throw new NotImplementedException();
        }

        private bool IsValidRequestLine(string[] requestLine)
        {
            if (requestLine.Length != 3 
                || requestLine[2] != GlobalConstants.HttpOneProtocolFragment)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private IEnumerable<string> ParsePlainRequestHeader(string[] requestLines)
        {
            for (int i = 1; i < requestLines.Length - 1; i++)
            {
                if (!string.IsNullOrEmpty(requestLines[i]))
                {
                    yield return requestLines[i];
                }
            }
        }

        private void ParseRequest(string requestString)
        {
            var splitRequestString = requestString
                .Split(new[] { GlobalConstants.HttpNewLine }, StringSplitOptions.None);

            var requestLine = splitRequestString[0].Trim()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLine))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();

            this.ParseRequestHeaders(this.ParsePlainRequestHeader(splitRequestString).ToArray());
            //this.ParseCookies();

            this.ParseRequestParameters(splitRequestString[splitRequestString.Length - 1]);
        }
    }
}
