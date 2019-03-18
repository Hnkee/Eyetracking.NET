# Eye Tracking .NET #

![build status](https://hnkee.visualstudio.com/Eyetracking.NET/_apis/build/status/Eyetracking.NET-CI)

### What is this repository for? ###

Minimal .NET wrapper for eyetracking

### How do I get set up? ###

Use nuget package Eyetracking.NET

### Contribution guidelines ###

### Who do I talk to? ###

The hand

### Sample ####

```
class Program
{
    static void Main(string[] args)
    {
        var eyetracker = Eyetracking.NET.Eyetracker.Desktop;
        while (!Console.KeyAvailable)
        {
            Console.WriteLine($"{eyetracker.X} , {eyetracker.Y}");
        }
    }
}
```