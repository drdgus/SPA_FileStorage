using Microsoft.EntityFrameworkCore;
using SPA_Sibintek_NET6.API.Core.DAL.Entities;

namespace SPA_Sibintek_NET6.API.Core.DAL
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<UserFileEntity> Files { get; set; }

        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {

        }
    }
}
