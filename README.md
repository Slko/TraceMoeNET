# Slko.TraceMoeNET

[![Build status](https://ci.appveyor.com/api/projects/status/on4ky6ahdkai1mbw?svg=true)](https://ci.appveyor.com/project/Slko/tracemoenet)
[![Build Status](https://travis-ci.org/Slko/TraceMoeNET.svg?branch=master)](https://travis-ci.org/Slko/TraceMoeNET)
[![NuGet](https://img.shields.io/nuget/vpre/Slko.TraceMoeNET.svg)](https://nuget.org/packages/Slko.TraceMoeNET/)

This is an updated fork of [TraceMoe.NET](https://github.com/Neuxz/TraceMoe.NET) library with the following improvements:

* Added [Nullable Reference Types](https://docs.microsoft.com/ru-ru/dotnet/csharp/nullable-references) support
* Removed dependency on [SkiaSharp](https://github.com/mono/SkiaSharp) library
* Improved API and documentation
* Removed unnecessary components (e.g. Discord bot)
* Added support for URL API (i.e. let the server download the file)
* Added cancellation support (via [CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken))
* And various other fixes and improvements...

## Installation

The latest version of the library is available from NuGet:

* [Slko.TraceMoeNET](https://nuget.org/packages/Slko.TraceMoeNET/)

## Requirements

The project is using C# 8.0 and .NET Standard 2.1 and therefore requires the following environment:

* **.NET Core SDK 3.0** or newer
* [Optional] **Visual Studio 2019 version 16.3** or newer with **.NET Core cross-platform development workload** (for building using **Visual Studio**)

## Example

```csharp
using Slko.TraceMoeNET;
using System;
using System.Threading.Tasks;

namespace Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var client = new TraceMoeClient(); // C# 8.0 using declaration
            var searchResult = await client.SearchByURLAsync("https://i.imgur.com/OkiJZFc.jpg");
            if (searchResult.Results.Length > 0)
            {
                // Should output this:
                // Most relevant result is Sakura Trick
                Console.WriteLine($"Most relevant result is {searchResult.Results[0].TitleRomaji}");
            }
            else
            {
                Console.WriteLine("Not found");
            }
        }
    }
}
```

This repository also contains a [simple console client](https://github.com/Slko/TraceMoeNET/blob/master/Tests/Slko.TraceMoeNET.ConsoleClient/Program.cs) for testing the library. You will need to compile it from source code to use.

## Compiling from source

### Using Visual Studio

#### Building

Clone the repository, open `Slko.TraceMoeNET.sln` in Visual Studio, and build the solution.

#### Testing

Set `Slko.TraceMoeNET.ConsoleClient` as startup project and press **Run**.

### Using Command Line

#### Building

```shell
$ git clone https://github.com/Slko/TraceMoeNET
$ cd TraceMoeNET
$ dotnet build
```

#### Testing

```shell
$ cd Tests/Slko.TraceMoeNET.ConsoleClient
$ dotnet run
```
