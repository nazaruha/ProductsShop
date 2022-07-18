using EXAM_ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
    /// Interaction logic for AddToBasketWindow.xaml
    /// </summary>
    public partial class AddToBasketWindow : Window
    {
        private string dirScripts { get; set; } = "Scripts";
        private SqlCommand cmd { get; set; }
        private Product product { get; set; }
        private Client client { get; set; }

        bool isUpload = false;

        public AddToBasketWindow(Product product, Client client, SqlCommand cmd)
        {
            InitializeComponent();
            this.product = product;
            this.client = client;
            txtPrice.Text = this.product.Price.ToString() + "$";
            this.product.Count = 1;
            lbProductName.Content = this.product.Name;
            this.cmd = cmd;
            isUpload = true;
        }

        private void imgIncrease_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int count = int.Parse(txtCountToBuy.Text);
                count++;
                product.Count = count;
                txtCountToBuy.Text = count.ToString();
                txtPrice.Text = (product.Price * count).ToString() + "$";
            }
            catch
            {
                return;
            }
            
        }

        private void imgDecrease_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int count = int.Parse(txtCountToBuy.Text);
                if (count == 1) return;
                count--;
                product.Count = count;
                txtCountToBuy.Text = count.ToString();
                txtPrice.Text = (product.Price * count).ToString() + "$";
            }
            catch
            {
                return;
            }
        }

        private void txtCountToBuy_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isUpload) return;
            try
            {
                int count = int.Parse(txtCountToBuy.Text);
                if (count <= 0)
                {
                    txtPrice.Text = "$";
                    return;
                }
                product.Count = count;
                txtPrice.Text = (product.Price * count).ToString() + "$";
            }
            catch
            {
                txtPrice.Text = "$";
            }
        }

        private void btnBasket_Copy_Click(object sender, RoutedEventArgs e)
        {
            int clientId = GetClientId();
            int productId = GetProductId();

            string script = File.ReadAllText($"{dirScripts}\\InsertBasket.sql");
            cmd.CommandText = script;
            cmd.Parameters.AddWithValue("@ClientIdField", clientId);
            cmd.Parameters.AddWithValue("@ProductIdField", productId);
            cmd.Parameters.AddWithValue("@CountField", product.Count);
            cmd.ExecuteNonQuery();
            this.Close();

        }

        private int GetClientId()
        {
            cmd.CommandText = "SELECT Id " +
                "FROM tblClients " +
                "WHERE Name = @NameField";
            cmd.Parameters.AddWithValue("@NameField", client.Name);

            var reader = cmd.ExecuteReader();
            reader.Read();
            int id = int.Parse(reader["Id"].ToString());
            cmd.Parameters.Clear();
            reader.Close();
            return id;
        }

        private int GetProductId()
        {
            cmd.CommandText = "SELECT Id " +
                "FROM tblProducts " +
                "WHERE Name = @NameField";
            cmd.Parameters.AddWithValue("@NameField", product.Name);

            var reader = cmd.ExecuteReader();
            reader.Read();
            int id = int.Parse(reader["Id"].ToString());
            cmd.Parameters.Clear();
            reader.Close();
            return id;
        }
    }
}
