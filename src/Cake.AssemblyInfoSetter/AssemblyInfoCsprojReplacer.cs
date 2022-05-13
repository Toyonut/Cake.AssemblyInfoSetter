using Cake.Core;
using Cake.Core.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace Cake.AssemblyInfoSetter
{
    public class AssemblyInfoCsprojReplacer
    {
        public Dictionary<string, string> PropertiesDictionary;
        private string FilePath;
        public XmlDocument XmlCsproj;

        public AssemblyInfoCsprojReplacer(ICakeContext context, FilePath filePath, AssemblyInfoProperties properties)
        {
            FilePath = filePath.MakeAbsolute(context.Environment).ToString();
            XmlCsproj = LoadCsproj(FilePath);
            PropertiesDictionary = ConvertPropertiesToDictionary(properties);
        }

        public Dictionary<string, string> ConvertPropertiesToDictionary(AssemblyInfoProperties properties)
        {
            var propertiesDictionary = new Dictionary<string, string>();

            foreach (var prop in properties.GetType().GetProperties())
            {
                if (prop.GetValue(properties, null) is not null)
                {
                    propertiesDictionary.Add(prop.ToString(), prop.GetValue(properties, null).ToString());
                }
            }

            return propertiesDictionary;
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

        public XmlDocument ReplaceProperties(XmlDocument csproj, Dictionary<string, string> assemblyInfoProperties)
        {
            foreach (var prop in assemblyInfoProperties)
            {
                var propertyRegex = $@"\[assembly:\s+{prop.Key}\(\"".*\""\)\]";
                var replacement = $@"[assembly: {prop.Key}(""{prop.Value}"")]";
            }

            return csproj;
        }

        public string Replace () 
        {
            this.XmlCsproj = ReplaceProperties(this.XmlCsproj, this.PropertiesDictionary);
            var path = SetFileText(this.FilePath, this.XmlCsproj);
            
            return path;
        }

        public string SetFileText(string absoluteFilePath, XmlDocument csproj)
        {
            try 
            {
                csproj.Save(absoluteFilePath);
            }
            catch (System.IO.FileNotFoundException)
            {
                throw;
            }

            return absoluteFilePath;
        }
    }
}
