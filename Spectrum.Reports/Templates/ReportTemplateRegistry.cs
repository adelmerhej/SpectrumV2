using System;
using System.Collections.Generic;
using System.IO;

namespace Spectrum.Reports.Templates
{
	public static class ReportTemplateRegistry
	{
		private static readonly object SyncRoot = new object();
		private static readonly List<IReportTemplateProvider> Providers = new List<IReportTemplateProvider>();

		public static void Register(IReportTemplateProvider provider)
		{
			if (provider == null) throw new ArgumentNullException(nameof(provider));
			lock (SyncRoot)
			{
				Providers.Add(provider);
			}
		}

		public static void RegisterModelTemplate<TModel>(Func<TModel, string> templatePathFactory) where TModel : class
		{
			Register(new ModelReportTemplateProvider<TModel>(templatePathFactory));
		}

		public static bool TryOpenTemplateStream(object model, out Stream templateStream)
		{
			templateStream = null;
			if (model == null)
				return false;

			lock (SyncRoot)
			{
				for (int i = 0; i < Providers.Count; i++)
				{
					var provider = Providers[i];
					if (!provider.CanHandle(model))
						continue;

					templateStream = provider.OpenTemplateStream(model);
					if (templateStream != null)
						return true;
				}
			}

			return false;
		}
	}
}
