using Glovebox.IoT.Base;

namespace Glovebox.IoT.Sensors {
    public class SensorError : SensorBase {

        public override double Current { get { return (int)SensorErrorCount; } }

        public SensorError(int SampleRateMilliseconds, string name)
            : base("error", "n", ValuesPerSample.One, SampleRateMilliseconds, name) {

            StartMeasuring();
        }

        protected override void Measure(double[] value) {
            value[0] = SensorErrorCount;
        }

        protected override string GeoLocation() {
            return string.Empty;
        }

        protected override void SensorCleanup() {
        }
    }
}
