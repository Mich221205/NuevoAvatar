using PV_NA_Matricula;
using PV_NA_Matricula.Repository;
using PV_NA_Matricula.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<IMatriculaRepository, MatriculaRepository>();
builder.Services.AddScoped<IMatriculaService, MatriculaService>();
builder.Services.AddScoped<IPreMatriculaRepository, PreMatriculaRepository>();
builder.Services.AddScoped<IPreMatriculaService, PreMatriculaService>();
builder.Services.AddScoped<IEstudianteRepository, EstudianteRepository>();
builder.Services.AddScoped<IEstudianteService, EstudianteService>();
builder.Services.AddScoped<IDireccionRepository, DireccionRepository>();
builder.Services.AddScoped<IDireccionService, DireccionService>();
builder.Services.AddScoped<INotasRepository, NotasRepository>();
builder.Services.AddScoped<INotasService, NotasService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPreMatriculaEndpoints();
app.MapMatriculaEndpoints();
app.MapEstudianteEndpoints();
app.MapDireccionEndpoints();
app.MapNotasEndpoints();

app.Run(); 
