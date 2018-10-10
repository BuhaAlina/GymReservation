using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GymScheduler.ViewModels
{
    public class Timetable
    {


        public int Id { get; set; }
        [Required]
        [DisplayName("Time")]
        public System.TimeSpan StartTime { get; set; }
        public int CategoryId { get; set; }
        public int StudioId { get; set; }
        public int ClassTypeId { get; set; }
        public int UserId { get; set; }


        public string CategoryName { get; set; }
        public string StudioName { get; set; }
        public string ClassName { get; set; }
        public string UserName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Remote("IsDateValid", "TimeTable", ErrorMessage = "Date can't be before today")]
        public System.DateTime Date { get; set; }
        public bool isActive { get; set; }
    }
}