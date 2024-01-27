using System.ComponentModel.DataAnnotations;

namespace AjorApi.Models.Dtos
{
    public class GetOrganizationsDto
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Type { get; set; } 
    }
}