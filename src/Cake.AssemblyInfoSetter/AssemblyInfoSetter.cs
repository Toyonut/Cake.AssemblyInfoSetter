using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Annotations;

namespace Cake.AssemblyInfoSetter;

[CakeAliasCategory("AssemblyInfoSetter")]
public static class AssemblyInfoSetter
{
    [CakeMethodAlias]
    public static void SetAssemblyInfo(this ICakeContext context, string globPattern) 
    {
        var files = context.Globber.GetFiles(globPattern);

        foreach (var file in files) {
            Console.WriteLine(file.FullPath.ToString());
        }
    }
}
