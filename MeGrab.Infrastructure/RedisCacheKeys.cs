using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeGrab.Infrastructure
{
    public sealed class RedisCacheKeys
    {
        public static readonly string RedisKey_Intraday_RedPackets = "Intraday_RedPackets";

        public static readonly string RedisKey_RedPackets_ByStartDateTime = "RedPackets_ByStartDateTime";

        public static readonly string RedisKey_RedPackets_ByExpireDateTime = "RedPackets_ByExpireDateTime";
    }
}
