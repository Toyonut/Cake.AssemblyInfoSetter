using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Annotations;
using System.Collections.Concurrent;

namespace Cake.AssemblyInfoSetter
{
	[CakeAliasCategory("AssemblyInfoSetter")]
	public static class AssemblyInfoSetter
	{
		public static string[] AllowedExtensions = new string[] {
			".cs",
			".csproj"
		};

		[CakeMethodAlias]
		public static FilePath[] SetAssemblyInfo(this ICakeContext context, string globPattern, AssemblyInfoProperties properties)
		{
			var files = context.Globber.GetFiles(globPattern);

			var results = new ConcurrentBag<FilePath>();

			Parallel.ForEach(files, file =>
			{
				if (!AllowedExtensions.Contains(file.GetExtension().ToString()))
				{
					throw (new FormatException($"file format not supported: {file.FullPath}"));
				}

				if (file.GetExtension().ToString() == ".cs")
				{
					var replacer = new AssemblyInfoCsReplacer(context, file.FullPath, properties);
					var fileReplacementPath = replacer.Replace();
					results.Add(fileReplacementPath);
				}

				if (file.GetExtension().ToString() == ".csproj")
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
