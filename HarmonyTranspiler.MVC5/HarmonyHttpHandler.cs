using System.IO;
using System.Web;

namespace HarmonyTranspiler.MVC5
{
    public class HarmonyHttpHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var physicalPath = context.Server.MapPath(context.Request.AppRelativeCurrentExecutionFilePath);
            var source = File.ReadAllText(physicalPath);
            var path = context.Request.Path;

            if (path.StartsWith("/Scripts/"))
            {
                path = path.Substring(9);
            }

            var moduleName = path.Replace(".es6", "").Replace(".js", "");
            var transpiler = new Transpiler(moduleName);
            var es6Code = transpiler.Transpile(source);

            context.Response.ContentType = "text/javascript";
            context.Response.Write(es6Code);
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
