using Eyetracking.NET.Tobii;
using Tobii.StreamEngine;

namespace Eyetracking.NET
{
    internal class TobiiDesktopOriginTracker : TobiiTrackerBase, IEyePositionTracker
    {
        internal TobiiDesktopOriginTracker(ApiContext api) : base(api, false, true)
        {
        }

        public float LeftX { get; private set; }
        public float LeftY { get; private set; }
        public float LeftZ { get; private set; }
        public float RightX { get; private set; }
        public float RightY { get; private set; }
        public float RightZ { get; private set; }

        protected override void GazeOriginCallback(ref tobii_gaze_origin_t data)
        {
            if (data.left_validity == tobii_validity_t.TOBII_VALIDITY_VALID && data.right_validity == tobii_validity_t.TOBII_VALIDITY_VALID)
            {
                LeftX = data.left.x;
                LeftX = data.left.y;
                LeftX = data.left.z;
                RightX = data.right.x;
                RightX = data.right.y;
                RightX = data.right.z;
            }
        }
    }
}
