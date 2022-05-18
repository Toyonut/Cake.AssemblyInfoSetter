using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using System.Text.RegularExpressions;

namespace Cake.AssemblyInfoSetter
{
	public class AssemblyInfoCsReplacer
	{
		public Dictionary<string, string> PropertiesDictionary;
		private string FilePath;
		public string FileText;
		public ICakeContext Context;

		public AssemblyInfoCsReplacer(ICakeContext context, FilePath filePath, AssemblyInfoProperties properties)
		{
			Context = context;
			FilePath = filePath.MakeAbsolute(Context.Environment).ToString();
			FileText = GetFileText(FilePath);
			PropertiesDictionary = properties.ConvertToDictionary();
		}

		public string GetFileText(string absoluteFilePath)
		{
			return File.ReadAllText(absoluteFilePath);
		}

		public string ReplaceProperties(string assemblyInfoText, Dictionary<string, string> assemblyInfoProperties)
		{
			foreach (var prop in assemblyInfoProperties)
			{
				var propertyRegex = $@"^\[assembly:\s+{prop.Key}\(\"".*\""\)\]";
				var replacement = $@"[assembly: {prop.Key}(""{prop.Value}"")]";

				assemblyInfoText = Regex.Replace(assemblyInfoText, propertyRegex, replacement, RegexOptions.Multiline);
			}

			return assemblyInfoText;
		}

		public string Replace()
		{
			this.FileText = ReplaceProperties(this.FileText, this.PropertiesDictionary);
			var path = SetFileText(this.FilePath, this.FileText);

			return path;
		}

		public string SetFileText(string absoluteFilePath, string FileText)
		{
			var tries = 5;
			while (true)
			{
				try
				{
					File.WriteAllText(absoluteFilePath, FileText);
					break; // success!
				}
				catch
				{
					if (--tries == 0)
					{
						throw;
					}
					Thread.Sleep(500);
				}
				Context.Log.Verbose($"Retrying save to {absoluteFilePath}");
			}

			return absoluteFilePath;
		}
	}
}
