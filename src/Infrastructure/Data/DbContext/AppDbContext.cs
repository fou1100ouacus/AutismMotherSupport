using Domain.Models.IdentityUser;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbContext
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        #region Db Tables
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Child> Children { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);

    builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

    // ====================== حل مشكلة One-to-One ======================
    builder.Entity<Child>()
        .HasOne(c => c.Mother)           // Child ليه Mother واحدة
        .WithOne(u => u.Child)           // AppUser ليه Child واحد
        .HasForeignKey<Child>(c => c.MotherId)   // Foreign Key موجود في جدول Child
        .OnDelete(DeleteBehavior.Cascade);       // لو حذفنا الأم نحذف الطفل (اختياري)
    // ================================================================
}
    }
}
