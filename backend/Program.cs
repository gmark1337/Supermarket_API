using Amazon.Runtime;
using Amazon.S3;
using backend;
using backend.Controllers;
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
    options.AddPolicy("localTesting", policy =>
    {
        policy.WithOrigins(builder.Configuration["ConnectionStrings:Frontend"], builder.Configuration["ConnectionStrings:NodeJs_API"])
        .AllowAnyHeader()
        .AllowAnyMethod();
    });

    options.AddPolicy("Deployment", policy =>
    {
        policy.WithOrigins("https://thankful-rock-027ccfc03.2.azurestaticapps.net")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddSingleton<ExternalFlyerService>();
builder.Services.AddSingleton<ExternalPdfService>();
builder.Services.AddSingleton<BlobService>();
builder.Services.AddSingleton<FeedbackController>();

builder.Services.AddHttpClient<ExternalFlyerService>();
builder.Services.AddHttpClient<ExternalPdfService>();


var r2settings = builder.Configuration.GetSection("CloudFlareConnectionStrings");
string accountId = r2settings["AccountID"];
string accessKey = r2settings["AccessKeyID"];
string secretKey = r2settings["SecretAccessKeyID"];

var awsCred = new BasicAWSCredentials(accessKey, secretKey);

var s3Config = new AmazonS3Config
{
    ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
    ForcePathStyle = true,
    SignatureVersion = "4",
    AuthenticationRegion = "auto"
};

builder.Services.AddSingleton<IAmazonS3>(new AmazonS3Client(awsCred, s3Config));

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("localTesting");
}

app.UseHttpsRedirection();
app.UseRouting();

//app.UseCors("_myAllowSpecificOrigins");
//app.UseCors("localTesting");

app.MapControllers();

app.MapGet("/", () => "Backend is running!");

app.Run();
