#tool nuget:?package=vswhere&version=2.6.7
#tool nuget:?package=GitVersion.CommandLine&version=4.0.0
#tool nuget:?package=NUnit.ConsoleRunner&version=3.10.0

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

// Define directories.
var sourceDir = Directory("./source");
var outputDir = Directory("./stage");
var packageDir = outputDir + Directory("package");
var publishDir = outputDir + Directory("publish");
var solution = File("./Eyetracking.NET.sln");

DirectoryPath msbuildInstallationPath;
FilePath msbuildExePath;
GitVersion version;

Setup(ctx =>
{
    // Executed BEFORE the first task.
    Information("Running tasks...");

    // workaround to not pick msbuild from VS2019 Preview
    msbuildInstallationPath = VSWhereLatest(new VSWhereLatestSettings { Requires = "Microsoft.Component.MSBuild" });
    msbuildExePath = msbuildInstallationPath.CombineWithFilePath("MSBuild/current/Bin/MSBuild.exe");

    version = GitVersion();
    Information($"Version: {version.SemVer}");
    Information($"Git branch: {version.BranchName}");
    Information($"Build provider: {BuildSystem.Provider}");
});

Teardown(ctx =>
{
    // Executed AFTER the last task.
    Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean").Does(() =>
{
    CleanDirectory(outputDir);
    EnsureDirectoryExists(publishDir);
});

Task("Restore").Does(() =>
{
    var settings = new NuGetRestoreSettings
    {
        MSBuildPath = msbuildExePath.GetDirectory()
    };

    NuGetRestore(solution);
});

Task("UpdateVersionNumbers").Does(() =>
{
    CreateAssemblyInfo(outputDir + File("AssemblyVersion.generated.cs"), new AssemblyInfoSettings
    {
        Version = version.MajorMinorPatch,
        FileVersion = version.MajorMinorPatch,
        InformationalVersion = version.InformationalVersion,
    });
});

Task("Build").Does(() =>
{
    var settings = new MSBuildSettings
    {
        ToolPath = msbuildExePath,
        Configuration = configuration,
    }
        .WithTarget("Rebuild");

    MSBuild(solution, settings);
});

Task("Test").Does(() => {
    NUnit3("./source/**/bin/Release/*.Tests.dll", new NUnit3Settings
   {
       //X86 = true,
       Results = new[]
       {
            new NUnit3Result
            {
                FileName = outputDir + File("TestResult.xml")
            }
        },
   });
});

Task("Pack").Does(() => {
    var libDir = packageDir + Directory("lib/net35");
    EnsureDirectoryExists(libDir);

    var nuspecFileName = "Eyetracking.NET.nuspec";
    var eyetrackingProjectDir = sourceDir + Directory("Eyetracking.NET");
    CopyFileToDirectory(eyetrackingProjectDir + File(nuspecFileName), packageDir);
    CopyFiles($"{eyetrackingProjectDir + Directory("bin/" + configuration)}/*.dll" , libDir);

    // A file named readme will show up after installation
    // see: https://docs.microsoft.com/en-us/nuget/create-packages/creating-a-package#creating-the-nuspec-file
    CopyFile(eyetrackingProjectDir + File("readme-after-installation.txt"), packageDir + File("readme.txt"));

    var settings = new NuGetPackSettings
    {
        Version = version.NuGetVersion,
        License = new NuSpecLicense() { Type = "expression", Value = "MIT" },
        BasePath = packageDir,
        OutputDirectory = publishDir
    };
    NuGetPack(packageDir + File(nuspecFileName), settings);
});


Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("UpdateVersionNumbers")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Pack")
    ;

RunTarget(target);