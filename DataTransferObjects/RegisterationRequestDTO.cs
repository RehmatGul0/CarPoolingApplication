using System.ComponentModel.DataAnnotations;


namespace CarPoolingApp.DataTransferObjects
{
    public class RegisterationRequestDTO
    {
        [Required]
        public string name { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [StringLength(11)]
        [MinLength(6)]
        public string password { get; set; }
        [Required]
        [Phone]
        [StringLength(11)]
        [MaxLength(11)]
        public string phone { get; set; }
        [StringLength(1)]
        public string gender { get; set; }
    }
    public class AdminRegisterationRequestDTO {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
