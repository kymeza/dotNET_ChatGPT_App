using Backend;
using Backend.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Load configuration from appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");
// Get the key from configuration
var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);
// Add JWT Bearer authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Set to true in production
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true, // Validate the secret key
            IssuerSigningKey = new SymmetricSecurityKey(key),

            ValidateIssuer = false, // Set to true if you have a valid issuer
            ValidIssuer = jwtSettings["Issuer"], // Load Issuer from configuration

            ValidateAudience = false, // Set to true if you have a valid audience
            ValidAudience = jwtSettings["Audience"], // Load Audience from configuration

            ValidateLifetime = true, // Validate token expiry
            ClockSkew = TimeSpan.FromSeconds(180) // For Clock Drifting
        };
        // Enable the token to be passed in the SignalR query string
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // If the request is for SignalR, extract the token
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/chatStreamingHub")))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireChatPermission", policy =>
        policy.RequireClaim("Permission", "Chat"));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("https://localhost:7288") // Specify the exact origin
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()); // Allow credentials is important for SignalR
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

// Add configuration or service dependencies here.
ServicesConfiguration.RegisterServices(builder.Services, builder.Configuration);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapControllers();

app.UseRouting();

app.MapHub<ChatStreamingHub>("/chatStreamingHub");

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.Run();