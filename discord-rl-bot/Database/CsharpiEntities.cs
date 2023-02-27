using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CodyTedrick.DiscordBot.Database;

public class CsharpiEntities : DbContext
{
    public virtual DbSet<UserInfo?> UserInfo{ get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder {DataSource = "csharpi.db"};
        var connectionString = connectionStringBuilder.ToString();
        var connection = new SqliteConnection(connectionString);
        optionsBuilder.UseSqlite(connection);
    }
}