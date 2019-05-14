Wpf samplecode:

public partial class MainWindow : Window
{
    private readonly Eyetracker _tracker = new  Eyetracking.NET.Eyetracker();

    public MainWindow()
    {
        InitializeComponent();
        CompositionTarget.Rendering += (sender, args) => Content = $"{_tracker.X} ,  {_tracker.Y}";
    }
}

Console samplecode:

class Program
{
    static void Main(string[] args)
    {
        var eyetracker = new Eyetracking.NET.EyetrackerVR();
        while (!Console.KeyAvailable)
        {
            Console.WriteLine($"{eyetracker.X} , {eyetracker.Y} , {eyetracker.Z}");
        }
    }
}