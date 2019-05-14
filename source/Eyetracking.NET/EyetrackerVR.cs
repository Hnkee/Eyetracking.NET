namespace Eyetracking.NET
{
    public class EyetrackerVR : IEyetrackerVr
    {
        private static IEyetrackerVr _eyetrackerVR;

        public float X => _eyetrackerVR.X;
        public float Y => _eyetrackerVR.Y;
        public float Z => _eyetrackerVR.Z;

        public EyetrackerVR()
        {
            if (!EyetrackerFactory.Default.CanCreateEyetrackerVR) throw new CannotCreateEyetrackerException();
            if (_eyetrackerVR == null) _eyetrackerVR = EyetrackerFactory.Default.CreateVR();
        }
    }
}
