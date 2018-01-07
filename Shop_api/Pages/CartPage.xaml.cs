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
using System.ComponentModel;

namespace Shop_api
{
    /// <summary>
    /// Interakční logika pro CartPage.xaml
    /// </summary>
    public partial class CartPage : Page, INotifyPropertyChanged
    {
        public int TotalPrice { get; set; }
        string _test;
        public string Test
        {
            get { return _test; }
            set
            {
                _test = value;
                OnPropertyChanged("Test");
                //QuantityChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        RestClient client = new RestClient(Shared.Url);
        public CartPage()
        {
            InitializeComponent();
            DataContext = this;
            IsLogged();
            //ShowOrders();
            ShowItems();
            
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
            var request = new RestRequest(Method.PUT);
            request.AddParameter("Type", "change_order_status");
            request.AddParameter("Data", "none");
            var response = client.Execute<Input>(request);
            Shared.ShowInfo(response.Content);
            Input responseInput = SimpleJson.DeserializeObject<Input>(response.Content);
            int OrderId;  
            if (Int32.TryParse(responseInput.Data, out OrderId) == false)
            {
                MessageBox.Show("chyba /server neposlal id dokoncene objednvaky", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            } else
            {
                this.NavigationService.Navigate(new OrderFinishPage(OrderId, TotalPrice));
            }
            //int OrderId = responseInput.Data;

            
        }

        /*private void listViewItem_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            Item item2 = item.Content as Item;
            var request = new RestRequest(Method.DELETE);
            request.AddParameter("Type", "delete_item_from_order");
            ItemNevim nevim = new ItemNevim();
            nevim.Id = item2.Id;
            //nevim.OrderId = 
            request.AddParameter("Data", SimpleJson.SerializeObject(nevim));
            var response = client.Execute<Input>(request);
            this.NavigationService.Navigate(new CartPage());
        }*/

        private void listViewItem_MouseClick(object sender, RoutedEventArgs e)
        {
            TextBlock button = sender as TextBlock;
            Item item2 = button.DataContext as Item;
            this.NavigationService.Navigate(new DetailPage(item2));
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Item item2 = button.DataContext as Item;
            var request = new RestRequest(Method.DELETE);
            request.AddParameter("Type", "delete_item_from_order");
            ItemNevim nevim = new ItemNevim();
            nevim.Id = item2.Id;
            //nevim.OrderId = 
            request.AddParameter("Data", SimpleJson.SerializeObject(nevim));
            var response = client.Execute<Input>(request);
            this.NavigationService.Navigate(new CartPage());
        }

        /*private void ShowOrders()
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
        }*/

        private void ShowItems()
        {
            var request = new RestRequest(Method.GET);
            request.AddParameter("Type", "get_cart_items");
            request.AddParameter("Data", "none");
            var response = client.Execute<Input>(request);
            Shared.ShowInfo(response.Content);
            Input responseInput = SimpleJson.DeserializeObject<Input>(response.Content);
            if(responseInput.Type == "error")
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
        private void EnterPressed(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) {
                //sender.Focus();
                //TextBox sender2 = sender as TextBox;
                listview2.Focus();
                //QuantityChanged(sender, e);
            }
        }
        private void QuantityChanged(object sender, RoutedEventArgs e)
        {
            TextBox item = sender as TextBox;
            Item item2 = new Item();
            item2 = item.DataContext as Item;
            var request = new RestRequest(Method.PUT);
            request.AddParameter("Type", "change_quantity");
            ItemNevim2 nevim = new ItemNevim2();
            nevim.ItemId = item2.Id;
            if (item2.Quantity > 250)
            {
                MessageBox.Show("Maximální počet kusů je 250", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                nevim.Quantity = 250;
            }
            else
            {
                nevim.Quantity = item2.Quantity;
            }
            
            request.AddParameter("Data", SimpleJson.SerializeObject(nevim));
            var response = client.Execute<Input>(request);
            Shared.ShowInfo(response.Content);
            this.NavigationService.Navigate(new CartPage());
            //NavigationService.Navigate(new CartPage());

        }
        private void Minus(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Item item = new Item();
            item = button.DataContext as Item;
            var request = new RestRequest(Method.PUT);
            request.AddParameter("Type", "change_quantity");
            ItemNevim2 nevim = new ItemNevim2();
            nevim.ItemId = item.Id;
            if(item.Quantity == 1)
            {
                //MessageBox.Show("", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                //Button button = sender as Button;
                //Item item2 = button.DataContext as Item;
                var request2 = new RestRequest(Method.DELETE);
                request.AddParameter("Type", "delete_item_from_order");
                //ItemNevim nevim = new ItemNevim();
                //nevim.Id = item2.Id;
                //nevim.OrderId = 
                //nevim.Quantity = 
                request.AddParameter("Data", SimpleJson.SerializeObject(nevim));
                var response = client.Execute<Input>(request);
                Shared.ShowInfo(response.Content);
                this.NavigationService.Navigate(new CartPage());
            }
            else
            {
                nevim.Quantity = item.Quantity-1;
                request.AddParameter("Data", SimpleJson.SerializeObject(nevim));
                var response = client.Execute<Input>(request);
                Shared.ShowInfo(response.Content);
                this.NavigationService.Navigate(new CartPage());
            }
        }
        private void Plus(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Item item = new Item();
            item = button.DataContext as Item;
            var request = new RestRequest(Method.PUT);
            request.AddParameter("Type", "change_quantity");
            ItemNevim2 nevim = new ItemNevim2();
            nevim.ItemId = item.Id;
            if (item.Quantity >= 250)
            {
                MessageBox.Show("Maximální počet kusů je 250", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                nevim.Quantity = item.Quantity+1;
                request.AddParameter("Data", SimpleJson.SerializeObject(nevim));
                var response = client.Execute<Input>(request);
                Shared.ShowInfo(response.Content);
                this.NavigationService.Navigate(new CartPage());
            }
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

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}
