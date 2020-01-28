using System;

namespace SIS.MvcFramework.ViewEngine
{
    public class SisViewEngine : IviewEngine
    {
        public string GetHtml<T>(string viewContent, T model)
        {
            string csharpHtmlCode = GetCsharpCode(viewContent);

            string code = $@"
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using SIS.MvcFramework.ViewEngine;

namespace AppViewCodeNamespace
{{
    public class AppVIewCode : IView
    {{
        public string GetHtml()
        {{
            var html = new StringBuilder();
            
            {csharpHtmlCode}

            return html.ToString();
        }}
    }}
}}
";

            var view = CompileAndInstance(code);
            var htmlResult = view?.GetHtml();
            return htmlResult;
        }

        private IView CompileAndInstance(string code)
        {
            throw new NotImplementedException();
        }

        private string GetCsharpCode(string viewContent)
        {
            throw new NotImplementedException();
        }
    }
}
