using Crematory.Models;
using Crematory.ViewModels;
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

namespace Crematory.Views
{
    /// <summary>
    /// Логика взаимодействия для AddEditServiceNotePage.xaml
    /// </summary>
    /// 
    public partial class AddEditServiceNotePage : Window
    {
        private ServiceModel _currentService = new ServiceModel();
        private readonly AddEditServiceViewModel _viewModel = new AddEditServiceViewModel();
        private readonly PageFunctionStatus _status;

        public AddEditServiceNotePage()
        {
            InitializeComponent();

            _status = PageFunctionStatus.AddNewNote;
            DataContext = _currentService;
            DeleteButton.Visibility = Visibility.Hidden;
        }
        public AddEditServiceNotePage(ServiceModel service)
        {
            InitializeComponent();
            _status = PageFunctionStatus.EditNote;

            DeleteButton.Visibility = Visibility.Visible;
            _currentService = service;
            DataContext = _currentService;
        }

        public async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Ви впевнені, що хочете видалити цей елемент?",
                "Підтвердження видалення",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning );

            if (result == MessageBoxResult.No)
                return;

            var operationResult = await _viewModel.DeleteService(_currentService);
             MessageBox.Show(operationResult ? "Операція пройшла успішно" : "Виникла помилка при виконанні операції");
            
            Back();
        }
        public async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {

            if (!ValidateData(_currentService))
                return;

            MessageBoxResult result = MessageBox.Show(
                "Ви впевнені, що хочете продовжити операцію?",
                "Підтвердження",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
                return;

            bool operationResult;
            if (_status == PageFunctionStatus.AddNewNote)
            {
                operationResult = await _viewModel.AddService(_currentService);
            }
            else
            {
                operationResult = await _viewModel.UpdateService(_currentService);
            }
            MessageBox.Show(operationResult ? "Операція пройшла успішно" : "Виникла помилка при виконанні операції");

            Back();
        }
        private void Back()
        {
            var adminPanel = Application.Current.Windows.OfType<AdminPanel>().FirstOrDefault();

            if (adminPanel != null)
            {
                adminPanel.Show();
            }
            else
            {
                var newAdminPanel = new AdminPanel();
                newAdminPanel.Show();
            }

            this.Close();
        }
        private void Back(object sender, RoutedEventArgs e)
        {
            Back();
        }
        private bool ValidateData(ServiceModel service)
        {
            if (String.IsNullOrWhiteSpace(service.Name)) 
            {
                MessageBox.Show("Заповніть поле 'Назва'!");
                return false;
            }

            if (service.Price <= 0)
            {
                MessageBox.Show("Ціна вказана неправильно!");
                return false;
            }

            return true;
        }
    }
}
