# Eyetracking .NET #

[![](https://hnkee.visualstudio.com/Eyetracking.NET/_apis/build/status/Eyetracking.NET-CI)](#)

[![Nuget](https://img.shields.io/nuget/v/Eyetracking.NET.svg)](https://www.nuget.org/packages/Eyetracking.NET/)

### What is this repository for? ###

Minimal .NET wrapper for eyetracking

### What is eyetracking? ###

Knowing what the user is looking at, normalized coordinates on the screen (direction in VR)

### How do I get set up? ###

Use nuget package [Eyetracking.NET](https://www.nuget.org/packages/Eyetracking.NET/)

### Who do I talk to? ###

The hand

### Sample ###

```C#
using Eyetracking.NET;

class Program
{
    static void Main(string[] args)
    {
        var eyetracker = Eyetracker.Desktop;
        while (!Console.KeyAvailable)
        {
            Console.WriteLine($"{eyetracker.X} , {eyetracker.Y}");
        }
    }
}
```
