using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LoginRegistration.Processor
{
    public class TokenProcessing
    {

        public readonly IConfiguration configuration;
        public TokenProcessing(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public static string  CreateToken(string Role,string Email,string key, string Issuer)
        {
            try
            {
                var symmetricSecuritykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var signingCreds = new SigningCredentials(symmetricSecuritykey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Role,Role ));
                claims.Add(new Claim("Email", Email));
                //claims.Add(new Claim("", ));
               // claims.Add(new Claim("TokenType", type));

                var token = new JwtSecurityToken (Issuer,
                   Issuer,
                    claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: signingCreds);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
