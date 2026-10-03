var builder = WebApplication.CreateBuilder(args);   // 1. configurar la app

builder.Services.AddControllers();                  // 2. soporte de controladores
builder.Services.AddEndpointsApiExplorer();         // 3. permite descubrir endpoints
builder.Services.AddSwaggerGen();                   // 4. genera la documentaci[on

var app = builder.Build();                          // 5. "congela" la configuraci[on

if (app.Environment.IsDevelopment())                // 6. solo en tu m[aquina
{
    app.UseSwagger();                               // 7. publica el JSON de la API
    app.UseSwaggerUI();                             // 8. publica la p[agina /swagger
}

app.UseHttpsRedirection();                          // 9. redirige HTTP a HTTPS
app.MapControllers();                               // 10. conecta los controladores

app.Run();