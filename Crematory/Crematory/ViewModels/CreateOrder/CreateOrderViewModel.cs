using Crematory.enums;
using Crematory.Interfaces;
using Crematory.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Crematory.ViewModels.CreateOrder
{
    public class CreateOrderViewModel(
        IServiceRepository serviceRepository, ICrematoryRepository crematoryRepository,
        IDeceasedRepository deceasedRepository, IContactPersonRepository contactPersonRepository,
        IScheduleRepository sheduleRepository, IOrderRepository orderRepository) : INotifyPropertyChanged
    {
        private const EditingPagesStatus addNewNote = EditingPagesStatus.AddNewNote;
        private readonly IScheduleRepository _scheduleRepository = sheduleRepository;
        private readonly IServiceRepository _serviceRepository = serviceRepository;
        private readonly ICrematoryRepository _crematoryRepository = crematoryRepository;
        private readonly IDeceasedRepository _deceasedRepository = deceasedRepository;
        private readonly IContactPersonRepository _contactPersonRepository = contactPersonRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;

        public event PropertyChangedEventHandler? PropertyChanged;
        public DateTime? DeathDateForPicker
        {
            get => DeceasedData.DeathDate.ToDateTime(TimeOnly.MinValue);
            set
            {
                if (value.HasValue)
                {
                    DeceasedData.DeathDate = DateOnly.FromDateTime(value.Value);
                    OnPropertyChanged(nameof(DeathDateForPicker));
                }
            }
        }
        public DateTime? BirthDateForPicker
        {
            get => DeceasedData.BirthDate.ToDateTime(TimeOnly.MinValue);
            set
            {
                if (value.HasValue)
                {
                    DeceasedData.BirthDate = DateOnly.FromDateTime(value.Value);
                    OnPropertyChanged(nameof(BirthDateForPicker));
                }
            }
        }

        private CrematoryModel _selectedCrematory = new();
        public CrematoryModel SelectedCrematory
        {
            get => _selectedCrematory;
            set
            {
                if (_selectedCrematory != value)
                {
                    _selectedCrematory = value;
                    OnPropertyChanged(nameof(SelectedCrematory));
                }
            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly EditingPagesStatus status = addNewNote;
        public ObservableCollection<ServiceModel> ServiceData { get; set; } = [];
        public ObservableCollection<CrematoryModel> CrematoryData { get; set; } = [];
        public DeceasedModel DeceasedData { get; set; } = new DeceasedModel()
        {
            BirthDate = DateOnly.FromDateTime(DateTime.Today),
            DeathDate = DateOnly.FromDateTime(DateTime.Today)
        };
        public ContactPersonModel ContactPersonData { get; set; } = new ContactPersonModel();
        public OrderModel OrderData { get; set; } = new OrderModel() { CremationDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) };
        private ObservableCollection<TimePeriod> _freeTime = [];
        public TimeOnly CremationTime { get; set; } = new TimeOnly( hour: 12, minute: 00);
        public ObservableCollection<TimePeriod> FreeTime
        {
            get => _freeTime;
            set
            {
                if (_freeTime != value)
                {
                    _freeTime = value;
                    OnPropertyChanged(nameof(FreeTime));
                }
            }
        }

        public decimal GetServicePrice() => ServiceData.Where(s => s.IsSelected).Sum(s => s.Price);
        public async Task LoadServicesAsync()
        {
            ServiceData.Clear();
            var services = await _serviceRepository.GetAllServicesAsync();

            foreach (var item in services)
            {
                ServiceData.Add(item);
            }
        }
        public async Task LoadCrematoriesAsync()
        {
            CrematoryData.Clear();
            var crematories = await _crematoryRepository.GetAllCrematoriesAsync();

            foreach (var item in crematories)
            {
                CrematoryData.Add(item);
            }
        }
        public async Task CreateOrderNote()
        {
            if (!IsInputsValid())
                return;

            try
            {
                if (status == EditingPagesStatus.AddNewNote)
                {
                    var freeTime = await GetFreeTime();
                    if (freeTime.Count <= 0)
                        return;

                    var temp = OrderData.CremationDateTime;
                    OrderData.CremationDateTime = new DateTime(
                        temp.Year,
                        temp.Month,
                        temp.Day,
                        CremationTime.Hour,
                        CremationTime.Minute,
                        CremationTime.Second
                    );


                    var StartTime = TimeOnly.FromDateTime(OrderData.CremationDateTime);
                    var EndTime = TimeOnly.FromDateTime(OrderData.CremationDateTime.Add(OrderData.CremationDuration));

                    bool isWithinFreeTime = false;

                    foreach (var timeSlot in freeTime)
                    {
                        if (StartTime >= timeSlot.StartAsTimeOnly() &&
                            EndTime <= timeSlot.EndAsTimeOnly())
                        {
                            isWithinFreeTime = true;
                            break;
                        }
                    }

                    if (!isWithinFreeTime)
                    {
                        MessageBox.Show("Час кремації має бути в межах доступного часу!");
                        return;
                    }


                    var deceasedId = await _deceasedRepository.GetDeceasedIdAsync(DeceasedData);

                    if (deceasedId <= 0)
                    {
                        var decRes = await _deceasedRepository.InsertDeceasedAsync(DeceasedData);
                        deceasedId = await _deceasedRepository.GetDeceasedIdAsync(DeceasedData);

                        if (!decRes || deceasedId < 0)
                        {
                            MessageBox.Show("Виникла помилка під час додавання даних про померлого!");
                            return;
                        }
                    }

                    var contactPersonId = await _contactPersonRepository.GetContactPersonIdAsync(ContactPersonData);

                    if (contactPersonId <= 0)
                    {
                        var conRes = await _contactPersonRepository.InsertContactPersonAsync(ContactPersonData);
                        contactPersonId = await _contactPersonRepository.GetContactPersonIdAsync(ContactPersonData);

                        if (!conRes || contactPersonId < 0)
                        {
                            MessageBox.Show("Виникла помилка під час додавання даних про контактну особу!");
                            return;
                        }
                    }
                    OrderData.OrderDate = DateOnly.FromDateTime(DateTime.Now);
                    OrderData.ContactPersonId = contactPersonId;
                    OrderData.DeceasedId = deceasedId;
                    OrderData.CrematoryId = SelectedCrematory.Id;

                    var orderId = await _orderRepository.InsertOrderAsync(OrderData);

                    List<ServiceModel> selectedServices = ServiceData.Where(s => s.IsSelected).ToList();
                    
                    
                    
                    if (orderId <= 0)
                    {
                        MessageBox.Show("Виникла помилка під час додавання даних про замовлення!");

                        return;
                    }
                    else
                    {
                        MessageBox.Show("Операція пройла успішно!");
                        
                        if (selectedServices != null && selectedServices.Count > 0)
                            await _serviceRepository.AddSelectedServices(selectedServices, orderId);

                        return;
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка {ex.Message}");
            }
        }
        public async Task LoadFreeTimeAsync()
        {
            FreeTime.Clear();

            var freeTime = await GetFreeTime();

            if (freeTime.Count <= 0)
                return;

            foreach (var t in freeTime)
            {
                FreeTime.Add(t);
            }
        }
        private async Task<List<TimePeriod>> GetFreeTime()
        {
            var DayOfWeek = OrderData.CremationDateTime.DayOfWeek;
            if (SelectedCrematory == null)
            {
                MessageBox.Show("Оберіть крематорій!");
                return []; ;
            }

            var crematoryId = SelectedCrematory.Id;
            if (crematoryId <= 0)
            {
                MessageBox.Show("Оберіть крематорій!");
                return [];
            }

            return await _scheduleRepository.GetFreeTimeAsync(crematoryId, DateOnly.FromDateTime(OrderData.CremationDateTime));
        }
        public void ChangeCheckboxState(object sender, bool state)
        {
            var checkBox = sender as CheckBox;
            var context = checkBox?.DataContext as ServiceModel;
            if (context == null)
                return;

            context.IsSelected = state;
        }
        private bool IsInputsValid()
        {
            if (DeceasedData.FullName == null || DeceasedData.FullName.Length < 0)
            {
                MessageBox.Show("Введіть ім'я померлого!");
                return false;
            }
            if (ContactPersonData.FullName == null || ContactPersonData.FullName.Length < 0)
            {
                MessageBox.Show("Введіть ім'я контактної особи!");
                return false;
            }
            if (!IsDateOnlyValid(DeceasedData.BirthDate) || !IsDateOnlyValid(DeceasedData.DeathDate))
            {
                MessageBox.Show("Неправильний формат дати! (Дані про померлого)");
                return false;
            }
            if (ContactPersonData.PhoneNumber == null || (!Regex.IsMatch(ContactPersonData.PhoneNumber, @"^\+380\d{9}$") &&
                    !Regex.IsMatch(ContactPersonData.PhoneNumber, @"^0\d{9}$")))
            {
                MessageBox.Show("Неправильний формат номеру!");
                return false;
            }
            if (ContactPersonData.Address == null || ContactPersonData.Address.Length < 0)
            {
                MessageBox.Show("Введіть адресу контактної особи!");
                return false;
            }
            if (DeceasedData.Gender < 0)
            {
                MessageBox.Show("Оберіть стать померлого!");
                return false;
            }
            if (SelectedCrematory == null || SelectedCrematory.Id <= 0)
            {
                MessageBox.Show("Оберіть крематорій!");
                return false;
            }

            if (!IsDateTimeValid(OrderData.CremationDateTime))
            {
                MessageBox.Show("Неправильньна дата або час початку!");
                return false;
            }
            if (OrderData.CremationDuration.Hours <= 0)
            {
                MessageBox.Show("Неправильньно вказана тривалість!");
                return false;
            }
            if (OrderData.StandardPrice <= 0)
            {
                MessageBox.Show("Введіть початкову ціну послуги!");
                return false;
            }

            return true;

            bool IsDateTimeValid(DateTime dateTime)
            {
                if (dateTime.Year == 0 || dateTime.Month == 0 || dateTime.Day == 0)
                {
                    return false;
                }

                return true;
            }
            bool IsDateOnlyValid(DateOnly date)
            {
                if (date.Year == 1 || date.Month == 0 || date.Day == 0)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
