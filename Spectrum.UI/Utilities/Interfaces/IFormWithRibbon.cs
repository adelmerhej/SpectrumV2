using DevExpress.XtraBars.Ribbon;

namespace Spectrum.Utilities.Interfaces
{
	public interface IFormWithRibbon
	{
		RibbonControl MainRibbon { get; }
		RibbonPage DefaultPage { get; }
	}
}
