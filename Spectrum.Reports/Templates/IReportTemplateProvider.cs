using System.IO;

namespace Spectrum.Reports.Templates
{
    public interface IReportTemplateProvider
    {
        bool CanHandle(object model);
        string GetTemplatePath(object model);
        Stream OpenTemplateStream(object model);
    }
}
