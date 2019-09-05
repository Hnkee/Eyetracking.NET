namespace Eyetracking.NET
{
    public interface IHeadTracker
    {
        /// <summary>
        /// Position X mm
        /// </summary>
        float X { get; }

        /// <summary>
        /// Position Y mm
        /// </summary>

        float Y { get; }

        /// <summary>
        /// Position Z mm
        /// </summary>
        float Z { get; }

        float RotationX { get; }

        float RotationY { get; }

        float RotationZ { get; }
    }
}