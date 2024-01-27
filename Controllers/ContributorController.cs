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
    public class ContributorController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public ContributorController(IMapper mapper, DBContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        // [HttpGet]
        // public async Task<IEnumerable<GetContributorDto>> GetContributor()
        // {
        //     var contributors = await _context.Contributors.Include(c => c.Contributions)
        //                             .Include(c => c.Users).ToListAsync();
        //     var contributorsDto = _mapper.Map<IEnumerable<GetContributorDto>>(contributors);
        //     return contributorsDto;
        // }

        // [HttpGet("test0")]
        // public async Task<int> Get()
        // {
        // var numberOfContributors = await _context.Contributors
        //                             .Where(c => c.ContributionId == 1)
        //                             .ToListAsync();
        //     return numberOfContributors.Count;
        // }

        // [HttpPost]
        // public async Task<IActionResult> CreateContributor(CreateContributorDto model)
        // {
        //     Contributor contributor = _mapper.Map<Contributor>(model);
        //     var user = await _context.Users.FindAsync(contributor.UsersId);
        //     var contribution = await _context.Contributions.FindAsync(contributor.ContributionId);
        //     if (user != null && contribution != null)
        //     {
        //         var numberOfContributors = await _context.Contributors
        //                             .Where(c => c.ContributionId == contribution.Id)
        //                             .ToListAsync();
        //         if (numberOfContributors.Count >= contribution.MaxContributors)
        //         { return StatusCode(404, "Max Contributors Reached"); }
                // var maxContributors = contribution.MaxContributors;
                // var allPositions = Enumerable.Range(1, maxContributors).ToList();
                // var takenPositions = _context.Contributors.Where(c => c.ContributionId == contribution.Id)
                //          .Select(p => p.Position).ToList();
                // foreach (int i in takenPositions)
                // { allPositions.Remove(i); }
                // var rand = new Random();
                // var index = rand.Next(0, allPositions.Count - 1);
                // contributor.Position = allPositions[index];
                // await _context.Contributors.AddAsync(contributor);
        //         await _context.SaveChangesAsync();
        //         return CreatedAtAction(nameof(GetContributor), new { id = contributor.Id }, contributor);
        //     }
        //     return StatusCode(404, "User or Contribution Does Not Exist");
        // }


        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, CreateContributorDto model)
        {
            var contributor = await _context.Contributors.FindAsync(id);
            if (contributor == null) return NotFound();
            _mapper.Map(model, contributor);
            _context.Contributors.Update(contributor);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var contributor = await _context.Contributors.FindAsync(id);
            if (contributor == null) return NotFound();
            _context.Contributors.Remove(contributor);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}