using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GymScheduler.ViewModels
{
    public class Category
    {

        public int Id { get; set; }
        [Required]
        [MinLength(5)]
        public string Name { get; set; }
    }
}