using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Entities.Events;
using GestaoDefeitos.Domain.Identity;
using GestaoDefeitos.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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
        public DbSet<DefectChangeEvent> DefectHistory { get; set; }
        public DbSet<DefectComment> DefectComments { get; set; }
        public DbSet<DefectAttachment> DefectAttachments { get; set; }
        public DbSet<DefectRelation> DefectRelations { get; set; }
        public DbSet<TrelloUserStory> TrelloUserStories { get; set; }
        public DbSet<DefectMailLetter> DefectMailLetters { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ContributorConfiguration());
            builder.ApplyConfiguration(new ProjectContributorConfiguration());
            builder.ApplyConfiguration(new DefectRelationConfiguration());
            builder.ApplyConfiguration(new DefectChangeEventConfigurations());
            builder.ApplyConfiguration(new DefectCommentConfiguration());
            builder.ApplyConfiguration(new TrelloUserStoryConfiguration());
            builder.ApplyConfiguration(new DefectAttachmentConfiguration());
            builder.ApplyConfiguration(new DefectConfiguration());
            builder.ApplyConfiguration(new DefectMailLetterConfiguration());
            builder.ApplyConfiguration(new ProjectConfiguration());
            builder.ApplyConfiguration(new TagConfiguration());
            builder.ApplyConfiguration(new DefectAttachmentExtraConfiguration());
            builder.ApplyConfiguration(new DefectCommentExtraConfiguration());
            builder.ApplyConfiguration(new DefectChangeEventExtraConfiguration());
            builder.ApplyConfiguration(new TrelloUserStoryConfiguration());
        }
    }
}
