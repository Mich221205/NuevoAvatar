using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;
using PV_NA_Academico;
using PV_NA_Academico.Repository;
using PV_NA_Academico.Services;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 🔹 Configuración de servicios HTTP externos
// ==========================================
builder.Services.AddHttpClient("BitacoraClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5210"); // Bitácora (GEN1)
});

builder.Services.AddHttpClient("AuthClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5233"); // UsuariosRoles (HU USR5)
});

// ==========================================
// 🔹 Configuración Swagger + Seguridad JWT
// ==========================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Académico - ACA1 y ACA2",
        Version = "v1",
        Description = "Servicio para consultar el historial académico y el listado de estudiantes (HU ACA1 y HU ACA2)."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT con el formato: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// ==========================================
// 🔹 Repositorios y servicios
// ==========================================
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<HistorialRepository>();
builder.Services.AddScoped<HistorialService>();
builder.Services.AddScoped<ListadoEstudiantesRepository>();
builder.Services.AddScoped<ListadoEstudiantesService>();

builder.Services.AddHttpClient();

var app = builder.Build();


app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/swagger") ||
        context.Request.Path.StartsWithSegments("/validate"))
    {
        await next();
        return;
    }

    var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

    if (string.IsNullOrWhiteSpace(token))
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Token requerido.");
        return;
    }

    var authClient = context.RequestServices.GetRequiredService<IHttpClientFactory>().CreateClient("AuthClient");
    var response = await authClient.GetAsync($"/login/validate?token={token}");

    if (!response.IsSuccessStatusCode)
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Token inválido o expirado.");
        return;
    }

    await next();
});


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API Académico - ACA1 y ACA2 v1");
    });
}


app.MapHistorialEndpoints();
app.MapListadoEstudiantesEndpoints();

app.Run();
