var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basic Web API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Add some minimal API endpoints
app.MapGet("/", () => "Welcome to Basic Web API! Visit /swagger for API documentation.");

app.MapGet("/health", () => new { Status = "Healthy", Timestamp = DateTime.UtcNow });

app.MapGet("/api/info", () => new 
{ 
    Title = "Basic Web API",
    Version = "1.0.0",
    Description = "A simple Web API demonstrating basic concepts",
    Environment = app.Environment.EnvironmentName,
    ServerTime = DateTime.UtcNow
});

app.Run();
