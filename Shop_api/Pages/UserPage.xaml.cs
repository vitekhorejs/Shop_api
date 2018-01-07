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
    /// Interakční logika pro UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        RestClient client = new RestClient(Shared.Url);
        User2 user = new User2();
        public UserPage()
        {
            InitializeComponent();
            IsLogged();
            GetUserData();
            DataContext = user;
        }
        public void GetUserData()
        {
            var request = new RestRequest(Method.GET);
            request.AddParameter("Type", "get_userinfo");
            request.AddParameter("Data", "none");
            var response = client.Execute<Input>(request);
            Input responseInput = SimpleJson.DeserializeObject<Input>(response.Content);
            user = SimpleJson.DeserializeObject<User2>(responseInput.Data);
            
        }

        public void IsLogged()
        {
            if (Shared.Logged)
            {
                client.CookieContainer = Shared.cookiecon;
                UserPanel.Visibility = Visibility.Visible;
                USER.Content = Shared.LoggedUserMail;
                LoginPanel.Visibility = Visibility.Hidden;
                //Info.Visibility = Visibility.Hidden;
            }
            else
            {
                UserPanel.Visibility = Visibility.Hidden;
                LoginPanel.Visibility = Visibility.Visible;
                //Info.Visibility = Visibility.Visible;
            }

        }
        private void Register_Button(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RegisterPage());
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            var request = new RestRequest(Method.PUT);
            request.AddParameter("Type", "userdata_update");
            request.AddParameter("Data", SimpleJson.SerializeObject(user));
            var response = client.Execute(request);
            Shared.ShowInfo(response.Content);
            if(response.Content == "" || response.Content == null)
            {
                MessageBox.Show("Změny byly uloženy.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var request2 = new RestRequest(Method.POST);
            request2.AddParameter("Type", "logout");
            request2.AddParameter("Data", "none");
            var response2 = client.Execute(request2);
            //client.CookieContainer = null;
            Shared.cookiecon = new System.Net.CookieContainer();
            Shared.Logged = false;
            Shared.LoggedUserMail = null;
            Shared.LoggedUserPermission = 0;
            this.NavigationService.Navigate(new MainPage());
            //navigate to mainpage
        }

        private void User_Clicked(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new UserPage());
        }
        private void Orders_Clicked(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new OrderPage());
        }
    }
}
