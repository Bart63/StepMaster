using StepMaster.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StepMaster.Services
{
    public interface IGoogleManager
    {
        void Login(Action<GoogleUser, string> OnLoginComplete);
        void Logout();
    }
}

