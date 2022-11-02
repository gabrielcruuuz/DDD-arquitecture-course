using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Api.IntegrationTest
{
    public class TestLogin : BaseIntegration
    {
        [Fact]
        public async Task TestDoToken()
        {
            await AddToken();
        }
    }
}
