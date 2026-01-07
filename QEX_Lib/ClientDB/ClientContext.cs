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
    public class ClientContext : DbContext
    {
        public ClientContext(DbContextOptions<ClientContext> options) : base(options) { }
        public DbSet<CustomFieldPosition> CustomFields => Set<CustomFieldPosition>();
        public DbSet<CustomProfileField> CustomProfileFields => Set<CustomProfileField>();
        public DbSet<Extension> Extensions => Set<Extension>();
        public DbSet<Setting> Settings => Set<Setting>();
        public DbSet<SettingElementStyle> SettingStyles => Set<SettingElementStyle>();
        public DbSet<SettingTheme> SettingThemes => Set<SettingTheme>();
        public DbSet<UserAvatarList> UserAvatarLists => Set<UserAvatarList>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            base.OnModelCreating(modelBuilder);
        
        public static ClientContext Create(string ConnectionString) => 
            new ClientContext(new DbContextOptionsBuilder<ClientContext>().
                UseSqlServer(ConnectionString).Options);
        
    }
}
