using Microsoft.EntityFrameworkCore;

namespace PeopleWebApp.Data
{
    public class PeopleContext : DbContext
    {
        public PeopleContext(DbContextOptions<PeopleContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Person> Persons { get; set; }
    }
}
