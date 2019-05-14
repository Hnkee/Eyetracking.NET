namespace Eyetracking.NET
{
    public class EyetrackerFactory : IEyetrackerFactory
    {
        private static IEyetrackerFactory _default;

        public static IEyetrackerFactory Default
        {
            get
            {
                if (_default == null) _default = new TobiiEyetrackerFactory();
                return _default;
            }
            set { _default = value; }
        }

        public bool CanCreateEyetracker => Default.CanCreateEyetracker;
        public bool CanCreateEyetrackerVR => Default.CanCreateEyetrackerVR;

        public IEyetracker Create()
        {
            if (!CanCreateEyetracker) throw new CannotCreateEyetrackerException();
            return Default.Create();
        }

        public IEyetrackerVr CreateVR()
        {
            if (!CanCreateEyetrackerVR) throw new CannotCreateEyetrackerException();
            return Default.CreateVR();
        }
    }
}
