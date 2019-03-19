using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    const string LibraryProjectName = "Eyetracking.NET";
    
    public static int Main() => Execute<Build>(x => x.Default);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;

    AbsolutePath SourceRootDirectory => RootDirectory / "source";
    AbsolutePath StageDirectory => RootDirectory / "stage";
    AbsolutePath BuildDirectory => StageDirectory / "build";
    AbsolutePath PublishDirectory => StageDirectory / "publish";

    Project LibraryProject => Solution.AllProjects.First(p => p.Name == LibraryProjectName);

    Target Default => t => t
        .DependsOn(Clean, Restore, Compile, Pack);

    Target Clean => t => t
        .Executes(() =>
        {
            EnsureCleanDirectory(StageDirectory);

            foreach (var dir in GlobDirectories(SourceRootDirectory, "**/bin", "**/obj"))
            {
                EnsureCleanDirectory(dir);
            }
        });

    Target Restore => t => t
        .After(Clean)
        .Executes(() =>
        {
            NuGetTasks.NuGetRestore(s => s
                .SetTargetPath(LibraryProject)
                .SetPackagesDirectory(RootDirectory / "packages")
            );
        });

    Target Compile => t => t
        .After(Restore)
        .Executes(() =>
        {
            MSBuild(s => s
                .SetTargetPath(LibraryProject)
                .SetTargets("Rebuild")
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.GetNormalizedAssemblyVersion())
                .SetFileVersion(GitVersion.GetNormalizedFileVersion())
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .SetMaxCpuCount(Environment.ProcessorCount)
                .SetOutDir(BuildDirectory)
                .SetNodeReuse(IsLocalBuild));
        });

    Target Pack => t => t
        .After(Compile)
        .Executes(() =>
        {
            BuildDirectory.GlobFiles("*.pdb")
                .ForEach(DeleteFile);

            BuildDirectory.GlobFiles("*.dll")
                .ForEach(f => MoveFileToDirectory(f, BuildDirectory / "lib" / "net45"));

            // A file named readme will show up after installation
            // see: https://docs.microsoft.com/en-us/nuget/create-packages/creating-a-package#creating-the-nuspec-file
            RenameFile(BuildDirectory / "readme-after-installation.txt", BuildDirectory / "readme.txt");

            var specPath = LibraryProject.Path.ToString().Replace(".csproj", ".nuspec");
            NuGetTasks.NuGetPack(new NuGetPackSettings()
                .DisableBuild()
                .SetTargetPath(specPath)
                .SetBasePath(BuildDirectory)
                .SetOutputDirectory(PublishDirectory)
                .SetVersion(GitVersion.NuGetVersion)
            );
        });

    Target Publish => _ => _
        .Executes(() =>
        {
        });
}
