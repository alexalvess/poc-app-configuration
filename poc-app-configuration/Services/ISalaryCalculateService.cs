namespace poc_app_configuration.Services;

public interface ISalaryCalculateService
{
    Task<decimal> Calculate(decimal grossSalary);
}
