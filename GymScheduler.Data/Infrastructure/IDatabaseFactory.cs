using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymScheduler.Domain;

namespace GymScheduler.Data.Infrastructure
{
    public interface IDatabaseFactory
    {

        GymEntities Get();
        void Dispose();

    }
}
