using AdidasBot;
using Cryptlex;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
    public partial class ActivateWindow : MetroWindow
    {
        private MetroDialogSettings dialogSettings = new MetroDialogSettings {
            MaximumBodyHeight = 25, DialogMessageFontSize = 16, DialogTitleFontSize = 20 };

        public ActivateWindow()
        {
            InitializeComponent();
        }

        private async void buttonActivate_Click(object sender, RoutedEventArgs e)
        {
            string alias = textBoxAlias.Text;
            string pKey = textBoxLicenseKey.Text;

            if(alias == string.Empty)
            {
                await this.ShowMessageAsync("Error!", "Alias can't be empty!", MessageDialogStyle.Affirmative, this.dialogSettings);
                return;
            }

            int status;
            LexActivator.SetExtraActivationData(alias);
            

            status = LexActivator.SetProductKey(pKey);

            if(status == LexActivator.LA_OK)
            {
                if (LexActivator.ActivateProduct() == LexActivator.LA_OK)
                {
                    await this.ShowMessageAsync("Success!", "License Activated! Run software again!", MessageDialogStyle.Affirmative, this.dialogSettings);
                    this.Close();
                }else
                {
                    await this.ShowMessageAsync("Error!", "License key is not valid!", MessageDialogStyle.Affirmative, this.dialogSettings);
                }

            }
            else
            {
                await this.ShowMessageAsync("Error!", "License key is not valid!", MessageDialogStyle.Affirmative, this.dialogSettings);
            }

        }

    }
}
