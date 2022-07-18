using System.IO;
using EXAM_ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Interaction logic for BasketWindow.xaml
    /// </summary>
    public partial class BasketWindow : Window
    {
        private Client client { get; set; }
        private SqlCommand cmd { get; set; }
        private Product product { get; set; }
        private string connectionDB { get; set; } = "";
        private string database { get; set; } = "";
        private string dirScripts { get; set; } = "Scripts";
        private int clientId = 0;

        public BasketWindow(Client client, SqlCommand cmd, string connectionDB, string database)
        {
            InitializeComponent();
            this.client = client;
            this.cmd = cmd;
            this.connectionDB = connectionDB;
            this.database = database;
            clientId = GetClientId();
            FillDataGrid();
        }

        private void FillDataGrid()
        {
            dgBasket.Items.Clear();

            string script = File.ReadAllText($"{dirScripts}\\viewClient'sBasket.sql");
            cmd.CommandText = script;
            cmd.Parameters.AddWithValue("@ClientIdField", clientId);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ClientBasketProduct clientBasketProduct = new ClientBasketProduct() { Id = int.Parse(reader["Id"].ToString()), Name = reader["Product"].ToString(), Count = int.Parse(reader["Count"].ToString()) };
                    clientBasketProduct.Price = int.Parse(reader["Price"].ToString()) * clientBasketProduct.Count;
                    dgBasket.Items.Add(clientBasketProduct);
                }
            }
            cmd.Parameters.Clear();
        }

        private int GetClientId()
        {
            cmd.Parameters.Clear();
            cmd.CommandText = "SELECT Id " +
                "FROM tblClients " +
                "WHERE Name = @NameField AND Phone = @PhoneField";
            cmd.Parameters.AddWithValue("@NameField", client.Name);
            cmd.Parameters.AddWithValue("@PhoneField", client.Phone);
            var reader = cmd.ExecuteReader();
            reader.Read();
            int id = int.Parse(reader["Id"].ToString());
            cmd.Parameters.Clear();
            reader.Close();
            return id;
        }

        private void imgIncrease_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (dgBasket.SelectedItem == null) return;
            int selectedIndex = dgBasket.SelectedIndex;
            string script = File.ReadAllText($"{dirScripts}\\UpdateBasket.sql");
            cmd.CommandText = script;
            cmd.Parameters.AddWithValue("@NewCountField", (dgBasket.SelectedItem as ClientBasketProduct).Count + 1);
            cmd.Parameters.AddWithValue("@ClientIdField", clientId);
            cmd.Parameters.AddWithValue("@ProductIdField", (dgBasket.SelectedItem as ClientBasketProduct).Id);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            FillDataGrid();
            dgBasket.SelectedIndex = selectedIndex;
        }

        private void imgDecrease_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (dgBasket.SelectedItem == null) return;
            int selectedIndex = dgBasket.SelectedIndex;
            int newCount = (dgBasket.SelectedItem as ClientBasketProduct).Count - 1;
            if (newCount < 1)
            {
                cmd.CommandText = "DELETE tblBasket " +
                    @"WHERE ClientId = @ClientIdField AND ProductId = @ProductIdField";
                cmd.Parameters.AddWithValue("@ClientIdField", clientId);
                cmd.Parameters.AddWithValue("@ProductIdField", (dgBasket.SelectedItem as ClientBasketProduct).Id);
                cmd.ExecuteNonQuery();
            }
            else
            {
                string script = File.ReadAllText($"{dirScripts}\\UpdateBasket.sql");
                cmd.CommandText = script;
                cmd.Parameters.AddWithValue("@NewCountField", newCount);
                cmd.Parameters.AddWithValue("@ClientIdField", clientId);
                cmd.Parameters.AddWithValue("@ProductIdField", (dgBasket.SelectedItem as ClientBasketProduct).Id);
                cmd.ExecuteNonQuery();
            }
            cmd.Parameters.Clear();
            FillDataGrid();
            dgBasket.SelectedIndex = selectedIndex;
        }

        private void btnPurchase_Click(object sender, RoutedEventArgs e)
        {
            int totalPrice = 0;
            foreach (var products in dgBasket.Items)
            {
                totalPrice += (products as ClientBasketProduct).Price;
            }

            cmd.CommandText = "DELETE tblBasket " +
                @"WHERE ClientId = @ClientIdField";
            cmd.Parameters.AddWithValue("@ClientIdField", clientId);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            FillDataGrid();
            MessageBox.Show("Your total price is: " + totalPrice, "Kinda Check", MessageBoxButton.OK);
        }
    }
}
