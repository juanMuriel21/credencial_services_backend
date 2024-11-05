using System.Text;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("*") // Reemplaza con la URL de BlazorWasm
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
var app = builder.Build();

app.UseCors(); // Habilita CORS

var credenciales = new List<Credencial>();
const string correctKey = "123456789";

app.MapPost("/api/authenticate", ([FromBodyAttribute]string key) => {
    Console.WriteLine(key);
    if (key == correctKey) {
        return Results.Ok(true);
    }

    return Results.Ok(false);
});

app.MapPost("/api/credenciales", (Credencial credencial) => {
    credenciales.Add(credencial);

    return Results.Ok();
});

app.MapGet("/api/credenciales", () => {

    return Results.Ok(credenciales);

});

app.MapGet("/api/credenciales/{servicio}", (string servicio) => {
    var credencial = credenciales.FirstOrDefault(c => c.servicio.Equals(servicio, StringComparison.OrdinalIgnoreCase));
    if (credencial != null)
    {
        return Results.Ok(credencial);
    }
    return Results.NotFound("Credencial no encontrada.");
});

app.MapDelete("/api/credenciales/{servicio}", (string servicio) => {
    var credencial = credenciales.FirstOrDefault(c => c.servicio.Equals(servicio, StringComparison.OrdinalIgnoreCase));
    if (credencial != null)
    {
        credenciales.Remove(credencial);
        return Results.Ok("Credencial eliminada.");
    }
    return Results.NotFound("Credencial no encontrada.");
});

app.Run();

public class Credencial
{
    public string servicio { get; set; }
    public string? usuario { get; set; }
    public string? password { get; set; }
}