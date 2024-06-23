using BlogAPI.Models;
using BlogAPI.Models.DatabaseSchema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        public DbSet<ArticleModel> ArticleModels { get; set; }
        public DbSet<CommentModel> CommentModels { get; set; }

    }
}