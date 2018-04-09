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
using System.IO;

namespace Shop_api
{
    /// <summary>
    /// Interakční logika pro MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        //Category category = new Category();
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
            }
            else
            {
                UserPanel.Visibility = Visibility.Hidden;
                LoginPanel.Visibility = Visibility.Visible;
                Info.Visibility = Visibility.Visible;
            }

        }

        private async void ShowCategories()
        {
            /*try
            {*/
                bool InternetConnection = InternetAvailability.IsInternetAvailable();
                if (InternetConnection == true)
                {
                    //Synchronize.SyncCategories();
                    var request = new RestRequest(Method.GET);
                    request.AddParameter("Type", "get_categories");
                    request.AddParameter("Data", "ahoj");
                    var response = client.Execute<Input>(request);
                    Input responseInput = SimpleJson.DeserializeObject<Input>(response.Content);
                    List<Category> categories = SimpleJson.DeserializeObject<List<Category>>(responseInput.Data);
                    ListBoxCategories.ItemsSource = categories;
                    await Database.Remake();
                    List<CategoryLocal> localCategories = new List<CategoryLocal>();
                    Directory.CreateDirectory("Images");
                    foreach (Category katgorie in categories)
                    {
                        CategoryLocal kat = new CategoryLocal();
                        kat.Id = katgorie.Id;
                        kat.Name = katgorie.Name;
                        kat.Description = katgorie.Description;
                        kat.Image_path = AppDomain.CurrentDomain.BaseDirectory + katgorie.Relative_Image_Path;
                        WebClient client = new WebClient();
                        client.DownloadFile(katgorie.Image_path, katgorie.Relative_Image_Path);
                        localCategories.Add(kat);
                    }
                    await Database.SaveItemAsync(localCategories);
                }
                else
                {
                    //MessageBox.Show("OFFLINE Režim", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
                    InternetStatus.Visibility = Visibility.Visible;
                    var itemsFromDb = await Database.GetCategoriesAsync();
                    ListBoxCategories.ItemsSource = itemsFromDb;
                }
            /*}
            catch(Exception e)
            {

            }*/
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
