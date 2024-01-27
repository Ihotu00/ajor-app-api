namespace AjorApi.Data
{
    public interface ITenantService
    {
        public int GetOrganization();
    }

    public class TenantService : ITenantService
    {
        public int OrganizationId;
        public TenantService(IHttpContextAccessor context)
        {
            if (context.HttpContext != null)
            {
                if (context.HttpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                {
                    if (int.TryParse(tenantId, out var id)) OrganizationId = id;
                    else throw new Exception("Invalid Tenant!");
                }
                // else throw new Exception("Invalid Tenant!");
            }
        }
        public int GetOrganization()
        {
            return OrganizationId;
        }
    }
}