using GestaoDefeitos.Application.TrelloIntegration;
using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Identity;
using GestaoDefeitos.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestaoDefeitos.Infrastructure.Database
{
    public class AppDbContext : IdentityDbContext<
        Contributor,
        ApplicationRole,
        Guid,
        ApplicationUserClaim,
        ApplicationUserRole,
        ApplicationUserLogin,
        ApplicationRoleClaim,
        ApplicationUserToken>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Defect> Defects { get; set; }
        public DbSet<ProjectContributor> ProjectContributors { get; set; }
        public DbSet<ContributorNotification> ContributorNotifications { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<DefectHistory> DefectHistory { get; set; }
        public DbSet<DefectComment> DefectComments { get; set; }
        public DbSet<DefectAttachment> DefectAttachments { get; set; }
        public DbSet<DefectRelation> DefectRelations { get; set; }
        public DbSet<DefectDetailsView> DefectDetailsView { get; set; }
        public DbSet<TrelloUserStory> TrelloUserStories { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ContributorConfiguration());
            builder.ApplyConfiguration(new ProjectContributorConfiguration());
            builder.ApplyConfiguration(new DefectRelationConfiguration());
            builder.ApplyConfiguration(new DefectHistoryConfigurations());
            builder.ApplyConfiguration(new DefectCommentConfiguration());
            builder.ApplyConfiguration(new TrelloUserStoryConfiguration());
            builder.ApplyConfiguration(new DefectAttachmentConfiguration());

            builder.Entity<DefectDetailsView>().HasNoKey().ToView("vw_DefectDetails");
        }
    }
}
