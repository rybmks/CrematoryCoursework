using Crematory.Models;
using Crematory.Interfaces;
using Crematory.DataAccess;
using System.Collections.ObjectModel;


namespace Crematory.ViewModels.AdminWindow
{
    public class AdminPanelViewModel
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly ICrematoryRepository _crematoryRepository;
        private readonly IScheduleRepository _scheduleRepository;


        public AdminPanelViewModel(
            IServiceRepository serviceRepository,
            ICrematoryRepository crematoryRepository,
            IScheduleRepository scheduleRepository)
        {
            _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
            _crematoryRepository = crematoryRepository ?? throw new ArgumentNullException(nameof(crematoryRepository));
            _scheduleRepository = scheduleRepository ?? throw new ArgumentNullException(nameof(scheduleRepository));
        }
        public ObservableCollection<ServiceModel> ServiceData { get; set; } = new ObservableCollection<ServiceModel>();
        public ObservableCollection<CrematoryModel> CrematoryData { get; set; } = new ObservableCollection<CrematoryModel>();
        public ObservableCollection<CrematoryScheduleModel> CrematoryScheduleData { get; set; } = new ObservableCollection<CrematoryScheduleModel>();

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
        public async Task LoadScheduleAsync(CrematoryModel crematory)
        {
            CrematoryScheduleData.Clear();

            var schedule = await _scheduleRepository.GetScheduleForCrematoryAsync(crematory.Id);

            foreach (var item in schedule)
            {
                CrematoryScheduleData.Add(item);
            }
        }
    }
}
