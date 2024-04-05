using BusinessLogicLayer.InterfaceBL;
using BusinessLogicLayer.InterfaceBL;
using BusinessLogicLayer.InterfaceBL.Labels;
using BusinessLogicLayer.InterfaceBL.NotesInterface;
using BusinessLogicLayer.ServiceBL.CollaborationServiceBL;
using BusinessLogicLayer.ServiceBL.LabelService;
using BusinessLogicLayer.ServiceBL.NotesService;
using BusinessLogicLayer.ServiceBL.UserService;
using CommonLayer.Model.Email;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Interface.CollaborationRL;
using RepositoryLayer.Interface.LabelRL;
using RepositoryLayer.Interface.NoteService;
using RepositoryLayer.Interface.UserService;
using RepositoryLayer.Service;
using System.Text;




var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IRegistrationBL,RegistrationServiceBL>();
builder.Services.AddScoped<IRegistration, RegistrationService>();
builder.Services.AddScoped<IAuthonticationService, AuthonticationService>();
builder.Services.AddScoped<INotesBL, NotesServiceBL>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<ICollaborationBL, CollaborationBL>();
builder.Services.AddScoped<ICollaborationRL, CollaborationRL>();
builder.Services.AddScoped<INotesLabelBL, NotesLabelBL>();
builder.Services.AddScoped<INotesLabelRL, NotesLabelRL>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped(sp => sp.GetRequiredService<IOptions<EmailSettings>>().Value);






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
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

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
