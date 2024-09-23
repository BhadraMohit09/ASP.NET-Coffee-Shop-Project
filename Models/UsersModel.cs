using System.ComponentModel.DataAnnotations;

namespace FormCRUDB_MAB.Models
{
    public class UsersModel
    {
        [Required]

        public int UserID { get; set; }
        [Required]

        public string? UserName { get; set; }
        [Required]

        public string? UserEmail { get; set; }
        [Required]

        public string? Password { get; set; }
        [Required]

        public string? MobileNo { get; set; }
        [Required]

        public string? Address { get; set; }
        
        public string IsActive { get; set; }
    }

    public class UserLoginModel
    {
        [Required(ErrorMessage = "UserID is required.")]
        public int UserID { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
