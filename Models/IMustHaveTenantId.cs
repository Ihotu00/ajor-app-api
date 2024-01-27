namespace AjorApi.Models
{
    public interface IMustHaveTenantId
    {
        public int OrganizationId { get; set; }
    }
}