using Common.Abstract.Configurations;
using Common.Abstract.Middleware;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Model;
using Serilog;
using Serilog.Events;


//Directory.SetCurrentDirectory(AppContext.BaseDirectory);
//SET OUTPUT TEMPLATE
Serilog.Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
.WriteTo.Console()
.WriteTo.File($"{AppContext.BaseDirectory}" + @"Logs\\log.txt", rollingInterval: RollingInterval.Day)
.CreateLogger();

var builder = WebApplication.CreateBuilder(args);


IConfiguration _configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                             .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                             .Build();
builder.Services.AddLogging(configuration => configuration.ClearProviders());

// Add services to the container.
Settings _settings = new();
builder.Configuration.GetSection("Settings").Bind(_settings, c => c.BindNonPublicProperties = true);

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(
//      "CorsPolicy",
//      builder => builder.WithOrigins(_settings.CorsUrl)
//      .AllowAnyMethod()
//      .AllowAnyHeader()
//      .AllowCredentials());
//});

builder.Services.AddScoped();
builder.Services.AddLogging(configuration => configuration.ClearProviders());// Removes default console logger
builder.Services.Configure<AuthCredential>(_configuration.GetSection("AuthCredential"));
builder.Services.Configure<Jwt>(_configuration.GetSection("Jwt"));
builder.Services.AddDbContext<Context>(item => item.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"), x => x.UseNodaTime()));
builder.Services.AddIdentity<AppUser, AppRole>()
      .AddEntityFrameworkStores<Context>()
      .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = false;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 3;
});
builder.Services.AddMvc();
builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(IISDefaults.AuthenticationScheme);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddApiVersioning(x =>
{
    //x.DefaultApiVersion = new ApiVersion(1, 0);
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.ReportApiVersions = true;
});
//LgAAAB_@_LCAAAAAAAAApzqDQyKfHRNLY1NbENyM80VFVRtrQ0NzYztLKwsDQ2MHIwNo6Njo830DY00tXVBgAGvJ4yLgAAAA_!__!_
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(_settings.ApiVersion.Version, new OpenApiInfo { Title = _settings.ApiVersion.Title, Version = _settings.ApiVersion.Num });
    c.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
    {
        Description = "OAuth2",
        Name = "auth",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
        {
            new OpenApiSecurityScheme
            {
            Reference = new OpenApiReference
                {
                Type = ReferenceType.SecurityScheme,
                Id = "OAuth2"
                },
                Scheme = "oauth2",
                Name = "auth",
                In = ParameterLocation.Header,

            },
            new List<string>()
            }
        });
});
builder.Host.UseSerilog();
var app = builder.Build();
app.UseCors("CorsPolicy");
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseMiddleware<OAuth>();
app.UseAuthorization();
app.UseAuthentication();
app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    //app.UseCors(builder =>
    //{
    //    builder.AllowAnyHeader();
    //    builder.AllowAnyMethod();
    //    builder.WithOrigins(_settings.CorsUrl);
    //    builder.AllowCredentials();
    //});
}
app.UseSwagger().UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", _settings.ApiVersion.Title); });

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseDefaultFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.MapRazorPages();
app.Run();