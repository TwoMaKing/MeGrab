using Eagle.Domain.Repositories;
using Eagle.Repositories.Lite;
using MeGrab.Domain.Models;
using MeGrab.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeGrab.Domain.Repositories.Lite
{
    public class RedPacketGrabActivityRepository : LiteRepository<RedPacketGrabActivity, Guid>, IRedPacketGrabActivityRepository
    {
        public RedPacketGrabActivityRepository(IRepositoryContext repositoryContext)
            : base(repositoryContext) 
        { 
        }
    }
}
