using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;

namespace poc_app_configuration.Services;

public class SalaryCalculateService : ISalaryCalculateService
{
    private readonly IFeatureManager _featureManager;

    public SalaryCalculateService(IFeatureManager featureManager)
        => _featureManager = featureManager;

    public async Task<decimal> Calculate(decimal grossSalary)
    {
        decimal netSalary = default;

        if(await _featureManager.IsEnabledAsync("NacionalSecurityTax"))
        {
            if (grossSalary <= 1045)
                netSalary = grossSalary - (grossSalary * (decimal)(7.5 / 100));
            else if(grossSalary > 1045 && grossSalary <= (decimal)2089.60)
                netSalary = grossSalary - (grossSalary * (decimal)(9 / 100));
            else if(grossSalary > (decimal)2089.60 && grossSalary <= (decimal)3134.40)
                netSalary = grossSalary - (grossSalary * (decimal)(12 / 100));
            else if(grossSalary > (decimal)3134.40 && grossSalary <= (decimal)6101.06)
                netSalary = grossSalary - (grossSalary * (decimal)(14 / 100));
            else
                netSalary = grossSalary - (decimal)713.10;
        }

        if(await _featureManager.IsEnabledAsync("IncomeTax"))
            netSalary = netSalary - IncomeTax(grossSalary);

        return netSalary;
    }

    private decimal IncomeTax(decimal grossSalary)
    {
        if (grossSalary <= (decimal)1903.98)
            return grossSalary;
        else if (grossSalary > (decimal)1903.98 && grossSalary <= (decimal)2826.65)
            return (grossSalary * (decimal)(7.5 / 100));
        else if (grossSalary > (decimal)2826.65 && grossSalary <= (decimal)3751.05)
            return (grossSalary * (decimal)(15 / 100));
        else if (grossSalary > (decimal)3751.05 && grossSalary <= (decimal)4665.68)
            return (grossSalary * (decimal)(22.5 / 100));
        else
            return (grossSalary * (decimal)(27.5 / 100));
    }
}
