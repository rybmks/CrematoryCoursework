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
    /// Логика взаимодействия для AddEditSchedulePage.xaml
    /// </summary>
    public partial class AddEditSchedulePage : Window
    {
        private CrematoryScheduleModel _currentSchedule = new CrematoryScheduleModel();
        private readonly AddEditScheduleViewModule _viewModel = new AddEditScheduleViewModule();
        private readonly PageFunctionStatus _status;

        public AddEditSchedulePage(int crematoryId)
        {
            InitializeComponent();

            _status = PageFunctionStatus.AddNewNote;
            _currentSchedule.CrematoryId = crematoryId;
            DataContext = _currentSchedule;
            DeleteButton.Visibility = Visibility.Hidden;
        }
        public AddEditSchedulePage(CrematoryScheduleModel schedule)
        {
            InitializeComponent();
            _status = PageFunctionStatus.EditNote;

            DeleteButton.Visibility = Visibility.Visible;
            _currentSchedule = schedule;
            DataContext = _currentSchedule;
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

            var operationResult = await _viewModel.DeleteSchedule(_currentSchedule);
            MessageBox.Show(operationResult ? "Операція пройшла успішно" : "Виникла помилка при виконанні операції");

            Back();
        }
        public async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {

            if (!ValidateData(_currentSchedule))
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
                operationResult = await _viewModel.AddSchedule(_currentSchedule);
            }
            else
            {
                operationResult = await _viewModel.UpdateSchedule(_currentSchedule);
            }
            string message = operationResult
                 ? "Операція пройшла успішно."
                 : "Виникла помилка при виконанні операції. Переконайтесь, що новий розклад не накладається на вже існуючі записи.";

            MessageBox.Show(message, operationResult ? "Успіх" : "Помилка",
                MessageBoxButton.OK,
                operationResult ? MessageBoxImage.Information : MessageBoxImage.Warning);
           
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
        private bool ValidateData(CrematoryScheduleModel schedule)
        {
            if (String.IsNullOrWhiteSpace(schedule.DayOfWeek))
            {
                MessageBox.Show("Заповніть поле 'День'!");
                return false;
            }
            if (!TimeSpan.TryParse(schedule.OpenTime.ToString(), out _) || 
                !TimeSpan.TryParse(schedule.CloseTime.ToString(), out _))
            {
                MessageBox.Show("Час внесено неправильно!");
                return false;
            }
            if (schedule.OpenTime > schedule.CloseTime)
            {
                MessageBox.Show("Відкриття має бути раніше, ніж закриття!");
                return false;
            }

            return true;
        }
    }
}

