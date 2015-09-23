using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IndustrialMonitoring.ProcessDataServiceReference;
using SharedLibrary;

namespace IndustrialMonitoring
{
    /// <summary>
    /// Interaction logic for WindowHorn.xaml
    /// </summary>
    public partial class WindowHorn : Window
    {
        private ProcessDataServiceClient _processDataServiceClient;
        private Timer _timer;

        public WindowHorn()
        {
            InitializeComponent();
        }

        public ProcessDataServiceClient ProcessDataService
        {
            get { return _processDataServiceClient; }
            set { _processDataServiceClient = value; }
        }

        public Timer Timer1
        {
            get { return _timer; }
            set { _timer = value; }
        }

        private void WindowHorn_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Thread t1 = new Thread(() =>
                      {
                          var horn = ProcessDataService.GetMuteHorn();

                          Dispatcher.BeginInvoke(new Action(() =>
                          {
                              PowerButtonState.Value = horn;
                          }));
                      });

                t1.Start();

                //Timer1=new Timer(Fetch,null,0,2000);

                Timer1 = new Timer(state =>
                  {
                      bool hornHMI = ProcessDataService.GetHornHMI();
                      bool horn2 = ProcessDataService.GetHorn();
                      bool muteHorn = ProcessDataService.GetMuteHorn();

                      Dispatcher.BeginInvoke(new Action(() =>
                      {
                          LedHorn.Value = horn2;
                          LedAlarm.Value = hornHMI;
                          PowerButtonState.Value = muteHorn;
                          BusyIndicator2.IsBusy = false;
                      }));
                  }, null, 0, 2000);
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void PowerButtonState_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            try
            {
                if (PowerButtonState.Value)
                {
                    TextBlockState.Text = "Mute";
                }
                else
                {
                    TextBlockState.Text = "Normal";
                }
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void PowerButtonState_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                BusyIndicator2.IsBusy = true;

                if (PowerButtonState.Value)
                {
                    ProcessDataService.MuteHorn();
                }
                else
                {
                    ProcessDataService.UnMuteHorn();
                }
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                MessageBox.Show(ex.Message);
            }
        }
    }
}
