using Cake.Core;
using Cake.Core.IO;

namespace Cake.AssemblyInfoSetter
{
    public class AssemblyInfoReplacer
    {
        private Version _version;

        private FilePath _filePath;
        private ICakeContext _context;

        public AssemblyInfoReplacement AssemblyFileVersion {get; private set;}

        public AssemblyInfoReplacer(ICakeContext context, Version version, FilePath filePath)
        {
            _version = version;

            _filePath = filePath;

            _context = context;

            AssemblyFileVersion = new AssemblyInfoReplacement (
                context = _context,
                filePath = _filePath,
                version = _version,
                @"\[assembly:\s+AssemblyFileVersion\(\""[\d|\.]*\""\)\]",
                $"[assembly: AssemblyFileVersion(\"{_version}\")]"
            );
        }
    }
}
