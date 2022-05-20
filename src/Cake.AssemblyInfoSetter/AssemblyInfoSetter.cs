using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Annotations;
using System.Collections.Concurrent;

namespace Cake.AssemblyInfoSetter
{
	[CakeAliasCategory("AssemblyInfoSetter")]
	public static class AssemblyInfoSetter
	{
		[CakeMethodAlias]
		public static FilePath[] SetAssemblyInfo(this ICakeContext context, string globPattern, AssemblyInfoProperties properties)
		{
			var files = context.Globber.GetFiles(globPattern);

			var results = new ConcurrentBag<FilePath>();

			Parallel.ForEach(files, file =>
			{
				var isCsproj = file.GetExtension().ToString() == ".csproj";
				var isAssemblyInfoCs = file.GetFilename().ToString().ToLower() == "assemblyinfo.cs";

				if (!isCsproj && !isAssemblyInfoCs)
				{
					throw (new FormatException($"file format not supported: {file.FullPath}"));
				}

				if (isAssemblyInfoCs)
				{
					var replacer = new AssemblyInfoCsReplacer(context, file.FullPath, properties);
					var fileReplacementPath = replacer.Replace();
					results.Add(fileReplacementPath);
				}

				if (isCsproj)
				{
					var replacer = new AssemblyInfoCsprojReplacer(context, file.FullPath, properties);
					var fileReplacementPath = replacer.Replace();
					results.Add(fileReplacementPath);
				}
			});

			return results.ToArray();
		}
	}
}
