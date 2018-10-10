using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GymScheduler.Domain;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace GymScheduler.ViewModels
{
    public class ClassType
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required(ErrorMessage ="Duration of the class is required")]
        [Range(30,60,ErrorMessage ="Time duration must be between 30 and 60 min")]
        public int Duration { get; set; }

        [Required]
        public int CategoryId { get; set; } 
       
        public string CategoryName { get; set; }
        
    }

    public class ClassTypeContext: DbContext
    {

        public DbSet<ClassType> Classes { get; set; }

    }

}