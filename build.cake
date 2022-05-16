var target = Argument("target", "RunAll");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
    {
        CleanDirectory($"./src/Cake.AssemblyInfoSetter/bin/{configuration}");
        CleanDirectory($"./tests/Cake.AssemblyInfoSetter.Tests/bin/{configuration}");
    });

Task("Build")
    .Does(() =>
    {
        DotNetBuild("./Cake.AssemblyInfoSetter.sln", new DotNetBuildSettings
        {
            Configuration = configuration,
        });
    });

Task("Test")
    .Does(() =>
    {
        DotNetTest("./Cake.AssemblyInfoSetter.sln", new DotNetTestSettings
        {
            Configuration = configuration,
            NoBuild = true,
        });
    });

Task("Package")
    .Does(() =>
    {
        DotNetPack("./src/Cake.AssemblyInfoSetter/Cake.AssemblyInfoSetter.csproj", new DotNetPackSettings
        {
            Configuration =configuration,
            OutputDirectory = "./output/"
        });
    });

Task("RunAll")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package")
    .Does(() =>
    {
        Information("Build Complete");
    });



//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);