namespace Eyetracking.NET
{
    public class Eyetracker : IEyetracker
    {
        private IEyetracker _eyetracker;

        public float X => _eyetracker.X;
        public float Y => _eyetracker.Y;

        public Eyetracker()
        {
            if (!EyetrackerFactory.Default.CanCreateEyetracker) throw new CannotCreateEyetrackerException();
            _eyetracker = EyetrackerFactory.Default.Create();
        }
    }
}
