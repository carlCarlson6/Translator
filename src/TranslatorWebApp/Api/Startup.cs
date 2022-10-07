namespace TranslatorWebApp.Api;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment) =>
        (_configuration, _environment) = (configuration, environment);

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddSwaggerGen()
            .AddEndpointsApiExplorer()
            .AddControllers();
        
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app
            .UseHttpsRedirection()
            .UseRouting()
            .UseEndpoints(builder => builder.MapControllers());
    }
}