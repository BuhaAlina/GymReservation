using GymScheduler.Data.Infrastructure;
using GymScheduler.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GymScheduler.Services
{
    public class CategoryService : ICategoryService
    {
        private DatabaseFactory factory;
        private IRepository<Domain.Category> categoryRepository;
        private IRepository<Domain.ClassType> classRepository;
        private readonly IUnitofWork unitOfWork;

        public CategoryService(DatabaseFactory factory)
        {

            this.factory = factory;
            this.categoryRepository = new Repository<Domain.Category>(factory);
            this.classRepository = new Repository<Domain.ClassType>(factory);
            this.unitOfWork = new UnitofWork(this.factory);

        }

        public void AddCategory(Category myCateg)
        {
            categoryRepository.Add(myCateg);
            unitOfWork.Commit();
        }

        public void EditCategory(Category myCateg)
        {
            categoryRepository.Update(myCateg);
            unitOfWork.Commit();
        }
        public void DeleteCategory(Category myCateg, int id)
        {
            var isUsed = classRepository.GetAll().FirstOrDefault(x => x.CategoryId == id);

            if (isUsed == null)
            {
                categoryRepository.Delete(myCateg);
                unitOfWork.Commit();
            }
            else
            {
                throw new ArgumentException();
            }


        }

    }
}
