using System.ComponentModel;
using System.Windows;
using Crematory.Models.DatabaseModels;
using Crematory.ViewModels.CreatedOrders;

namespace Crematory.Views.UserInterface
{
    /// <summary>
    /// Логика взаимодействия для SubmitOrderCompleting.xaml
    /// </summary>
    public partial class SubmitOrderCompleting : Window
    {
        private readonly CreatedOrdersViewModel _viewModel;
        private readonly CreatedOrders _ordersWindow;
        private readonly int _orderId;
        public SubmitOrderCompleting(CreatedOrdersViewModel viewModel, int orderId, CreatedOrders createdWindow)
        {
            InitializeComponent();
            _ordersWindow = createdWindow;
            _viewModel = viewModel;
            _orderId = orderId;
        }

        private void CompleteOrderButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Ви впевнені, що хочете відмітити замовлення як виконане? Надалі буде неможливо змінити статус замовлення!",
                "Завершення замовлення",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                var reason = CompletionReasonTextBox.Text;
                if (reason.Trim().Length < 1)
                    reason = "Виконано";

                CompletedOrderModel completedOrder = new() { OrderId = _orderId, CompetionReason = reason};
                _viewModel.CompleteOrder(completedOrder);
            }

            this.Close();
            _ordersWindow.Show();
        }
    }
}
