using System;
using DatabaseHelper;
using Store.Domain;
using System.Data;
using System.Data.SqlClient;

namespace Store.Repository
{
    public abstract class BaseRepository
    {
        protected Database _database;

        public BaseRepository()
        {
            _database = new Database("Server =.; Database = StoreG12; integrated security = true; pooling = true;");
        }
        
        public void Add(string procedureName, params string[] values)
        {

        }

        public void Edit(User user)
        {

        }

        public void Delete(User user)
        {

        }

    }
}
