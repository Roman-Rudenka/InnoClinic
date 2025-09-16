using Application;
using Application.Interfaces;
using Infrastructure;
using Presentation.Extensions;
using Presentation.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

Log.Logger = SerilogConfigurator.Configure().CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IRoleSeeder>();
    await seeder.SeedAsync();
}

using (var scope = app.Services.CreateScope())
{
    var roleSeeder = scope.ServiceProvider.GetRequiredService<IRoleSeeder>();
    await roleSeeder.SeedAsync();

    var userSeeder = scope.ServiceProvider.GetRequiredService<ICreatingReceptionService>();
    await userSeeder.CreateReceptionAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ApiExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();