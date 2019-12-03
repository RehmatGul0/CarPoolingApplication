using System;
using System.ComponentModel.DataAnnotations;


namespace CarPoolingApp.DataTransferObjects
{
    public class GetRideRequestDTO
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string session_id { get; set; }

        [Required]
        public Location pickUp { get; set; }

        [Required]
        public Location dropOff { get; set; }

        [Required]
        public int radius { get; set; }
    }
}
