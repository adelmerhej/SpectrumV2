using System;
using System.Drawing;

namespace Spectrum.Views.Transactions.Invoices.Classes
{
    internal sealed class ActivityTimelineEntry
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public string PerformedBy { get; set; }
        public Color AccentColor { get; set; }
        public int Sequence { get; set; }
        public bool DateOnly { get; set; }
    }
}
