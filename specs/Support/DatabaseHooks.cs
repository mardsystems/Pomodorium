//using Microsoft.Data.Sqlite;

//namespace Pomodorium.Support;

//[Binding]
//public class DatabaseHooks
//{
//    private PomodoriumContext _db;

//    [BeforeScenario(Order = 100)]
//    public void ResetDatabaseToBaseline(PomodoriumContext db)
//    {
//        _db = db;

//        // configure app to use in-memory database
//        //DataContext.CreateDataPersist = () => _dataPersist;

//        ClearDatabase();

//        //DomainDefaults.AddDefaultUsers();
//    }

//    private void ClearDatabase()
//    {
//        //_db.Database.CloseConnection();

//        _db.Database.EnsureDeleted();

//        _db.ChangeTracker.Clear();

//        SqliteConnection connection = (SqliteConnection)_db.Database.GetDbConnection();
//        connection.Open();
//        int rc;
//        rc = SQLitePCL.raw.sqlite3_db_config(connection.Handle, SQLitePCL.raw.SQLITE_DBCONFIG_RESET_DATABASE, 1, out _);
//        SqliteException.ThrowExceptionForRC(rc, connection.Handle);
//        rc = SQLitePCL.raw.sqlite3_exec(connection.Handle, "VACUUM");
//        SqliteException.ThrowExceptionForRC(rc, connection.Handle);
//        rc = SQLitePCL.raw.sqlite3_db_config(connection.Handle, SQLitePCL.raw.SQLITE_DBCONFIG_RESET_DATABASE, 0, out _);
//        SqliteException.ThrowExceptionForRC(rc, connection.Handle);

//        _db.Database.EnsureCreated();
//    }
//}
