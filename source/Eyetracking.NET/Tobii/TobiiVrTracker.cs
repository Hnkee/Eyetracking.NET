using Eyetracking.NET.Tobii;
using Tobii.StreamEngine;

namespace Eyetracking.NET
{
    internal sealed class TobiiVrTracker : TobiiTrackerBase, IEyetrackerVr
    {
        internal TobiiVrTracker(ApiContext api) : base(api, true)
        {
        }

        public float X { get; private set; }

        public float Y { get; private set; }
        
        public float Z { get; private set; }

        protected override void WearableCallback(ref tobii_wearable_data_t data)
        {
            if (data.gaze_direction_combined_validity == tobii_validity_t.TOBII_VALIDITY_VALID)
            {
                var gaze = data.gaze_direction_combined_normalized_xyz;
                X = gaze.x;
                Y = gaze.y;
                Z = gaze.z;
            }
        }
    }
}
