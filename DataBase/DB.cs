using OrganizationsEmployeesDictionaryWPF.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationsEmployeesDictionaryWPF.DataBase
{
    public static class DB
    {
        private static readonly string DBPath = Path.Combine(Directory.GetCurrentDirectory(),"DataBase", "database.db");
        private static SQLiteAsyncConnection db;

        public static async Task Initialize()
        {
            if (!File.Exists(DBPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(DBPath) ?? string.Empty);
                db = new SQLiteAsyncConnection(DBPath);

                await db.CreateTableAsync<Organization>();
                await db.CreateTableAsync<Employee>();
            }
        }

        public static SQLiteAsyncConnection GetConnection()
        {
            if(db == null)
            {
                db = new SQLiteAsyncConnection(DBPath);
            }
            return db;
        }
    }
}
