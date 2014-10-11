using System.IO;
using System.Reflection;
using MsieJavaScriptEngine;

namespace HarmonyTranspiler
{
    public class Transpiler
    {
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

            return engine.Evaluate<string>("compiler.toAMD();");

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
