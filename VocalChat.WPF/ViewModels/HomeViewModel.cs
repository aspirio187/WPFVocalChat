using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VocalChat.Shared.Models;
using VocalChat.WPF.Commands;
using VocalChat.WPF.Services;

namespace VocalChat.WPF.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly SignalRChatService _signalRChatService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string? _username;

        public string? Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private ConnectCommand _connectCommand;

        public ConnectCommand ConnectCommand => _connectCommand ??= new ConnectCommand(Connect);

        private StartStopCommand _startCommand;

        public StartStopCommand StartCommand => _startCommand ??= new StartStopCommand(Start);

        private StartStopCommand _stopCommand;

        public StartStopCommand StopCommand => _stopCommand ??= new StartStopCommand(Stop);

        public ObservableCollection<string> AvailableOutputs { get; set; }
        public ObservableCollection<string> AvailableInputs { get; set; }
        public ObservableCollection<UserHubModel> ConnectedUsers { get; set; } = new ObservableCollection<UserHubModel>();

        private string _selectedOutput;

        public string SelectedOutput
        {
            get { return _selectedOutput; }
            set
            {
                _selectedOutput = value;
                OnPropertyChanged();
            }
        }

        private string _selectedInput;

        public string SelectedInput
        {
            get { return _selectedInput; }
            set
            {
                _selectedInput = value;
                OnPropertyChanged();
            }
        }

        private string _contextId;

        public string ContextId
        {
            get { return _contextId; }
            set
            {
                _contextId = value;
                OnPropertyChanged();
            }
        }


        public Task SendAudioTask { get; set; }

        public HomeViewModel(SignalRChatService chatService)
        {
            _signalRChatService = chatService;

            AvailableOutputs = LoadOutputList();
            AvailableInputs = LoadInputList();

            _signalRChatService.NewUserArrived += ChatService_NewUserArrived;
            _signalRChatService.Connected += _signalRChatService_Connected;
        }

        private void _signalRChatService_Connected(string obj)
        {
            ContextId = obj;
        }

        private void ChatService_NewUserArrived(UserHubModel obj)
        {
            ConnectedUsers.Add(new UserHubModel()
            {
                Ip = obj.Ip,
                Port = obj.Port,
                Username = obj.Username,
            });
        }

        public ObservableCollection<string> LoadOutputList()
        {
            ObservableCollection<string> outputs = new ObservableCollection<string>();

            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                WaveOutCapabilities capabilities = WaveOut.GetCapabilities(i);
                outputs.Add(capabilities.ProductName);
            }

            return outputs;
        }

        public ObservableCollection<string> LoadInputList()
        {
            ObservableCollection<string> inputs = new ObservableCollection<string>();

            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                WaveInCapabilities capabilities = WaveIn.GetCapabilities(i);
                inputs.Add(capabilities.ProductName);
            }

            return inputs;
        }

        public async void Connect()
        {
            try
            {
                await _signalRChatService.Connect();
                string? userIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ad => ad.AddressFamily == AddressFamily.InterNetwork).ToString();
                await _signalRChatService.DeclareArrival(new UserHubModel()
                {
                    Username = _username,
                    Ip = userIp,
                    Port = new Random().Next(10000, 20000).ToString()
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }


        public void Start()
        {

        }

        private void WaveIn_DataAvailable(object? sender, WaveInEventArgs e)
        {
            // Send data to the hub

        }

        public void Stop()
        {

        }
    }
}
