﻿using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Annotations;
using System.Collections.Concurrent;

namespace Cake.AssemblyInfoSetter 
{
    [CakeAliasCategory("AssemblyInfoSetter")]
    public static class AssemblyInfoSetter
    {
        [CakeMethodAlias]
        public static FilePath[] SetAssemblyInfo(this ICakeContext context, string globPattern, string assemblyInfoField, string replacementValue)
        {
            var files = context.Globber.GetFiles(globPattern);

            var results = new ConcurrentBag<FilePath>();

            Parallel.ForEach (files, file => {
                var replacer = new AssemblyInfoReplacer(context, file.FullPath, assemblyInfoField, replacementValue);
                var fileReplacementPath = replacer.Replace();
                results.Add(fileReplacementPath);
            });

            return results.ToArray();
        }
    }
}
