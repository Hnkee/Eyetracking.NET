using Eyetracking.NET.Tobii;
using Tobii.StreamEngine;

namespace Eyetracking.NET
{
    internal class TobiiHeadTrackerWithGaze : TobiiTrackerBase, IHeadTracker
    {
        private float _gazeX;
        private float _gazeY;
        private GazeTracker _gazeTracker = new GazeTracker();

        internal TobiiHeadTrackerWithGaze(ApiContext api) : base(api, false, true)
        {
        }

        public IEyetracker Gaze => _gazeTracker;

        public float X { get; private set; }

        public float Y { get; private set; }

        public float Z { get; private set; }

        public float RotationX { get; private set; }

        public float RotationY { get; private set; }

        public float RotationZ { get; private set; }

        protected override void GazePointCallback(ref tobii_gaze_point_t gazePoint)
        {
            if (gazePoint.validity == tobii_validity_t.TOBII_VALIDITY_VALID)
            {
                _gazeTracker.X = gazePoint.position.x;
                _gazeTracker.Y = gazePoint.position.y;
            }
        }

        protected override void HeadPoseCallback(ref tobii_head_pose_t data)
        {
            if (data.position_validity== tobii_validity_t.TOBII_VALIDITY_VALID)
            {
                X = data.position_xyz.x;
                Y = data.position_xyz.y;
                Z = data.position_xyz.z;
            }
            if (data.rotation_x_validity == tobii_validity_t.TOBII_VALIDITY_VALID)
            {
                RotationX = data.rotation_xyz.x;
            }
            if (data.rotation_y_validity == tobii_validity_t.TOBII_VALIDITY_VALID)
            {
                RotationY = data.rotation_xyz.y;
            }
            if (data.rotation_z_validity == tobii_validity_t.TOBII_VALIDITY_VALID)
            {
                RotationZ = data.rotation_xyz.z;
            }
        }

        private class GazeTracker : IEyetracker
        {
            public float X { get; set; }

            public float Y { get; set; }
        }
    }

}
