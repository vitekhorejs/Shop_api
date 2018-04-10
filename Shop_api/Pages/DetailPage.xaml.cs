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
    /// Interakční logika pro DetailPage.xaml
    /// </summary>
    public partial class DetailPage : Page
    {
        RestClient client = new RestClient(Shared.Url);
        public DetailPage(Item selectedItem)
        {
            InitializeComponent();
            IsLogged();
            bool InternetConnection = InternetAvailability.IsInternetAvailable();
            if (InternetConnection != true)
            {
                OfflineUI();
            }
            item = selectedItem;
            ShowDetailedItem();
        }
        public void OfflineUI()
        {
            loginbutton.IsEnabled = false;
            registerbutton.IsEnabled = false;
            BuyButton.IsEnabled = false;
        }
        Item item = new Item();
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
            }

        }
        private void ShowDetailedItem()
        {
            Name.Content = item.Name;
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            bool InternetConnection = InternetAvailability.IsInternetAvailable();
            if (InternetConnection == true)
            {
                logo.UriSource = new Uri(item.Image_path);
            }
            else
            {
                logo.UriSource = new Uri(item.Relative_Image_Path);

            }
            //logo. = item.Image_path;
            logo.EndInit();
            Image.Source = logo;
            Price.Content = item.Price;
            Description.Text = item.Description;
        }

        private void User_Clicked(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new UserPage());
        }

        private void BuyItem(object sender, RoutedEventArgs e)
        {
            if (Shared.Logged)
            {
                var request = new RestRequest(Method.POST);
                
                request.AddParameter("Type", "add_to_cart");
                request.AddParameter("Data", SimpleJson.SerializeObject(item));
                var response = client.Execute(request);
                Shared.ShowInfo(response.Content);
                //MessageBox.Show(response.Content, "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Pro vložení položky do nákupního košíku se musíte nejdříve přihlásit.", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void Register_Button(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RegisterPage());
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
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}
