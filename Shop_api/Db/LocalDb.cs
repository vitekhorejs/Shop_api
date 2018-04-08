﻿using System;
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
            database.CreateTableAsync<Category>().Wait();
        }

        public Task<List<Category>> GetCategoriesAsync()
        {
            return database.Table<Category>().ToListAsync();
        }
        public Task<int> SaveItemAsync(Category item)
        {
            if (item.Id != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }
    }
}
