using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.ComponentModel.DataAnnotations;
using System;
using System.Data.Entity;

namespace Ahlam.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Last Modification Date")]
        public DateTime LastModificationDate { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Picture Id")]
        public string PictureId { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
       

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // configures one-to-many relationship
            //modelBuilder.Entity<UserWorkBinding>()
            //    .HasRequired<ApplicationUser>(s => s.User)
            //    .WithMany(g => g.userWorkBinding)
            //    .HasForeignKey<string>(s => s.UserId);

            //modelBuilder.Entity<UserWorkBinding>()
            //    .HasRequired<UserWork>(s => s.UserWork)
            //    .WithMany(g => g.userWorkBinding)
            //    .HasForeignKey<int>(s => s.UserWorkId);

            //modelBuilder.Entity<ServiceComment>()
            //    .HasRequired<Service>(s => s.Service)
            //    .WithMany(g => g.Comments)
            //    .HasForeignKey<int>(s => s.ServiceId);

            //modelBuilder.Entity<CompetitionPrize>()
            //    .HasRequired<Competition>(s => s.competition)
            //    .WithMany(g => g.prizes)
            //    .HasForeignKey<int>(s => s.CompetitionId);

            //modelBuilder.Entity<Service>()
            //    .HasRequired<ApplicationUser>(s => s.ServiceProvider)
            //    .WithMany(g => g.Services)
            //    .HasForeignKey<String>(s => s.ServiceProviderId);


            base.OnModelCreating(modelBuilder);
        }
        public System.Data.Entity.DbSet<Attachment> Attachments { get; set; }
        public System.Data.Entity.DbSet<ServicePath> ServicePaths { get; set; }

        public System.Data.Entity.DbSet<Dream> Dreams { get; set; }
        public System.Data.Entity.DbSet<Payment> Payments { get; set; }
    }
}