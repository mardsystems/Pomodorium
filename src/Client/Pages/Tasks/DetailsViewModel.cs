namespace Pomodorium.Pages.Tasks;

public class DetailsViewModel
{
    public Guid Id { get; set; }

    public DateTime? CreationDate { get; set; }

    public string? Description { get; set; }

    public long Version { get; set; }

    public DetailsViewModel(
        Guid id,
        DateTime? creationDate,
        string? description,
        long version)
    {
        Id = id;

        CreationDate = creationDate;

        Description = description;

        Version = version;
    }

    public DetailsViewModel(
        Guid id,
        string? description,
        long version)
    {
        Id = id;

        Description = description;

        Version = version;
    }

    public DetailsViewModel()
    {

    }
}
