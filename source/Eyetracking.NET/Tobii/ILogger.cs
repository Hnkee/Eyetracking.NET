namespace Eyetracking.NET.Tobii
{
    public interface ILogger
    {
        void Debug(string message);
        void Warning(string message);
        void Error(string message);
    }
}
