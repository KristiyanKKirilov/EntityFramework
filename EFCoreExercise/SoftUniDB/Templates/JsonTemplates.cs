using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftUni.Templates
{
    public static class JsonTemplates
    {
        public static object StudentTemplate = new 
        {
            FirstName = string.Empty,
            LastName = string.Empty,    
            Grade = 0
        };

    }
}
