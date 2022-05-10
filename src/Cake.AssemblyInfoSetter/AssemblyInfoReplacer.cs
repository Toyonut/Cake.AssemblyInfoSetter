using Cake.Core;
using Cake.Core.IO;
using System.Text.RegularExpressions;

namespace Cake.AssemblyInfoSetter
{
    public class AssemblyInfoReplacer
    {
        private Dictionary<string, Dictionary<string, string>> replacementsTable = new Dictionary<string, Dictionary<string, string>> {
            {
                "AssemblyFileVersion", new Dictionary<string, string> 
                {
                    {"RegexMatch", @"\[assembly:\s+AssemblyFileVersion\(\""[\d|\.]*\""\)\]"},
                    {"Replacement", "[assembly: AssemblyFileVersion(\"{0}\")]"}
                }
            }
        };

        private ICakeContext _context;
        private FilePath _filePath;
        private string _assemblyInfoField;
        private string _replacementValue;

        public AssemblyInfoReplacer(ICakeContext context, FilePath filePath, string assemblyInfoField, string replacementValue)
        {
            if (replacementsTable.ContainsKey(assemblyInfoField)) 
            {
                _assemblyInfoField = assemblyInfoField;
            }
            else
            {
                throw new ArgumentException($"AssemblyInfo field {assemblyInfoField} is not valid.");
            }

            _context = context;
            _filePath = filePath;
            _replacementValue = replacementValue;
        }

        public FilePath Replace () 
        {
            var absolutePath = _filePath.MakeAbsolute(_context.Environment);
            var fileText = File.ReadAllText(absolutePath.ToString());

            var updatedFile = Regex.Replace(
                fileText, 
                replacementsTable[_assemblyInfoField]["RegexMatch"],
                String.Format(replacementsTable[_assemblyInfoField]["Replacement"], _replacementValue)
            );

            File.WriteAllText(absolutePath.ToString(), updatedFile);

            return _filePath;
        }
    }
}
