using ApiFinanceiro.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiFinanceiro.Context;

public class IdentityContext(DbContextOptions<IdentityContext> options) : IdentityDbContext<ApplicationUser>(options)
{
}
