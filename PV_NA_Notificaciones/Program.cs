using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;
using PV_NA_Notificaciones;
using  PV_NA_Notificaciones
.Repository;
using  PV_NA_Notificaciones
.Services;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("BitacoraClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5210");

});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Notificaciones - IPN3",
        Version = "v1",
        Description = "Servicio para registrar y consultar bitácoras del sistema (IPN3)."
    });
});


builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<NotificacionRepository>();
builder.Services.AddScoped<NotificacionService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapNotificacionEndpoints();

app.Run();
