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
    /// Interakční logika pro CategoryPage.xaml
    /// </summary>
    public partial class CategoryPage : Page
    {
        private static LocalDb _database;
        public static LocalDb Database
        {
            get
            {
                if (_database == null)
                {
                    var fileHelper = new FileHelper();
                    _database = new LocalDb(fileHelper.GetLocalFilePath("database.db3"));
                }
                return _database;
            }
        }
        RestClient client = new RestClient(Shared.Url);
        public CategoryPage(Category category)
        {
            InitializeComponent();
            IsLogged();
            kategorie = category;
            ShowItems();
        }
        Category kategorie = new Category();
        public void IsLogged()
        {
            if (Shared.Logged)
            {
                client.CookieContainer = Shared.cookiecon;
                UserPanel.Visibility = Visibility.Visible;
                USER.Content = Shared.LoggedUserMail;
                LoginPanel.Visibility = Visibility.Hidden;
                Info.Visibility = Visibility.Hidden;
            }
            else
            {
                UserPanel.Visibility = Visibility.Hidden;
                LoginPanel.Visibility = Visibility.Visible;
                Info.Visibility = Visibility.Visible;
            }

        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
        private async void ShowItems()
        {
            bool InternetConnection = InternetAvailability.IsInternetAvailable();
            if (InternetConnection == true)
            {
                var request = new RestRequest(Method.GET);
                request.AddParameter("Type", "get_items_by_category");
                //Array categoryId = ["Category_id"][kategorie.Id];
                request.AddParameter("Data", SimpleJson.SerializeObject(kategorie));
                //MessageBox.Show(SimpleJson.SerializeObject(kategorie), "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
                var response = client.Execute<Input>(request);
                Input responseInput = SimpleJson.DeserializeObject<Input>(response.Content);
                List<Item> items = SimpleJson.DeserializeObject<List<Item>>(responseInput.Data);
                ListBoxItems.ItemsSource = items;
                CategoryName.Content = kategorie.Name;
            }
            else
            {
                InternetStatus.Visibility = Visibility.Visible;
                var itemsFromDb = await Database.GetCategoriesAsync();
                ListBoxItems.ItemsSource = itemsFromDb;
            }

        }

        private void User_Clicked(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new UserPage());
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var request2 = new RestRequest(Method.POST);
            request2.AddParameter("Type", "logout");
            request2.AddParameter("Data", "none");
            var response2 = client.Execute(request2);
            Shared.cookiecon = new System.Net.CookieContainer();
            Shared.Logged = false;
            Shared.LoggedUserMail = null;
            Shared.LoggedUserPermission = 0;
            this.NavigationService.Navigate(new MainPage());
        }

        private void Item_Clicked(object sender, RoutedEventArgs e)
        {
            var grid = sender as Grid;
            ListBoxItems.SelectedItem = grid.DataContext;
            Item selectedItem = ListBoxItems.SelectedItem as Item;
            //MessageBox.Show(selectedItem.Name, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            this.NavigationService.Navigate(new DetailPage(selectedItem));
        }
    }
}
