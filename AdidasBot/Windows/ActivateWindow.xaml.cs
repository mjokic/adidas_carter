using AdidasBot;
using Cryptlex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace AdidasCarterPro.Windows
{
    /// <summary>
    /// Interaction logic for ActivateWindow.xaml
    /// </summary>
    public partial class ActivateWindow : Window
    {
        public ActivateWindow()
        {
            InitializeComponent();
        }


        private bool checkProcess()
        {
            Process[] processes = Process.GetProcesses();

            int x = 0;
            foreach (Process process in processes)
            {
                if (process.ProcessName == "AdidasCarterEco")
                {
                    x += 1;
                }
            }

            if (x < 2)
            {
                return true;
            }

            return false;
        }

        private void buttonActivate_Click(object sender, RoutedEventArgs e)
        {
            string alias = textBoxAlias.Text;
            string pKey = textBoxLicenseKey.Text;

            if(alias == string.Empty)
            {
                MessageBox.Show("Alias can't be empty!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int status;
            LexActivator.SetExtraActivationData(alias);
            

            status = LexActivator.SetProductKey(pKey);

            if(status == LexActivator.LA_OK)
            {
                if (LexActivator.ActivateProduct() == LexActivator.LA_OK)
                {
                    MessageBox.Show("License Activated! Run software again!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }else
                {
                    MessageBox.Show("License key is not valid!", "Error " + status, MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                MessageBox.Show("License key is not valid!", "Error " + status, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}
