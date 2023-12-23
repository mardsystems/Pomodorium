namespace Pomodorium.Features.Maintenance;

public interface IReadOnlyDatabase
{
    Task EnsureCreated();

    Task EnsureDeleted();
}
