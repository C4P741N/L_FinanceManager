using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Web;
using MSota.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using MSota.BaseFormaters;
using MSota.ExtensibleMarkupAtLarge;
using MSota.DataServer;
using Microsoft.AspNetCore.Builder;
using MSota.Accounts;
using MSota.JavaScriptObjectNotation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

//Dependency Injection
builder.Services.AddScoped<MSota.Accounts.ITransactions, MSota.Accounts.Transactions>();
builder.Services.AddScoped<MSota.BaseFormaters.IFortmaterAtLarge, MSota.BaseFormaters.FortmaterAtLarge>();
builder.Services.AddScoped<IXmlProps, XmlProps>();
builder.Services.AddScoped<IXmlDataFotmater, XmlDataFotmater>();
builder.Services.AddScoped<IXmlExtractor, XmlExtractor>();
builder.Services.AddScoped<ISqlDataServer, SqlDataServer>();
builder.Services.AddScoped<ISQLDataAccess, SQLDataAccess>();
builder.Services.AddScoped<IFactions, Factions>();
builder.Services.AddScoped<IJsonExtractor, JsonExtractor>();
//builder.Services.AddScoped<IJsonProps, JsonProps>();

//SQL connection string
builder.Services.AddDbContext<DbContext>(x => 
        x.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnectionString")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.WebHost.UseUrls(new[] { "https://0.0.0.0:7176/" });

builder.WebHost.UseUrls("http://*:80", "https://*.443"); //you will be able now access to your application by http://your_local_machine_ip:80 for example http://192.168.1.4:80

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 2000 * 1024 * 1024; // 20 MB in bytes;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(webApi =>
{
    webApi.AllowAnyOrigin();
    //webApi.WithOrigins("http://localhost:3000/");
    webApi.AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

//app.Run("https://*:" + Environment.GetEnvironmentVariable("PORT"));

//https://www.restapitutorial.com/httpstatuscodes.html --> List of Error Codes
