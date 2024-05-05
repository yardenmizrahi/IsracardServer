// Create the builder
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using server.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "client\\client\\build";
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder.WithOrigins("http://localhost:3001").AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(); // Add this line to register Swagger services

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseSpaStaticFiles();
app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();


app.Run();