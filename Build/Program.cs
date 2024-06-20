using static Bullseye.Targets;
using static SimpleExec.Command;

namespace Build;

internal class Program
{
    const string ArtifactsDir = "artifacts";
    const string Clean = "clean";
    const string Build = "build";
    const string Test = "test";
    const string PushToGitHub = "push-github";
    const string PushToNugetOrg = "push-to-nuget-org";
    const string Solution = "Radix.sln";

    /// <summary>
    /// Deletes all files (except .gitignore) and subdirectories from specified path.
    /// </summary>
    /// <param name="path">The path whose files and subdirectories will be deleted</param>
    public static void CleanDirectory(string path)
    {
        var filesToDelete = Directory
            .GetFiles(path, "*.*", SearchOption.AllDirectories)
            .Where(f => !f.EndsWith(".gitignore"));
        foreach (var file in filesToDelete)
        {
            Console.WriteLine($"Deleting file {file}");
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        var directoriesToDelete = Directory.GetDirectories(path);
        foreach (var directory in directoriesToDelete)
        {
            Console.WriteLine($"Deleting directory {directory}");
            Directory.Delete(directory, true);
        }
    }

    static async Task Main(string[] args)
    {
        Target(Clean, () => {
            CleanDirectory(ArtifactsDir);
            Run("dotnet", $"clean {Solution}");
            }
        );



        Target(Build, () => Run("dotnet", $"build {Solution} -c Release"));

        Target(Test, () => Run("dotnet", $"test {Solution} -c Release"));

        var targets = new List<string>
        {
            Clean, Build, Test
        };

        var nugetVersion = Environment.GetEnvironmentVariable("NUGET_VERSION");
        var gitCommitId = Environment.GetEnvironmentVariable("GIT_COMMIT_ID");
        var ignore = new[] { ".github", "Build" };
        var projects = Directory.GetDirectories(".")
            .Where(d => !ignore.Contains(d))
            .Select(d => new DirectoryInfo(d).Name);

        foreach (var project in projects)
        {
            var packableProjects = Directory.GetFiles($"./{project}/", "*.csproj", SearchOption.AllDirectories);
            var packTarget = $"{project}-pack";
            Target(packTarget, DependsOn(Clean),
                packableProjects,
                packableProject => Run("dotnet", $"pack {packableProject} --no-build  -p:PackageVersion={nugetVersion} -c Release -o {ArtifactsDir}"));
            targets.Add(packTarget);
        }

        Target(PushToGitHub, () =>
        {
            var apiKey = Environment.GetEnvironmentVariable("PACKAGE_PUSH_TOKEN");

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Console.WriteLine("GITHUB_TOKEN not available. No packages will be pushed.");
                return;
            }
            Console.WriteLine($"Nuget API Key ({apiKey.Substring(0, 5)}) available. Pushing packages...");
            Run("dotnet", $"nuget push \"{ArtifactsDir}{Path.DirectorySeparatorChar}*.nupkg\" -s https://nuget.pkg.github.com/mcgppeters/index.json -k {apiKey} --skip-duplicate", noEcho: true);
        });
        targets.Add(PushToGitHub);

        Target(PushToNugetOrg, () =>
        {
            var apiKey = Environment.GetEnvironmentVariable("NUGETORG_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Console.WriteLine("NUGETORG_API_KEY not available. No packages will be pushed.");
                return;
            }
            Console.WriteLine($"Nuget API Key ({apiKey.Substring(0, 5)}) available. Pushing packages...");
            Run("dotnet", $"nuget push \"{ArtifactsDir}{Path.DirectorySeparatorChar}*.nupkg\" -s https://api.nuget.org/v3/index.json -k {apiKey} --skip-duplicate", noEcho: true);
        });
        targets.Add(PushToNugetOrg);

        Target("default", DependsOn(targets.ToArray()));

        await RunTargetsAndExitAsync(args);
    }
}
