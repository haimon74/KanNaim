
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;

namespace DashboardDataAccess
{
    public static class DatabaseHelper
    {
        public const string ConnectionStringName = "DashboardConnectionString";
        public const string ApplicationID = "fd639154-299a-4a9d-b273-69dc28eb6388";
        public readonly static Guid ApplicationGuid = new Guid(ApplicationID);

        public static DashboardDataContext GetDashboardData()
        {
            var db = new DashboardDataContext(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString);
            return db;
        }

        public static void Update<T>(T obj, Action<T> update) where T : class
        {
            using (var db = GetDashboardData())
            {   
                db.GetTable<T>().Attach(obj);
                update(obj);
                db.SubmitChanges();
            }
        }
        public static void UpdateAll<T>(List<T> items, Action<T> update) where T : class
        {
            using (var db = GetDashboardData())
            {
                Table<T> table = db.GetTable<T>();
                foreach (T item in items)
                {
                    table.Attach(item);
                    update(item);
                }

                db.SubmitChanges();
            }
        }
        public static void Delete<T>(T entity) where T : class,new()
        {
            using (var db = GetDashboardData())
            {
                Table<T> table = db.GetTable<T>();
                table.Attach(entity);
                table.DeleteOnSubmit(entity);
                db.SubmitChanges();
            }
        }
        public static void Insert<T>(T obj) where T : class
        {
            using (var db = GetDashboardData())
            {
                db.GetTable<T>().InsertOnSubmit(obj);
                db.SubmitChanges();
            }
        }
    }
}
