using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VocalChat.WPF.Codecs;

namespace VocalChat.WPF.Services
{
    public class VocalChatService
    {
        public int Port { get; set; }
        public string AddressReturn { get; set; } = default!;
        public WaveIn WaveInput { get; private set; } = default(WaveIn)!;
        public UdpClient UdpSender { get; private set; } = default!;
        public UdpClient UdpListener { get; private set; } = default!;
        public IWavePlayer WaveOut { get; private set; } = default!;
        public BufferedWaveProvider WaveProvider { get; private set; } = default!;
        public INetworkChatCodec SelectedCodec { get; private set; } = default!;
        public bool IsConnected { get; private set; }
        public IPEndPoint EndPoint { get; private set; } = default!;
        public int InputDeviceNumber { get; private set; }

        public VocalChatService()
        {

        }

        public void OpenAudio()
        {
            SelectedCodec = new UncompressedPcmChatCodec();
            EndPoint = new IPEndPoint(IPAddress.Parse(AddressReturn), Port);
        }

        private void Connect()
        {
            try
            {
                WaveInput = new WaveIn()
                {
                    BufferMilliseconds = 50,
                    DeviceNumber = InputDeviceNumber,
                    WaveFormat = SelectedCodec.RecordFormat
                };

                WaveInput.DataAvailable += WaveInput_DataAvailable;
                WaveInput.StartRecording();

                UdpSender = new UdpClient();
                UdpListener = new UdpClient(EndPoint.Port);

                UdpListener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                UdpSender.Connect(EndPoint);

                WaveOut = new WaveOut();
                WaveProvider = new BufferedWaveProvider(SelectedCodec.RecordFormat);
                WaveOut.Init(WaveProvider);
                WaveOut.Play();

                IsConnected = true;

                ListenerThreadState? state = new ListenerThreadState()
                {
                    Codec = SelectedCodec,
                    EndPoint = EndPoint
                };
                ThreadPool.QueueUserWorkItem(ListenerThread!, state);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void WaveInput_DataAvailable(object? sender, WaveInEventArgs e)
        {
            try
            {
                byte[] encoded = SelectedCodec.Encode(e.Buffer, 0, e.BytesRecorded)!;
                int num = UdpSender.Send(encoded, encoded.Length);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void ListenerThread(object state)
        {
            ListenerThreadState? listenerThreadstate = (ListenerThreadState)state;
            IPEndPoint endPoint = listenerThreadstate.EndPoint;

            try
            {
                while (IsConnected)
                {
                    try
                    {
                        byte[] buffer = UdpListener.Receive(ref endPoint);
                        byte[] decoded = listenerThreadstate.Codec.Decode(buffer, 0, buffer.Length)!;
                        WaveProvider.AddSamples(decoded, 0, decoded.Length);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
