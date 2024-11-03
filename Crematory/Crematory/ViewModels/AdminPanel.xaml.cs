using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Crematory.ViewModels
{
    public partial class AdminPanel : Window, INotifyPropertyChanged
    {
        private string? _selectedItem;

        public ObservableCollection<string> Items { get; set; }

        public string? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }

        private MainWindow _mainWindow;

        public AdminPanel(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            Items = new ObservableCollection<string> { "Крематорій", "Розклад", "Послуги" };

            DataContext = this;
        }

        private void BackToMain(object sender, RoutedEventArgs e)
        {
            _mainWindow.Show();
            this.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
