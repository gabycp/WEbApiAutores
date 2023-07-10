using System.ComponentModel.DataAnnotations;

namespace WEbApiAutores.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
