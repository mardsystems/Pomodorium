using Microsoft.Data.Sqlite;
using System.DomainModel.Storage;

namespace Pomodorium.Data.Sqlite.Features.Storage;

public class SqliteStore : IAppendOnlyStore
{
    private readonly string _connectionString;

    public SqliteStore(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<EventRecord>> ReadRecords(long maxCount)
    {
        var records = new List<EventRecord>();

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            const string sql = @"
SELECT Name, Version, Date, Data FROM EventStore
ORDER BY Name, Version
LIMIT 0, @take
";

            using var command = new SqliteCommand(sql, connection);

            command.Parameters.AddWithValue("@take", maxCount);

            using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var name = reader["Name"].ToString();

                var typeName = reader["TypeName"].ToString();

                var version = (long)reader["Version"];

                var dateString = reader["Date"].ToString();

                var date = Convert.ToDateTime(dateString);

                var data = (byte[])reader["Data"];

                if (name == null)
                {
                    throw new InvalidOperationException();
                }

                if (typeName == null)
                {
                    throw new InvalidOperationException();
                }

                records.Add(new EventRecord(name, version, date, typeName, data));
            }
        }

        return records;
    }

    public async Task<IEnumerable<EventRecord>> ReadRecords(string name, long afterVersion, long maxCount)
    {
        var records = new List<EventRecord>();

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            const string sql = @"
SELECT Version, Date, Data FROM EventStore
WHERE Name = @name AND Version > @version
ORDER BY Version
LIMIT 0, @take
";

            using var command = new SqliteCommand(sql, connection);

            command.Parameters.AddWithValue("@name", name);

            command.Parameters.AddWithValue("@version", afterVersion);

            command.Parameters.AddWithValue("@take", maxCount);

            using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                var typeName = reader["TypeName"].ToString();

                var version = (long)reader["Version"];

                var date = (DateTime)reader["Date"];

                var data = (byte[])reader["Data"];

                if (typeName == null)
                {
                    throw new InvalidOperationException();
                }

                records.Add(new EventRecord(name, version, date, typeName, data));
            }
        }

        return records;
    }

    public async Task<EventRecord> Append(string name, string typeName, DateTime date, byte[] data, long expectedVersion = -1)
    {
        using var connection = new SqliteConnection(_connectionString);

        connection.Open();

        using var transaction = connection.BeginTransaction();

        var version = await GetMaxVersion(name, expectedVersion, connection, transaction);

        const string sql = @"
INSERT INTO EventStore (Name, Version, Date, Data)
VALUES(@name, @version, @date, @data)
";

        using (var command = new SqliteCommand(sql, connection, transaction))
        {
            command.Parameters.AddWithValue("@name", name);

            command.Parameters.AddWithValue("@version", version + 1);

            command.Parameters.AddWithValue("@date", date);

            command.Parameters.AddWithValue("@data", data);

            await command.ExecuteNonQueryAsync();
        }

        transaction.Commit();

        return default!;
    }

    public Task Append(EventRecord tapeRecord)
    {
        throw new NotImplementedException();
    }

    private static async Task<long> GetMaxVersion(string name, long expectedVersion, SqliteConnection connection, SqliteTransaction transaction)
    {
        const string sql = @"
SELECT COALESCE(MAX(Version),0)
FROM EventStore
WHERE Name=@name
";

        using var command = new SqliteCommand(sql, connection, transaction);

        command.Parameters.AddWithValue("@name", name);

        var result = await command.ExecuteScalarAsync() ?? throw new InvalidOperationException();

        var version = (long)result;

        if (expectedVersion != -1)
        {
            if (version != expectedVersion)
            {
                throw new AppendOnlyStoreConcurrencyException(version, expectedVersion, name);
            }
        }

        return version;
    }

    public void Close()
    {

    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
