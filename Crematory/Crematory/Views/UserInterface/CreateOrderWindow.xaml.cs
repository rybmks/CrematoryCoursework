using Crematory.ViewModels.CreateOrder;
using System.Windows;
using Crematory.DataAccess;
using Crematory.Interfaces;
using Crematory.Models;

namespace Crematory.Views.UserInterface
{
    /// <summary>
    /// Логика взаимодействия для CreateOrderWindow.xaml
    /// </summary>
    public partial class CreateOrderWindow : Window
    {
        private readonly CreateOrderViewModel _viewModel;
        public CreateOrderWindow()
        {
            InitializeComponent();

            _viewModel = new CreateOrderViewModel(
                new ServiceRepository(), new CrematoryRepository(), 
                new DeceasedRepository(), new ContactPersonRepository(),
                new ScheduleRepository(),
                new OrderRepository());
            
            DataContext = _viewModel;
        }
        private async void UpdateForm()
        {
            await _viewModel.LoadServicesAsync();
            await _viewModel.LoadCrematoriesAsync();
        }
        public void Page_VisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                UpdateForm();
            }
        }
        private void BackToMain(object sender, RoutedEventArgs e)
        {
            var m = new MainWindow();
            m.Show();
            this.Hide();
        }
        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.CreateOrderNote();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadFreeTimeAsync();
        }

        private void GetServicesPrice(object sender, RoutedEventArgs e)
        {
            decimal price = _viewModel.GetServicePrice();
            MessageBox.Show($"Ціна за додаткові послуги: {price}");
        }

        private void Checkbox_Unchecked(object sender, RoutedEventArgs e) => _viewModel.ChangeCheckboxState(sender, false);
        
        private void CheckBox_Checked(object sender, RoutedEventArgs e) => _viewModel.ChangeCheckboxState(sender, true);
        
    }
}
