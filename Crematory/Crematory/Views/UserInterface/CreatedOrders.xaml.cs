using Crematory.DataAccess;
using Crematory.Models;
using Crematory.Models.AppModels;
using Crematory.Models.DatabaseModels;
using Crematory.ViewModels.CreatedOrders;
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

namespace Crematory.Views.UserInterface
{
    /// <summary>
    /// Логика взаимодействия для CreatedOrders.xaml
    /// </summary>
    public partial class CreatedOrders : Window
    {
        private readonly CreatedOrdersViewModel _viewModel;

        public CreatedOrders()
        {
            InitializeComponent();
            _viewModel = new(
                new OrderRepository(),
                new DeceasedRepository(), 
                new ContactPersonRepository(),
                new ServiceRepository());
            DataContext = _viewModel;
        }

        private void BackToMain(object sender, RoutedEventArgs e)
        {
            var m = new MainWindow();
            m.Show();
            this.Hide();
        }
        public void Page_VisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                UpdateForm();
            }
        }
        private void UpdateForm()
        {
            _viewModel.LoadPlannedOrdersAsync();
            _viewModel.LoadCompletedOrdersAsync();
        }

        private void CompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is FullOrderInfoModel fullOrderInfo)
            {
                var submitWindow = new SubmitOrderCompleting(_viewModel, fullOrderInfo.OrderId, this);
                submitWindow.Show();
                this.Hide();
            }
        }

        private void DeleteCompleted_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show(
                "Ви впевнені, що хочете видалити запис?",
                "Видалення",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
                );

            if (res == MessageBoxResult.No)
                return;

            if (sender is Button button && button.DataContext is FullOrderInfoModel fullOrderInfo)
            {
                _viewModel.DeleteCompleted(fullOrderInfo.OrderId);
            }
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show(
               "Ви впевнені, що хочете видалити запис?",
               "Видалення",
               MessageBoxButton.YesNo,
               MessageBoxImage.Warning
               );

            if (res == MessageBoxResult.No)
                return;

            if (sender is Button button && button.DataContext is FullOrderInfoModel fullOrderInfo)
            {
                _viewModel.DeleteOrder(fullOrderInfo.OrderId);
            }
        }

        private void GetFullInfo_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is FullOrderInfoModel fullOrderInfo)
            {
                var infoWindow = new MoreInfoWindow(_viewModel, fullOrderInfo);
                infoWindow.Owner = this;
                infoWindow.ShowDialog();
            }
        }
    }
}
