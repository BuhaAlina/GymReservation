using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GymScheduler.ViewModels
{
    public class Role
    {
       
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}