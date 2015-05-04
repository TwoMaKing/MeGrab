using Eagle.Common.Web;
using Eagle.Core;
using Eagle.Core.Query;
using Eagle.Core.SqlQueries;
using Eagle.Core.SqlQueries.Criterias;
using Eagle.Domain;
using Eagle.Domain.Repositories;
using MeGrab.Domain.Models;
using MeGrab.Domain.Repositories;
using MeGrab.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeGrab.Cache.Managements.RedPacketGrabActivity
{
    public class RedPacketActivityQueryCacheAction: IModelCacheAction
    {
        private IRepositoryContext repositoryContext;
        private IRedPacketGrabActivitySqlRepository redPacketGrabActivityRepository;

        public RedPacketActivityQueryCacheAction(IRepositoryContext repositoryContext,
                                                 IRedPacketGrabActivitySqlRepository redPacketGrabActivityRepository)
        {
            this.repositoryContext = repositoryContext;
            this.redPacketGrabActivityRepository = redPacketGrabActivityRepository;
        }

        public ISqlCriteriaExpression GetQueryCriteria(string cacheKey)
        {
          
            ISqlCriteriaExpression expr = null;

            return expr;
        }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public void Run(string cacheKey)
        {

        }

        protected IEnumerable<KeyValuePair<string, object>> GetQueryCriteriaParameters(string cacheKey)
        {
            return null;
        }

    }
}
