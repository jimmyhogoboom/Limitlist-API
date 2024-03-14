namespace limitlist_api.Utils;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

public record struct JWTConfig(SymmetricSecurityKey key, string issuer, string audience, DateTime validUntil);

public static class JWT
{
  public static string BuildJWTToken(JWTConfig jwtConfig, DateTime? validUntil = null)
  {
    if (validUntil is null)
    {
      validUntil = DateTime.Now.AddMinutes(5);
    }

    var creds = new SigningCredentials(jwtConfig.key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
      issuer: jwtConfig.issuer,
      audience: jwtConfig.audience,
      expires: validUntil,
      signingCredentials: creds);
    return new JwtSecurityTokenHandler().WriteToken(token);
  }

}
