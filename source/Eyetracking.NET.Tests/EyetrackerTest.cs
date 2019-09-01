using NUnit.Framework;

namespace Eyetracking.NET.Tests
{
    [TestFixture]
    public class EyetrackerTest
    {
        [Test]
        public void CreateNewEyetracker_WhenNoEyetrackersAreAvailable_ShouldThrow()
        {
            EyetrackerFactory.Default = new EyetrackerFactoryStub() { CanCreateEyetracker = false, CanCreateEyetrackerVR = true };
           
            Assert.Throws<CannotCreateEyetrackerException>(() => new Eyetracker());
        }

        [Test]
        public void CreateNewEyetrackerVR_WhenNoEyetrackersAreAvailableForVR_ShouldThrow()
        {
            EyetrackerFactory.Default = new EyetrackerFactoryStub() { CanCreateEyetracker = true, CanCreateEyetrackerVR = false };

            Assert.Throws<CannotCreateEyetrackerException>(() => new EyetrackerVR());
        }

        [Test]
        public void Eyetracker_WhenCreated_ShouldCreateEyetrackerFromDefaultFactory()
        {
            var expectedEyetracker = new EyetrackerStub() { X = .2f, Y = .23f, Z = .12f };
            EyetrackerFactory.Default = new EyetrackerFactoryStub() { CanCreateEyetracker = true, Eyetracker = expectedEyetracker };

            var eyetracker = new Eyetracker();

            Assert.That(eyetracker.X, Is.EqualTo(expectedEyetracker.X), "X");
            Assert.That(eyetracker.Y, Is.EqualTo(expectedEyetracker.Y), "Y");
        }

        [Test]
        public void EyetrackerVR_WhenVREyetrackerIsCreated_ShouldCreateInnerEyetrackerFromDefaultFactory()
        {
            var expectedEyetracker = new EyetrackerStub() { X = .34f, Y = .3f, Z = .8f };
            EyetrackerFactory.Default = new EyetrackerFactoryStub() { CanCreateEyetrackerVR = true, EyetrackerVR = expectedEyetracker };

            var eyetracker = new EyetrackerVR();

            Assert.That(eyetracker.X, Is.EqualTo(expectedEyetracker.X), "X");
            Assert.That(eyetracker.Y, Is.EqualTo(expectedEyetracker.Y), "Y");
            Assert.That(eyetracker.Z, Is.EqualTo(expectedEyetracker.Z), "Z");
        }

        [Test]
        public void EveryEyetracker_WhenCreated_UsesTheSameInstanceOfInnerEyetracker()
        {
            var expectedEyetracker = new EyetrackerStub();
            EyetrackerFactory.Default = new EyetrackerFactoryStub() { CanCreateEyetracker = true, Eyetracker = expectedEyetracker };

            var instances = EyetrackerStub.NumberOfCreatedInstances;

            var tracker1 = new Eyetracker();
            var tracker2 = new Eyetracker();

            Assert.That(EyetrackerStub.NumberOfCreatedInstances, Is.EqualTo(instances));
        }

        public class EyetrackerFactoryStub : IEyetrackerFactory
        {
            public IEyetracker Eyetracker;
            public IEyetrackerVr EyetrackerVR;
            public IEyetracker Create() => Eyetracker;
            public IEyetrackerVr CreateVR() => EyetrackerVR;
            public bool CanCreateEyetracker { get; set; }
            public bool CanCreateEyetrackerVR { get; set; }
        }

        public class EyetrackerStub : IEyetracker, IEyetrackerVr
        {
            public static int NumberOfCreatedInstances { get; private set; } = 0;

            public EyetrackerStub()
            {
                NumberOfCreatedInstances++;
            }

            public float X { get; set; } = .2f;
            public float Y { get; set; } = .32f;
            public float Z { get; set; } = .24f;
        }
    }
}

