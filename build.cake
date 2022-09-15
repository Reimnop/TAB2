var target = Argument("target", "BuildAndCopy");
var configuration = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Build")
    .Does(() =>
{
    DotNetBuild("./TAB2.sln", new DotNetBuildSettings
    {
        Configuration = configuration,
    });
});

Task("BuildAndCopy")
    .IsDependentOn("Build")
    .Does(() =>
{
    CreateDirectory($"./TAB2/bin/{configuration}/net6.0/Modules");
    CopyFile($"./TAB2.TestModule/bin/{configuration}/net6.0/TAB2.TestModule.dll", $"./TAB2/bin/{configuration}/net6.0/Modules/TAB2.TestModule.dll");
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);