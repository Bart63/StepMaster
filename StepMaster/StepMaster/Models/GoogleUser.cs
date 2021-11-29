using System;
using System.Collections.Generic;
using System.Text;

namespace StepMaster.Models
{
    public class GoogleUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Uri Picture { get; set; }

        public string IDToken { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
