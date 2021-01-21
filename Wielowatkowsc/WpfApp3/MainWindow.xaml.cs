using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker bg = new BackgroundWorker();
        public MainWindow()
        {
            InitializeComponent();
            bg.WorkerReportsProgress = true;
            bg.WorkerSupportsCancellation = true;

            bg.DoWork += DoWork_Handler;
            bg.ProgressChanged += ProgressChanged_Handler;
            bg.RunWorkerCompleted += RunWorkerCompleted_Handler;
        }

        private void DoWork_Handler( object sender, DoWorkEventArgs args )
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            for(int i = 1; i <=10; i++ )
            {
                if( worker.CancellationPending )
                {
                    args.Cancel = true;
                    break;
                }
                else
                {
                    worker.ReportProgress(i * 10);
                    
                    DoWork();
                }
            }
        }

        private void DoWork()
        {
            // ...
            Thread.Sleep(500);
            //return Task.Delay(500);
        }

        private void ProgressChanged_Handler(object sender, ProgressChangedEventArgs args)
        {
            progressBar.Value = args.ProgressPercentage;
        }

        private void RunWorkerCompleted_Handler(object sender, RunWorkerCompletedEventArgs args )
        {
            progressBar.Value = 0;
            if (args.Cancelled)
                MessageBox.Show("Przerwane");
            else
                MessageBox.Show("Normalne zakończenie");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            bg.CancelAsync();
            btnCancel.IsEnabled = false;
            btnProcess.IsEnabled = true;
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            btnCancel.IsEnabled = true;
            btnProcess.IsEnabled = false;
            bg.RunWorkerAsync();
        }
    }
}
