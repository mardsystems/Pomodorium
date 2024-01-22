namespace Pomodorium.Support;

[Binding]
public class DatabaseHooks
{
    private DatabaseContext _database;

    [BeforeScenario(Order = 100)]
    public void ResetDatabaseToBaseline(DatabaseContext database)
    {
        _database = database;

        // configure app to use in-memory database
        //DataContext.CreateDataPersist = () => _dataPersist;

        ClearDatabase();

        //DomainDefaults.AddDefaultUsers();
    }

    private void ClearDatabase()
    {
        Console.WriteLine("ClearDatabase");

        _database.EnsureDeleted();

        _database.EnsureCreated();
    }
}
