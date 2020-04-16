using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PickPixForEver.Services
{
    public interface IDataStore<T>
    {
        Task<int> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(int id);
        Task<T> FindItemAsync(int id);
        Task<IEnumerable<T>> GetItemsAsync();
        Task<IEnumerable<T>> GetItemsAsync(string searchTerm);
    }
}
