#r "src/Cake.AssemblyInfoSetter/bin/Debug/net6.0/Cake.AssemblyInfoSetter.dll"

var target = Argument("target", "Test");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("TestCS")
    .Does(() =>
{
   var res = SetAssemblyInfo("**/AssemblyInfo.cs", new AssemblyInfoProperties () {
       AssemblyFileVersion = "1.1.0.4",
       AssemblyVersion = "1.7.0.8",
       AssemblyCompany = "My Company"
   });

   foreach (var r in res)
   {
       Information(r);
   }
});

Task("TestCSProj")
    .Does(() =>
{
   var res = SetAssemblyInfo("**/*.csproj", new AssemblyInfoProperties () {
       AssemblyFileVersion = "1.1.0.4",
       AssemblyVersion = "1.7.0.8",
       AssemblyCompany = "My Company"
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