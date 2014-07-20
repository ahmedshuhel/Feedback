using System.Data.Entity;

namespace ComplaintBox.Web.Models
{
    public class CboxContext : DbContext
    {
        public CboxContext()
            : base("CBox")
        {

        }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Organization> Organization { get; set; }
        public DbSet<Settings>Settings {get; set;}


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SubjectConfiguration());
        }
    }
}