//using PV_NA_OfertaAcademica.Middleware;
//using PV_NA_OfertaAcademica.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PV_NA_OfertaAcademica;
using PV_NA_OfertaAcademica.Controllers;
using PV_NA_OfertaAcademica.Repository;
using PV_NA_OfertaAcademica.Services;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Conexion a la base de datos
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Oferta Académica - ACD4",
        Version = "v1",
        Description = "Servicio para administrar grupos académicos"
    });
});



builder.Services.AddScoped<GrupoRepository>();

// Dependencias del institucion
builder.Services.AddScoped<IInstitucionRepository, InstitucionRepository>();
builder.Services.AddScoped<IInstitucionService, InstitucionService>();

// dependencia Curso
builder.Services.AddScoped<ICursoRepository, CursoRepository>();
builder.Services.AddScoped<ICursoService, CursoService>();

// Dependencias del periodo
builder.Services.AddScoped<PeriodoRepository>();
builder.Services.AddScoped<PeriodoService>();
/*
//  Validación JWT 
builder.Services.AddHttpClient<TokenValidator>();
builder.Services.AddSingleton<TokenValidator>();
*/




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//  Middleware de validación de token
//app.UseJwtValidation();

app.MapInstitucionEndpoints();
app.MapCursoEndpoints();
app.MapGrupoEndpoints();
app.MapPeriodoEndpoints();

app.Run();

