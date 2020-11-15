using System.ComponentModel.DataAnnotations;

namespace Business.Models.Requests
{
    public class CredentialsModel
    {
        [Required] public string Login { get; set; } = null!;
        [Required] public string Password { get; set; } = null!;
    }
}