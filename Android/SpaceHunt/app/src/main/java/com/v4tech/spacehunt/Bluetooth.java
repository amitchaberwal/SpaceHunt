package com.v4tech.spacehunt;

import static com.v4tech.spacehunt.AppParams.*;
import com.v4tech.spacehunt.pantompkins.PanTompkins;
import android.app.Activity;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothSocket;
import android.os.CountDownTimer;
import android.content.Intent;
import android.util.Log;
import com.unity3d.player.UnityPlayer;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.ArrayList;
import java.util.Set;
import java.util.UUID;

public class Bluetooth {
    private BluetoothAdapter btAdapter = null;
    private BluetoothSocket btSocket = null;
    private BluetoothDevice device = null;
    private ConnectedThread mConnectedThread;
    private static String btAddress;
    ArrayList list;
    ArrayList<String> addresses = new ArrayList<>();
    private static final UUID MY_Serial_UUID = UUID.fromString("00001101-0000-1000-8000-00805F9B34FB");
    private Activity activity;
    private Set<BluetoothDevice>pairedDevices;

    RealtimeQrs realtimeQrs;
    public static final String START = "AF";
    public static final String STOP = "S";

    public Bluetooth() {
        activity = UnityPlayer.currentActivity;

    }
    public void TurnOn() {
        btAdapter = BluetoothAdapter.getDefaultAdapter();
        if (!btAdapter.isEnabled()) {
            Intent btStart = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
            activity.startActivity(btStart);
            UnityPlayer.UnitySendMessage("GameManager", "Plugin", "Bluetooth Enabled");
        }
    }
    public void TurnOff(){
        btAdapter = BluetoothAdapter.getDefaultAdapter();
        btAdapter.disable();
    }
    public void GetDevices(){
        btAdapter = BluetoothAdapter.getDefaultAdapter();
        pairedDevices = btAdapter.getBondedDevices();
        list  = new ArrayList();
        for (BluetoothDevice bt : pairedDevices) {
            list.add(bt.getName());
            addresses.add(bt.getAddress());
        }

        UnityPlayer.UnitySendMessage("GameManager", "DeviceList", String.valueOf(list));
        Log.d("Plugin Msg","Bounded Device List" + String.valueOf(list));

    }
    public void GetData(String devID) {
        resetStaticVariables();//VERY VERY IMPORTANT!!
        realtimeQrs = new RealtimeQrs(new PanTompkins(SAMPLING_RATE));
        btAdapter = BluetoothAdapter.getDefaultAdapter();
        pairedDevices = btAdapter.getBondedDevices();
        for (BluetoothDevice bt : pairedDevices) {
            addresses.add(bt.getAddress());
        }
        btAddress = addresses.get(Integer.valueOf(devID));
        Log.d("Plugin Msg","Device Address:" + btAddress);
        device = btAdapter.getRemoteDevice(btAddress);
        if (mConnectedThread != null) {
            mConnectedThread.interrupt();
        }
        if (btSocket != null) {
            try {
                btSocket.close();
            } catch (IOException e) {
            }
            btSocket = null;
        }

        try {
            btSocket = device.createInsecureRfcommSocketToServiceRecord(MY_Serial_UUID);
        } catch (IOException e) {
        }
        try {
            btSocket.connect();
            UnityPlayer.UnitySendMessage("GameManager", "Plugin", "Getting Data From Device");
        } catch (IOException e) {
        }
        mConnectedThread = new ConnectedThread(btSocket);
        mConnectedThread.start();
        realtimeQrs.startThread();

    }
    private class ConnectedThread extends Thread {

        private final OutputStream mmOutStream;
        private final InputStream mmInpStream;
        private final BluetoothSocket mmSocket;

        private ConnectedThread(BluetoothSocket socket) {
            OutputStream tmpOut = null;
            InputStream tmpInp = null;
            mmSocket = socket;
            try {
                tmpOut = mmSocket.getOutputStream();
                tmpInp = mmSocket.getInputStream();
            } catch (IOException e) {
            }
            mmOutStream = tmpOut;
            mmInpStream = tmpInp;
        }
        public void run() {

            while (true) {

                try {
                    int val = mmInpStream.read();
                    dataBuffer[producerIndex] = val;


                    if (producerIndex >= MAX_SAMPLES - 1)
                        producerIndex = 0;
                    else
                        producerIndex++;

                }
                catch (IOException e) {
                    break;
                }
            }

        }
        public void write(byte[] buffer) {
            try {
                Log.d("Plugin Msg","signal sent to device");
                mmOutStream.write(buffer);
            } catch (IOException ignored) {
            }
        }
    }
    public void sendSignal(final String msg){
        new CountDownTimer(600, 100) {
            @Override
            public void onTick(long millisUntilFinished) {
                if (msg.equals("AF")) {
                    mConnectedThread.write(START.getBytes());
                }
                if (msg.equals("S")) {
                    mConnectedThread.write(STOP.getBytes());
                }
            }
            @Override
            public void onFinish() {
            }
        }.start();
    }
}
