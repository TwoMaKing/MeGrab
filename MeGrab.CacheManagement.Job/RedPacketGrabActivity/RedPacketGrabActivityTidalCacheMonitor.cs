using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeGrab.Cache.Managements
{
    public class RedPacketGrabActivityTidalCacheMonitor : ITidalCacheMonitor
    {
        public int Interval
        {
            get 
            {
                return 1200;
            }
        }

        public string[] GetCacheKeys() 
        {
            return new string[] { "MeGrab_RedPacketGrabActivity_Query_By_StartDateTime",
                                  "MeGrab_RedPacketGrabActivity_Paging_By_NA_PageNo:1_PageSize:10",
                                  "MeGrab_RedPacketGrabActivity_Paging_By_StartDateTime_PageNo:1_PageSize:10"};
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Run(IModelCacheAction[] actions)
        {
            
        }

    }
}
