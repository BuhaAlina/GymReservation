using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GymScheduler.ViewModels
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
        [Remote("IsEmailValid", "User", AdditionalFields ="CurrentEmail", ErrorMessage = "This email already exists")]
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

        [Display(Name = "Access Level")]
        public int RoleId { get; set; }

        [Display(Name = "Accessibility")]
        public string RoleName { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [UIHint("YesNo")]
        public bool IsActive { get; set; }
    }
}