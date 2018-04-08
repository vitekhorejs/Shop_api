using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Shop_api
{
    public class LocalDb
    {
        private SQLiteAsyncConnection database;

        public LocalDb(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<CategoryLocal>().Wait();
        }

        public Task<List<CategoryLocal>> GetCategoriesAsync()
        {
            return database.Table<CategoryLocal>().ToListAsync();
        }
        public Task<int> SaveItemAsync(CategoryLocal item)
        {
            /*if (item.Id != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {*/
                return database.InsertAsync(item);
            //}
        }
    }
}
