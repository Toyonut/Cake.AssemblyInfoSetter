#r "src/Cake.AssemblyInfoSetter/bin/Debug/net6.0/Cake.AssemblyInfoSetter.dll"

var target = Argument("target", "Test");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Test")
    .Does(() =>
{
   var res = SetAssemblyInfo("**/AssemblyInfo.cs", new AssemblyInfoProperties () {
       AssemblyFileVersion = "1.2.0.9",
       AssemblyVersion = "1.2.0.9"
   });

   foreach (var r in res)
   {
       Information(r);
   }
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);