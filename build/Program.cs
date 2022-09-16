using System.IO;
using Cake.Common;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting;

public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .Run(args);
    }
}

public class BuildContext : FrostingContext
{
    public string MsBuildConfiguration { get; set; }
    
    public BuildContext(ICakeContext context)
        : base(context)
    {
        MsBuildConfiguration = context.Argument("configuration", "Debug");
    }
}

[TaskName("Clean Backend")]
public class CleanBackendTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Log.Information("Cleaning backend");
        context.CleanDirectory($"../TAB2/bin/{context.MsBuildConfiguration}");
    }
}

[TaskName("Clean Test Module")]
public class CleanTestModuleTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Log.Information("Cleaning test module");
        context.CleanDirectory($"../TAB2.TestModule/bin/{context.MsBuildConfiguration}");
    }
}

[TaskName("Build")]
public class BuildTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetBuild("../TAB2.sln", new DotNetBuildSettings
        {
            Configuration = context.MsBuildConfiguration
        });
    }
}

[TaskName("Copy Test Module")]
[IsDependentOn(typeof(CleanTestModuleTask))]
[IsDependentOn(typeof(BuildTask))]
public class CopyTestModuleTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        Directory.CreateDirectory($"../TAB2/bin/{context.MsBuildConfiguration}/net6.0/Modules");
        File.Copy($"../TAB2.TestModule/bin/{context.MsBuildConfiguration}/net6.0/TAB2.TestModule.dll", $"../TAB2/bin/{context.MsBuildConfiguration}/net6.0/Modules/TAB2.TestModule.dll", true);
    }
}

[TaskName("Default")]
[IsDependentOn(typeof(CopyTestModuleTask))]
public class DefaultTask : FrostingTask<BuildContext>
{
}