using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Web;
using MSota.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

//builder.Services.AddTransient<ITransactionsResponse, TransactionsResponse>();

//builder.Services.AddDbContext<DbContext>(options =>
//   options.UseSqlServer(builder.Configuration.GetConnectionString(/* ConectionString key from appsettings.json */)));

//var connectionString = builder.Configuration.GetConnectionString("SQLConnectionString");
//builder.Services.AddDbContext<DbContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddDbContext<DbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnectionString")));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
