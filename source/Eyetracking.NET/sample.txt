Wpf samplecode:

public partial class MainWindow : Window
{
    private readonly Eyetracker _tracker = Eyetracking.NET.Eyetracker.Desktop;

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
        var eyetracker = Eyetracking.NET.Eyetracker.Desktop;
        while (!Console.KeyAvailable)
        {
            Console.WriteLine($"{eyetracker.X} , {eyetracker.Y}");
        }
    }
}