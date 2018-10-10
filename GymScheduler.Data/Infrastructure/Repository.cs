using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymScheduler.Domain;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace GymScheduler.Data.Infrastructure
{
    public class Repository<T> : IRepository<T> where T:class
    {

        private GymEntities myGymEntities;
        private readonly IDbSet<T> dbSet;
        public Repository(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbSet = MyGymEntities.Set<T>();
        }

        public IDatabaseFactory DatabaseFactory { get; }

        protected GymEntities MyGymEntities
        {
            get { return myGymEntities ?? (myGymEntities = DatabaseFactory.Get()); }
        }
        //CRUD

        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            myGymEntities.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual int Count()
        {
            return dbSet.Count();
        }


        public void ExecuteCommand(string command, params object[] parameters)
        {
            myGymEntities.Database.ExecuteSqlCommand(command, parameters);
        }


        public IEnumerable<T> SqlQuery(string query, params object[] parameters)
        {
            return myGymEntities.Set<T>().SqlQuery(query, parameters);
        }

        public IEnumerable<T> ExecuteStoreQuery<T>(string query, params object[] parameters)
        {
            return  ((IObjectContextAdapter)myGymEntities).ObjectContext.ExecuteStoreQuery<T>(query, parameters);
        }


    }
}
