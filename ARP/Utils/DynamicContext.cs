using ARP.DataModels;
using ARP.DataModels.Static;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.TypeMapping;

namespace ARP.Utils
{
    public class DynamicContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Character> Characters { get; set; } = null!;
        public DbSet<Admin> Admins { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();
            optionsBuilder.UseNpgsql("Host=localhost;Database=RPMod;Username=postgres;Password=890142");
        }
    }

    public class StaticContext : DbContext
    {
        public DbSet<CommandInfo> Commands { get; set; }
        public DbSet<Localize> Strings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();
            optionsBuilder.UseNpgsql("Host=localhost;Database=StaticARP;Username=postgres;Password=890142");
        }
    }
}
