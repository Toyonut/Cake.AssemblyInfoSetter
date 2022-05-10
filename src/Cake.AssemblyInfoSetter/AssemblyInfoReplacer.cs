using Cake.Core;
using Cake.Core.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace Cake.AssemblyInfoSetter
{
    public class AssemblyInfoReplacer
    {
        private Dictionary<string, Dictionary<string, string>> replacementsTable = new Dictionary<string, Dictionary<string, string>> {
            {
                "AssemblyFileVersion", new Dictionary<string, string> {
                    {"RegexMatch", @"\[assembly:\s+AssemblyFileVersion\(\""[\d|\.]*\""\)\]"},
                    {"Replacement", "[assembly: AssemblyFileVersion(\"{0}\")]"}
                }
            }
        };

        private Version _version;
        private FilePath _filePath;
        private ICakeContext _context;

        public AssemblyInfoReplacer(ICakeContext context, FilePath filePath, Version version)
        {
            _version = version;
            _filePath = filePath;
            _context = context;
        }

        public FilePath Replace () {
            var absolutePath = _filePath.MakeAbsolute(_context.Environment);
            var fileText = File.ReadAllText(absolutePath.ToString());

            var updatedFile = Regex.Replace(
                fileText, 
                replacementsTable["AssemblyFileVersion"]["RegexMatch"],
                String.Format(replacementsTable["AssemblyFileVersion"]["Replacement"], _version)
            );

            File.WriteAllText(absolutePath.ToString(), updatedFile);

            return _filePath;
        }
    }
}
