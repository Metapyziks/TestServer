using System;
using System.Linq;
using WebServer;

namespace TestServer
{
	class Program
	{
		static int Main(string[] args)
		{
			string path = null;
			var port = "80";
			
			DefaultResourceServlet.EnableCaching = true;
			
			for (var i = 0; i < args.Length; ++i) {
				switch (args[i]) {
					case "--tsc":
						TypeScriptProject.TypeScriptCompilerPath = args[++i];
						break;
					case "--port":
						port = args[++i];
						break;
					case "--no-cache":
						DefaultResourceServlet.EnableCaching = false;
						break;
					default:
						if (path == null) path = args[i];
						break;
				}
			}
			
			if (string.IsNullOrWhiteSpace(path)) {
				Console.WriteLine("Expected path to directory containing content to serve.");
				return 1;
			}
			
			DefaultResourceServlet.ResourceDirectory = path;
			
			var server = new Server {
				ResourceRootUrl = "/"
			};
			
			try {
				TypeScriptProject.CompileAllProjects(path, true);
			} catch (Exception e) {
				Console.WriteLine("An error occurred while compiling TypeScript projects:{0}{1}", Environment.NewLine, e);
			}
			
			Console.WriteLine("Starting server...");
			
			server.AddPrefix(string.Format("http://+:{0}/", port));
			server.Run();
			
			return 0;
		}
	}
}
