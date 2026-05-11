using DevExpress.LookAndFeel;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using Spectrum.Models.Users;
using Spectrum.Properties;
using Spectrum.Utilities;
using Spectrum.Utilities.Interfaces;
using Spectrum.Utilities.Layout;
using Spectrum.Views.Accounting.Banks;
using Spectrum.Views.Accounting.Charts;
using Spectrum.Views.Accounting.CostCenter;
using Spectrum.Views.Accounting.FlowType;
using Spectrum.Views.Accounting.Journals;
using Spectrum.Views.Accounting.JournalType;
using Spectrum.Views.Common;
using Spectrum.Views.Common.Areas;
using Spectrum.Views.Common.Companies;
using Spectrum.Views.Common.Countries;
using Spectrum.Views.Common.Currencies;
using Spectrum.Views.Common.Departments;
using Spectrum.Views.Common.Services;
using Spectrum.Views.HumanResources.Common.BloodTypes;
using Spectrum.Views.HumanResources.Common.EmployeeTypes;
using Spectrum.Views.HumanResources.Employees;
using Spectrum.Views.HumanResources.Employees.Status;
using Spectrum.Views.HumanResources.HRCVs;
using Spectrum.Views.HumanResources.JobPositions;
using Spectrum.Views.HumanResources.Roles;
using Spectrum.Views.Main.Connections;
using Spectrum.Views.Main.Update;
using Spectrum.Views.Members.Clients;
using Spectrum.Views.Members.Engineers;
using Spectrum.Views.Projects;
using Spectrum.Views.Transactions.Expenses;
using Spectrum.Views.Transactions.Invoices;
using Spectrum.Views.Transactions.Receipts;
using Spectrum.Views.Transactions.Statements;
using Spectrum.Views.Users;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Spectrum.Views.Main
{
    public partial class MainForm : RibbonForm
    {
        private const string _formName = "MainForm";
        private int _formId;

        private bool _isVisible;
        private bool _isActive;
        private bool _isAdmin;

        private bool _resetMenu;
        private readonly bool _logOut;

        public MainForm()
        {
            InitializeComponent();

            InitializeBindings();
            WireUpBindings();
            ApplyPermissions();
            ApplyDefaults();

            UpdateStatus();

            defaultLookAndFeel1.LookAndFeel.SetSkinStyle(Settings.Default.ApplicationSkinName, Settings.Default.ApplicationPalette);
            LayoutsStyle.LoadLayoutMenu(mainMenu, CurrentUser.UserName, CurrentUser.Company);

            _logOut = true;
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            HelperApplication.CheckForUpdate();
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            TrySelectMergedPageForActiveChild();
        }

        private void MainForm_MdiChildActivate(object sender, EventArgs e)
        {
            TrySelectMergedPageForActiveChild();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var settings = Settings.Default;
            settings.SkinName = UserLookAndFeel.Default.SkinName;
            settings.Palette = UserLookAndFeel.Default.ActiveSvgPaletteName;
            if (!_resetMenu)
            {
                LayoutsStyle.SaveLayoutMenu(mainMenu, CurrentUser.UserName, CurrentUser.Company);
            }
        }

        #region Loading Region

        private void InitializeBindings()
        {

        }

        private void WireUpBindings()
        {

        }

        private void ApplyDefaults()
        {

        }

        private void ApplyPermissions()
        {

        }

        private void UpdateStatus()
        {
            //Get HOST NAME
            statusHost.Caption = @"Host: " + CurrentUser.HostName;
            statusDatabase.Caption = @"Database: " + CurrentUser.DatabaseName;
            statusAppName.Caption = @"App: " + HelperApplication.AssemblyDescription;
            statusVersion.Caption = Resources.VERSION + @" " + HelperApplication.AssemblyVersion;

            statusCompanyName.Caption = Resources.COMPANY_NAME + @" " + CurrentUser.Company;
            statusUserName.Caption = Resources.USER_NAME + @" " + CurrentUser.UserName;
            statusDate.Caption = Resources.TODAY + @" " + DateTime.Now.ToLongDateString();
        }

        #endregion

        #region Buttons Event

        private void btnEnd_ItemClick(object sender, ItemClickEventArgs e)
        {
            Application.Exit();
        }

        private void btnExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            ExitAndClean();
        }

        private void btnAbout_ItemClick(object sender, ItemClickEventArgs e)
        {
            ShowDialog(new AboutForm());
        }

        private void btnLiveUpdate_ItemClick(object sender, ItemClickEventArgs e)
        {
            ShowDialog(new CheckForUpdateForm());
        }

        private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnResetLayout_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("This will reset menu layout next login, to its default settings.\nAre you sure you want to continue?", "Reset Menu...",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==
                DialogResult.Yes)
            {
                _resetMenu = true;
                LayoutsStyle.ResetLayoutMenu(mainMenu, CurrentUser.UserName, CurrentUser.Company);
            }
        }

        #endregion

        #region CloseForm
        private void ExitAndClean()
        {
            //main form close
            if (_logOut)
            {
                Settings.Default.ApplicationSkinName = defaultLookAndFeel1.LookAndFeel.SkinName;
                Settings.Default.ApplicationPalette = defaultLookAndFeel1.LookAndFeel.ActiveSvgPaletteName;

                Settings.Default.Save();
                if (!IsDisposed)
                {
                    Close();
                }
            }
        }


        #endregion


        #region OpenForm Methods

        /// <summary>
        /// Opens a form instance, reusing any existing open instance of the same type.
        /// Pass <paramref name="isDialog"/> to show as modal dialog instead of MDI.
        /// </summary>
        private void OpenForm<T>(T myForm, bool isDialog = false) where T : Form
        {
            try
            {
                var existing = Application.OpenForms.Cast<Form>().FirstOrDefault(f => f.GetType() == typeof(T));
                if (existing != null)
                {
                    if (existing.WindowState == FormWindowState.Minimized)
                        existing.WindowState = FormWindowState.Normal;
                    existing.BringToFront();
                    existing.Focus();
                    return;
                }

                if (!isDialog)
                {
                    myForm.MdiParent = this;
                    myForm.Show();
                }
                else
                {
                    ShowDialog(myForm);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Main OpenForm Error Message!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Opens a form of type <typeparamref name="T"/> by creating a new instance only if not already open.
        /// This avoids passing already-constructed instances and keeps a single instance per type.
        /// </summary>
        private void OpenForm<T>(bool isDialog = false)
            where T : Form, new()
        {
            try
            {
                var existing = Application.OpenForms.Cast<Form>().OfType<T>().FirstOrDefault();
                if (existing != null)
                {
                    if (existing.WindowState == FormWindowState.Minimized)
                        existing.WindowState = FormWindowState.Normal;
                    existing.BringToFront();
                    existing.Focus();
                    return;
                }

                var myForm = new T();
                if (!isDialog)
                {
                    myForm.MdiParent = this;
                    myForm.Show();
                }
                else
                {
                    ShowDialog(myForm);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Main OpenForm Error Message!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ShowDialog(Form form)
        {
            if (form == null) return;
            using (form)
            {
                form.ShowDialog(this);
            }
        }

        private void ShowFeatureUnderDevelopment()
        {
            ShowDialog(new FeatureUnderDevelopmentForm());
        }

        private void TrySelectMergedPageForActiveChild()
        {
            try
            {
                var af = ActiveMdiChild as IFormWithRibbon;
                if (af == null || af.DefaultPage == null || af.DefaultPage.Text == null) return;
                var pageText = af.DefaultPage.Text;
                var mergedPages = ribbonMainForm?.MergedPages;
                RibbonPage targetPage = null;
                if (mergedPages != null)
                {
                    // Some DevExpress collections do not expose ContainsKey; indexer returns null if not found.
                    targetPage = mergedPages[pageText];
                }
                if (targetPage != null)
                {
                    ribbonMainForm.SelectedPage = targetPage;
                }
                UpdateStatus();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
        #endregion

        #region Dashboard Menu

        private void mnuDashboardActivities_Click(object sender, EventArgs e)
        {
            //OpenForm<DashboardActivitiesForm1>();
        }

        #endregion

        #region Operations Menu

        private void mnuProjects_Click(object sender, EventArgs e)
        {
            //OpenForm(new ProjectsListForm());
            OpenForm<ProjectsListForm>();
            // Example using new overload: OpenForm<ProjectsListForm>();
        }

        private void mnuInvoices_Click(object sender, EventArgs e)
        {
            OpenForm<InvoicesListForm>();
        }

        private void mnuMyInvoice_Click(object sender, EventArgs e)
        {
            OpenForm<MyInvoiceForm>();
        }

        private void mnuInvoicesExpenses_Click(object sender, EventArgs e)
        {
            OpenForm<ExpensesListForm>();
            //
        }

        private void mnuReceipts_Click(object sender, EventArgs e)
        {
            OpenForm<ReceiptsListForm>();
        }

        private void mnuAccountsStatement_Click(object sender, EventArgs e)
        {
            StatementOfAccountForm frm = new StatementOfAccountForm();
            frm.ShowDialog();
        }



        #endregion

        #region Members Menu

        private void mnuClients_Click(object sender, EventArgs e)
        {
            OpenForm<ClientsListForm>();
        }

        private void mnuEngineers_Click(object sender, EventArgs e)
        {
            OpenForm<EngineersListForm>();
        }



        #endregion

        #region HumanResources Menu

        private void mnuHrCVs_Click(object sender, EventArgs e)
        {
            OpenForm<HrCvsListForm>();
        }

        private void mnuEmployees_Click(object sender, EventArgs e)
        {
            OpenForm<EmployeesListForm>();
        }

        private void mnuDepartments_Click(object sender, EventArgs e)
        {
            OpenForm<DepartmentsListForm>();
        }

        private void mnuRoles_Click(object sender, EventArgs e)
        {
            OpenForm<RolesListForm>();
        }


        #endregion

        #region Accounting Menu

        private void mnuJournals_Click(object sender, EventArgs e)
        {
            OpenForm<JournalsListForm>();
        }

        private void mnuChartsOfAccount_Click(object sender, EventArgs e)
        {
            OpenForm<ChartsListForm>();
        }

        private void mnuCostCenter_Click(object sender, EventArgs e)
        {
            OpenForm<CostCentersListForm>();
        }

        private void mnuFlowType_Click(object sender, EventArgs e)
        {
            OpenForm<FlowTypeListForm>();
        }

        private void mnuJournalTypes_Click(object sender, EventArgs e)
        {
            OpenForm<JournalTypesListForm>();
        }

        private void mnuCurrencies_Click(object sender, EventArgs e)
        {
            OpenForm<CurrenciesListForm>();
        }

        private void mnuExchangeList_Click(object sender, EventArgs e)
        {
            ShowFeatureUnderDevelopment();
        }

        private void mnuBanksList_Click(object sender, EventArgs e)
        {
            OpenForm<BanksListForm>();
        }

        private void mnuStatementOfAccount_Click(object sender, EventArgs e)
        {
            ShowFeatureUnderDevelopment();
        }

        private void mnuTrialBalance_Click(object sender, EventArgs e)
        {
            ShowFeatureUnderDevelopment();
        }


        #endregion

        #region Settings Menu

        private void mnuUsersList_Click(object sender, System.EventArgs e)
        {
            OpenForm<UsersListForm>();

        }

        private void mnuCompaniesList_Click(object sender, EventArgs e)
        {
            OpenForm<CompaniesListForm>();
        }

        private void mnuBranches_Click(object sender, EventArgs e)
        {
            OpenForm<BranchesListForm>();
        }

        private void mnuServicesList_Click(object sender, EventArgs e)
        {
            OpenForm<ServicesListForm>();

        }

        private void mnuAreas_Click(object sender, EventArgs e)
        {
            OpenForm<AreasListForm>();
        }

        #endregion

        #region Countries Menu
        private void mnuCountries_Click(object sender, EventArgs e)
        {
            OpenForm<CountriesListForm>();
        }

        private void mnuCities_Click(object sender, EventArgs e)
        {
            OpenForm<CitiesListForm>();
        }

        private void mnuContinents_Click(object sender, EventArgs e)
        {
            OpenForm<ContinentsListForm>();
        }

        private void mnuRegions_Click(object sender, EventArgs e)
        {
            OpenForm<RegionsListForm>();
        }

        private void mnuProvinces_Click(object sender, EventArgs e)
        {
            OpenForm<ProvincesListForm>();
        }

        private void mnuDistricts_Click(object sender, EventArgs e)
        {
            OpenForm<DistrictsListForm>();
        }

        #endregion

        #region Main Settings Menu Events

        private void mnuDatabaseSettings_Click(object sender, EventArgs e)
        {
            ShowDialog(new ServerConfigurationForm());
        }

        private void mnuGeneralSettings_Click(object sender, EventArgs e)
        {
            ApiKeySettingForm frm = new ApiKeySettingForm();
            frm.ShowDialog();
        }


        #endregion

        private void mnuStatus_Click(object sender, EventArgs e)
        {
            OpenForm<StatusListForm>();
        }

        private void mnuLocations_Click(object sender, EventArgs e)
        {
            OpenForm<LocationsListForm>();
        }

        private void mnuJobPositions_Click(object sender, EventArgs e)
        {
            OpenForm<JobPositionsListForm>();
        }

        private void mnuBloodType_Click(object sender, EventArgs e)
        {
            OpenForm<BloodTypesListForm>();
        }

        private void mnuEmployeeTypes_Click(object sender, EventArgs e)
        {
            OpenForm<EmployeeTypesListForm>();
        }

        private void mnuSuppliers_Click(object sender, EventArgs e)
        {
            ShowFeatureUnderDevelopment();
        }
    }
}