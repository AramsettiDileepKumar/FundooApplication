using BusinessLogicLayer.InterfaceBL;
using BusinessLogicLayer.InterfaceBL.Labels;
using BusinessLogicLayer.InterfaceBL.NotesInterface;
using BusinessLogicLayer.ServiceBL.CollaborationServiceBL;
using BusinessLogicLayer.ServiceBL.LabelService;
using BusinessLogicLayer.ServiceBL.NotesService;
using BusinessLogicLayer.ServiceBL.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using RepositoryLayer.Context;
using RepositoryLayer.Interface.CollaborationRL;
using RepositoryLayer.Interface.LabelRL;
using RepositoryLayer.Interface.NoteService;
using RepositoryLayer.Interface.UserService;
using RepositoryLayer.Service;
using StackExchange.Redis;
using System.Text;
using IRegistrationBL = BusinessLogicLayer.InterfaceBL.IRegistrationBL;
using RegistrationServiceBL = BusinessLogicLayer.ServiceBL.UserService.RegistrationServiceBL;




var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IRegistrationBL, RegistrationServiceBL>();
builder.Services.AddScoped<RepositoryLayer.Interface.UserService.IRegistration, RepositoryLayer.Service.RegistrationService>();
builder.Services.AddScoped<IAuthonticationService, AuthonticationService>();
builder.Services.AddScoped<INotesBL, NotesServiceBL>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<ICollaborationBL, CollaborationBL>();
builder.Services.AddScoped<ICollaborationRL, CollaborationRL>();
builder.Services.AddScoped<INotesLabelBL, NotesLabelBL>();
builder.Services.AddScoped<INotesLabelRL, NotesLabelRL>();

//Logger------------------------------------------------------------------------------------------
var logpath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
NLog.GlobalDiagnosticsContext.Set("LogDirectory", logpath);
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();


//Redis-----------------------------------------------------------------------
builder.Services.AddSingleton<ConnectionMultiplexer>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>(); // Retrieve the IConfiguration object
    var redisConnectionString = configuration.GetConnectionString("RedisCacheUrl");
    return ConnectionMultiplexer.Connect(redisConnectionString);
});
//-----------------------------------------------------------------------


// Get the secret key from the configuration
var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Secret"]);
// Add authentication services with JWT Bearer token validation to the service collection
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    // Add JWT Bearer authentication options
    .AddJwtBearer(options =>
    {
        // Configure token validation parameters
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Specify whether the server should validate the signing key
            ValidateIssuerSigningKey = true,

            // Set the signing key to verify the JWT signature
            IssuerSigningKey = new SymmetricSecurityKey(key),

            // Specify whether to validate the issuer of the token (usually set to false for development)
            ValidateIssuer = false,// true, // imade changes 

            // Specify whether to validate the audience of the token (usually set to false for development)
            ValidateAudience = false,// true // i made changes
        };
    });


builder.Services.AddControllers();


// Configure Swagger/OpenAPI
// Configure Swagger generation options
builder.Services.AddSwaggerGen(c =>
{
    // Define Swagger document metadata (title and version)
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fundoo Notes", Version = "v1" });

    // Configure JWT authentication for Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        // Describe how to pass the token
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization", // The name of the header containing the JWT token
        In = ParameterLocation.Header, // Location of the JWT token in the request headers
        Type = SecuritySchemeType.Http, // Specifies the type of security scheme (HTTP in this case)
        Scheme = "bearer", // The authentication scheme to be used (in this case, "bearer")
        BearerFormat = "JWT" // The format of the JWT token
    });

    // Specify security requirements for Swagger endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            // Define a reference to the security scheme defined above
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" // The ID of the security scheme (defined in AddSecurityDefinition)
                }
            },
            new string[] {} // Specify the required scopes (in this case, none)
        }
    });
});


var app = builder.Build();

// Enable middleware to serve generated Swagger as JSON endpoint
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

    // Set the OAuth2 configuration for Swagger UI
    c.OAuthClientId("swagger-ui");
    c.OAuthAppName("Swagger UI");
});

// Configure the HTTP request pipeline
app.UseHttpsRedirection();

// Enable authentication middleware
app.UseAuthentication();

// Enable authorization middleware
app.UseAuthorization();

// Map controller routes
app.MapControllers();

// Execute the request pipeline
app.Run();
