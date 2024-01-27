namespace AjorApi.Models.Dtos
{
    public class GetContributionDto
    {
        public int Id { get; set; }
        public int MaxAmount { get; set; }
        public int MaxContributors { get; set; }
        public string? Name { get; set; }
        public int AmountPerContributor { get; set; }

    }
}