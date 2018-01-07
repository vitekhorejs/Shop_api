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
using System.Net;

namespace Shop_api
{
    /// <summary>
    /// Interakční logika pro LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        User user = new User();
        RestClient client = new RestClient(Shared.Url);
        public LoginPage()
        {
            InitializeComponent();
            DataContext = user;
            Mail.Focus();
            
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void EnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login_Click(null, null);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            user.Password = Password.Password;
        }
        public void Login_Click(object sender, RoutedEventArgs e)
        {
            if (Mail.Text == "" || Mail.Text == null)
            {
                MessageBox.Show("Zadejte Email", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (!IsValidEmail(Mail.Text))
            {
                MessageBox.Show("Zadejte správný email", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (Password.Password == "" || Password.Password == null)
            {
                MessageBox.Show("Zadejte heslo", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //var client = new RestClient(Shared.Url);
                var request = new RestRequest(Method.POST);
                request.AddParameter("Type", "login");
                request.AddParameter("Data", SimpleJson.SerializeObject(user));

                var response = client.Execute<Input>(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Shared.ShowInfo(response.Content);
                    //User user1 = SimpleJson.DeserializeObject(response.Data.Data);
                    //User uzivatel = SimpleJson.DeserializeObject
                    //MessageBox.Show(response.Data.Data, "nevim", MessageBoxButton.OK, MessageBoxImage.Warning);
                    //MessageBox.Show(user1.Id.ToString(), "nevim", MessageBoxButton.OK, MessageBoxImage.Warning);
                    CookieContainer cookiecon = new CookieContainer();
                    Input responseInput = SimpleJson.DeserializeObject<Input>(response.Content);
                    if (response.StatusCode == HttpStatusCode.OK && responseInput.Type.Equals("data"))
                    {
                        var cookie = response.Cookies.FirstOrDefault();
                        Shared.cookiecon.Add(new Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain));
                        //User usr = SimpleJson.DeserializeObject<User>(responze.Data);
                        //Shared.LoggedUserId = usr.Id;
                        
                        int x = 0;
                        if (Int32.TryParse(responseInput.Data, out x))
                        {
                            Shared.LoggedUserPermission = x;
                            Shared.Logged = true;
                            Shared.LoggedUserMail = user.Mail;
                            client.CookieContainer = Shared.cookiecon;
                            this.NavigationService.Navigate(new MainPage());
                        }
                        else
                        {
                            MessageBox.Show("Nepodařilo se načíst Opravnění uživatele.", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
                            //client.CookieContainer = Shared.cookiecon;
                            //Shared.LoggedUserPermission = 0;
                            this.NavigationService.Refresh();
                        }                
                    }
                    
                }
                else
                {
                    MessageBox.Show("Při komunikaci se serverem došlo k chybě.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                /*CookieContainer _cookieJar = new CookieContainer();
                var sessionCookie = response.Cookies.SingleOrDefault(x => x.Name == "ASP.NET_SessionId");
                if (sessionCookie != null)
                {
                    _cookieJar.Add(new Cookie(sessionCookie.Name, sessionCookie.Value, sessionCookie.Path, sessionCookie.Domain));
                }*/
                

                //var client2 = new RestClient(Shared.Url);
                

            }
            
        }
        /*private void showsessionid(object sender, RoutedEventArgs e)
        {
            var request2 = new RestRequest(Method.GET);
            var response2 = client.Execute(request2);
            MessageBox.Show(response2.Content, ":)", MessageBoxButton.OK, MessageBoxImage.Error);
        }*/
        
    }
}
