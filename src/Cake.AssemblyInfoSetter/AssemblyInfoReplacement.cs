using System.Text.RegularExpressions;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.AssemblyInfoSetter
{
    public class AssemblyInfoReplacement
    {
        private ICakeContext _context;
        private FilePath _filePath;
        private Version _version;
        private string _regex;
        private string _replacement;

        public AssemblyInfoReplacement(ICakeContext context,FilePath filePath, Version version, string regex, string replacement)
        {
            _context = context;
            _filePath = filePath;
            _version = version;
            _regex = regex;
            _replacement = replacement;
        }

        public void replace () {
            var absolutePath = _filePath.MakeAbsolute(_context.Environment);
            var fileText = File.ReadAllText(absolutePath.ToString());

            var updatedFile = Regex.Replace(fileText, _regex, _replacement);

            File.WriteAllText(absolutePath.ToString(), updatedFile);
        }
    }
}