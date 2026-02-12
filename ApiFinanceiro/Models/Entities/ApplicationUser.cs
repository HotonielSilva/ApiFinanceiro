using Microsoft.AspNetCore.Identity;

namespace ApiFinanceiro.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public bool Ativo { get; set; } = true;

        public string Nickname { get; set; } = string.Empty;
    }

}
