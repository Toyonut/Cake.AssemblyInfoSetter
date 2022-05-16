var target = Argument("target", "Package");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
    {
        
    });

Task("Build")
    .Does(() =>
    {

    });

Task("Test")
    .Does(() =>
    {

    });

Task("Package")
    .Does(() =>
    {

    });

Task("RunAll")
    .DependsOn("Clean")
    .DependsOn("Build")
    .DependsOn("Test")
    .DependsOn("Package")
    .Does(() =>
    {
        information("Build Complete")
    });



//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);