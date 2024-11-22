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
        public ObservableCollection<OrderModel> PlannedOrders = new();
        public ObservableCollection<OrderModel> CompletedCremations = new();
        public CreatedOrdersViewModel()
        {
            
        }
        
        public async void LoadCompletedOrdersAsync() 
        {
        
        }
        public async void LoadPlannedOrdersAsync()
        {

        }
    }
}
