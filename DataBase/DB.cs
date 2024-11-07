using OrganizationsEmployeesDictionaryWPF.Models;
using OrganizationsEmployeesDictionaryWPF.Service;
using SQLite;
using SQLitePCL;
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
        private static readonly string DBPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase", "database.db");
        private static SQLiteAsyncConnection db;

        public static async Task Initialize()
        {

            if (!File.Exists(DBPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(DBPath) ?? string.Empty);
            }

            db = new SQLiteAsyncConnection(DBPath);

            await db.CreateTableAsync<Organization>();
            await db.CreateTableAsync<Employee>();
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
            try
            {
                await db.InsertAsync(entity);
                Logger.Instance.LogInformation($"данные успешно добавлены в БД, тип данных: {typeof(T).Name}");
            }
            catch(Exception ex)
            {
                Logger.Instance.LogError($"Ошибка добавления объекта в базу данных: {ex.Message}");
            }
            
        }
        public static async Task AddAllAsync<T>(List<T> entities) where T : new()
        {
            try
            {
                await db.InsertAllAsync(entities);
                Logger.Instance.LogInformation($"Записи успешно добавлены в базу данных, тип данных: {typeof(T).Name}");
            }
            catch(Exception ex)
            {
                Logger.Instance.LogError($"Ошибка добавления объектов в базу данных: {ex.Message}");
            }
            
        }

        public static async Task<List<T>> GetAllTable<T>() where T : new()
        {
            try
            {
                var result = await db.Table<T>().ToListAsync();
                Logger.Instance.LogInformation($"Успешно полученны данные из таблицы. Получено {result.Count} записей типа {typeof(T).Name}");
                return result;
            }
            catch(Exception ex)
            {
                Logger.Instance.LogError($"Ошибка получения данных из таблицы: {ex.Message}");
                return new List<T>();
            }
                
        }

        public static async Task<T> GetByIdAsync<T>(int id) where T: new()
        {
            try
            {
                var result = await db.FindAsync<T>(id);
                Logger.Instance.LogInformation($"Успешно получены данные из таблицы по id:{id}, тип данных: {typeof(T).Name}");
                return result;
            }
            catch(Exception ex)
            {
                Logger.Instance.LogError($"Ошибка получения данных из базы: {ex.Message}");
                return new T();
            }
            
        }

        public static async Task UpdateAsync<T>(T entity) where T: new()
        {

            await db.UpdateAsync(entity);
            Logger.Instance.LogInformation($"Данные успешно обновлены, тип данных: {typeof(T).Name}") ;
        }

        public static async Task DeleteById<T>(int id) where T : new()
        {
            try
            {
                var entity = await db.FindAsync<T>(id);
                if (entity != null)
                {
                    await db.DeleteAsync(entity);
                }
                Logger.Instance.LogInformation($"Данные {typeof(T).Name} c id: {id} успешно удалены");
            }
            catch(Exception ex)
            {
                Logger.Instance.LogError($"Ошибка удаления данных: {ex.Message}"); 
            }
        }
        public static async Task DeleteAsync<T>(T entity) where T: new()
        {
            try
            {
                await db.DeleteAsync(entity); 
                Logger.Instance.LogInformation($"Данные {typeof(T).Name} успешно удалены");
            }
            catch (Exception ex)
            {
                Logger.Instance.LogError($"Ошибка удаления данных: {ex.Message}");
            }
        }


    }
}
