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
    /// Interakční logika pro OrderFinishPage.xaml
    /// </summary>
    public partial class OrderFinishPage : Page
    {
        
        RestClient client = new RestClient(Shared.Url);
        int OrderId;
        public int TotalPrice { get; set; }
        public OrderFinishPage(int _OrderId, int _TotalPrice)
        {
            InitializeComponent();
            DataContext = this;
            OrderId = _OrderId;
            TotalPrice = _TotalPrice;
            
            IsLogged();
        }
        
        public void IsLogged()
        {
            if (Shared.Logged)
            {
                client.CookieContainer = Shared.cookiecon;
                UserPanel.Visibility = Visibility.Visible;
                USER.Content = Shared.LoggedUserMail;
                //LoginPanel.Visibility = Visibility.Hidden;
                OrderIdLabel.Content = OrderId;
                //TotalPriceText.Text = TotalPrice.ToString();

                

            }
            else
            {
                UserPanel.Visibility = Visibility.Hidden;
                this.NavigationService.Navigate(new MainPage());
                //LoginPanel.Visibility = Visibility.Visible;
            }

        }
        private void User_Clicked(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new UserPage());
        }
        private void MainPageNavigate(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPage());
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
