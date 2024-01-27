using System.ComponentModel.DataAnnotations;
using AjorApi.Models.Dtos;

namespace AjorApi.Models
{
    public class Organization
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Type { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        public static implicit operator Organization(CreateOrganizationDto v)
        {
            throw new NotImplementedException();
        }
    }
}