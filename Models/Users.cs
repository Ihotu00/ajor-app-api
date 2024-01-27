using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AjorApi.Models
{
    public class Users: IMustHaveTenantId
    {
        [Key]
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime DoB { get; set; }
        public string? Address { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public ICollection<Contributor>? Contributions { get; set; }
    }
}
