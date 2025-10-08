using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PV_NA_UsuariosRoles.Entities;
using PV_NA_UsuariosRoles.Repository;
using PV_NA_UsuariosRoles.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios del contenedor DI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración JWT desde appsettings.json
// Configuración JWT desde appsettings.json
// Configuración de opciones de token desde appsettings.json
builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection("Jwt"));
var jwt = builder.Configuration.GetSection("Jwt").Get<TokenOptions>()!;

// ? VALIDACIÓN DE CONFIGURACIÓN
if (string.IsNullOrEmpty(jwt.Secret) || jwt.Secret.Length < 16)
    throw new InvalidOperationException("JWT Secret debe tener al menos 16 caracteres");

if (string.IsNullOrEmpty(jwt.Issuer))
    throw new InvalidOperationException("JWT Issuer no configurado");

if (string.IsNullOrEmpty(jwt.Audience))
    throw new InvalidOperationException("JWT Audience no configurado");

var key = Encoding.UTF8.GetBytes(jwt.Secret);


// Configuración de autenticación JWT Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Registro de dependencias para inyección
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<DbConnectionFactory>();
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<SesionRepository>();

var app = builder.Build();

// Configuración del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();