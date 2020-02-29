using ObjectPool.Example.Scenario;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace ObjectPool.Example
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private ICommand runWithoutObjectPoolCommand;
        private ICommand runWithMyObjectPoolCommand;

        public MainWindowViewModel()
        {
            runWithoutObjectPoolCommand = null;
            runWithMyObjectPoolCommand = null;

            WithoutObjectPoolRunButtonCaption = "Run example without an object pool";
            WithMicrosoftsObjectPoolRunButtonCaption = "Run example with Microsofts ObjectPool";
            WithMyObjectPoolRunButtonCaption = "Run example with my ObjectPool";
        }

        public string WithoutObjectPoolRunButtonCaption { get; set; }
        public string WithMicrosoftsObjectPoolRunButtonCaption { get; set; }
        public string WithMyObjectPoolRunButtonCaption { get; set; }

        public string OutputText { get; set; }

        public ICommand RunWithoutObjectPoolCommand
        {
            get
            {
                return runWithoutObjectPoolCommand ?? 
                    (runWithoutObjectPoolCommand = new RelayCommand(
                        (obj) =>
                        {
                            var task = new TaskWithoutPool();
                            var result = task.DoTheTask();
                            OutputText = $"{result.SummaryOfCompletedTask}\n" +
                            "\n" +
                            $"TIME SPENT --> {result.MillisecondsSpentOnTask} milliseconds";

                            Notify("OutputText");
                        }));
            }
        }

        public ICommand RunWithMyObjectPoolCommand
        {
            get
            {
                return runWithMyObjectPoolCommand ??
                    (runWithMyObjectPoolCommand = new RelayCommand(
                        (obj) =>
                        {
                            var task = new TaskWithPool();
                            var result = task.DoTheTask();
                            OutputText = $"{result.SummaryOfCompletedTask}\n" +
                            "\n" +
                            $"TIME SPENT --> {result.MillisecondsSpentOnTask} milliseconds";

                            Notify("OutputText");
                        }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Notify(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
