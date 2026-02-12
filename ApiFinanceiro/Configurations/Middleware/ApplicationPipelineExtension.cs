using ApiFinanceiro.Hubs;

namespace ApiFinanceiro.Configurations.Middleware;

public static class ApplicationPipelineExtension
{
    public static void UseApplicationPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Financeiro v1");
                c.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHub<ProgressoHub>("/ProgressoHub");
    }

}
