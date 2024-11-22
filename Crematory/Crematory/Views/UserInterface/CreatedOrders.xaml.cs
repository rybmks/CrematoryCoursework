using Crematory.ViewModels.CreatedOrders;
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

namespace Crematory.Views.UserInterface
{
    /// <summary>
    /// Логика взаимодействия для CreatedOrders.xaml
    /// </summary>
    public partial class CreatedOrders : Window
    {
        private readonly CreatedOrdersViewModel _viewModel;
        public CreatedOrders()
        {
            InitializeComponent();
            _viewModel = new();
            DataContext = _viewModel;
        }

        private void BackToMain(object sender, RoutedEventArgs e)
        {
            var m = new MainWindow();
            m.Show();
            this.Hide();
        }
    }
}
