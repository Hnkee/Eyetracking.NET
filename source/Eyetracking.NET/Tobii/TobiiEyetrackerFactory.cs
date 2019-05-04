using Eyetracking.NET.Tobii;

namespace Eyetracking.NET
{
    public class TobiiEyetrackerFactory : IEyetrackerFactory
    {
        private static ApiContext _api;
        private static IEyetracker _desktop;
        private static IEyetrackerVr _vr;
        private static IEyePositionTracker _eyePositionTracker;
        private static IEyetracker Desktop => _desktop ?? (_desktop = new TobiiDesktopTracker(_api));
        private static IEyetrackerVr VR => _vr ?? (_vr = new TobiiVrTracker(_api));

        private static IEyePositionTracker EyePositionTracker  => _eyePositionTracker ?? (_eyePositionTracker = new TobiiDesktopOriginTracker(_api));

        static TobiiEyetrackerFactory()
        {
            _api = new ApiContext();
        }

        //TODO:
        public bool CanCreateEyetracker { get; } = true;
        public bool CanCreateEyetrackerVR { get; } = true;

        public IEyetracker Create()
        {
            return Desktop;
        }

        public IEyetrackerVr CreateVR()
        {
            return VR;
        }

        public IEyePositionTracker CreatePositionTracker()
        {
            return EyePositionTracker;
        }
    }
}
