# Launchpad

Launchpad is a CLI tool for Vostok.

Use Launchpad to create a boilerplate Vostok-instrumented application. Right now, there's an ASP.NET Core 2.0 application [template](https://github.com/vostok/launchpad/tree/master/templates/aspnetcore) which compiles to a binary file that you can deploy manually. Lauchpad will support more ready-to-deploy application templates and cloud environments.

In the future, Launchpad will be capable of controlling Vostok applications' deployment and scaling.

## Installation

[.NET Core 2 SDK](https://www.microsoft.com/net/core) is a prerequisite. Check that you have `dotnet` in your `PATH`:

```
$ dotnet --version
2.0.0
```

Clone this repository:

```
$ git clone https://github.com/vostok/launchpad.git
```

Two scripts will be available to you:

* `vostok.cmd` for Windows,
* `vostok.sh` for *nix and macOS.

## Usage

Create an ASP.NET Core 2.0 application:

```
$ ./vostok.sh create --name TestProject
Project TestProject created in 'output' directory.

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

Platform-independent libraries are built automatically on the first run of `vostok.cmd` or `vostok.sh`.

Tweak the code and run it either from your IDE or from command line:

```
$ dotnet run --project output/TestProject/TestProject/TestProject.csproj
```

By default, the application is bound to `http://[::]:33333`. It is an echo server which awaits for `GET` requests to any URI and returns back that URI and a distributed trace id:

```
$ curl -s localhost:33333 | python -m json.tool
{
    "traceId": "http://localhost:6301/bedfdf88-704d-493f-a458-2999d0b57e09",
    "url": "http://localhost:33333/"
}
```

You need an up and running Vostok infrastructure to collect and see logs, traces and metrics. This infrastructure may be already installed by your DevOps specialist on dedicated hosts or in the cloud.

You can also run a Vostok infrastructure locally using [Spaceport](https://github.com/vostok/spaceport). All Launchpad-generated applications are preconfigured to work with Spaceport out of the box.
