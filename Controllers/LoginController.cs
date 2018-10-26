using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public ActionResult<string> Valid([FromBody] User user)
        {  
            if(user.Username.Equals("treinaweb") && user.Password.Equals("treinaweb"))
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var securityToken = new JwtSecurityToken(
                                issuer: "TodoAPI",
                                audience: "TodoAPI",
                                claims: claims,
                                expires: DateTime.UtcNow.AddMinutes(30),
                                signingCredentials: new SigningCredentials(
                                                            new SymmetricSecurityKey(Encoding.ASCII.GetBytes("todo-security-key")),
                                                            SecurityAlgorithms.HmacSha256Signature)
                                );

                return new JwtSecurityTokenHandler().WriteToken(securityToken);
            }
            else
                return Unauthorized();
        }  
    }
}