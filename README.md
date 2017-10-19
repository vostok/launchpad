# Launchpad

CLI control application for Vostok-instrumented applications.

## Get Started

[.NET Core 2 SDK](https://www.microsoft.com/net/core) is a prerequisite. Check that you have `dotnet` in your `PATH`:

```
$ dotnet --version
2.0.0
```

Clone this repository. Two scripts will be available to you: `launchpad.cmd` for Windows and `launchpad.sh` for *nix. Platform-independent libraries will be built on first launch automatically.

Right now, Launchpad is capable of creating a boilerplate C# project:

```
$ ./launchpad.sh create --name TestProject

$ tree output/TestProject
output/TestProject
├── TestProject
│   ├── Controllers
│   │   └── HomeController.cs
│   ├── EntryPoint.cs
│   ├── TestProject.csproj
│   └── appsettings.json
├── TestProject.sln
└── nuget.config
```

You can tweak code and run it either from your IDE or from command line with:

```
dotnet run --project output/TestProject/TestProject/TestProject.csproj
```

Of course, you will need up and running Vostok infrastructure to see logs, traces and metrics. You may already have this infrastructure installed by your DevOps specialist on separate hosts, or in the cloud. But chances are, you just want to test out the entire system locally. Then, use [Spaceport](https://github.com/vostok/spaceport).

All Launchpad-generated applications are preconfigured to work with Spaceport out of the box.
