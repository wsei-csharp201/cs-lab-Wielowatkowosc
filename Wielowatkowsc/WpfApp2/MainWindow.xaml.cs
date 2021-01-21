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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationToken ct;
        CancellationTokenSource cts;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            btnCancel.IsEnabled = true;
            btnProcess.IsEnabled = false;

            cts = new CancellationTokenSource();
            ct = cts.Token;

            for (int i = 0; i < 10; i++)
            {
                if (ct.IsCancellationRequested)
                    break;

                try
                {
                    await DoWork();
                    progressBar.Value = (i + 1) * 10;
                }
                catch(TaskCanceledException)
                {
                    progressBar.Value = i * 10;
                }
            }
            if( ct.IsCancellationRequested )
                MessageBox.Show("Zadanie przerwane");
            else
                MessageBox.Show("Zadanie skończone");

            progressBar.Value = 0;
            btnCancel.IsEnabled = false;
            btnProcess.IsEnabled = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            btnCancel.IsEnabled = false;
            btnProcess.IsEnabled = true;

            cts.Cancel();
        }

        private Task DoWork()
        {
            // ...
            return Task.Delay(500);
        }
    }
}
