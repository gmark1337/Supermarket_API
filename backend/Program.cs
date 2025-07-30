using backend;
using backend.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

builder.Services.AddSerilog(
    options => options
    .MinimumLevel.Information()
    .WriteTo.Console());


builder.Services.AddCors(options =>
{
    options.AddPolicy("_myAllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("https://localhost:5002", "https://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddSingleton<ExternalFlyerService>();
builder.Services.AddSingleton<ExternalPdfService>();

builder.Services.AddHttpClient<ExternalFlyerService>();
builder.Services.AddHttpClient<ExternalPdfService>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("_myAllowSpecificOrigins");

app.MapControllers();

app.Run();
