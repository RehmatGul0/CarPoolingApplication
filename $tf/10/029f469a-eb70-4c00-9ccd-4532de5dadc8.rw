
using System;
using System.ComponentModel.DataAnnotations;

namespace CarPoolingApp.DataTransferObjects
{
    public class SetPreferenceRequestDTO
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string session_id { get; set; }

        [Required]
        [RegularExpression(@"^[MF]$")]
        [StringLength(1)]
        public string gender;

        [Required]
        public bool notification ;

    }
}
