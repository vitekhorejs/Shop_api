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
using System.IO;
using System.Net;

namespace Shop_api
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //RestClient client = new RestClient(Shared.Url);
            bool InternetConnection = InternetAvailability.IsInternetAvailable();
            if (InternetConnection == true)
            {
                Synchronize();
            }
            MainPage MainPage = new MainPage();
            frame.NavigationService.Navigate(MainPage);

        }
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
        public async void Synchronize()
        {

            RestClient client = new RestClient(Shared.Url);

            var request = new RestRequest(Method.GET);
            request.AddParameter("Type", "get_categories");
            request.AddParameter("Data", "ahoj");
            var response = client.Execute<Input>(request);
            Input responseInput = SimpleJson.DeserializeObject<Input>(response.Content);
            List<Category> categories = SimpleJson.DeserializeObject<List<Category>>(responseInput.Data);
            //ListBoxCategories.ItemsSource = categories;
            await Database.Remake();

            List<CategoryLocal> localCategories = new List<CategoryLocal>();
            List<ItemLocal> localItems = new List<ItemLocal>();
            Directory.CreateDirectory("Images");
            foreach (Category kategorie in categories)
            {
                CategoryLocal kat = new CategoryLocal();
                kat.Id = kategorie.Id;
                kat.Name = kategorie.Name;
                kat.Description = kategorie.Description;
                kat.Image_path = AppDomain.CurrentDomain.BaseDirectory + kategorie.Relative_Image_Path;
                WebClient client2 = new WebClient();
                client2.DownloadFile(kategorie.Image_path, kategorie.Relative_Image_Path);
                localCategories.Add(kat);

                var request2 = new RestRequest(Method.GET);
                request.AddParameter("Type", "get_items_by_category");
                request.AddParameter("Data", SimpleJson.SerializeObject(kategorie));
                var response2 = client.Execute<Input>(request);
                Input responseInput2 = SimpleJson.DeserializeObject<Input>(response2.Content);
                List<Item> items = SimpleJson.DeserializeObject<List<Item>>(responseInput2.Data);
                foreach (Item item in items)
                {
                    ItemLocal itemlocal = new ItemLocal();
                    itemlocal.Id = item.Id;
                    itemlocal.Name = item.Name;
                    itemlocal.Category_id = kategorie.Id;
                    itemlocal.Description = item.Description;
                    itemlocal.Price = item.Price;
                    itemlocal.Quantity = item.Quantity;
                    itemlocal.Image_path = AppDomain.CurrentDomain.BaseDirectory + item.Relative_Image_Path;
                    client2.DownloadFile(item.Image_path, item.Relative_Image_Path);
                    localItems.Add(itemlocal);
                }

            }
            await Database.SaveCategoriesAsync(localCategories);
            await Database.SaveItemsAsync(localItems);
        }
        

        
    }
}
