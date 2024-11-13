using Crematory.DataAccess;
using Crematory.DatabaseServices;
using Crematory.Models;
using Crematory.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Crematory.ViewModels
{
    public class AdminPanelViewModel
    {
        public ObservableCollection<ServiceModel> ServiceData { get; set; } = new ObservableCollection<ServiceModel>();
        public ObservableCollection<CrematoryModel> CrematoryData { get; set; } = new ObservableCollection<CrematoryModel>();
        public ObservableCollection<CrematoryScheduleModel> CrematoryScheduleData { get; set; } = new ObservableCollection<CrematoryScheduleModel>();

        public async Task LoadServicesAsync()
        {
            ServiceData.Clear();

            var services = await GetDataService.GetServicesDataAsync();

            foreach (var item in services)
            {
                ServiceData.Add(item);
            }
        }
        public async Task LoadCrematoriesAsync()
        {
            CrematoryData.Clear();

            var crematories = await GetDataService.GetCrematoryDataAsync();
            
            foreach (var item in crematories)
            {
                CrematoryData.Add(item);
            }
        }
        public async Task LoadScheduleAsync(CrematoryModel crematory)
        {
            CrematoryScheduleData.Clear();

            var schedule = await GetDataService.GetScheduleForCrematoryAsync(crematory.Id);

            foreach (var item in schedule)
            {
                CrematoryScheduleData.Add(item);
            }
        }
    }
}
