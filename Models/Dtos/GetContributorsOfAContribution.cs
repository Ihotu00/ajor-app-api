namespace AjorApi.Models.Dtos
{
    public class GetContributorsOfAContributionDto
    {
        public int UsersId { get; set; }
        public string? UsersFirstName { get; set; }
        public string? UsersLastName { get; set; }
        public int Position { get; set; }
    }
}