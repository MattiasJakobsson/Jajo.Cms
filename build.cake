var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Debug");
var buildNumber = Argument<string>("buildnumber", "0");
var nugetApiKey = Argument<string>("nugetApiKey", "");

var solutions = GetFiles("./**/*.sln");
var solutionPaths = solutions.Select(solution => solution.GetDirectory());

var version = "1.0.0";
var packageVersion = string.Format("{0}.{1}", version, buildNumber);

Task("Clean")
    .Does(() =>
	{
		foreach(var path in solutionPaths)
		{
			Information("Cleaning {0}", path);
			CleanDirectories(path + "/**/bin/" + configuration);
			CleanDirectories(path + "/**/obj/" + configuration);
		}

		CleanDirectories("./build");
	});

Task("Version")
	.Does(() => {
		CreateAssemblyInfo("./src/CommonAssemblyInfo.cs", new AssemblyInfoSettings {
			Version = version,
			FileVersion = version,
			InformationalVersion = packageVersion,
			Copyright = string.Format("Copyright (c) Mattias Jakobsson 2015 - {0}", DateTime.Now.Year)
		});
	});

Task("BootstrapPaket")
	.Does(() => {
		if(!System.IO.File.Exists("./.paket/paket.exe"))
		{
			using(var process = StartAndReturnProcess("./.paket/paket.bootstrapper.exe"))
			{
				Information("Initializing paket");
				process.WaitForExit();
				Information("Exit code: {0}", process.GetExitCode());
			}
		}
	});

Task("Restore")
	.IsDependentOn("BootstrapPaket")
    .Does(() =>
{
    using(var process = StartAndReturnProcess("./.paket/paket.exe", new ProcessSettings{ Arguments = "restore" }))
	{
		Information("Restoring packages");
		process.WaitForExit();
		Information("Exit code: {0}", process.GetExitCode());
	}
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
	.IsDependentOn("Version")
	.IsDependentOn("Bottles")
    .Does(() =>
	{
		foreach(var solution in solutions)
		{
			Information("Building {0}", solution);
			MSBuild(solution, settings => 
				settings.SetPlatformTarget(PlatformTarget.MSIL)
					.WithProperty("TreatWarningsAsErrors","false")
					.WithTarget("Build")
					.SetConfiguration(configuration));
		}
	});

Task("Bottles")
	.Does(() => {
		var directories = System.IO.Directory.GetDirectories("./src/");

		foreach(var directory in directories)
		{
			if(System.IO.File.Exists(string.Format("{0}/.package-manifest", directory)))
			{
				using(var process = StartAndReturnProcess("./tools/bottles/bottles.exe", new ProcessSettings { Arguments = "assembly-pak " + directory }))
				{
					Information("Executing bottles for directory: {0}", directory);
					process.WaitForExit();
					Information("Exit code: {0}", process.GetExitCode());
				}
			}
		}
	});

Task("PaketPack")
	.IsDependentOn("BootstrapPaket")
	.Does(() => {
		using(var process = StartAndReturnProcess("./.paket/paket.exe", new ProcessSettings { Arguments = "pack output build buildconfig " + configuration + " version " + packageVersion }))
		{
			Information("Building packages");
			process.WaitForExit();
			Information("Exit code: {0}", process.GetExitCode());
		}
	});

Task("PaketPush")
	.IsDependentOn("PaketPack")
	.Does(() => {
		var packages = System.IO.Directory.GetFiles("./build", "*.nupkg");

		foreach(var package in packages)
		{
			using(var process = StartAndReturnProcess("./.paket/paket.exe", new ProcessSettings { Arguments = "push url https://www.myget.org/F/jajo file \"" + package + "\" apikey " + nugetApiKey }))
			{
				Information("Pushing packages");
				process.WaitForExit();
				Information("Exit code: {0}", process.GetExitCode());
			}	
		}
	});

Task("Test")
	.IsDependentOn("Build")
	.Does(() => {
		Fixie("**/*.Tests/bin/" + configuration + "/*.Tests.dll", new FixieSettings {
			TeamCity = true
		});
	});

Task("Default")
    .IsDependentOn("Build");

Task("ci")
	.IsDependentOn("Test")
	.IsDependentOn("PaketPush")
	.Does(() => {
		if(TeamCity.IsRunningOnTeamCity)
		{
			TeamCity.SetBuildNumber(packageVersion);
		}
	});

RunTarget(target);