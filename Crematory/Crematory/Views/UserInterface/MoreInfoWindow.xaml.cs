using Crematory.Models.AppModels;
using Crematory.Models.DatabaseModels;
using Crematory.ViewModels.CreatedOrders;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Crematory.Views.UserInterface
{
    public partial class MoreInfoWindow : Window, INotifyPropertyChanged
    {
        
        private DeceasedModel _deceased = new();
        private ContactPersonModel _contactPerson = new();
        public event PropertyChangedEventHandler? PropertyChanged;
        public DeceasedModel Deceased
        {
            get => _deceased;
            set
            {
                _deceased = value;
                OnPropertyChanged();
            }
        }
        public ContactPersonModel ContactPerson
        {
            get => _contactPerson;
            set
            {
                _contactPerson = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ServiceModel> Services { get; set; } = [];

        private readonly FullOrderInfoModel _fullOrderInfo;
        private readonly CreatedOrdersViewModel _viewModel;
        public decimal StandardPrice { get; }
        public MoreInfoWindow(CreatedOrdersViewModel viewModel, FullOrderInfoModel fullOrderInfo)
        {
            InitializeComponent();
            _fullOrderInfo = fullOrderInfo;
            _viewModel = viewModel;
            LoadFullInfo();
            DataContext = this;
        }
        private async void LoadFullInfo()
        {
            Deceased = await _viewModel.GetDeceasedByIdAsync(_fullOrderInfo.DeceasedId);
            ContactPerson = await _viewModel.GetContactPersonByIdAsync(_fullOrderInfo.ContactPersonId);

            Services.Clear();
            var services = await _viewModel.LoadOrderedServicesAsync(_fullOrderInfo.OrderId);

            foreach (var s in services)
            {
                Services.Add(s);
            }
        }
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
