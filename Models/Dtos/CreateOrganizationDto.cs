namespace AjorApi.Models.Dtos
{
    public class CreateOrganizationDto
    {
        public string? Name { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Type { get; set; } 
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}

