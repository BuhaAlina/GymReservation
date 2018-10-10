using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymScheduler.Domain;

namespace GymScheduler.Services
{
    
        public interface IStudioService
        {
            void AddStudio(Studio myStudio);
            void EditStudio(Studio myStudio);
            void DeleteStudio(Studio myStudio, int id);
        }
    
}
