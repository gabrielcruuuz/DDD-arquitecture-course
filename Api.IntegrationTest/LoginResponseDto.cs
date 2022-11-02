using System;
using System.Collections.Generic;
using System.Text;

namespace Api.IntegrationTest
{
    public class LoginResponseDto
    {
        public bool authenticated { get; set; }
        public string message { get; set; }
        public DateTime creted { get; set; }
        public DateTime expiration { get; set; }
        public string acessToken { get; set; }
        public string email { get; set; }
        public string name { get; set; }
    }
}
