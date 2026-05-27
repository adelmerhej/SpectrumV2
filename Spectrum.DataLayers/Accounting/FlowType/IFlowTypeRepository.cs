using Spectrum.Models.Accounting.FlowType;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Accounting.FlowType
{
    public interface IFlowTypeRepository
    {
        // CRUD Operations
        Task<List<FlowTypeModel>> GetFlowTypesAsync();
        Task<FlowTypeModel> GetFlowTypeByIdAsync(string id);
        Task<FlowTypeModel> GetDefaultFlowTypeAsync(string excludedId = null);
        Task<string> AddNewFlowTypeAsync(FlowTypeModel flowType);
        Task<bool> UpdateFlowTypeAsync(FlowTypeModel flowType);
        Task<bool> DeleteFlowTypeAsync(string id);

        // A custom query example
        Task<FlowTypeModel> GetFlowTypeByName(string name);
        Task<long> GetCountAsync();
    }
}
