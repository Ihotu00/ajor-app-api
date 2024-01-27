using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AjorApi.Data;
using AjorApi.Models;
using AjorApi.Models.Configurations;
using AjorApi.Models.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AjorApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public UsersController(IMapper mapper, DBContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet, Authorize(Roles = "Manager")]
        public async Task<IEnumerable<GetUsersDto>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            var usersDto = _mapper.Map<IEnumerable<GetUsersDto>>(users);
            return usersDto;
        }

        [HttpGet("{id}"), Authorize]
        public async Task<GetUsersDto> GetUsersById(int id)
        {
            var users = await _context.Users.FindAsync(id);
            var usersDto = _mapper.Map<GetUsersDto>(users);
            return usersDto;
        }

        [HttpGet("Contributions"), Authorize(Roles = "Operator")]
        public async Task<IEnumerable<GetContributorDto>> GetContributor([FromQuery] int id)
        {
            var contributions = await _context.Contributors.Include(c => c.Contributions).Where(c => c.UsersId == id)
                                    .ToListAsync();
            var contributionDto = _mapper.Map<IEnumerable<GetContributorDto>>(contributions);
            return contributionDto;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrganization(CreateUsersDto model)
        {
            var userName = await _context.Users.Where(o => o.Username == model.Username).FirstOrDefaultAsync();
            if (userName != null) throw new Exception("Username is taken");
            else
            {
                var user = _mapper.Map<Users>(model);
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login login)
        {
            var user = await _context.Users.IgnoreQueryFilters()
                    .Where(o => o.Username == login.Username)
                    .FirstOrDefaultAsync();
            if (user == null) {return NotFound();}
            if (user.Password == login.Password)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Role, "Operator"),
                    new(ClaimTypes.Sid, user.OrganizationId.ToString())
                };
                var tokenOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5282",
                    audience: "https://localhost:5282",
                    claims: claims,
                    expires: DateTime.Now.AddDays(5),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new { Token = tokenString });
            }
            return Unauthorized();
        }

        [HttpPut("{id}"), Authorize(Roles = "Operator")]
        public async Task<IActionResult> Edit(int id, EditUsersDto model)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            _mapper.Map(model, user);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}"), Authorize(Roles = "Operator")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}