using BenchmarkDotNet.Attributes;

namespace IKVM.Benchmarks
{

    [MemoryDiagnoser]
    public class FloatArray
    {

        static float Average(float[] array)
        {
            float sum = 0;
            for (int i = 0; i < array.Length; i++)
                sum += array[i];

            return sum / array.Length;
        }

        IKVM.Benchmarks.Java.FloatArray javaImpl;

        [Params(1_000, 10_000, 100_000, 1_000_000)]
        public int ArraySize;

        [Params(100, 1_000)]
        public int LoopCount;

        float[] Array;

        [GlobalSetup]
        public void Setup()
        {
            Array = new float[ArraySize];
            for (int i = 0; i < Array.Length; i++)
                Array[i] = i;

            javaImpl = new Java.FloatArray();
            javaImpl.ArraySize = ArraySize;
            javaImpl.LoopCount = LoopCount;
            javaImpl.Setup();
        }

        [BenchmarkCategory("AverageFloatArray")]
        [Benchmark(Baseline = true)]
        public void CSharpAverageFloatArray()
        {
            for (int i = 0; i < LoopCount; i++)
                Average(Array);
        }

        [BenchmarkCategory("AverageFloatArray")]
        [Benchmark]
        public void JavaAverageFloatArray()
        {
            javaImpl.AverageFloatArray();
        }

    }

}
