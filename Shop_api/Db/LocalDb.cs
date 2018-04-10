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
            database.CreateTableAsync<ItemLocal>().Wait();

        }

        public Task<int> SaveCategoriesAsync(List<CategoryLocal> items)
        {
            return database.InsertAllAsync(items);
        }

        public Task<int> SaveItemsAsync(List<ItemLocal> items)
        {
            return database.InsertAllAsync(items);
        }
        

        public Task<List<CategoryLocal>> GetCategoriesAsync()
        {
            return database.Table<CategoryLocal>().ToListAsync();
        }
        public Task<List<ItemLocal>> GetItemsByIdAsync(int Category_id)
        {
            //return database.Table<ItemLocal>().ToListAsync();
            return database.Table<ItemLocal>().Where(i => i.Category_id == Category_id).ToListAsync();
        }
        /*public Task<int> SaveItemAsync(CategoryLocal item)
        {
            return database.InsertAsync(item);
        }*/
        public async Task Remake()
        {
            await database.DropTableAsync<CategoryLocal>();
            await database.DropTableAsync<ItemLocal>();
            await database.CreateTableAsync<CategoryLocal>();
            await database.CreateTableAsync<ItemLocal>();

        }
    }

}
