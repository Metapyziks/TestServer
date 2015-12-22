using System;
using WebServer;

namespace TestServer
{
	class Program
	{
		static int Main(string[] args)
		{
			if (args.Length != 1) {
				Console.WriteLine("Expected path to directory containing content to serve.");
				return 1;
			}
			
			DefaultResourceServlet.ResourceDirectory = args[0];
			
			var server = new Server {
				ResourceRootUrl = "/"
			};
			
			TypeScriptProject.CompileAllProjects(args[0], true);
			
			Console.WriteLine("Starting server...");
			
			server.AddPrefix("http://+:80/");
			server.Run();
			
			return 0;
		}
	}
}
