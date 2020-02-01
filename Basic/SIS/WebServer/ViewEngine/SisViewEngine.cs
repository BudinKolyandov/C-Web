﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

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
            string[] lines = viewContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            var cSharpCode = new StringBuilder();
            var supportedOperators = new[] { "for", "if", "else" };

            foreach (var line in lines)
            {
                if (line.TrimStart().StartsWith("{") || line.TrimStart().StartsWith("}"))
                {
                    cSharpCode.AppendLine(line);
                }
                else if (supportedOperators.Any(x => line.TrimStart().StartsWith("@"+x)))
                {
                    var atLocation = line.IndexOf("@");
                    var cSharpLine = line.Remove(atLocation, 1);
                    cSharpCode.AppendLine(cSharpLine);
                }
                else
                {
                    if (!line.Contains("@"))
                    {
                        var cSharpLine = $"html.AppendLine(@\"{line.Replace("\"", "\"\"")});";
                        cSharpCode.AppendLine(cSharpLine);
                    }
                    else
                    {
                        var cSharpStringToAppend = $"html.AppendLIne(@\"";
                        while (line.Contains("@"))
                        {
                            var atLocation = line.IndexOf("@");
                            var plainText = line.Substring(0, atLocation);
                            //cSharpStringToAppend += plainText + "\" + " + ... + "@\"";
                        }

                        cSharpStringToAppend += "\");"; 
                        cSharpCode.AppendLine(cSharpStringToAppend);
                    }
                }

            }


            return cSharpCode.ToString();
        }
    }
}