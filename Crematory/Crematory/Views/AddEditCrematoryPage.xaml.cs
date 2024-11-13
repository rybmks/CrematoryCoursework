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
    /// Логика взаимодействия для AddEditCrematoryPage.xaml
    /// </summary>
    public partial class AddEditCrematoryPage : Window
    {
        private CrematoryModel _currentCrematory = new CrematoryModel();
        private readonly AddEditCrematoryViewModel _viewModel = new AddEditCrematoryViewModel();
        private readonly PageFunctionStatus _status;

        public AddEditCrematoryPage()
        {
            InitializeComponent();

            _status = PageFunctionStatus.AddNewNote;
            DataContext = _currentCrematory;
            DeleteButton.Visibility = Visibility.Hidden;
        }
        public AddEditCrematoryPage(CrematoryModel crematory)
        {
            InitializeComponent();
            _status = PageFunctionStatus.EditNote;

            DeleteButton.Visibility = Visibility.Visible;
            _currentCrematory = crematory;
            DataContext = _currentCrematory;
        }

        public async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Ви впевнені, що хочете видалити цей елемент?",
                "Підтвердження видалення",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
                return;

            var operationResult = await _viewModel.DeleteCrematory(_currentCrematory);
            MessageBox.Show(operationResult ? "Операція пройшла успішно" : "Виникла помилка при виконанні операції");

            Back();
        }
        public async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {

            if (!ValidateData(_currentCrematory))
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
                operationResult = await _viewModel.AddCrematory(_currentCrematory);
            }
            else
            {
                operationResult = await _viewModel.UpdateCrematory(_currentCrematory);
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
        private bool ValidateData(CrematoryModel crematory)
        {
            if (String.IsNullOrWhiteSpace(crematory.Name))
            {
                MessageBox.Show("Заповніть поле 'Назва'!");
                return false;
            }
            if (String.IsNullOrWhiteSpace(crematory.Address))
            {
                MessageBox.Show("Заповніть поле 'Адреса'!");
                return false;
            }
            if (String.IsNullOrWhiteSpace(crematory.ContactInfo))
            {
                MessageBox.Show("Заповніть поле 'Контактна інформація'!");
                return false;
            }


            return true;
        }
    }
}
