using System;
using Npgsql;

class AddMigrationProgram
{
    static void Main()
    {
        var connectionString = "Host=ep-royal-grass-a1ge7i1b-pooler.ap-southeast-1.aws.neon.tech;Username=neondb_owner;Password=npg_NdcQ2i6FsoZx;Database=neondb";
        using var conn = new NpgsqlConnection(connectionString);
        conn.Open();
        using var cmd = new NpgsqlCommand("INSERT INTO \"__EFMigrationsHistory\" (\"MigrationId\", \"ProductVersion\") VALUES ('20251007150022_InitialCreate', '9.0.0')", conn);
        cmd.ExecuteNonQuery();
        Console.WriteLine("Migration history added.");
    }
}
