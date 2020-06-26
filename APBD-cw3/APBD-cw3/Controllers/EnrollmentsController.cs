using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using APBD_cw3.DAL_Data_Access_layer_;
using APBD_cw3.DTOs.Requests;
using APBD_cw3.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Wyklad5.Services;

namespace APBD_cw3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]

    public class EnrollmentsController : ControllerBase
    {

        private readonly IStudentDbService _dbService;

        public IConfiguration Configuration { get; set; }

        public EnrollmentsController(IStudentDbService service, IConfiguration configuration)
        {
            _dbService = service;
            Configuration = configuration;
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult EnrollStudent(EnrollStudentRequest request) 
        {

            return _dbService.EnrollStudent(request);
        }

        [HttpPost("promotions")]
        [Authorize(Roles = "employee")]
        public IActionResult promoteStudent(string studiesName, int semester) 
        {
            return _dbService.PromoteStudents(semester, studiesName);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Login(LoginRequest request)
        {

            AuthHandler auth = new AuthHandler(_dbService);
            if (auth.HandleAuthenticateAsync(request) != Accepted())
                return BadRequest("wrong credentials");

            var claims = new[]
{
                new Claim(ClaimTypes.NameIdentifier, request.login),
                new Claim(ClaimTypes.Name, "PJATK")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "Me",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = Guid.NewGuid()
            });
        }

    }
}