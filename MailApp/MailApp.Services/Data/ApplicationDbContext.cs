using Microsoft.EntityFrameworkCore;
using ReactorData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailApp.Services.Data;

class ApplicationDbContext : DbContext
{
    private readonly string? _databasePath;
    private const string _dbName = "MailApp.db";

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPathProvider pathProvider)
            : base(options)
    {
        _databasePath = Path.Combine(pathProvider.GetDefaultLocalMachineCacheDirectory() ?? throw new InvalidOperationException(), _dbName);

        if (File.Exists(_databasePath))
        {
            File.Delete(_databasePath);
        }
    }

    public ApplicationDbContext()
    {

    }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Message> Messages => Set<Message>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            if (_databasePath == null)
            {
                optionsBuilder.UseSqlite($"Data Source=dummy");
            }
            else
            {
                optionsBuilder.UseSqlite($"Data Source={_databasePath}")
#if DEBUG
                    .EnableSensitiveDataLogging()
#endif
                    ;
            }
        }

        base.OnConfiguring(optionsBuilder);
    }


}
