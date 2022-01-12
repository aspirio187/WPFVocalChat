using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VocalChat.WPF.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<MMDevice> AvailableOutputs { get; set; }
        public ObservableCollection<MMDevice> AvailableInputs { get; set; }

        private MMDevice _selectedOutput;

        public MMDevice SelectedOutput
        {
            get { return _selectedOutput; }
            set
            {
                _selectedOutput = value;
                OnPropertyChanged();
            }
        }

        private MMDevice _selectedInput;

        public MMDevice SelectedInput
        {
            get { return _selectedInput; }
            set
            {
                _selectedInput = value;
                OnPropertyChanged();
            }
        }



        public HomeViewModel()
        {
            AvailableOutputs = LoadOutputList();
            AvailableInputs = LoadInputList();
        }

        public ObservableCollection<MMDevice> LoadOutputList()
        {
            ObservableCollection<MMDevice> outputs = new ObservableCollection<MMDevice>();

            using (MMDeviceEnumerator? enumarator = new MMDeviceEnumerator())
            {
                foreach (MMDevice? output in enumarator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
                {
                    outputs.Add(output);
                }
            }

            return outputs;
        }

        public ObservableCollection<MMDevice> LoadInputList()
        {
            ObservableCollection<MMDevice> inputs = new ObservableCollection<MMDevice>();

            using (MMDeviceEnumerator? enumerator = new MMDeviceEnumerator())
            {
                foreach (MMDevice? input in enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active))
                {
                    inputs.Add(input);
                }
            }

            return inputs;
        }
    }
}
