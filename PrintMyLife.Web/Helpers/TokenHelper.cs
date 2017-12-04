using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using PrintMyLife.Core.Authentication.Entities;
using PrintMyLife.Core.Configuration;

namespace PrintMyLife.Web.Helpers
{
  public class Token{
    public string TokenId { get; set; }
    public DateTime ExpirationDate { get; set; }
  }

  public static class TokenHelper
  {
    public static Token GenerateToken(AuthenticationSettings authSettings, User user)
    {
      var token = new Token() { ExpirationDate = DateTime.UtcNow.AddDays(authSettings.ExpirationDurationInDays) };

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Issuer = authSettings.Issuer,
        Audience = authSettings.Audience,
        NotBefore = DateTime.UtcNow,
        IssuedAt = DateTime.UtcNow,
        Subject = new ClaimsIdentity(new Claim[]
          {
            new Claim(ClaimTypes.Sid, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
          }),
        Expires = token.ExpirationDate,
        SigningCredentials = GenerateSigningCredentials(authSettings.SecretKey)
      };
      var tokenHandler = new JwtSecurityTokenHandler();
      var tokenId = tokenHandler.CreateToken(tokenDescriptor);
      token.TokenId = tokenHandler.WriteToken(tokenId);
      return token;

      SigningCredentials GenerateSigningCredentials(string secretKey)
      {
        var encodedKey = Encoding.ASCII.GetBytes(secretKey);
        var symecticSecurityKey = new SymmetricSecurityKey(encodedKey);
        return new SigningCredentials(symecticSecurityKey, SecurityAlgorithms.HmacSha256Signature);
      }
    }
  }
}
