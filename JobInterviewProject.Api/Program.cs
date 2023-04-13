using JobInterviewProject.Api.Extensions;
using JobInterviewProject.Api.Services;
using JobInterviewProject.Infrastructure.Database;
using JobInterviewProject.Infrastructure.Database.Repository;
using JobInterviewProject.Infrastructure.Database.UnitOfWork;
using JobInterviewProject.Infrastructure.Services;
using JobInterviewProject.Infrastructure.Services.Service_Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
                                            options.UseSqlServer
                                            (builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthenticationSettings(builder.Configuration);

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

//Add Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmployeesService, EmployeesService>();
builder.Services.AddScoped<IAuthorizedUserService, AuthorizedUserService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IEmployeesRepository, EmployeesRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.Run();
