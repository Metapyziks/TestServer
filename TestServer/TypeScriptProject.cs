using System;
using System.IO;
using System.Diagnostics;

namespace TestServer
{
	class TypeScriptProject
	{
		public static string TypeScriptCompilerPath { get; set; }
		
		public static void CompileAllProjects(string rootPath, bool watchForChanges)
		{
			if (TypeScriptCompilerPath == null) {
				throw new Exception("No path to tsc specified.");
			}
			
			var configPath = Path.Combine(rootPath, "tsconfig.json");
			if (File.Exists(configPath)) CompileProject(rootPath, watchForChanges);
			
			foreach (var subDir in Directory.GetDirectories(rootPath))
			{
				CompileAllProjects(subDir, watchForChanges);
			}
		}
		
		private static void CompileProject(string path, bool watchForChanges)
		{
			Console.WriteLine("Compiling TypeScript project at '{0}'", path);
			
			var startInfo = new ProcessStartInfo(TypeScriptCompilerPath);
			
			if (watchForChanges) {
				startInfo.Arguments = "--watch";
			}
			
			startInfo.CreateNoWindow = true;
			startInfo.RedirectStandardOutput = true;
			startInfo.UseShellExecute = false;
			startInfo.WorkingDirectory = path;
			
			var process = new Process();
			
			process.StartInfo = startInfo;
			process.EnableRaisingEvents = true;
			
			process.OutputDataReceived += (sender, e) => {
				if (e.Data.Contains("error")) Console.ForegroundColor = ConsoleColor.Red;
				else if (e.Data.Contains("warning")) Console.ForegroundColor = ConsoleColor.Yellow;
				else if (e.Data.Contains("Compilation complete")) Console.ForegroundColor = ConsoleColor.Green;
				
				Console.WriteLine("[tsc] {0}", e.Data);
				Console.ResetColor();
			};
			
			process.Start();
			process.BeginOutputReadLine();
		}
	}
}
