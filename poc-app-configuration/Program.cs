using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using poc_app_configuration.Services;

var buider = Host
    .CreateDefaultBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

buider
    .ConfigureAppConfiguration(cfg 
        => cfg.AddAzureAppConfiguration(opt 
            => opt.Connect(configuration.GetConnectionString("AppConfiguration")).UseFeatureFlags()));

buider.ConfigureServices(services =>
{
    services.AddAzureAppConfiguration();
    services.AddFeatureManagement();

    services.AddScoped<ISalaryCalculateService, SalaryCalculateService>();
});


var app = buider.Build();

var salaryService = app.Services.GetRequiredService<ISalaryCalculateService>();
var netSalary = await salaryService.Calculate(1000);

Console.WriteLine($"Net salary: {netSalary}");

await app.RunAsync();