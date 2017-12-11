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
    /// Interakční logika pro CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        RestClient client = new RestClient(Shared.Url);
        public CartPage()
        {
            InitializeComponent();
            IsLogged();
            ShowItems();
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

        private void Objednat(object sender, RoutedEventArgs e)
        {

        }

        private void ShowOrders()
        {
            var request = new RestRequest(Method.GET);
            request.AddParameter("Type", "get_orders");
            //Array categoryId = ["Category_id"][kategorie.Id];
            request.AddParameter("Data", "none");
            //MessageBox.Show(SimpleJson.SerializeObject(kategorie), "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
            var response = client.Execute<Input>(request);
            Input responseInput = SimpleJson.DeserializeObject<Input>(response.Content);
            List<Order> orders = SimpleJson.DeserializeObject<List<Order>>(responseInput.Data);
            //MessageBox.Show(orders.ToString(), "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
            listview.ItemsSource = orders;
            //CategoryName.Content = kategorie.Name;
        }

        private void ShowItems()
        {
            /*var request = new RestRequest(Method.GET);
            request.AddParameter("Type", "get_cart_items");
            //Array categoryId = ["Category_id"][kategorie.Id];
            request.AddParameter("Data", "none");
            //MessageBox.Show(SimpleJson.SerializeObject(kategorie), "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
            var response = client.Execute<Input>(request);
            Input responseInput = SimpleJson.DeserializeObject<Input>(response.Content);
            List<Item> items = SimpleJson.DeserializeObject<List<Item>>(responseInput.Data);
            listview.ItemsSource = items;
            //CategoryName.Content = kategorie.Name;*/
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
