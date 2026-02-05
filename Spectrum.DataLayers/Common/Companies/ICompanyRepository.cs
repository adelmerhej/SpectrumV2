using Spectrum.Models.Common.Companies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Common.Companies
{
	public interface ICompanyRepository
	{
		// CRUD Operations
		Task<List<CompanyModel>> GetCompaniesAsync();
		Task<CompanyModel> GetCompanyByIdAsync(string id);
		Task<string> AddNewCompanyAsync(CompanyModel company);
		Task<bool> UpdateCompanyAsync(CompanyModel company);
		Task<bool> DeleteCompanyAsync(string id);

		// A custom query example
		Task<CompanyModel> GetCompanyByName(string name);
		Task<long> GetCountAsync();
	}
}
