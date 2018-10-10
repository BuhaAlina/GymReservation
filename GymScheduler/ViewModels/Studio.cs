using System.ComponentModel.DataAnnotations;

namespace GymScheduler.ViewModels
{
    public class Studio
    {
        public int Id { get; set; }

        [Required]
        [MinLength(4)]
        public string Name { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Enter a number betwen 0 and 100")]
        public int? Capacity { get; set; }

    }
}