﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RecipeManagement.Application.Abstractions.IServices;
using RecipeManagement.Domain.Entities.DTOs;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecipeManagement.Application.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;

        public AuthService(IConfiguration config, IUserService userService)
        {
            _config = config;
            _userService = userService;
        }

        public async Task<ResponseLogin> GenerateToken(RequestLogin user)
        {
            if (await UserExist(user))
            {
                var result = await _userService.GetByLogin(user.Login);

                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Role, result.Role),
                    new Claim("Login", user.Login),
                    new Claim("UserID", result.Id.ToString()),
                    new Claim("CreatedDate", DateTime.UtcNow.ToString()),
                };

                return await GenerateToken(claims);
            }

            return new ResponseLogin()
            {
                Token = "Unauthorized"
            };
        }

        public async Task<ResponseLogin> GenerateToken(IEnumerable<Claim> additionalClaims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var exDate = Convert.ToInt32(_config["JWT:ExpireDate"] ?? "1");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow).ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64)
            };

            if (additionalClaims?.Any() == true)
                claims.AddRange(additionalClaims);


            var token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(exDate),
                signingCredentials: credentials);

            return new ResponseLogin()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        public async Task<bool> UserExist(RequestLogin user)
        {
            var result = await _userService.GetByLogin(user.Login);

            if (user.Login == result.Login && user.Password == result.Password)
            {
                return true;
            }

            return false;
        }
    }
}