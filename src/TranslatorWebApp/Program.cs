using TranslatorWebApp;
using TranslatorWebApp.Api;

Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder => 
        webBuilder.UseStartup<Startup>())
    .Build()
    .Run();