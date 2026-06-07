using Spectrum.Models.Common.Items;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Common.Items
{
    public interface IItemRepository
    {
        // CRUD Operations
        Task<List<ItemModel>> GetItemsAsync();
        Task<ItemModel> GetItemByIdAsync(string id);
        Task<string> AddNewItemAsync(ItemModel item);
        Task<bool> UpdateItemAsync(ItemModel item);
        Task<bool> DeleteItemAsync(string id);
        // A custom query example
        Task<ItemModel> GetItemByName(string name);
        Task<long> GetCountAsync();
    }
}
