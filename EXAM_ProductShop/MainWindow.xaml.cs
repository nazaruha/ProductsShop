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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using EXAM_ProductShop.Models;

namespace EXAM_ProductShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string database { get; set; } = "ProductsShop2";
        private string dirScripts { get; set; } = "Scripts";
        public string connectionDB { get; set; } = "Data Source=.;Integrated Security=True;";
        private SqlConnection con { get; set; }
        private SqlCommand cmd { get; set; }

        public MainWindow(Client client)
        {
            InitializeComponent();

            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            con = new SqlConnection(connectionDB + $"Initial Catalog={database}");
            con.Open();
            cmd = con.CreateCommand();

            AddClientToDB(client);
            GetCategories();

        }

        private void AddClientToDB(Client client)
        {
            string script = File.ReadAllText($"{dirScripts}\\viewCurrentClient.sql");
            cmd.CommandText = script;
            cmd.Parameters.AddWithValue("@NameField", client.Name);
            cmd.Parameters.AddWithValue("@PhoneField", client.Phone);

            var reader = cmd.ExecuteReader();
            reader.Read();
            try
            {
                Client checkClient = new Client() { Name = reader["Name"].ToString(), Phone = reader["Phone"].ToString() };
            }
            catch
            {
                script = File.ReadAllText($"{dirScripts}\\InsertClient.sql");
                cmd.CommandText = script;
                cmd.ExecuteNonQuery();
            }
            reader.Close();
        }

        private void GetCategories()
        {
            cmd.CommandText = "SELECT Name " +
                "FROM tblCategories";

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    cb_Categoriies.Items.Add(reader["Name"]);
                }
            }
        }

        private int GetCategoryId()
        {
            cmd.CommandText = "SELECT Id " +
                "FROM tblCategories " +
                @"WHERE Name = @SelectedName";
            cmd.Parameters.AddWithValue("@SelectedName", cb_Categoriies.SelectedItem.ToString());

            var reader = cmd.ExecuteReader();
            reader.Read();

            int index = Int32.Parse(reader["Id"].ToString());
            cmd.Parameters.Clear();
            reader.Close();
            return index;
        }

        private int GetSubCategoryId()
        {
            cmd.CommandText = "SELECT Id " +
                "FROM tblSubCategories " +
                @"WHERE Name = @SelectedName";
            cmd.Parameters.AddWithValue("@SelectedName", cb_SubCategories.SelectedItem.ToString());

            var reader = cmd.ExecuteReader();
            reader.Read();

            int index = Int32.Parse(reader["Id"].ToString());
            cmd.Parameters.Clear();
            reader.Close();
            return index;
        }

        private void cb_Categoriies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_Categoriies.SelectedIndex == -1)
            {
                ClearDataGrid();
                cb_Categoriies.Items.Clear();
                return;
            }
            cb_SubCategories.Items.Clear();

            int CategoryId = GetCategoryId();
            cmd.CommandText = "SELECT Name " +
                "FROM tblSubCategories " +
                @"WHERE CategoryId = @Id";
            cmd.Parameters.AddWithValue("@Id", CategoryId);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    cb_SubCategories.Items.Add(reader["Name"].ToString());
                }
            }
            FillDataGridByCategory(CategoryId);
        }

        private void cb_SubCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_SubCategories.SelectedIndex == -1)
            {
                ClearDataGrid();
                int CategoryId = GetCategoryId();
                FillDataGridByCategory(CategoryId);
                return;
            }

            int SubCateryId = GetSubCategoryId();
            FillDataGridBySubCategory(SubCateryId);
        }

        private void FillDataGridByCategory(int CategoryId)
        {
            if (cb_Categoriies.SelectedIndex == -1)
            {
                ClearDataGrid();
                return;
            }

            string script = File.ReadAllText($"{dirScripts}\\viewProductsByCategory.sql");
            cmd.CommandText = script;
            cmd.Parameters.AddWithValue("@IdField", CategoryId);

            List<Product> products = new List<Product>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Product product = new Product() { Id = Int32.Parse(reader["Id"].ToString()), Name = reader["Product"].ToString(), Price = Int32.Parse(reader["Price"].ToString()), Category = reader["Category"].ToString(), SubCategory = reader["Sub-Category"].ToString() };
                    dgProducts.Items.Add(product);
                }
            }
        }

        private void FillDataGridBySubCategory(int SubCategoryId)
        {
            ClearDataGrid();
            if (cb_SubCategories.SelectedIndex == -1) return;

            string script = File.ReadAllText($"{dirScripts}\\viewProductsBySubCategory.sql");
            cmd.CommandText = script;
            cmd.Parameters.AddWithValue("@IdField", SubCategoryId);

            List<Product> products = new List<Product>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Product product = new Product() { Id = Int32.Parse(reader["Id"].ToString()), Name = reader["Product"].ToString(), Price = Int32.Parse(reader["Price"].ToString()), Category = reader["Category"].ToString(), SubCategory = reader["Sub-Category"].ToString() };
                    dgProducts.Items.Add(product);
                }
            }
        }

        private void ClearDataGrid()
        {
            dgProducts.Items.Clear();
        }

        
    }
}
