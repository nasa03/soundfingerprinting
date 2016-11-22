﻿namespace SoundFingerprinting.Audio.NAudio.Test
{
    using Moq;
    using Moq.Protected;

    using global::NAudio.Wave;

    using NUnit.Framework;

    [TestFixture]
    public class NAudioSoundCaptureServiceTest
    {
        private readonly Mock<INAudioFactory> naudioFactory = new Mock<INAudioFactory>(MockBehavior.Strict);
        private readonly Mock<ISamplesAggregator> samplesAggregator = new Mock<ISamplesAggregator>(MockBehavior.Strict);

        private NAudioSoundCaptureService soundCaptureService;

        [SetUp]
        public void SetUp()
        {
            soundCaptureService = new NAudioSoundCaptureService(samplesAggregator.Object, naudioFactory.Object);
        }

        [Test]
        public void TestReadFromMicrophone()
        {
            var waveInEvent = new Mock<WaveInEvent>(MockBehavior.Strict);
            naudioFactory.Setup(factory => factory.GetWaveInEvent(5512, 1)).Returns(waveInEvent.Object);
            float[] samples = new float[1024];
            const int SecondsToRecord = 10;
            samplesAggregator.Setup(agg => agg.ReadSamplesFromSource(It.IsAny<ISamplesProvider>(), SecondsToRecord, 5512))
                .Returns(samples);
            waveInEvent.Protected().Setup("Dispose", new object[] { true });

            float[] resultSamples = soundCaptureService.ReadMonoSamples(5512, SecondsToRecord);

            Assert.AreSame(samples, resultSamples);
        }
    }
}
