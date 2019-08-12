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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Win32;

namespace Lab_SQLServer_Users
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connString;
        string FilePath = "C:/Users/Negro/source/repos/Lab-SQLServer-Users/Lab-SQLServer-Users";
        SqlConnection sqlConn;
        
        public MainWindow()
        {
            // Step 1: Get the connection string from the json file
            ReadConnectionString();

            // Step 2: Open a SQL connection
            OpenSQLConnection();

            // Step 3: Create WPF
            InitializeComponent();

            // Step 4: Load the DataGrid with data from database
            SetDataGrid();

            // Step 5: Set the text box to display the connection string
            SetTextBoxConnectionString();
        }
       
        private void ReadConnectionString()
        {
            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.SetBasePath(FilePath);
            configurationBuilder.AddJsonFile("config.json");

            IConfiguration config = configurationBuilder.Build();

            connString = config["Data:DefaultConnection:ConnectionString"];
        }

        private void OpenSQLConnection()
        {
            // Open the sql connection
            sqlConn = new SqlConnection(connString);
            sqlConn.Open();
        }

        private void SetDataGrid()
        {
            string sql = "SELECT * FROM Login";

            SqlCommand command = new SqlCommand(sql, sqlConn);

            SqlDataReader reader = command.ExecuteReader();

            DataGridLogin.ItemsSource = reader;
        }

        private void SetTextBoxConnectionString()
        {
            TextBoxConnectionString.Text = connString;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddData();
        }

        private void AddData()
        {
            string sql = string.Format(
                                "INSERT INTO Login" +
                                "(First, Last, Email, Password) Values" +
                                "('{0}', '{1}', '{2}', '{3}')", 
                                TextBoxFirst.Text, TextBoxLast.Text, 
                                TextBoxEmail.Text, PasswordBoxPassword.Password);

            SqlCommand command = new SqlCommand(sql, sqlConn);

            int rowsAffected = command.ExecuteNonQuery();

            ClearTextBoxes();

            SetDataGrid();
        }

        private void ClearTextBoxes()
        {
            TextBoxFirst.Clear();
            TextBoxLast.Clear();
            TextBoxEmail.Clear();
            PasswordBoxPassword.Clear();

            TextBoxFirst.Focus();
        }
    }
}
