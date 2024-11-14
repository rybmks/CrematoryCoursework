using Crematory.DataAccess;
using Crematory.Models;
using Crematory.enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crematory.Interfaces;
using System.Windows;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace Crematory.ViewModels.CreateOrder
{
    public class CreateOrderViewModel : INotifyPropertyChanged
    {
        private CrematoryModel _selectedCrematory = new CrematoryModel();
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
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private EditingPagesStatus status = EditingPagesStatus.AddNewNote;
        public ObservableCollection<ServiceModel> ServiceData { get; set; } = new ObservableCollection<ServiceModel>();
        private readonly IServiceRepository _serviceRepository;
        public ObservableCollection<CrematoryModel> CrematoryData { get; set; } = new ObservableCollection<CrematoryModel>();
        private readonly ICrematoryRepository _crematoryRepository;
        public DeceasedModel DeceasedData { get; set; } = new DeceasedModel();
        private readonly IDeceasedRepository _deceasedRepository;
        public ContactPersonModel ContactPersonData { get; set; } = new ContactPersonModel();
        private readonly IContactPersonRepository _contactPersonRepository;
        public OrderModel OrderData { get; set; } = new OrderModel() { CremationDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)};
        public readonly IScheduleRepository _scheduleRepository;

        public CreateOrderViewModel(
            IServiceRepository serviceRepository, ICrematoryRepository crematoryRepository, 
            IDeceasedRepository deceasedRepository, IContactPersonRepository contactPersonRepository,
            IScheduleRepository sheduleRepository)
        {
            _serviceRepository = serviceRepository;
            _crematoryRepository = crematoryRepository;
            _deceasedRepository = deceasedRepository;
            _contactPersonRepository = contactPersonRepository;
            _scheduleRepository = sheduleRepository;
        }


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
                var deceasedId = await _deceasedRepository.GetDeceasedIdAsync(DeceasedData);

                if (deceasedId <= 0)
                {
                    var res = await _deceasedRepository.InsertDeceasedAsync(DeceasedData);
                    deceasedId = await _deceasedRepository.GetDeceasedIdAsync(DeceasedData);

                    if (!res || deceasedId < 0)
                    {
                        MessageBox.Show("Виникла помилка під час додавання даних про померлого!");
                        return;
                    }
                }

                var contactPersonId = await _contactPersonRepository.GetContactPersonIdAsync(ContactPersonData);

                if (contactPersonId <= 0)
                {
                    var res = await _contactPersonRepository.InsertContactPersonAsync(ContactPersonData);
                    contactPersonId = await _contactPersonRepository.GetContactPersonIdAsync(ContactPersonData);

                    if (!res || contactPersonId < 0)
                    {
                        MessageBox.Show("Виникла помилка під час додавання даних про контактну особу!");
                        return;
                    }
                }

                if (status == EditingPagesStatus.AddNewNote)
                {
                    OrderData.OrderDate = DateOnly.FromDateTime(DateTime.Now);
                    OrderData.ContactPersonId = contactPersonId;
                    OrderData.DeceasedId = deceasedId;

                    MessageBox.Show(OrderData.ToString());
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
            //var DayOfWeek = OrderData.CremationDateTime.DayOfWeek;
            //if (SelectedCrematory == null)
            //{
            //    MessageBox.Show("Оберіть крематорій!");
            //    return;
            //}

            //var crematoryId = SelectedCrematory.Id;
            //if (crematoryId <= 0)
            //{
            //    MessageBox.Show("Оберіть крематорій!");
            //    return;
            //}

            //var schedule = await _scheduleRepository.GetCrematoryScheduleForDayAsync(crematoryId, OrderData.OrderDate.DayOfWeek);

            //MessageBox.Show(schedule.ToString());
        }
        private bool IsInputsValid()
        {
            if (SelectedCrematory == null|| SelectedCrematory.Id <= 0)
            {
                MessageBox.Show("Оберіть крематорій!");
                return false;
            }
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
            if (DeceasedData.Gender < 0)
            {
                MessageBox.Show("Оберіть стать померлого!");
                return false;
            }
            if (OrderData.StandardPrice <= 0)
            {
                MessageBox.Show("Введіть початкову ціну послуги!");
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
            if (!IsDateTimeValid(OrderData.CremationDateTime))
            {
                MessageBox.Show("Неправильний формат дати (Дата початку)!");
                return false;
            }
            if (!IsDateOnlyValid(DeceasedData.BirthDate) || !IsDateOnlyValid(DeceasedData.DeathDate))
            {
                MessageBox.Show("Неправильний формат дати!");
                return false;
            }

            return true;
            bool IsDateTimeValid(DateTime dateTime)
            {
                if (dateTime.Year == 0 || dateTime.Month == 0 || dateTime.Day == 0 ||
                     dateTime.Hour == 0 || dateTime.Minute == 0)
                {
                    return false;
                }

                return true;
           }
           bool IsDateOnlyValid(DateOnly date)
           {
                if (date.Year == 0 || date.Month == 0 || date.Day == 0)
                {
                    return false;
                }

                return true;
           }
        }
       
    }
}
