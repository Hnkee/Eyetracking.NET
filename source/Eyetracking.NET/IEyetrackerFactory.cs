namespace Eyetracking.NET
{
    public interface IEyetrackerFactory
    {
        IEyetracker Create();

        IEyetrackerVr CreateVR();

        bool CanCreateEyetracker { get; }

        bool CanCreateEyetrackerVR { get; }
    }
}
