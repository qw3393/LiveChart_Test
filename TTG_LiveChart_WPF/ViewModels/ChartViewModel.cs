using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Configurations;
using TTG_LiveChart_WPF.Models;

namespace TTG_LiveChart_WPF.ViewModels
{
    public class ChartViewModel : ViewModelBase
    {
        private ChartValues<float> _chartValues;
        private ChartValues<PoltModel> _chartValues2;
        private ICommand _updateChartCommand;
        private ICommand _startChartCommand;
        private ICommand _stopChartCommand;
        private float _trend;
        //public ChartValues<PoltModel> ChartValues2 { get; set; }
        public ChartViewModel()
        {
            var mapper = Mappers.Xy<PoltModel>().Y(model => model.Value);           //use the value property as Y

            //lets save the mapper globally.
            Charting.For<PoltModel>(mapper);
            ChartValues2 = new ChartValues<PoltModel>();
            IsReading = false;
        }

        public ChartValues<float> ChartValues
        {
            get
            {
                return _chartValues;
            }
            set
            {
                if (_chartValues != value)
                {
                    _chartValues = value;
                    OnChanged("ChartValues");
                }
            }
        }
        public ChartValues<PoltModel> ChartValues2
        {
            get
            {
                return _chartValues2;
            }
            set
            {
                if (_chartValues2 != value)
                {
                    _chartValues2 = value;
                    OnChanged("ChartValues2");
                }
            }
        }

        public ICommand UpdateChartCommand
        {
            get
            {
                if (_updateChartCommand == null)
                    _updateChartCommand = new RelayCommand(() => Plot());
                return _updateChartCommand;
            }
        }
        public ICommand StartChartCommand
        {
            get
            {
                if (_startChartCommand == null)
                    _startChartCommand = new RelayCommand(() => PlotStart());
                return _startChartCommand;
            }
        }
        public ICommand StopChartCommand
        {
            get
            {
                if (_stopChartCommand == null)
                    _stopChartCommand = new RelayCommand(() => PlotStop());
                return _stopChartCommand;
            }
        }

        private void Plot()
        {
            var r = new Random();
            //ChartValues = new ChartValues<float> { 7, 4, 5, 6};
            float[] a = { r.Next(-10, 10), r.Next(-10, 10), r.Next(-10, 10), r.Next(-10, 10) };
            ChartValues = new ChartValues<float>(a);
        }
        private void PlotStart()
        {
            IsReading = true;
            if (IsReading) Task.Factory.StartNew(Read);
        }
        private void PlotStop()
        {
            IsReading = false;
        }
        public bool IsReading { get; set; }

        private void Read()
        {
            var r = new Random();

            while (IsReading)
            {
                Thread.Sleep(50);

                _trend += r.Next(-8, 10);

                ChartValues2.Add(new PoltModel
                {
                    Value = _trend
                });

                //lets only use the last 150 values
                if (ChartValues2.Count > 150) ChartValues2.RemoveAt(0);
            }
        }
    }
}
