using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymScheduler.Domain;
using System.Data.Entity.Validation;


namespace GymScheduler.Data.Infrastructure
{
    public class UnitofWork: IUnitofWork
    {


        private readonly IDatabaseFactory databaseFactory;
        private GymEntities myGymEntities;
        public UnitofWork(IDatabaseFactory databaseFactory)
        {
            this.databaseFactory = databaseFactory;
        }


        protected GymEntities DataContext
        {
            get { return myGymEntities ?? (myGymEntities = databaseFactory.Get()); }
        }

        public void Commit()
        {
            try
            {
                DataContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }
                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                );
            }
        }
        }
}
