using EXAM_ProductShop.Models;
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
using System.Windows.Shapes;

namespace EXAM_ProductShop
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        public LogInWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (CheckTxts())
            {
                Client client = new Client() { Name = txtName.Text, Phone = txtPhone.Text };
                MainWindow mainWindow = new MainWindow(client);
                this.Close();
                mainWindow.ShowDialog();
            }
        }

        private bool CheckTxts()
        {
            if (String.IsNullOrWhiteSpace(txtName.Text) || String.IsNullOrWhiteSpace(txtPhone.Text))
                return false;
            return true;
        }
    }
}
