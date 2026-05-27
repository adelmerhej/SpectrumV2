namespace Spectrum.Models.Administration.Dashboard
{
    public class ActivityDataModel
    {
        public string ActivityName { get; set; }
        public string Status { get; set; }
        public string Month { get; set; }
        public decimal Revenue { get; set; }
        public int ProjectCount { get; set; }
        public int EngineersCount { get; set; }
        public int ActivityCount { get; set; }
    }
}
