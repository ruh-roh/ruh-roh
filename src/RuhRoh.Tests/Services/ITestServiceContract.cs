using System.Collections.Generic;
using System.Threading.Tasks;
using RuhRoh.Tests.Models;

namespace RuhRoh.Tests.Services
{
    public interface ITestServiceContract
    {
        IEnumerable<TestItem> GetItems();
        TestItem GetItemById(int id);

        Task<TestItem> GetItemByIdAsync(int id);
    }
}
