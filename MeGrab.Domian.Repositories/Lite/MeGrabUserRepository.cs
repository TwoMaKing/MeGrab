﻿using Eagle.Common.Lambda;
using Eagle.Core.QuerySepcifications;
using Eagle.Domain.Repositories;
using Eagle.Repositories.Lite;
using Eagle.Web.Security;
using MeGrab.Domain.Models;
using MeGrab.Domain.Repositories;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MeGrab.Domain.Repositories.Lite
{
    public class MeGrabUserRepository : LiteRepository<MeGrabUser>, IMeGrabUserRepository
    {
        public MeGrabUserRepository(IRepositoryContext repositoryContext) : base(repositoryContext) 
        { }

        protected override MeGrabUser DoFindByKey(int id)
        {
            using (IDbConnection connection = this.LiteRepositoryContext.LiteConnectionFactory.CreateDbConnection())
            {
                var expression = connection.From<User>().Join<Membership>((u, m) => u.Id == m.UserId).Where(u => u.Id == id);

                return connection.Single<MeGrabUser>(expression);
            }
        }

        protected override MeGrabUser DoFind(ISpecification<MeGrabUser> specification)
        {
            if (specification == null)
            {
                throw new ArgumentNullException("The specification for query cannot be null.");
            }

            using (IDbConnection dbConnection = this.LiteRepositoryContext.LiteConnectionFactory.OpenDbConnection())
            {
                MeGrabUser user = dbConnection.Single<MeGrabUser>(specification.GetExpression());

                dbConnection.LoadReferences<MeGrabUser>(user);

                return user;
                //Expression<Func<MeGrabUser, bool>> expression = specification.GetExpression();

                //string field = "Name"; //LambdaUtil.GetMemberName(expression); 
                //object value = "Philips"; //LambdaUtil.GetValue(expression);
                
                //connection.Open();

                //User user = connection.Single<User>("SELECT * FROM WEBAPP_USERS WHERE " + field + " = @field", new { field = value });
                //Membership membership = connection.Single<Membership>("SELECT * FROM WEBAPP_MEMBERSHIP WHERE USERID = @id", new { id = user.Id });

                //MeGrabUser registeredUser = new MeGrabUser();
                //registeredUser.Id = user.Id;
                //registeredUser.Name = user.Name;
                //registeredUser.Email = membership.Email;
                //registeredUser.CellPhoneNo = membership.CellPhoneNo;

                //if (connection.State != ConnectionState.Closed)
                //{
                //    connection.Close();
                //}

                //return registeredUser;
            }
        }
    }
}
