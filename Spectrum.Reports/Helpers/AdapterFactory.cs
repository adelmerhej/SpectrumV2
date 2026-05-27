using Spectrum.Models.HumanResources.Employees;
using Spectrum.Models.Projects;
using Spectrum.Reports.Adapters.HumanResources.Employees;
using Spectrum.Reports.Adapters.Projects;
using Spectrum.Reports.Interfaces;
using System;

namespace Spectrum.Reports.Helpers
{
    public static class AdapterFactory
    {
        public static IReportAdapter Create(ProjectModel project)
        {
            return new ProjectReportAdapter(project);
        }

        public static IReportAdapter Create(EmployeeModel employee)
        {
            return new EmployeeReportAdapter(employee);
        }

        public static IReportAdapter Create(object model)
        {
            if (model is ProjectModel)
                return Create((ProjectModel)model);

            if (model is EmployeeModel)
                return Create((EmployeeModel)model);

            throw new NotSupportedException("No report adapter is registered for model type '" + model?.GetType().FullName + "'.");
        }
    }
}
