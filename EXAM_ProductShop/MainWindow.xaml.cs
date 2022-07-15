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

namespace EXAM_ProductShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string database { get; set; } = "ProductsShop2";
        public string connectionDB { get; set; } = "Data Source=.;Integrated Security=True;";
        private SqlConnection con { get; set; }
        private SqlCommand cmd { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            con = new SqlConnection(connectionDB + $"Initial Catalog={database}");
            con.Open();
            cmd = con.CreateCommand();

            GetCategories();

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

        private void cb_Categoriies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_Categoriies.SelectedIndex == -1)
            {
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


        }
    }
}
