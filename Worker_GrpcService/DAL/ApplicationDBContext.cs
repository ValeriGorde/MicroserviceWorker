using Microsoft.EntityFrameworkCore;
using Worker_GrpcService.DAL.Models;

namespace Worker_GrpcService.DAL
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }
        }
        public DbSet<Worker> Workers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Worker>().HasData(new List<Worker>
            {
                new()
                {
                    Id = 1,
                    FirstName = "Валерия",
                    LastName = "Гордеева",
                    Patronymic = "Александровна",
                    BirthDate = "14.11.2001",
                    Gender = "Женский",
                    HasChildren = false
                }
            });
        }
    }
}
