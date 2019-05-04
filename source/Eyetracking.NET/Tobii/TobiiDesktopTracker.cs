using Eyetracking.NET.Tobii;
using Tobii.StreamEngine;

namespace Eyetracking.NET
{
    internal class TobiiDesktopTracker : TobiiTrackerBase, IEyetracker
    {
        internal TobiiDesktopTracker(ApiContext api) : base(api, false)
        {
        }

        public float X { get; private set; }

        public float Y { get; private set; }

        protected override void GazePointCallback(ref tobii_gaze_point_t gazePoint)
        {
            if (gazePoint.validity == tobii_validity_t.TOBII_VALIDITY_VALID)
            {
                X = gazePoint.position.x;
                Y = gazePoint.position.y;
            }
        }
    }
}
