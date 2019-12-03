using System;
using System.ComponentModel.DataAnnotations;

namespace CarPoolingApp.DataTransferObjects
{
    public class CancelTripRequestDTO
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string session_id { get; set; }

        [Required]
        public long tripID { get; set; }
    }
}
