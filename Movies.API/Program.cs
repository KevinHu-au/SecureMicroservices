using Microsoft.EntityFrameworkCore;
using Movies.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<MoviesAPIContext>(options =>
        options.UseInMemoryDatabase("Movies"));

builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                  options.Authority = "https://localhost:5005";
                  options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                  {
                    ValidateAudience = false
                  };
                });


builder.Services.AddAuthorization(options =>
{
  //Add the authorization policy to allow only the user claims contains claim type of 'client_id' with claim vlue of 'movieClient'
  options.AddPolicy("ClientIdPolicy", policy => policy.RequireClaim("client_id", "movieClient"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

SeedData(app);

app.Run();

void SeedData(WebApplication app)
{
  using var scope = app.Services.CreateScope();
  var services = scope.ServiceProvider;
  var context = services.GetRequiredService<MoviesAPIContext>();
  MoviesContextSeed.SeedAsync(context);
}