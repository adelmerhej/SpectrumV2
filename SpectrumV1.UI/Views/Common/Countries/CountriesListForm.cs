using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using SpectrumV1.DataLayers.Common.Countries;
using SpectrumV1.Models.Common.Countries;
using SpectrumV1.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectrumV1.Views.Common.Countries
{
	public partial class CountriesListForm : RibbonForm, IFormWithRibbon
	{
		private ContinentModel _continentModel = new ContinentModel();
		private IList<ContinentModel> _continents = new List<ContinentModel>();

		private readonly ContinentRepository _continentRepository = new ContinentRepository();

		//Init permissionvariables
		private bool _canAdd = true;
		private bool _canEdit = true;
		private bool _canDelete = true;
		private bool _canPrint = true;
		private bool _isAdmin = true;
		private bool _isProtected = true;

		#region Implementation of IFormWithRibbon

		public RibbonControl MainRibbon => rcCountriesList;
		public RibbonPage DefaultPage => rpCountriesList;


		#endregion

		public CountriesListForm()
		{
			InitializeComponent();
		}
	}
}