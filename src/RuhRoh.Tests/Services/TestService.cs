using System.Collections.Generic;

namespace RuhRoh.Tests.Services
{
    public class TestService : ITestServiceContract
    {
        public IEnumerable<int> GetIdList()
        {
            return new [] { 1, 2, 3, 4 };
        }
    }
}
