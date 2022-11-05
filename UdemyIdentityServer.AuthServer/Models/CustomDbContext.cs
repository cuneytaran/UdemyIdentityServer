using Microsoft.EntityFrameworkCore;

namespace UdemyIdentityServer.AuthServer.Models
{
    public class CustomDbContext:DbContext
    {
        //nuget package den 
        //Microsoft.EntityFrameworkCore
        //Microsoft.EntityFrameworkCore.SqlServer
        //Microsoft.EntityFrameworkCore.Tools

        public CustomDbContext(DbContextOptions<CustomDbContext> opts):base(opts)
        {

        }

        public DbSet<CustomUser> customUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CustomUser>().HasData(
                new CustomUser() { Id = 1,Email="cuneytaran@gmail.com",Password="12345",City="İstanbul",UserName="caran" },
                new CustomUser() { Id = 2,Email="mehmetaran@gmail.com",Password="12345",City="Konya",UserName="caran2" },           
                new CustomUser() { Id = 3,Email="cemilaran@gmail.com",Password="12345",City="İzmir",UserName="caran3" }          
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
