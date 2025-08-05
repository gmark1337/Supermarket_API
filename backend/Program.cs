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
        policy.WithOrigins(builder.Configuration["ConnectionStrings:Frontend"], builder.Configuration["ConnectionStrings:NodeJs_API"])
        .AllowAnyHeader()
        .AllowAnyMethod();
    });

    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://thankful-rock-027ccfc03.2.azurestaticapps.net")
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

//app.UseCors("_myAllowSpecificOrigins");
app.UseCors("AllowFrontend");

app.MapControllers();

app.MapGet("/", () => "Backend is running!");

app.Run();
