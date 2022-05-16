using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Polly;
using System.Xml;

namespace Cake.AssemblyInfoSetter
{
    public class AssemblyInfoCsprojReplacer
    {
        public Dictionary<string, string> PropertiesDictionary;
        private string FilePath;
        public XmlDocument XmlCsproj;
        public ICakeContext Context;

        public AssemblyInfoCsprojReplacer(ICakeContext context, FilePath filePath, AssemblyInfoProperties properties)
        {
            Context = context;
            FilePath = filePath.MakeAbsolute(Context.Environment).ToString();
            XmlCsproj = LoadCsproj(FilePath);
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
            var propertyGroup = csproj.SelectSingleNode("Project/PropertyGroup");            

            if (propertyGroup != null)
            {
                foreach (var prop in PropertiesDictionary)
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
                throw(new XmlException("node ProjectGroup not found, check your csproj format."));
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
            var retryPolicy = Policy
                .Handle<System.IO.FileNotFoundException>()
                .WaitAndRetry(retryCount: 5, sleepDurationProvider: _ => TimeSpan.FromMilliseconds(500), onRetry: (exception, retryCount) =>
                {
                    Context.Log.Verbose($"Retrying save to {absoluteFilePath}");
                });

            retryPolicy.Execute(() => {
                csproj.Save(absoluteFilePath);
            });

            return absoluteFilePath;
        }
    }
}
