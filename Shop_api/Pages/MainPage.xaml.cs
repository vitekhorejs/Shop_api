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
        //Category category = new Category();
        RestClient client = new RestClient(Shared.Url);
        public MainPage()
        {
            InitializeComponent();
            IsLogged();
            ShowCategories();
            //DataContext = category; 
        }
        
        public void IsLogged()
        {
            if (Shared.Logged)
            {
                client.CookieContainer = Shared.cookiecon;
                UserPanel.Visibility = Visibility.Visible;
                USER.Content = Shared.LoggedUserMail;
                LoginPanel.Visibility = Visibility.Hidden;
                Info.Visibility = Visibility.Hidden;
            } else
            {
                UserPanel.Visibility = Visibility.Hidden;
                LoginPanel.Visibility = Visibility.Visible;
                Info.Visibility = Visibility.Visible;
            }

        }
        private void ShowCategories()
        {
            var request = new RestRequest(Method.GET);
            request.AddParameter("Type", "get_categories");
            request.AddParameter("Data", "ahoj");
            var response = client.Execute<Input>(request);
            //List categories = response.Data.Data;
            //string kategorie = response.Data.Data; 
            //MessageBox.Show(response.Content, "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
            Input responseInput = SimpleJson.DeserializeObject<Input>(response.Content);
            //MessageBox.Show(responseInput.Data, "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
            //Categories categories = SimpleJson.DeserializeObject<Categories>(responseInput.Data);
            List<Category> categories = SimpleJson.DeserializeObject<List<Category>>(responseInput.Data);
            ListBoxCategories.ItemsSource = categories;
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
            client.CookieContainer = null;
            Shared.cookiecon = null ;
            Shared.Logged = false;
            Shared.LoggedUserMail = null;
            Shared.LoggedUserPermission = 0;
            //navigate to mainpage
        }

        private void User_Clicked(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new UserPage());
        }

        private void Item_Clicked(object sender, RoutedEventArgs e)
        {

            /*Category kategorie = new Category();
            kategorie = sender as Category;*/
            var grid = sender as Grid;
            ListBoxCategories.SelectedItem = grid.DataContext;
            Category selectedItem = ListBoxCategories.SelectedItem as Category;
            this.NavigationService.Navigate(new CategoryPage(selectedItem));

        }
        /*private void showsessionid(object sender, RoutedEventArgs e)
        {
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            MessageBox.Show(response.Content, ":)", MessageBoxButton.OK, MessageBoxImage.Error);
        }*/
    }
}
