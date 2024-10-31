using OrganizationsEmployeesDictionaryWPF.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

        public static async Task AddAsync<T>(T entity) where T: new()
        {
            await db.InsertAsync(entity);
        }

        public static async Task<List<T>> GetAllTable<T>() where T : new()
        {
            return await db.Table<T>().ToListAsync();
        }

        public static async Task<T> GetByIdAsync<T>(int id) where T: new()
        {
            return await db.FindAsync<T>(id);
        }

        public static async Task UpdateById<T>(int id) where T: new()
        {
            var entity = await db.FindAsync<T>(id);
            if (entity != null)
            {
                await db.UpdateAsync(entity);
            }
        }

        public static async Task DeleteById<T>(int id) where T : new()
        {
            var entity = await db.FindAsync<T>(id);
            if(entity != null)
            {
                await db.DeleteAsync(entity);
            }
        }

    }
}
