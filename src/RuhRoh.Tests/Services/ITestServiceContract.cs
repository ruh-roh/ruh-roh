using System.Collections.Generic;
using RuhRoh.Tests.Models;

namespace RuhRoh.Tests.Services
{
    public interface ITestServiceContract
    {
        IEnumerable<TestItem> GetItems();
        TestItem GetItemById(int id);
    }
}
