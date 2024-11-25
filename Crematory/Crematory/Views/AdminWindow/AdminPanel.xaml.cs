using Crematory.Interfaces;
using Crematory.DataAccess;
using Crematory.ViewModels.AdminWindow;
using System.Windows;
using System.Windows.Controls;
using Crematory.Models.DatabaseModels;

namespace Crematory.Views.AdminWindow
{
    public partial class AdminPanel : Window
    {
        private readonly AdminPanelViewModel _viewModel;
        public AdminPanel()
        {
            InitializeComponent();
            _viewModel = new AdminPanelViewModel(new ServiceRepository(), new CrematoryRepository(), new ScheduleRepository());

            DataContext = _viewModel;
        }
        
        private async void UpdateForm()
        {
            await _viewModel.LoadServicesAsync();
            await _viewModel.LoadCrematoriesAsync();
        }
        public void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button serviceButton && serviceButton.DataContext is ServiceModel service)
            {
                var a = new AddEditServiceNotePage(service);
                a.Show();
                
            }
            else if (sender is Button crematoryButton && crematoryButton.DataContext is CrematoryModel crematory)
            {
                var a = new AddEditCrematoryPage(crematory);
                a.Show();
            }
            else if (sender is Button scheduleButton && scheduleButton.DataContext is CrematoryScheduleModel schedule)
            {
                object c = CrematoryComboBox.SelectedItem;
                if (CrematoryComboBox.SelectedItem is CrematoryModel cr)
                {
                    schedule.CrematoryId = cr.Id;
                }
                else
                {
                    MessageBox.Show("Оберіть крематорій");
                    return;
                }

                var a = new AddEditSchedulePage(schedule);
                a.Show();
            }

            this.Hide();
        }
        public void Page_VisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                UpdateForm();
            }
        }
        public void AddServiceNote_Click(object sender, RoutedEventArgs e)
        {
            var a = new AddEditServiceNotePage();
            a.Show();
            this.Hide();
        }
        public void AddCrematoryNote_Click(object sender, RoutedEventArgs e)
        {
            var a = new AddEditCrematoryPage();
            a.Show();
            this.Hide();
        }
        public void AddScheduleNote_Click(object sender, RoutedEventArgs e)
        {
            object c = CrematoryComboBox.SelectedItem;
            int id;
            if (CrematoryComboBox.SelectedItem is CrematoryModel cr)
            {
                id = cr.Id;
            }
            else
            {
                MessageBox.Show("Оберіть крематорій");
                return;
            }

            var a = new AddEditSchedulePage(id);
            a.Show();
            this.Hide();
        }
        private void BackToMain(object sender, RoutedEventArgs e)
        {
            var m = new MainWindow();
            m.Show();
            this.Hide();
        }
        private async void CrematoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CrematoryComboBox.SelectedItem is CrematoryModel crematory)
            {
                await _viewModel.LoadScheduleAsync(crematory);
            }
            else
            {
                _viewModel.CrematoryScheduleData.Clear();
            }
        }
    }
}
