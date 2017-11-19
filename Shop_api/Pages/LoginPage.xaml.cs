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
    /// Interakční logika pro LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        User user = new User();
        public LoginPage()
        {
            InitializeComponent();
            DataContext = user;
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
                var client = new RestClient(Shared.GetUrl());
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
                }
                else
                {
                    MessageBox.Show("Při komunikaci se serverem došlo k chybě.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }
    }
}
