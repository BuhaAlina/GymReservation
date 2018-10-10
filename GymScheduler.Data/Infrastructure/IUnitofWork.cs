using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymScheduler.Data.Infrastructure
{
    public interface IUnitofWork
    {
        void Commit();
    }
}
