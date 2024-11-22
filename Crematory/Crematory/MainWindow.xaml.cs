using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Crematory.Views.AdminWindow;
using Crematory.Views.UserInterface;

namespace Crematory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ToAdminPanel(object sender, RoutedEventArgs e)
        {
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.Show();
            this.Close();
        }

        private void CreateNote_Click(object sender, RoutedEventArgs e)
        {
            CreateOrderWindow createOrder = new CreateOrderWindow();
            createOrder.Show();
            this.Close();
        }

        private void CheckNotes_Click(object sender, RoutedEventArgs e)
        {
            CreatedOrders createdOrders = new CreatedOrders();
            createdOrders.Show();
            this.Close();
        }
    }
}