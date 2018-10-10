using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymScheduler.Domain;

namespace GymScheduler.Data.Infrastructure
{
    public class DatabaseFactory :Disposable, IDatabaseFactory
    {

        private GymEntities dataEntities;

        public GymEntities Get()
        {
            return dataEntities ?? (dataEntities = new GymEntities());
        }

        protected override void DisposeCore()
        {
            dataEntities?.Dispose();
        }



    }
}
