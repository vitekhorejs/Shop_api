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
    /// Interakční logika pro MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            IsLogged();
        }
        RestClient client = new RestClient(Shared.Url);
        public void IsLogged()
        {
            
            if (Shared.Logged)
            {
                client.CookieContainer = Shared.cookiecon;
            }
        }
        
        private void Register_Button(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RegisterPage());
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void User_Clicked(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new UserPage());
        }

        private void Item_Clicked(object sender, RoutedEventArgs e)
        {

        }
        private void showsessionid(object sender, RoutedEventArgs e)
        {
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            MessageBox.Show(response.Content, ":)", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
