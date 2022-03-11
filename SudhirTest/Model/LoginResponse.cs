using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Model
{
    public class LoginResponse
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public LoginResult Result { get; set; }


    }
}
