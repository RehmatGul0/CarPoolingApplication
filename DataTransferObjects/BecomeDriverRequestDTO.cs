using System.ComponentModel.DataAnnotations;

namespace CarPoolingApp.DataTransferObjects
{
    public class BecomeDriverRequestDTO
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string session_id { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int model { get; set; }
        [Required]
        public string plate { get; set; }
        public string description { get; set; }
    }
}
