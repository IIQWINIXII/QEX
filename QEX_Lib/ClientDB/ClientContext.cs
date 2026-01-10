using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Options;
using QEX_Lib.ClientDB.Model;

namespace QEX_Lib.ClientDB
{
    public class ClientContext(DbContextOptions<ClientContext> options) : DbContext(options)
    {
        public static string ConnectionString { get; set; } = "Server=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" +
        Path.Combine(AppContext.BaseDirectory, "Resources", "Data", "QEX_CLIENT_DB.mdf") +
        ";Integrated Security=True;Connect Timeout=30;";

        public DbSet<CustomFieldPosition> CustomFields => Set<CustomFieldPosition>();
        public DbSet<CustomProfileField> CustomProfileFields => Set<CustomProfileField>();
        public DbSet<Extension> Extensions => Set<Extension>();
        public DbSet<Setting> Settings => Set<Setting>();
        public DbSet<SettingElementStyle> SettingStyles => Set<SettingElementStyle>();
        public DbSet<SettingTheme> SettingThemes => Set<SettingTheme>();
        public DbSet<UserAvatar> UserAvatarLists => Set<UserAvatar>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            base.OnModelCreating(modelBuilder);
        
        public static ClientContext Create() => 
            new(new DbContextOptionsBuilder<ClientContext>().
                UseSqlServer(ConnectionString).Options);
        
    }
}
