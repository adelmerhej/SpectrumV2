using SpectrumV1.Models.Accounting.FlowType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumV1.DataLayers.Accounting.FlowType
{
    public interface IFlowTypeRepository
    {
        // CRUD Operations
        Task<List<FlowTypeModel>> GetFlowTypesAsync();
        Task<FlowTypeModel> GetFlowTypeByIdAsync(string id);
        Task<string> AddNewFlowTypeAsync(FlowTypeModel flowType);
        Task<bool> UpdateFlowTypeAsync(FlowTypeModel flowType);
        Task<bool> DeleteFlowTypeAsync(string id);

        // A custom query example
        Task<FlowTypeModel> GetFlowTypeByName(string name);
        Task<long> GetCountAsync();
    }
}
