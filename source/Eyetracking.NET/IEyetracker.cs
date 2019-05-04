namespace Eyetracking.NET
{
    public interface IEyetracker
    {
        /// <summary>
        /// Normalized position x on screen
        /// </summary>
        float X { get; }

        /// <summary>
        /// Normalized position y on screen 
        /// </summary>
        float Y { get; }
    }

    public interface IEyetrackerVr
    {
        /// <summary>
        /// Normalized direction X
        /// </summary>
        float X { get; }

        /// <summary>
        /// Normalized direction Y
        /// </summary>

        float Y { get; }

        /// <summary>
        /// Normalized direction Z
        /// </summary>
        float Z { get; }
    }


    public interface IEyePositionTracker
    {
        float LeftX { get; }

        float LeftY { get; }

        float LeftZ { get; }

        float RightX { get; }

        float RightY { get; }

        float RightZ { get; }
    }
}