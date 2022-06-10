using Angular_SPA_Sibintek.API.Core.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Angular_SPA_Sibintek.API.Core.DAL
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<UserFileEntity> Files { get; set; }

        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {

        }
    }
}
