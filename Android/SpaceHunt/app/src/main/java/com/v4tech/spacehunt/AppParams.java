package com.v4tech.spacehunt;

public class AppParams {
	public static final int SAMPLING_RATE = 250;
    public static final int PEAK_FOUND_HR = -1;
    public static final int NO_PEAK_HR = 0;

    static final int MAX_SAMPLES = SAMPLING_RATE * 120;
	static int[] dataBuffer;
	static int producerIndex;
	static int consumerIndex;
	public static long timestamp = 0;

	private static int[] HR = new int[5];
	private static int HRindex = 0;

    public static void addHR(int heartRate) {
		HR[HRindex] = heartRate;
		HRindex = HRindex < 4 ? HRindex + 1 : 0;
	}

	static int getAvgHR() {
		float sum = 0;
		int count = 0;
		for (int i = 0; i < 5; i++) {
			if (HR[i] == 0)
				break;

			sum += HR[i];
			count++;
		}
		return Math.round(sum / count);
	}

	static void resetStaticVariables() {
		HR = new int[5];
		HRindex = 0;
		dataBuffer = new int[MAX_SAMPLES];
		producerIndex = consumerIndex = 0;
		timestamp = 0;
	}
}
