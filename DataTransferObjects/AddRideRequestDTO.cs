using System;
using System.ComponentModel.DataAnnotations;
namespace CarPoolingApp.DataTransferObjects
{
    public class AddRideRequestDTO
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string session_id { get; set; }

        [Required]
        public int seats { get; set; }
        [Required]
        public int fee { get; set; }
        [Required]
        public string startLocation { get; set; }

        [Required]
        public string endLocation { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-mm-dd HH:mm:ss}")]
        public DateTime time { get; set; }


        [Required]
        public Location[] locations { get; set; }
    }
    public class Location
    {
        [Required]
        public float lat { get; set; }
        [Required]
        public float lon { get; set; }
    }
}
