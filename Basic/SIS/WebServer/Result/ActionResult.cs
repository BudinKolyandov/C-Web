using SIS.HTTP.Enums;
using SIS.HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework.Result
{
    public abstract class ActionResult : HttpResponse
    {
        protected ActionResult(HttpResponseStatusCode httpResponseStatusCode)
            : base(httpResponseStatusCode)
        {

        }

    }
}
