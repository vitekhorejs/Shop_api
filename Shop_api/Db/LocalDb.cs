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
            database.CreateTableAsync<Item>().Wait();



        }
    }
}
