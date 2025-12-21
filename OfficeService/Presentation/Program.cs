using Application.Interfaces;
using Application.Services;
using Infrastructure;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DbOptions>(builder.Configuration.GetSection("DbOptions"));
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var dbOptions = serviceProvider.GetRequiredService<IOptions<DbOptions>>().Value;
    options.UseNpgsql(dbOptions.DefaultConnection);
});

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IOfficeService, OfficeService>();
builder.Services.AddScoped<IOfficeRepository, OfficeRepository>();

builder.Services.AddControllers();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();