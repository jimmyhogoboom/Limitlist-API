/*
 * Copyright 2024 Jimmy Hogoboom
 *
 * This file is part of the LimitList API.
 *
 * LimitList API is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
 *
 * LimitList API is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with LimitList API. If not, see <https://www.gnu.org/licenses/>.
 */

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using limitlist_api.Models;
using limitlist_api.Utils;

var builder = WebApplication.CreateBuilder(args);

var jwtConfig = new JWTConfig
{
  key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"] ?? "")),
  issuer = builder.Configuration["JWT:ValidIssuer"] ?? "",
  audience = builder.Configuration["JWT:ValidIssuer"] ?? "",
  // Default to 5 minute token
  validUntil = DateTime.Now.AddMinutes(Convert.ToDouble(builder.Configuration["JWT:TokenExpiry"] ?? "5"))
};

// Add services to the container.
builder.Services.AddControllers();
// TODO: use a more permanent DB solution
builder.Services.AddDbContext<LimitListContext>(opt => opt.UseInMemoryDatabase("LimitList"));

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
  .AddEntityFrameworkStores<LimitListContext>()
  .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
  {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
  })
.AddJwtBearer(options =>
  {
    options.SaveToken = true;
    options.TokenValidationParameters.ValidIssuer = jwtConfig.issuer;
    options.TokenValidationParameters.ValidAudience = jwtConfig.audience;
    options.TokenValidationParameters.IssuerSigningKey = jwtConfig.key;
    //   ValidateIssuer = true,
    //   ValidateAudience = true,
    //   ValidateLifetime = false,
    //   ValidateIssuerSigningKey = true
  });

builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.AddSecurityDefinition("jwt_auth", new OpenApiSecurityScheme()
  {
    Name = "Bearer",
    BearerFormat = "JWT",
    Scheme = "Bearer",
    Description = "Authorization token from user-jwts.",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.Http,
  });

  c.AddSecurityRequirement(new OpenApiSecurityRequirement()
  {
    {
      new OpenApiSecurityScheme()
      {
        Reference = new OpenApiReference()
        {
          Id = "jwt_auth",
          Type = ReferenceType.SecurityScheme
        }
      },
      new string[] { }
    },
  });
});

// builder.Services.AddAuthorizationBuilder()
// .AddPolicy("admin_limitlist", policy =>
//     policy
//       .RequireRole("admin")
//       .RequireClaim("scope", "limitlist_api"))
// .AddPolicy("defaultpolicy", policy =>
// {
//   policy.RequireAuthenticatedUser();
//   policy.AuthenticationSchemes = new List<string>{ JwtBearerDefaults.AuthenticationScheme };
// });

var app = builder.Build();

app.MapIdentityApi<IdentityUser>();

// app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
  .RequireAuthorization();
// .RequireAuthorization("admin_limitlist");

app.MapPost(
  "/security/createToken",
  [AllowAnonymous] (IdentityUser user) =>
  {
    // TODO: Verfiy user password
    // TODO: Include user authorizations?

    return Results.Ok(JWT.BuildJWTToken(jwtConfig));

    //         var issuer = builder.Configuration["Jwt:Issuer"];
    //         var audience = builder.Configuration["Jwt:Audience"];
    //         var key = Encoding.ASCII.GetBytes
    //         (builder.Configuration["Jwt:Key"]);
    //         var tokenDescriptor = new SecurityTokenDescriptor
    //         {
    //             Subject = new ClaimsIdentity(new[]
    //             {
    //                 new Claim("Id", Guid.NewGuid().ToString()),
    //                 new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
    //                 new Claim(JwtRegisteredClaimNames.Email, user.UserName),
    //                 new Claim(JwtRegisteredClaimNames.Jti,
    //                 Guid.NewGuid().ToString())
    //             }),
    //             Expires = DateTime.UtcNow.AddMinutes(5),
    //             Issuer = issuer,
    //             Audience = audience,
    //             SigningCredentials = new SigningCredentials
    //             (new SymmetricSecurityKey(key),
    //             SecurityAlgorithms.HmacSha512Signature)
    //         };
    //         var tokenHandler = new JwtSecurityTokenHandler();
    //         var token = tokenHandler.CreateToken(tokenDescriptor);
    //         var jwtToken = tokenHandler.WriteToken(token);
    //         var stringToken = tokenHandler.WriteToken(token);
    //         return Results.Ok(stringToken);

    //     return Results.Unauthorized();
  }
);

app.Run();
