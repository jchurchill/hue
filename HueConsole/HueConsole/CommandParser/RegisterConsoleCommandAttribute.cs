using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueConsole.CommandParser
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    class RegisterConsoleCommandAttribute : Attribute
    {
        public RegisterConsoleCommandAttribute()
        {
        }
    }
}
