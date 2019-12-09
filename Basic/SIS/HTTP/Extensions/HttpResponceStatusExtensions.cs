using SIS.HTTP.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Extensions
{
    public static class HttpResponceStatusExtensions
    {
        public static string GetStatusLine(this HttpResponseStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpResponseStatusCode.Ok:return "200 OK";
                case HttpResponseStatusCode.Created:return "201 Created";
                case HttpResponseStatusCode.Found: return "302 Found";
                case HttpResponseStatusCode.SeeOther: return "303 See Other";
                case HttpResponseStatusCode.BadReqest: return "400 Bad Reqest";
                case HttpResponseStatusCode.Forbiden: return "403 Forbiden";
                case HttpResponseStatusCode.NotFound: return "404 Not Found";
                case HttpResponseStatusCode.InternalServerError: return "500 Internal Server Error";
            }
            return null;
        }
    }
}
