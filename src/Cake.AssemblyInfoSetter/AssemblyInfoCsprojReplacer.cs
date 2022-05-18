using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using System.Xml;

namespace Cake.AssemblyInfoSetter
{
	public class AssemblyInfoCsprojReplacer
	{
		public Dictionary<string, string> PropertiesDictionary;
		private string FilePath;
		public XmlDocument XmlCsproj;
		public ICakeContext Context;

		public AssemblyInfoCsprojReplacer(ICakeContext context, string filePath, AssemblyInfoProperties properties)
		{
			Context = context;
			FilePath = filePath;
			XmlCsproj = LoadCsproj(FilePath);
			PropertiesDictionary = properties.ConvertToDictionary();
		}

		public XmlDocument LoadCsproj(string absoluteFilePath)
		{
			var doc = new XmlDocument();

			try
			{
				doc.Load(absoluteFilePath);
			}
			catch (System.IO.FileNotFoundException)
			{
				throw;
			}

			return doc;
		}

		public XmlDocument ReplaceProperties(XmlDocument csproj, Dictionary<string, string> assemblyInfoPropertiesDict)
		{
			var propertyGroup = csproj.SelectSingleNode("Project/PropertyGroup");

			if (propertyGroup != null)
			{
				foreach (var prop in assemblyInfoPropertiesDict)
				{
					var el = propertyGroup.SelectSingleNode(prop.Key);

					if (el == null)
					{
						var at = csproj.CreateElement(prop.Key);
						at.InnerText = prop.Value;

						propertyGroup.AppendChild(at);
					}
					else
					{
						el.InnerText = prop.Value;
					}
				}
			}
			else
			{
				throw (new XmlException("node ProjectGroup not found, check your csproj format."));
			}

			return csproj;
		}

		public string Replace()
		{
			this.XmlCsproj = ReplaceProperties(this.XmlCsproj, this.PropertiesDictionary);
			var path = SetFileText(this.FilePath, this.XmlCsproj);

			return path;
		}

		public string SetFileText(string absoluteFilePath, XmlDocument csproj)
		{
			var tries = 5;
			while (true)
			{
				try
				{
					csproj.Save(absoluteFilePath);
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
