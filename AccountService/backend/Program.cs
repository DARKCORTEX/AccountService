var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Aquí se agregan los controladores
builder.Services.AddOpenApi();

// CORS para permitir el acceso desde el frontend en localhost:3000
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);

// Este middleware permite que las rutas de los controladores sean accesibles.
app.UseRouting(); 

// Mapea las rutas para los controladores
app.MapControllers();  // Esto es necesario para que tu AuthController y demás controladores estén disponibles

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();