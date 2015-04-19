using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using Eagle.Core;
using Eagle.Core.Application;
using Eagle.Domain.Repositories;
using MeGrab.Domain;
using MeGrab.Domain.Models;
using MeGrab.Domain.Repositories;
using MeGrab.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace MeGrab.RedPacketActivity.Storage
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AppRuntime.Instance.Create();

            this.InitializeRedPacketGrabActivityMQConsumer();
        }

        public void InitializeRedPacketGrabActivityMQConsumer()
        {
            string brokerUri = ConfigurationManager.AppSettings["RedPacketGrabActivityStoringMQ"];

            //创建连接工厂
            IConnectionFactory connectionfactory = new ConnectionFactory(brokerUri);
            //通过工厂构建连接
            IConnection connection = connectionfactory.CreateConnection();
            //这个是连接的客户端名称标识
            connection.ClientId = "MQ.StoringRedPacketActivity.ConnectionId";
            //启动连接，监听的话要主动启动连接
            connection.Start();
            //通过连接创建一个会话
            ISession session = connection.CreateSession();
            //通过会话创建一个消费者，这里就是Queue这种会话类型的监听参数设置
            IMessageConsumer consumer = session.CreateConsumer(new ActiveMQQueue("MQ.StoringRedPacketActivity"));
            //注册监听事件
            consumer.Listener += new MessageListener(StoreRedPacketGrabActivity); 
        }

        private void StoreRedPacketGrabActivity(IMessage message)
        {
            RedPacketGrabActivity redPacketGrabActivity = message.ToObject<RedPacketGrabActivity>();

            using (IRepositoryContext repositoryContext = ServiceLocator.Instance.GetService<IRepositoryContext>())
            {
                IRedPacketGrabActivityRepository repository = (IRedPacketGrabActivityRepository)
                                                               repositoryContext.GetRepository<RedPacketGrabActivity, Guid>();

                repository.Add(redPacketGrabActivity);

                repositoryContext.Commit();
            }
        }
    }
}