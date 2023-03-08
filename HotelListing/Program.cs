using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Profile;
using HotelListing.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
        path: "C:\\Users\\SamRayane\\Documents\\Visual Studio 2022\\visual stadio\\Logs\\asp.net core\\hotelListing\\log-.txt",
        outputTemplate: "{TimeStamp: yyyy-MM-dd HH:mm:ss.fff zzz}[{Level:u3}] {Message:lg}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Information
    ).CreateLogger();
try
{
    Log.Information("Application Is Starting");



    var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

    builder.Host.UseSerilog();
    builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddAutoMapper(typeof(MappingProfile));
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnectionString"));
    });
    builder.Services.AddCors(o =>
    {
        o.AddPolicy("AllowAll",builder=>
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
    });

    builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

    var app = builder.Build();

// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors("AllowAll");

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}


catch (Exception e)
{
    Log.Fatal(e, "Application Failed to start");
}
finally
{
    Log.CloseAndFlush();
}
