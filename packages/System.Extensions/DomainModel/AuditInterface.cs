namespace System.DomainModel;

[Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public interface AuditInterface
{
    string GetUserId();

    DateTime GetCreationDate();
}
