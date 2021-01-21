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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private Task DoWork()
        {
            // ...
            return Task.Delay(4000);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            btnClick.IsEnabled = false;
            lblStatus.Content = "Trwają obliczenia ...";
            //Thread.Sleep(4000);  //brak responsywności
            await DoWork();
            lblStatus.Content = "Obliczenia skończone ...";
            btnClick.IsEnabled = true;
        }
    }
}
