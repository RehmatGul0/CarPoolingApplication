
using System;
using System.ComponentModel.DataAnnotations;

namespace CarPoolingApp.DataTransferObjects
{
    public class BookTripRequestDTO
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string session_id { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-mm-dd HH:mm:ss}")]
        public DateTime time { get; set; }

        [Required]
        public long ride_id;

        [Required]
        public int seats;

    }
}
