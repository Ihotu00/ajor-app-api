namespace AjorApi.Models.Dtos
{
    public class EditUsersDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime DoB { get; set; }
        public string? Address { get; set; }
    }
}