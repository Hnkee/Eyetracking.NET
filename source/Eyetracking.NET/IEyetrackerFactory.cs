namespace Eyetracking.NET
{
    /// <summary>
    /// Responsible for creating the eyetracker
    /// </summary>
    public interface IEyetrackerFactory
    {
        /// <summary>
        /// Creates a new eyetracker
        /// </summary>
        /// <returns>IEeyetracker instance</returns>
        IEyetracker Create();

        /// <summary>
        /// Creates a new 
        /// </summary>
        /// <returns></returns>
        IEyetrackerVr CreateVR();

        bool CanCreateEyetracker { get; }

        bool CanCreateEyetrackerVR { get; }
    }
}
