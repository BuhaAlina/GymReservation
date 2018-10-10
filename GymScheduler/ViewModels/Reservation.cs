using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using GymScheduler.Domain;

namespace GymScheduler.ViewModels
{
    public class Reservation
    {
        

        public int Id { get; set; }
        
        [Remote("DuplicityCheck", "Reservation", AdditionalFields = "TimetableId", ErrorMessage = "Duplicate!")]
        public int UserId { get; set; }
        [Remote("CapacityCheck", "Reservation",  ErrorMessage = "You can`t make this reservation!")]
        public int TimetableId { get; set; }

        


        [DisplayName("User Name")]
        public string FullName { get; set; }
        [DisplayName("Reservation time")]
        public TimeSpan Time { get; set; }
        [DisplayName("Reservation date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public  System.DateTime Date { get; set; }
        
        public string Class { get; set; }
        
        public int Studio { get; set; }
    }
}
