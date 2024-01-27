namespace AjorApi.Models.Dtos
{
    public class CreateContributionDto
    {
        public string? Name { get; set; }
        public int MaxContributors { get; set; }
        public int AmountPerContributor { get; set; }
        public int OrganizationId { get; set; }
    }
}