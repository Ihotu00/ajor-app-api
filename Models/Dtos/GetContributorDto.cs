namespace AjorApi.Models.Dtos
{
    public class GetContributorDto
    {
        // public int UsersId { get; set; }
        public string? ContributionsAmountPerContributor { get; set; }
        public string? ContributionsName { get; set; }
        public int Position { get; set; }
    }
}