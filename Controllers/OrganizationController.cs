using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AjorApi.Data;
using AjorApi.Models;
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
    public class OrganizationController : ControllerBase
    {
        private readonly TenantDbContext _context;
        private readonly IMapper _mapper;

        public OrganizationController(IMapper mapper, TenantDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<GetOrganizationsDto>> GetOrganization()
        {
            var org = await _context.Organizations.ToListAsync();
            var orgDto = _mapper.Map<IEnumerable<GetOrganizationsDto>>(org);
            return orgDto;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrganization(CreateOrganizationDto model)
        {
            var userName = await _context.Organizations.Where(o => o.Username == model.Username).FirstOrDefaultAsync();
            if (userName != null) throw new Exception("Username is taken");
            else
            {
                var org = _mapper.Map<Organization>(model);            
                await _context.Organizations.AddAsync(org);
                await _context.SaveChangesAsync(); 
                return CreatedAtAction(nameof(GetOrganization), new {id = org.Id}, org);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login login)
        {
            var user = await _context.Organizations.Where(o => o.Username == login.Username).FirstOrDefaultAsync();
            if (user == null) {return NotFound();}
            if (user.Password == login.Password)
            {
                var claims = new List<Claim>()
                {
                    new(ClaimTypes.Role, "Manager"),
                    new(ClaimTypes.NameIdentifier, user.Id.ToString())
                };
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokenOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5282",
                    audience: "https://localhost:5282",
                    claims: claims,
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new { Token = tokenString });
            }
            return Unauthorized();
        }


        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, CreateOrganizationDto model)
        {
            var org = await _context.Organizations.FindAsync(id);
            if (org == null) return NotFound();
            _mapper.Map(model, org);            
            _context.Organizations.Update(org);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var org = await _context.Organizations.FindAsync(id);
            if (org == null) return NotFound();
            _context.Organizations.Remove(org);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}