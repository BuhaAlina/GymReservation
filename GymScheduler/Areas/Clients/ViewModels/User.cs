using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GymScheduler.Areas.Clients.ViewModels
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "This is not a valid email address")]
        [Remote("IsEmailValid", "User", AdditionalFields = "CurrentEmail", ErrorMessage = "This email already exists")]
        public string Email { get; set; }

        public string CurrentEmail { get; set; }

        [Display(Name = "Street")]
        public string StreetName { get; set; }

        [Display(Name = "Street Number")]
        public string StreetNo { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        [Display(Name = "Telephone Number")]
        public string TelephoneNo { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}