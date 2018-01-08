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
using System.Windows.Navigation;
using System.Windows.Shapes;
using RestSharp;

namespace Shop_api
{
    /// <summary>
    /// Interakční logika pro OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        RestClient client = new RestClient(Shared.Url);
        public OrderPage()
        {
            InitializeComponent();
            IsLogged();
            ShowOrders();
        }
        List<Item> items = new List<Item>();
        Item item = new Item();
        private void User_Clicked(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new UserPage());
        }

        public void IsLogged()
        {
            if (Shared.Logged)
            {
                client.CookieContainer = Shared.cookiecon;
                UserPanel.Visibility = Visibility.Visible;
                USER.Content = Shared.LoggedUserMail;
                LoginPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                UserPanel.Visibility = Visibility.Hidden;
                LoginPanel.Visibility = Visibility.Visible;
                this.NavigationService.Navigate(new MainPage());
            }

        }

        /*private void Objednat(object sender, RoutedEventArgs e)
        {
            var request = new RestRequest(Method.PUT);
            request.AddParameter("Type", "change_order_status");
            request.AddParameter("Data", "none");
            var response = client.Execute<Input>(request);
        }*/
        private void listViewItem_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            Order item2 = item.DataContext as Order;
            
            this.NavigationService.Navigate(new OrderDetailPage(item2.Id));
        }

        private void ShowOrders()
        {
            var request = new RestRequest(Method.GET);
            request.AddParameter("Type", "get_orders");
            //Array categoryId = ["Category_id"][kategorie.Id];
            request.AddParameter("Data", "none");
            //MessageBox.Show(SimpleJson.SerializeObject(kategorie), "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
            var response = client.Execute<Input>(request);
            Shared.ShowInfo(response.Content);
            Input responseInput = SimpleJson.DeserializeObject<Input>(response.Content);
            if (responseInput.Type == "error")
            {

            }
            else
            {
                List<Order> orders = SimpleJson.DeserializeObject<List<Order>>(responseInput.Data);
                //MessageBox.Show(orders.ToString(), "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
                listview.ItemsSource = orders;
                //CategoryName.Content = kategorie.Name;
            }

        }
        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new UserPage());
        }
        private void HideOrder(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Button button = sender as Button;
            Order item2 = button.DataContext as Order;
            var request = new RestRequest(Method.DELETE);
            request.AddParameter("Type", "hide_order");
            request.AddParameter("Data", item2.Id);
            var response = client.Execute<Input>(request);
            Shared.ShowInfo(response.Content);
            this.NavigationService.Navigate(new OrderPage());
        }


        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var request2 = new RestRequest(Method.POST);
            request2.AddParameter("Type", "logout");
            request2.AddParameter("Data", "none");
            var response2 = client.Execute(request2);
            client.CookieContainer = null;
            Shared.cookiecon = null;
            Shared.Logged = false;
            Shared.LoggedUserMail = null;
            Shared.LoggedUserPermission = 0;
            this.NavigationService.Navigate(new MainPage());
        }
    }
}
