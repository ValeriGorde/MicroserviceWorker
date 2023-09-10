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
        public DbSet<Gender> Genders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Gender>().HasData(new List<Gender>
            {
                    new() {Id = 1, Name = "Женский"},
                    new() {Id = 2, Name = "Мужской"}
            });

            modelBuilder.Entity<Worker>().HasData(new List<Worker>
            {
                new()
                {
                    Id = 1,
                    FirstName = "Валерия",
                    LastName = "Гордеева",
                    Patronymic = "Александровна",
                    BirthDate = "14.11.2001",
                    GenderId = 1,
                    HasChildren = false
                }
            });
        }
    }
}
