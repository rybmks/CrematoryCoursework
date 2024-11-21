using Crematory.Interfaces;
using Crematory.Models;
using Crematory.DataAccess;
using Crematory.ViewModels.AdminWindow;
using System.Windows;
using Crematory.enums;

namespace Crematory.Views.AdminWindow
{
    /// <summary>
    /// Логика взаимодействия для AddEditCrematoryPage.xaml
    /// </summary>
    public partial class AddEditCrematoryPage : Window
    {
        private CrematoryModel _currentCrematory = new CrematoryModel();
        private readonly AddEditCrematoryViewModel _viewModel = new AddEditCrematoryViewModel(new CrematoryRepository());
        private readonly EditingPagesStatus _status;

        public AddEditCrematoryPage()
        {
            InitializeComponent();

            _status = EditingPagesStatus.AddNewNote;
            DataContext = _currentCrematory;
            DeleteButton.Visibility = Visibility.Hidden;
        }
        public AddEditCrematoryPage(CrematoryModel crematory)
        {
            InitializeComponent();
            _status = EditingPagesStatus.EditNote;

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

            try
            {
                bool operationResult;
                if (_status == EditingPagesStatus.AddNewNote)
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
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
