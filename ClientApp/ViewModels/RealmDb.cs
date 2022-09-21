using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdminApp.ViewModels
{
    public static class RealmDb<T> where T : RealmObject, new()
    {
        static Realm db;
        public static void Add(T data)
        {
            db = Realm.GetInstance();
            db.Write(() =>
            {
                db.Add(data);
            });
        }
        public static T Get(Expression<Func<T, bool>> predicate)
        {
            db = Realm.GetInstance();
            var data = db.All<T>().FirstOrDefault(predicate);
            return data;
        }
        public static List<T> GetAll()
        {
            //var config = RealmConfiguration.DefaultConfiguration;
            //Realm.DeleteRealm(config);
            db = Realm.GetInstance();
            var data = db.All<T>().ToList();
            return data;
        }
        public static void Delete(Expression<Func<T, bool>> predicate)
        {
            db = Realm.GetInstance();
            db.Write(() =>
            {
                db.Remove(Get(predicate));
            });
        }
        public static void Update(T model, Expression<Func<T, bool>> predicate)
        {
            //push
            db = Realm.GetInstance();
            var data = Get(predicate);
            if (data != null)
            {
                db.Write(() =>
                {
                    data = model;
                    db.Add(data, true);
                });
            }
        }
    }

}
