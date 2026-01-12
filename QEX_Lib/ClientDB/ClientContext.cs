using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.Options;
using QEX_Lib.ClientDB.Model;
using System.Diagnostics;
using System.Data.Common;

namespace QEX_Lib.ClientDB
{
    public class ClientContext(DbContextOptions<ClientContext> options) : DbContext(options)
    {
        // Шаблон единой строки подключения для SQLite (формируется с реальным путем в рантайме)
        public static string? ConnectionStringTemplate { get; set; } = "Data Source={0};Cache=Shared;Mode=ReadWriteCreate;";

        public DbSet<CustomFieldPosition> CustomFields => Set<CustomFieldPosition>();
        public DbSet<CustomProfileField> CustomProfileFields => Set<CustomProfileField>();
        public DbSet<Extension> Extensions => Set<Extension>();
        public DbSet<Setting> Settings => Set<Setting>();
        public DbSet<SettingStyle> SettingsStyles => Set<SettingStyle>();
        public DbSet<SettingTheme> SettingsThemes => Set<SettingTheme>();
        public DbSet<UserAvatar> UsersAvatars => Set<UserAvatar>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            base.OnModelCreating(modelBuilder);

        public static ClientContext Create() =>
            new ClientContext(new DbContextOptionsBuilder<ClientContext>()
                .UseSqlite(string.Format(ConnectionStringTemplate ?? "Data Source={0}", 
                Path.Combine(AppContext.BaseDirectory, "Resources", "Data", "QEX_DB_CL")))
                .Options);
       
    }
}
