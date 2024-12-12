package IKVM.Benchmarks.Java;

import java.lang.*;
import java.util.*;

public class FloatArray {

    static float Average(float[] array) {
	    float sum = 0;
	    for (int i = 0; i < array.length; i++) {
		    sum += array[i];
		}

	    return sum / array.length;
    }

	public int ArraySize;
	public int LoopCount;
	float[] Array;

    public void Setup()
    {
		Array = new float[ArraySize];
	    for (int i = 0; i < Array.length; i++) {
		    Array[i] = i;
		}
    }

    public void AverageFloatArray() {
	    for (int i = 0; i < LoopCount; i++) {
		    Average(Array);
		}
    }

}
