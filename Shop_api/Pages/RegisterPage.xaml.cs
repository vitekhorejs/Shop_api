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
    /// Interakční logika pro RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        User user = new User();
        public RegisterPage()
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
        private void Register_Button(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RegisterPage());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e )
        {
            user.Password = Password.Password;
        }

        private void Register_Clicked(object sender, RoutedEventArgs e)
        {

            if (Mail.Text == "" || Mail.Text == null)
            {
                MessageBox.Show("Zadejte Email", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (!IsValidEmail(Mail.Text))
            {
                MessageBox.Show("Zadejte správný email", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (Name.Text == "" || Name.Text == null)
            {
                MessageBox.Show("Zadejte Jméno", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (Surname.Text == "" || Surname.Text == null)
            {
                MessageBox.Show("Zadejte Příjmení", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (Password.Password == "" || Password.Password == null)
            {
                MessageBox.Show("Zadejte heslo", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (Password2.Password == "" || Password2.Password == null)
            {
                MessageBox.Show("Zadejte heslo znovu", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (Password.Password != Password2.Password)
            {
                MessageBox.Show("Zadaná hesla se neshodují", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (Password.SecurePassword.Length < 5)
            {
                MessageBox.Show("Heslo musí obsahovat alespoň 5 znaků", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var client = new RestClient(Shared.Url);
                //string txt = SimpleJson.SerializeObject(user);
                //text.Text = txt;
                var request = new RestRequest(Method.POST);
                request.AddParameter("Type", "register_user");
                request.AddParameter("Data", SimpleJson.SerializeObject(user));
                var response = client.Execute<Input>(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Shared.ShowInfo(response.Content);
                    int my_error;
                    Input responseInput = SimpleJson.DeserializeObject<Input>(response.Content);
                    if (Int32.TryParse(responseInput.Data, out my_error) == false)
                    { 
                        if (my_error == 1004)
                        {
                            my_error = 0;
                            this.NavigationService.Navigate(new LoginPage());
                        }
                    }
                    Password.Password = "";
                    Password2.Password = "";

                }
                else
                {
                    MessageBox.Show("Při komunikaci se serverem došlo k chybě.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    Password.Password = "";
                    Password2.Password = "";
                }

                
            }
        }
        
    }
}
