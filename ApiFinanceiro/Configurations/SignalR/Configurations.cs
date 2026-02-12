namespace ApiFinanceiro.Configurations.SignalR;

public static class SignalRConfig
{
    public static void AddSignalRConfiguration(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            services.AddSignalR();
        }
        else
        {
            var connectionString = configuration.GetConnectionString("SignalRConnection");
            services.AddSignalR().AddAzureSignalR(connectionString);
        }
    }
}
