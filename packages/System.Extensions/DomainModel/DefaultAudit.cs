namespace System.DomainModel
{
    public class DefaultAudit : AuditInterface
    {
        public DateTime GetCreationDate()
        {
            return DateTime.Now;
        }

        public string GetUserId()
        {
            return Thread.CurrentPrincipal?.Identity?.Name ?? string.Empty;
        }
    }
}
