using System.ComponentModel.DataAnnotations;

namespace CarPoolingApp.DataTransferObjects
{
    public class GetRequestDTO
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string session_id {get; set;}
    }
}
