var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers(); // this is kinda registering the controllers - it scans Classes ending with Controller or ingerited from ControllerBase.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers(); // this where the actual mapping happens 

app.Run();
