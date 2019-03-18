using Eyetracking.NET.Tobii;

namespace Eyetracking.NET
{
    public class Eyetracker 
    {
        private static ApiContext _api;
        private static IEyetracker _desktop;
        private static IEyetrackerVr _vr;

        static Eyetracker()
        {
            _api = new ApiContext();
        }

        public static IEyetracker Desktop
        {
            get
            {
                return _desktop ?? (_desktop = new TobiiDesktopTracker(_api));
            }
        }

        public static IEyetrackerVr VR
        {
            get
            {
                return _vr ?? (_vr = new TobiiVrTracker(_api));
            }
        }
    }
}
