using Eagle.Core;
using Eagle.Core.SqlQueries;
using Eagle.Core.SqlQueries.Criterias;
using Eagle.Core.SqlQueries.DialectProvider;
using Eagle.Domain;
using Eagle.Domain.Application;
using Eagle.Domain.Repositories;
using Eagle.Web.Caches;
using EmitMapper;
using MeGrab.DataObjects;
using MeGrab.Domain.Models;
using MeGrab.Domain.Repositories;
using MeGrab.Infrastructure;
using MeGrab.ServiceContracts;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeGrab.Application
{
    public class RedPacketDispatchServiceImpl : ApplicationService, IRedPacketDispatchService
    {
        private IMeGrabUserSqlRepository userRepository;

        public RedPacketDispatchServiceImpl(IRepositoryContext repositoryContext, 
                                            IMeGrabUserSqlRepository userRepository) 
            : base(repositoryContext) 
        {
            this.userRepository = userRepository;
        }

        public void Dispatch(DispatchRequest dispatchRequest)
        {
            RedPacketGrabActivityDataObject redPacketGrabActivityDataObject = dispatchRequest.RedPacketGrabActivity;
            RedPacketGrabActivity redPacketGrabActivity = redPacketGrabActivityDataObject.MapTo();

            ISqlCriteriaExpression expression = SqlQueryDialectProviderFactory.Default.SqlCriteriaExpression();
            expression.Equals("Name", dispatchRequest.DispatcherName);
            MeGrabUser currentDispatcher = userRepository.Find(expression);

            redPacketGrabActivity.Dispatch(currentDispatcher);
        }

    }
}
