using Microsoft.Data.Sqlite;
using System.DomainModel.Storage;

namespace Pomodorium.Data;

public class IndexDbStore : IAppendOnlyStore
{
    private readonly string _connectionString;

    public IndexDbStore(string connectionString)
    {
        _connectionString = connectionString;
    }

    public EventRecord Append(string name, string typeName, DateTime date, byte[] data, long expectedVersion = -1)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var version = GetMaxVersion(name, expectedVersion, connection, transaction);

                const string sql = @"
INSERT INTO EventStore (Name, TypeName, Version, Date, Data)
VALUES(@name, @version, @date, @data)
";

                using (var command = new SqliteCommand(sql, connection, transaction))
                {
                    command.Parameters.AddWithValue("@name", name);

                    command.Parameters.AddWithValue("@typeName", name);

                    command.Parameters.AddWithValue("@version", version + 1);

                    command.Parameters.AddWithValue("@date", date);

                    command.Parameters.AddWithValue("@data", data);

                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
        }

        return null;
    }

    public void Append(EventRecord tapeRecord)
    {
        throw new NotImplementedException();
    }

    private static long GetMaxVersion(string name, long expectedVersion, SqliteConnection connection, SqliteTransaction transaction)
    {
        const string sql = @"
SELECT COALESCE(MAX(Version),0)
FROM EventStore
WHERE Name=@name
";

        using (var command = new SqliteCommand(sql, connection, transaction))
        {
            command.Parameters.AddWithValue("@name", name);

            var version = (long)command.ExecuteScalar();

            if (expectedVersion != -1)
            {
                if (version != expectedVersion)
                {
                    throw new AppendOnlyStoreConcurrencyException(version, expectedVersion, name);
                }
            }

            return version;
        }
    }

    public IEnumerable<EventRecord> ReadRecords(string name, long afterVersion, long maxCount)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            const string sql = @"
SELECT Version, Date, Data FROM EventStore
WHERE Name = @name AND Version > @version
ORDER BY Version
LIMIT 0, @take
";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@name", name);

                command.Parameters.AddWithValue("@version", afterVersion);

                command.Parameters.AddWithValue("@take", maxCount);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var typeName = reader["TypeName"].ToString();

                        var version = (long)reader["Version"];

                        var date = (DateTime)reader["Date"];

                        var data = (byte[])reader["Data"];

                        yield return new EventRecord(name, typeName, version, date, data);
                    }
                }
            }
        }
    }

    public IEnumerable<EventRecord> ReadRecords(long afterVersion, long maxCount)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            const string sql = @"
SELECT Name, Version, Date, Data FROM EventStore
WHERE Version > @version
ORDER BY Version
LIMIT 0, @take
";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@version", afterVersion);

                command.Parameters.AddWithValue("@take", maxCount);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = reader["Name"].ToString();

                        var typeName = reader["TypeName"].ToString();

                        var version = (long)reader["Version"];

                        var dateString = reader["Date"].ToString();

                        var date = Convert.ToDateTime(dateString);

                        var data = (byte[])reader["Data"];

                        yield return new EventRecord(name, typeName, version, date, data);
                    }
                }
            }
        }
    }

    public void Close()
    {

    }

    public void Dispose()
    {

    }
}
