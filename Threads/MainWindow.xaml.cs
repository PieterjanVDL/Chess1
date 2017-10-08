using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Threading;

namespace Threads
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

        

        private void btnPrintNumbers_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= 10; i++)
            {
                listBoxNumbers.Items.Add(i);
            }
        }

        private void btnTimeConsumingWork_Click(object sender, RoutedEventArgs e)
        {

            btnTimeConsumingWork.IsEnabled = false;
            btnPrintNumbers.IsEnabled = false;

            Thread workerThread = new Thread(DoTimeConsumingWork);
            workerThread.Start();
            btnTimeConsumingWork.IsEnabled = true;
            btnPrintNumbers.IsEnabled = true;

        }

        private void DoTimeConsumingWork() {

            Thread.Sleep(5000);
        }
    }


}
