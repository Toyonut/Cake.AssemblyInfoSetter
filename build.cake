var target = Argument("target", "RunAll");
var configuration = Argument("configuration", "Release");
var version = EnvironmentVariable<string>("VERSION", "1.0.0.0");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
    {
        CleanDirectory($"./src/Cake.AssemblyInfoSetter/bin/{configuration}");
        CleanDirectory($"./tests/Cake.AssemblyInfoSetter.Tests/bin/{configuration}");
        CleanDirectory($"./output");
    });

Task("Build")
    .Does(() =>
    {
        DotNetBuild("./Cake.AssemblyInfoSetter.sln", new DotNetBuildSettings
        {
            Configuration = configuration,
            MSBuildSettings = new DotNetMSBuildSettings {
                AssemblyVersion = version,
                FileVersion = version,
                Version = version,
            }
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
            OutputDirectory = "./output/",
            MSBuildSettings = new DotNetMSBuildSettings {
                AssemblyVersion = version,
                FileVersion = version,
                Version = version,
            }
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
