using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BlogAPI.Services;
using BlogAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver.Linq;

namespace BlogAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController: ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IAppSettings _configuration;
    
    public AuthController(IAuthService authService, IAppSettings config)
    {
        _authService = authService;
        _configuration = config;
    }
    
    // GET : /api/Auth
    [HttpGet("{id}")]
    public ActionResult<UserResponse> GetUser(string id)
    {
        var existingUser = _authService.GetUserById(id);

        if (existingUser == null)
        {
            return NotFound();
        }

        var Response = new UserResponse
        {
            Id = existingUser.Id,
            Email = existingUser.Email,
            Username = existingUser.Username,
        };
        
        return Ok(Response);
    }
    // POST: /api/Auth
    [HttpPost]
    public ActionResult<UserResponse> Register([FromBody] UserResgisterRequest request)
    {
        var userNameCheck = _authService.GetUserByUserName(request.Username);
        var userEmailCheck = _authService.GetUserByEmail(request.Email);
        
        if (userNameCheck!= null)
        {
            return BadRequest(new {message = "Username already exists"});
        }

        if (userEmailCheck != null)
        {
            return BadRequest(new {message = "Email already exists"});
        }
        
        User newuser = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = PasswordHelper.HashPassword(request.Password)
        };

        _authService.UserRegister(newuser);

        UserResponse newResponse = new UserResponse
        {
            Id = newuser.Id,
            Username = newuser.Username,
            Email = newuser.Email
        };
        
        return CreatedAtAction(nameof(GetUser), new { id = newResponse.Id},newResponse);
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim("Id", user.Id),
            //new Claim(ClaimTypes.Role, "Admin"),
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
        _configuration.Token));
        
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: cred);
        
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
    // POST: /api/Auth/login
    [HttpPost("login")]
    public ActionResult<string> Login(UserLoginRequest request)
    {
        var user = _authService.GetUserByUserName(request.Username);
        if (user == null)
        {
            return BadRequest(new {message = "User not Found"});
        }

        if (!PasswordHelper.verifyPassword(request.Password, user.PasswordHash))
        {
            return BadRequest(new {message = "Incorrect Password"});
        }

        string token = CreateToken(user);
        return Ok(token);
    }
    
    
}