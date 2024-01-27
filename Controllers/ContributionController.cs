using AjorApi.Data;
using AjorApi.Models;
using AjorApi.Models.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AjorApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager")]
    public class ContributionController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public ContributionController(IMapper mapper, DBContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<GetContributionDto>> GetContribution()
        {
            var contributions = await _context.Contributions.ToListAsync();
            var contributionsDto = _mapper.Map<IEnumerable<GetContributionDto>>(contributions);
            return contributionsDto;
        }

        [HttpGet("Contributors")]
        public async Task<IEnumerable<GetContributorsOfAContributionDto>> GetContributor([FromQuery] int id)
        {
            var users = await _context.Contributors.Include(c => c.Users).Where(c => c.ContributionId == id)
                            .ToListAsync();
            var usersDto = _mapper.Map<IEnumerable<GetContributorsOfAContributionDto>>(users);
            return usersDto;
        }

        [HttpPost]
        public async Task<IActionResult> CreateContribution(CreateContributionDto model)
        {
            var contribution = _mapper.Map<Contribution>(model);
            contribution.MaxAmount = contribution.MaxContributors * contribution.AmountPerContributor;
            await _context.Contributions.AddAsync(contribution);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetContribution), new { id = contribution.Id }, contribution);
        }

        [HttpPost("Contributor")]
        public async Task<IActionResult> CreateContributor(CreateContributorDto model)
        {
            Contributor contributor = _mapper.Map<Contributor>(model);
            var user = await _context.Users.FindAsync(contributor.UsersId);
            var contribution = await _context.Contributions.FindAsync(contributor.ContributionId);

            if (user != null && contribution != null)
            {
                var existingContributors = await _context.Contributors.Where(c => 
                                                c.ContributionId == contribution.Id)
                                                .Select(c => c.UsersId).ToListAsync();
                if (existingContributors.Contains(user.Id)) { return StatusCode(404, "Contributor already exists"); }
                var numberOfContributors = await _context.Contributors
                                    .Where(c => c.ContributionId == contribution.Id)
                                    .ToListAsync();
                if (numberOfContributors.Count >= contribution.MaxContributors)
                { return StatusCode(404, "Max Contributors Reached"); }
                var maxContributors = contribution.MaxContributors;
                var allPositions = Enumerable.Range(1, maxContributors).ToList();
                var takenPositions = _context.Contributors.Where(c => c.ContributionId == contribution.Id)
                         .Select(p => p.Position).ToList();
                foreach (int i in takenPositions)
                { allPositions.Remove(i); }
                var rand = new Random();
                var index = rand.Next(0, allPositions.Count - 1);
                contributor.Position = allPositions[index];
                await _context.Contributors.AddAsync(contributor);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetContributor), new { id = contributor.Id }, contributor);
            }
            return StatusCode(404, "User or Contribution Does Not Exist");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, CreateContributionDto model)
        {
            var contribution = await _context.Contributions.FindAsync(id);
            if (contribution == null) return NotFound();
            _mapper.Map(model, contribution);
            _context.Contributions.Update(contribution);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var contribution = await _context.Contributions.FindAsync(id);
            if (contribution == null) return NotFound();
            _context.Contributions.Remove(contribution);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}