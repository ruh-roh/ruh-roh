using System.Collections.Generic;

namespace RuhRoh.Tests.Services
{
    public interface ITestServiceContract
    {
        IEnumerable<int> GetIdList();
    }
}
