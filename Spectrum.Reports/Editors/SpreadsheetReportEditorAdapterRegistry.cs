using Spectrum.Reports.Interfaces;
using System;
using System.Collections.Generic;

namespace Spectrum.Reports.Editors
{
    public static class SpreadsheetReportEditorAdapterRegistry
    {
        private static readonly object SyncRoot = new object();
        private static readonly List<Func<ISpreadsheetReportEditorAdapter>> Factories = new List<Func<ISpreadsheetReportEditorAdapter>>();

        static SpreadsheetReportEditorAdapterRegistry()
        {
            Register(() => new OrderSpreadsheetReportEditorAdapter());
        }

        public static void Register(Func<ISpreadsheetReportEditorAdapter> factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            var probe = factory();
            if (probe == null) throw new InvalidOperationException("Spreadsheet editor adapter factory returned null.");
            var adapterType = probe.GetType();

            lock (SyncRoot)
            {
                for (int i = 0; i < Factories.Count; i++)
                {
                    var existing = Factories[i]();
                    if (existing != null && existing.GetType() == adapterType)
                        return;
                }
                Factories.Add(factory);
            }
        }

        public static ISpreadsheetReportEditorAdapter Resolve(IReportAdapter reportAdapter)
        {
            if (reportAdapter == null) return null;

            lock (SyncRoot)
            {
                for (int i = 0; i < Factories.Count; i++)
                {
                    var candidate = Factories[i]();
                    if (candidate != null && candidate.CanHandle(reportAdapter))
                        return candidate;
                }
            }

            return null;
        }
    }
}
