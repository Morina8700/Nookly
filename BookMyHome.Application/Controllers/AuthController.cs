using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookMyHome.Application.DTO.Auth;
using BookMyHome.Application.Services;
using BookMyHome.Domain.Models;
using Host = BookMyHome.Domain.Models.Host;
using BookMyHome.Persistence.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookMyHome.Application.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserRepository _userRepository;
    private readonly PasswordService _passwordService;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserRepository userRepository,
        PasswordService passwordService,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register(
        RegisterRequest request)
    {
        var name = request.Name.Trim();
        var email = request.Email.Trim().ToLowerInvariant();
        var role = request.Role.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Name is required.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest("Email is required.");
        }

        if (request.Password.Length < 8)
        {
            return BadRequest(
                "Password must be at least 8 characters long.");
        }

        if (!role.Equals("Host", StringComparison.OrdinalIgnoreCase)
            && !role.Equals("Guest", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Role must be Host or Guest.");
        }

        if (await _userRepository.EmailExistsAsync(email))
        {
            return Conflict("An account with this email already exists.");
        }

        User user = role.Equals(
            "Host",
            StringComparison.OrdinalIgnoreCase)
            ? new Host(name, email)
            : new Guest(name, email);

        user.SetPasswordHash(
            _passwordService.Hash(request.Password));

        await _userRepository.AddAsync(user);

        var normalizedRole = user is Host
            ? "Host"
            : "Guest";

        return Ok(CreateLoginResponse(user, normalizedRole));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(
        LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user =
            await _userRepository.GetByEmailAsync(email);

        if (user == null
            || string.IsNullOrWhiteSpace(user.PasswordHash)
            || !_passwordService.Verify(
                request.Password,
                user.PasswordHash))
        {
            return Unauthorized("Invalid email or password.");
        }

        var role = user switch
        {
            Host => "Host",
            Guest => "Guest",
            _ => null
        };

        if (role == null)
        {
            return Unauthorized(
                "The user has an unsupported account type.");
        }

        return Ok(CreateLoginResponse(user, role));
    }

    private LoginResponse CreateLoginResponse(
        User user,
        string role)
    {
        return new LoginResponse
        {
            Token = CreateToken(user, role),
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Role = role
        };
    }

    private string CreateToken(User user, string role)
    {
        var keyValue = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "JWT key is missing from configuration.");

        var issuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException(
                "JWT issuer is missing from configuration.");

        var audience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException(
                "JWT audience is missing from configuration.");

        var expirationMinutes =
            _configuration.GetValue<int>(
                "Jwt:ExpirationMinutes");

        if (expirationMinutes <= 0)
        {
            throw new InvalidOperationException(
                "JWT expiration must be greater than zero.");
        }

        var claims = new[]
        {
            new Claim(
                JwtRegisteredClaimNames.Sub,
                user.UserId.ToString()),

            new Claim(
                ClaimTypes.NameIdentifier,
                user.UserId.ToString()),

            new Claim(
                ClaimTypes.Name,
                user.Name),

            new Claim(
                ClaimTypes.Email,
                user.Email),

            new Claim(
                ClaimTypes.Role,
                role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(keyValue));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                expirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}