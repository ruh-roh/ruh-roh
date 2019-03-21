using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RuhRoh.Tests.Models;

namespace RuhRoh.Tests.Services
{
    public class TestService : ITestServiceContract
    {
        private readonly TestItem[] _data = 
        {
            new TestItem
            {
                Id = 1, TextField1 = "1", TextField2 = "1 - 2"
            },
            new TestItem
            {
                Id = 2, TextField1 = "2", TextField2 = "2 - 2"
            },
            new TestItem
            {
                Id = 3, TextField1 = "3", TextField2 = "3 - 2"
            }
        };

        public IEnumerable<TestItem> GetItems()
        {
            return _data;
        }

        public TestItem GetItemById(int id)
        {
            return _data.FirstOrDefault(x => x.Id == id);
        }

        public Task<TestItem> GetItemByIdAsync(int id)
        {
            return Task.FromResult(_data.FirstOrDefault(x => x.Id == id));
        }
    }
}
