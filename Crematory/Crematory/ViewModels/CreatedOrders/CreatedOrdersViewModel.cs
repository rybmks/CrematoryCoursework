using Crematory.DataAccess;
using Crematory.Interfaces;
using Crematory.Models.AppModels;
using Crematory.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.ViewModels.CreatedOrders
{
    public class CreatedOrdersViewModel
    {
        public ObservableCollection<FullOrderInfoModel> PlannedOrders { get; set; } = new();
        public ObservableCollection<FullOrderInfoModel> CompletedOrders { get; set; } = new();

        private readonly IOrderRepository _orderRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IDeceasedRepository _deceasedRepository;
        private readonly IContactPersonRepository _contactPersonRepository;

        public CreatedOrdersViewModel(
            IOrderRepository orderRepository, 
            IDeceasedRepository deceasedRepository,
            IContactPersonRepository contactPersonRepository,
            IServiceRepository serviceRepository)
        {
            _orderRepository = orderRepository;
            _deceasedRepository = deceasedRepository;
            _serviceRepository = serviceRepository;
            _contactPersonRepository = contactPersonRepository;
        }
        
        public async void LoadCompletedOrdersAsync() 
        {
            CompletedOrders.Clear();
            var res = await _orderRepository.GetCompletedOrdersAsync();

            foreach (var r in res)
            {
                CompletedOrders.Add(r);
            }
        }
        public async void LoadPlannedOrdersAsync()
        {
            PlannedOrders.Clear();
            var res = await _orderRepository.GetAllPlannedOrdersAsync();

            foreach (var r in res)
            {
                PlannedOrders.Add(r);
            }
        }
       
        public async Task<List<ServiceModel>> LoadOrderedServicesAsync(int orderId)
        {
            return await _serviceRepository.GetServicesForOrderAsync(orderId);
        }
        public async Task<DeceasedModel> GetDeceasedByIdAsync(int id) => await _deceasedRepository.GetDeceasedById(id);
        public async Task<ContactPersonModel> GetContactPersonByIdAsync(int id) => await _contactPersonRepository.GetContactPersonById(id);
        public async void CompleteOrder(CompletedOrderModel order)
        {
            await _orderRepository.InsertCompletedOrder(order);
        }
        public async void DeleteCompleted(int orderId)
        {
            await _orderRepository.DeleteCompletedAsync(orderId);
            LoadCompletedOrdersAsync();
        }
        public async void DeleteOrder(int orderId)
        {
            await _orderRepository.DeleteOrderAsync(orderId);
            LoadPlannedOrdersAsync();
        }
    }
}
