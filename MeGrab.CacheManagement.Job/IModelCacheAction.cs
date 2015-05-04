using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeGrab.Cache.Managements
{
    public interface IModelCacheAction
    {
        

        void Run(string cacheKey);
    }
}
