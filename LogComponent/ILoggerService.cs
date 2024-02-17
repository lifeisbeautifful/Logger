using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogComponent
{
    public interface ILoggerService
    {
        void AddLogLines(string message, int count);
    }
    

    
}
