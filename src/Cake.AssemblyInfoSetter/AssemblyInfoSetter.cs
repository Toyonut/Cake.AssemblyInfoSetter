using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Annotations;
using System.Collections.Concurrent;

namespace Cake.AssemblyInfoSetter 
{
    [CakeAliasCategory("AssemblyInfoSetter")]
    public static class AssemblyInfoSetter
    {
        [CakeMethodAlias]
        public static FilePath[] SetAssemblyInfo(this ICakeContext context, string globPattern, string version)
        {
            var files = context.Globber.GetFiles(globPattern);

            var results = new ConcurrentBag<FilePath>();

            Parallel.ForEach (files, file => {
                var replacer = new AssemblyInfoReplacer(context, new Version(version), file.FullPath);

                replacer.AssemblyFileVersion.replace();                
            });

            return results.ToArray();
        }
    }
}
