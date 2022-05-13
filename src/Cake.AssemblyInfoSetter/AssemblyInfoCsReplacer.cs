using Cake.Core;
using Cake.Core.IO;
using System.Text.RegularExpressions;

namespace Cake.AssemblyInfoSetter
{
    public class AssemblyInfoCsReplacer
    {
        public Dictionary<string, string> PropertiesDictionary;
        private string FilePath;
        public string FileText;

        public AssemblyInfoCsReplacer(ICakeContext context, FilePath filePath, AssemblyInfoProperties properties)
        {
            FilePath = filePath.MakeAbsolute(context.Environment).ToString();
            FileText = GetFileText(FilePath);
            PropertiesDictionary = ConvertPropertiesToDictionary(properties);
        }

        public Dictionary<string, string> ConvertPropertiesToDictionary(AssemblyInfoProperties properties)
        {
            var propertiesDictionary = new Dictionary<string, string>();

            foreach (var prop in properties.GetType().GetProperties())
            {
                var value = prop.GetValue(properties, null);

                if (value is not null)
                {
                    propertiesDictionary.Add(prop.Name, value.ToString()!);
                }
            }

            return propertiesDictionary;
        }

        public string GetFileText(string absoluteFilePath)
        {
            return File.ReadAllText(absoluteFilePath);
        }

        public string ReplaceProperties(string assemblyInfoText, Dictionary<string, string> assemblyInfoProperties)
        {
            foreach (var prop in assemblyInfoProperties)
            {
                var propertyRegex = $@"\[assembly:\s+{prop.Key}\(\"".*\""\)\]";
                var replacement = $@"[assembly: {prop.Key}(""{prop.Value}"")]";

                assemblyInfoText = Regex.Replace(assemblyInfoText, propertyRegex, replacement);
            }

            return assemblyInfoText;
        }

        public FilePath Replace () {
            this.FileText = ReplaceProperties(this.FileText, this.PropertiesDictionary);
            SetFileText(this.FilePath, this.FileText);
            
            return this.FilePath;
        }

        public string SetFileText(string absoluteFilePath, string FileText)
        {
            File.WriteAllText(FileText, FileText);
            return absoluteFilePath;
        }
    }
}
