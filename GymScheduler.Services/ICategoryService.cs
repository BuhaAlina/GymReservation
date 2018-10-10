using GymScheduler.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymScheduler.Services
{
    public interface ICategoryService
    {
        void AddCategory(Category myCateg);
        void EditCategory(Category myCateg);
        void DeleteCategory(Category myCateg, int id);
    }
}
