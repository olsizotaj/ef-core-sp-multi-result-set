using EFCore.ReadMultiResultSetFromSP.DBLayer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextFactory<EfAppDbContext>((p, o) =>
{
    o.UseSqlServer("Server=.;Database=DemoDb;integrated security=sspi;TrustServerCertificate=True", x =>
    {
        x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        x.MigrationsAssembly(typeof(EfAppDbContext).Assembly.FullName);
    });
}, ServiceLifetime.Scoped);
builder.Services.AddScoped<IRepository, BaseRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
