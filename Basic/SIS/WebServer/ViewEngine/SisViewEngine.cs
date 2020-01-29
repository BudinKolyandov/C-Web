using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

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
    public class AppViewCode : IView
    {{
        public string GetHtml()
        {{
            var html = new StringBuilder();
            html.Append(""Hello from memory!!!"");

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
            var compilation = CSharpCompilation.Create("AppViewAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference
                .CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference
                .CreateFromFile(typeof(IView).Assembly.Location));

            var netStandardAssembly = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();

            foreach (var assembly in netStandardAssembly)
            {
                compilation = compilation.AddReferences(MetadataReference.CreateFromFile(Assembly.Load(assembly).Location));
            }

            compilation = compilation.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(code));

            using (var memoryStream = new MemoryStream())
            {
                var compilationResult = compilation.Emit(memoryStream);
                if (!compilationResult.Success)
                {
                    foreach (var error in compilationResult.Diagnostics)
                    {
                        Console.WriteLine(error.GetMessage());
                    }
                    return null;
                }
                memoryStream.Seek(0, SeekOrigin.Begin);
                var assemblyBytes = memoryStream.ToArray();
                var assembly = Assembly.Load(assemblyBytes);

                var type = assembly.GetType("AppViewCodeNamespace.AppViewCode");
                if (type == null)
                {
                    Console.WriteLine("AppViewCore not found!");
                    return null;
                }

                var instance = Activator.CreateInstance(type);
                if (instance == null)
                {
                    Console.WriteLine("AppViewCore can't be instanciated!");
                    return null;
                }

                return instance as IView;
            }
            
        }

        private string GetCsharpCode(string viewContent)
        {
            return null;
        }
    }
}
