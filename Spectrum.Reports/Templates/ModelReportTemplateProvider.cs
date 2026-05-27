using System;
using System.IO;

namespace Spectrum.Reports.Templates
{
    public sealed class ModelReportTemplateProvider<TModel> : IReportTemplateProvider where TModel : class
    {
        private readonly Func<TModel, string> _templatePathFactory;

        public ModelReportTemplateProvider(Func<TModel, string> templatePathFactory)
        {
            _templatePathFactory = templatePathFactory ?? throw new ArgumentNullException(nameof(templatePathFactory));
        }

        public bool CanHandle(object model)
        {
            return model is TModel;
        }

        public string GetTemplatePath(object model)
        {
            var typed = model as TModel;
            if (typed == null) return null;
            return _templatePathFactory(typed);
        }

        public Stream OpenTemplateStream(object model)
        {
            var path = GetTemplatePath(model);
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return null;
            return File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
    }
}
