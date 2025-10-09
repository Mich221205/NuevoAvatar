using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;
using PV_NA_Pagos;
using PV_NA_Pagos.Repository;
using PV_NA_Pagos.Services;
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
        Title = "API Factura - IPN1",
        Version = "v1",
        Description = "Servicio para consultar el historial académico de los estudiantes (HU IPN1)."
    });
});


builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<FacturaRepository>();
builder.Services.AddScoped<FacturaService>();
builder.Services.AddScoped<PagoRepository>();
builder.Services.AddScoped<PagoService>();

builder.Services.AddHttpClient();
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapFacturaEndpoints();
app.MapPagoEndpoints();

app.Run();
