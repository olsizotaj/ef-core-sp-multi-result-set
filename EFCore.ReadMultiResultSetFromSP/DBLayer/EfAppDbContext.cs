using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ReadMultiResultSetFromSP.DBLayer
{
    public class EfAppDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
    {
        public EfAppDbContext(DbContextOptions<EfAppDbContext> options) : base(options)
        {
        }
    }
}