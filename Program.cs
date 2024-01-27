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

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using limitlist_api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// TODO: use a more permanent DB solution
builder.Services.AddDbContext<LimitListContext>(opt => opt.UseInMemoryDatabase("LimitList"));
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

builder.Services.AddAuthentication("Bearer")
  .AddJwtBearer();

builder.Services.AddAuthorization();

builder.Services.AddAuthorizationBuilder()
  .AddPolicy("admin_limitlist", policy =>
      policy
        .RequireRole("admin")
        .RequireClaim("scope", "limitlist_api"));

var app = builder.Build();

// app.UseCors();
// app.UseAuthentication();
// app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers()
  .RequireAuthorization("admin_limitlist");

app.Run();
