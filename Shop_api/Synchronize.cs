using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Net;

namespace Shop_api
{
    public class Synchronize
    {
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
        static RestClient client = new RestClient(Shared.Url);
        public static void SyncCategories()
        {
            var request = new RestRequest(Method.GET);
            request.AddParameter("Type", "get_categories");
            request.AddParameter("Data", "ahoj");
            var response = client.Execute<Input>(request);
            Input responseInput = SimpleJson.DeserializeObject<Input>(response.Content);
            List<Category> categories = SimpleJson.DeserializeObject<List<Category>>(responseInput.Data);
            foreach (Category katgorie in categories)
            {
                katgorie.Image_path = Shared.Url + katgorie.Image_path;
                Database.SaveItemAsync(katgorie);
            }
        }


    }
}
