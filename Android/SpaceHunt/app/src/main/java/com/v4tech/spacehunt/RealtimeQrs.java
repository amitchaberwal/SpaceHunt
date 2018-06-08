package com.v4tech.spacehunt;

import android.util.Log;
import com.unity3d.player.UnityPlayer;
import com.v4tech.spacehunt.pantompkins.PanTompkins;
import static com.v4tech.spacehunt.AppParams.*;

class RealtimeQrs {
    private ReadThread _thread;
	private Long lastConsumeTime = 0L;
    private boolean paused = false;
    private PanTompkins panTom;
	private boolean runThread;
    private final int millisBehindRealtime;
    private static float samplingInterval = 1000f / SAMPLING_RATE;

    private static int QRSdetection(int val, PanTompkins panTom) {
        timestamp += (long) samplingInterval;
        // processing pipeline
        int heartRate = panTom.next(val, timestamp);
        if (PanTompkins.QRS.qrsCurrent.segState == PanTompkins.QRS.SegmentationStatus.FINISHED) {
            PanTompkins.QRS.qrsCurrent.segState = PanTompkins.QRS.SegmentationStatus.PROCESSED;
        }
        return heartRate;
    }

	RealtimeQrs(PanTompkins panTompkins) {
		this.panTom = panTompkins;
        millisBehindRealtime = 1000;
    }

	private void setRunning(boolean val) {
		runThread = val;
	}

	void initializeThread() {
		_thread = new ReadThread();
		runThread = false;
	}

	void startThread() {
		_thread = new ReadThread();
		setRunning(true);
		_thread.start();
	}

	void pauseThread() {
        paused = true;
    }
    void resumeThread() {
        paused = false;
    }

	void stopThread() {
		setRunning(false);
		if (_thread != null) {
			try {
				_thread.join();
			} catch (InterruptedException e) {
				e.printStackTrace();
			} finally {
				_thread = null;
			}
		}
	}

	private class ReadThread extends Thread {
		public void run() {
			try {
				Thread.sleep(millisBehindRealtime);
				lastConsumeTime = System.currentTimeMillis();
				while (runThread) {
					Thread.sleep(20);
					if (!paused)
						runPanTompkins();
					else {
						lastConsumeTime = System.currentTimeMillis();
					}
				}
			} catch (InterruptedException e1) {
				e1.printStackTrace();
			}
		}
	}



	private void runPanTompkins() {
		try {

            int diff = producerIndex - consumerIndex;
			if (diff < 0)
				diff += MAX_SAMPLES;

			long currentTime = System.currentTimeMillis();
			long elapsed = currentTime - lastConsumeTime;
			int timeSamples = Math
					.round((elapsed * SAMPLING_RATE) / 1000f);
			if (diff > timeSamples) {
				diff = timeSamples;
			}
			lastConsumeTime = currentTime;

			while (diff > 0) {
                float sampleVal = dataBuffer[consumerIndex];

				//QRS detection
				int heartRate = QRSdetection((int) sampleVal, panTom);
                if(heartRate != NO_PEAK_HR) {
                    if (heartRate == PEAK_FOUND_HR) {
                    	int bpm = getAvgHR();
						Log.d("Plugin Msg","Plugin BPM:" + bpm);
						//HearRate in BPM Found,Send to Unity
						UnityPlayer.UnitySendMessage("GameManager", "BPM", String.valueOf(bpm));

					} else {
                        addHR(heartRate);
                    }
                }

				consumerIndex++;
				if (consumerIndex >= MAX_SAMPLES) {
					consumerIndex = 0;
				}

				diff--;
			}
		} catch (Exception ignored) {
		}
	}
}
