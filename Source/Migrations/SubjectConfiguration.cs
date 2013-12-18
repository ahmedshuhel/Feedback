using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace ComplaintBox.Web.Models
{
    public class SubjectConfiguration : EntityTypeConfiguration<Subject>
    {
        public SubjectConfiguration()
        {
            HasRequired(s => s.Organization)
                .WithMany()
                .HasForeignKey(s => s.OrganizationId)
                .WillCascadeOnDelete(false);
        }
    }
}
