using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeGrab.Cache.Managements
{
    public interface ITidalCacheMonitor
    {        
        int Interval { get; }
        
        void Reset();

        void Run(IModelCacheAction[] actions);
    }
}
