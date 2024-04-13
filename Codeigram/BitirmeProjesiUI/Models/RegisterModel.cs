using System.ComponentModel.DataAnnotations;

namespace BitirmeProjesiUI.Models
{
    public class RegisterModel
    {
        [Required]
        public string Name { get; set; }
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string RePassword { get; set; }


    }
}
