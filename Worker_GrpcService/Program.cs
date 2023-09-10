using Microsoft.EntityFrameworkCore;
using Worker_GrpcService.DAL;
using Worker_GrpcService.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
// Добавляем контекст БД
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GenderService>();
app.MapGrpcService<WorkerService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
