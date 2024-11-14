using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crematory.ViewModels.CreateOrder;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Crematory.DataAccess;
using Crematory.enums;
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
                new ScheduleRepository());
            
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
        private async void Date_Changed(object sender, SelectionChangedEventArgs e)
        {
            await _viewModel.LoadFreeTimeAsync();
        }
    }
}
