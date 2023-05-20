using System.ComponentModel.DataAnnotations;

namespace Movies.Api.Models
{
    public class AccountModel
    {
        [Required]
        public string  UserName { get; set; }

        [Required]
        public string  Password { get; set; }
    }
}