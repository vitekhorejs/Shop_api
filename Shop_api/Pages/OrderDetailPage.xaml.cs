using RestSharp;
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

namespace Shop_api
{
    /// <summary>
    /// Interakční logika pro OrderDetailPage.xaml
    /// </summary>
    public partial class OrderDetailPage : Page
    {
        RestClient client = new RestClient(Shared.Url);
        public int TotalPrice { get; set; }
        public int OrderId;
        public OrderDetailPage(int _OrderId)
        {
            InitializeComponent();
            OrderId = _OrderId;
            DataContext = this;
            IsLogged();
            ShowItems();
        }
        private void GoBack(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
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
        private void ShowItems()
        {
            var request = new RestRequest(Method.GET);
            request.AddParameter("Type", "get_order_items");
            request.AddParameter("Data", OrderId);
            var response = client.Execute<Input>(request);
            Shared.ShowInfo(response.Content);
            Input responseInput = SimpleJson.DeserializeObject<Input>(response.Content);
            if (responseInput.Type == "error")
            {

            }
            else
            {
                List<ItemId> items = SimpleJson.DeserializeObject<List<ItemId>>(responseInput.Data);
                List<Item> itemy = new List<Item>();
                foreach (ItemId item in items)
                {
                    var request2 = new RestRequest(Method.GET);
                    request2.AddParameter("Type", "get_item_by_id");
                    request2.AddParameter("Data", item.Item_id);
                    var response2 = client.Execute<Input>(request2);
                    Shared.ShowInfo(response.Content);
                    Input responseInput2 = SimpleJson.DeserializeObject<Input>(response2.Content);
                    Item item2 = SimpleJson.DeserializeObject<Item>(responseInput2.Data);
                    item2.Quantity = item.Quantity;
                    itemy.Add(item2);
                    TotalPrice += item2.Price;
                }
                listview2.ItemsSource = itemy;
                //TotalPriceLabel.Content = string.Format("{0:c}", "gClosingBalance");
                //TotalPriceLabel.Content = TotalPrice;
            }

        }

        private void listViewItem_MouseClick(object sender, RoutedEventArgs e)
        {
            TextBlock button = sender as TextBlock;
            Item item2 = button.DataContext as Item;
            this.NavigationService.Navigate(new DetailPage(item2));
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var request2 = new RestRequest(Method.POST);
            request2.AddParameter("Type", "logout");
            request2.AddParameter("Data", "none");
            var response2 = client.Execute(request2);
            Shared.cookiecon = new System.Net.CookieContainer();
            //Shared.cookiecon = null;
            Shared.Logged = false;
            Shared.LoggedUserMail = null;
            Shared.LoggedUserPermission = 0;
            this.NavigationService.Navigate(new MainPage());
        }
    }
}
