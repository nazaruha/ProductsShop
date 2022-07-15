using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace DB_Worker
{
    class Program
    {
        static string database = "ProductsShop2";
        static string dirSql = "SqlTables";
        static string dirScripts = "Scripts";
        static SqlConnection con;
        static SqlCommand cmd;

        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            string connectionDB = "Data Source=.;Integrated Security=True;";
            con = new SqlConnection(connectionDB);
            con.Open();
            if (!IsDBExists())
            {
                cmd.CommandText = $"CREATE DATABASE {database}";
                cmd.ExecuteNonQuery();
                Console.WriteLine("DB is created"); ;
            }
            con = new SqlConnection(connectionDB + $"Initial Catalog={database}");
            con.Open();
            cmd = con.CreateCommand();
            Console.WriteLine("END");
        }

        static bool IsDBExists()
        {
            cmd = con.CreateCommand();
            cmd.CommandText = "SELECT name FROM master.sys.databases";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader["name"].ToString() == database)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static void GenerateTables()
        {
            string[] tables = { "tblGroups.sql", "tblStudents.sql", "tblSubjects.sql", "tblStudentSubjects.sql" }; // tblSubjects.sql (incorrect syntax near int.)
            foreach (var table in tables)
            {
                ExecuteCommandFromFile(table);
            }
        }

        static void ExecuteCommandFromFile(string file)
        {
            string sql = ReadSqlFile(file);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        static string ReadSqlFile(string file)
        {
            string sql = File.ReadAllText($"{dirSql}\\{file}");
            return sql;
        }
    }
}
