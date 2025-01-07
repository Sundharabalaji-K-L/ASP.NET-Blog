using BlogAPI.Models;
using BlogAPI.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
  options.OperationFilter<SecurityRequirementsOperationFilter>();
});



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// // CORS policy to allow all origins 
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll", policy =>
//     {
//         policy.AllowAnyOrigin()  // Allow all origins
//             .AllowAnyMethod()  // Allow any HTTP method (GET, POST, PUT, DELETE, etc.)
//             .AllowAnyHeader(); // Allow any headers
//     });
// });

builder.Services.Configure<MongoDBDatabaseSettings>(
    builder.Configuration.GetSection(nameof(MongoDBDatabaseSettings)));

builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("JWT"));

builder.Services.AddSingleton<IMongoDBDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDBDatabaseSettings>>().Value);


builder.Services.AddSingleton<IAppSettings>(sp=>
    sp.GetRequiredService<IOptions<AppSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(
    new MongoClient(builder.Configuration.GetValue<string>("MongoDBDatabaseSettings:ConnectionString")));

builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers();

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

// // Enable CORS
// app.UseCors("AllowAll");

app.MapControllers();

app.Run();