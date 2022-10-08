namespace TranslatorWebApp.Api.Infrastructure;

public static class WebExtensions
{
    public const string ApiTestsEnvironmentName = "ApiTests";
    public const string TestEnvironmentName = "test";
    public const string DevEnvironmentName = "dev";

    public static bool RunningTests(this IHostEnvironment webHostEnvironment) => 
        webHostEnvironment.EnvironmentName == ApiTestsEnvironmentName;
        
    public static bool IsTest(this IHostEnvironment webHostEnvironment) => 
        webHostEnvironment.EnvironmentName.Equals(TestEnvironmentName, StringComparison.OrdinalIgnoreCase);
        
    public static bool IsDev(this IHostEnvironment webHostEnvironment) =>
        webHostEnvironment.EnvironmentName.Equals(DevEnvironmentName, StringComparison.OrdinalIgnoreCase);
}