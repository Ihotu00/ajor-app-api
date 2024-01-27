using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AjorApi.Models
{
    public class Contribution: IMustHaveTenantId
    {
        [Key]
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public int MaxAmount { get; set; }
        public int MaxContributors { get; set; }    
        public int AmountPerContributor { get; set; }   
        public string? Name { get; set; }
        public ICollection<Contributor>? Contributors { get; set; }
    }
}