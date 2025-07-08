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

builder.Services.AddSingleton<IFlyerService, FlyerService>();
builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddSingleton<ExternalFlyerService>();


builder.Services.AddHttpClient<ExternalFlyerService>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapControllers();

app.Run();
