using ApiFinanceiro.Configurations.DependencyInjection;
using ApiFinanceiro.Configurations.Middleware;
using ApiFinanceiro.Configurations.SignalR;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Configuração de serviços
builder.Services.AddSignalRConfiguration(builder.Configuration, builder.Environment);
builder.Services.AddApplicationServices();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// 🔄 Configuração do pipeline
app.UseApplicationPipeline();

await app.RunAsync();
