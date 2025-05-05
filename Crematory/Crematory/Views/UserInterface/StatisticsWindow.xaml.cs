using Crematory.DataAccess;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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

namespace Crematory.Views.UserInterface
{
    public partial class StatisticsWindow : Window
    {
        public StatisticsWindow()
        {
            InitializeComponent();
            LoadServiceStats();
            LoadMoneyStats();
        }
        private void LoadMoneyStats()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString;
            string query = SqlQueries.MonthRevenue;

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    servicesDataGrid2.ItemsSource = dt.DefaultView;
                }
            }
        }

        private void LoadServiceStats()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString;
            string query = SqlQueries.MostPopularServices;

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    servicesDataGrid.ItemsSource = dt.DefaultView;
                }
            }
        }
    }
}
