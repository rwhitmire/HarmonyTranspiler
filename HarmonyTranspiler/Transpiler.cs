using System.IO;
using System.Reflection;
using MsieJavaScriptEngine;

namespace HarmonyTranspiler
{
    public class Transpiler
    {
        private string _moduleName;

        public Transpiler()
        {
            
        }

        public Transpiler(string moduleName)
        {
            _moduleName = moduleName;
        }

        public string Transpile(string script)
        {
            var engine = new MsieJsEngine();

            engine.Execute("module = {}; exports = {};");

            var transpiler = GetTranspiler();

            engine.Execute(transpiler);

            script = EscapeQuotes(script);
            script = EscapeNewLines(script);

            var code = string.Format("var compiler = new module.exports.Compiler('{0}');", script);
            engine.Execute(code);

            var compiledCode = engine.Evaluate<string>("compiler.toAMD();");
            compiledCode = SetModuleName(compiledCode);

            return compiledCode;
        }

        private string SetModuleName(string code)
        {
            if (string.IsNullOrWhiteSpace(_moduleName)) return code;

            var newDefine = string.Format("define(\"{0}\", ", _moduleName);
            return code.Replace("define(", newDefine);
        }

        private string EscapeNewLines(string script)
        {
            return script.Replace("\r\n", @"\n \ \r\n");
        }

        private static string GetTranspiler()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "HarmonyTranspiler.Resources.es6-module-transpiler.js";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                var result = reader.ReadToEnd();
                return result;
            }
        }

        private static string EscapeQuotes(string script)
        {
            return script
                .Replace("'", "\\'")
                .Replace("\"", "\\\"");
        }
    }
}
