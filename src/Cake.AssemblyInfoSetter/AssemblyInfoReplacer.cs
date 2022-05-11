using Cake.Core;
using Cake.Core.IO;
using System.Text.RegularExpressions;

namespace Cake.AssemblyInfoSetter
{
    public class AssemblyInfoReplacer
    {
        private ICakeContext _context;
        private FilePath _filePath;
        private AssemblyInfoProperties _properties;

        public AssemblyInfoReplacer(ICakeContext context, FilePath filePath, AssemblyInfoProperties properties)
        {
            _context = context;
            _filePath = filePath;
            _properties = properties;
        }

        public FilePath Replace () 
        {
            var absolutePath = _filePath.MakeAbsolute(_context.Environment);
            var fileText = File.ReadAllText(absolutePath.ToString());

            var setAssemblyProperties = _properties
                                .GetType()
                                .GetProperties()
                                .Where(p => p.GetValue(_properties, null) is not null);

            foreach (var prop in setAssemblyProperties)
            {
                var propertyRegex = @"\[assembly:\s+@@propName\(\""[\d|\.]*\""\)\]".Replace("@@propName", prop.Name);
                var propValue = prop.GetValue(_properties, null);
                var replacement = $"[assembly: {prop.Name}(\"{propValue}\")]";

                fileText = Regex.Replace(fileText, propertyRegex, replacement);
            }

            File.WriteAllText(absolutePath.ToString(), fileText);

            return _filePath;
        }
    }
}
