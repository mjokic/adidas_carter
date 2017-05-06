using AdidasBot.Model;
using System;
using System.Collections;
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
using System.Windows.Shapes;

namespace AdidasBot.Windows
{
    /// <summary>
    /// Interaction logic for AssingProxiesWindow.xaml
    /// </summary>
    public partial class AssingProxiesWindow : Window
    {
        private Job job;

        public AssingProxiesWindow()
        {
            InitializeComponent();
            textBlockProxyTo.Text = "All Tasks";
            buttonManualAssingProxy.IsEnabled = false;

            initDataGridAssingProxy();
        }

        public AssingProxiesWindow(Job j)
        {
            this.job = j;

            InitializeComponent();
            textBlockProxyTo.Text = j.PID + "_" + j.Size;

            initDataGridAssingProxy();
        }

        private void initDataGridAssingProxy()
        {
            dataGridAssingProxy.ItemsSource = Manager.proxies;
            dataGridAssingProxy.AutoGenerateColumns = false;
            dataGridAssingProxy.IsReadOnly = true;
            dataGridAssingProxy.SelectionMode = DataGridSelectionMode.Single;

            DataGridTextColumn c = new DataGridTextColumn();
            c.Header = "IP";
            c.Binding = new Binding("IP");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            dataGridAssingProxy.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Port";
            c.Binding = new Binding("Port");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            dataGridAssingProxy.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Username";
            c.Binding = new Binding("Username");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            dataGridAssingProxy.Columns.Add(c);

            c = new DataGridTextColumn();
            c.Header = "Password";
            c.Binding = new Binding("Password");
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            dataGridAssingProxy.Columns.Add(c);

        }

        private void buttonAutoAssingProxies_Click(object sender, RoutedEventArgs e)
        {

            // creating quque because
            // it's easiest way to use
            // proxies over again
            Queue q = new Queue();
            foreach (Proxy p in Manager.proxies)
            {
                q.Enqueue(p);
            }

            // assinging proxies automatically
            foreach (Job j in Manager.jobs)
            {
                // getting proxy from queue
                // assinging it to job
                // putting proxy back to queue
                Proxy currentProxy = q.Dequeue() as Proxy;
                j.Proxy = currentProxy;
                q.Enqueue(currentProxy);

            }

            this.Close();
        }

        private void buttonManualAssingProxy_Click(object sender, RoutedEventArgs e)
        {
            // select proxy from datagrid
            // and assing it to selected job
            Proxy selectedProxy = dataGridAssingProxy.SelectedItem as Proxy;
            Console.WriteLine(selectedProxy);

            if(selectedProxy != null)
            {
                this.job.Proxy = selectedProxy;
                this.Close();

            }
            else
            {
                MessageBox.Show("Select proxy!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }

    }
}
