using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using Eagle.Core;
using Eagle.Core.Application;
using Eagle.Core.Log;
using Eagle.Domain.Repositories;
using MeGrab.Domain;
using MeGrab.Domain.Models;
using MeGrab.Domain.Repositories;
using MeGrab.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace MeGrab.RedPacketActivity.Storage
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static IConnectionFactory activeMQConnectionfactory;
        private static IConnection redPacketGrabActivityMQConnection;
        private static ISession redPacketGrabActivityMQsession;
        private static IMessageConsumer redPacketGrabActivityConsumer;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AppRuntime.Instance.Create();

            InitializeRedPacketGrabActivityMQConsumer();
        }

        private static void InitializeRedPacketGrabActivityMQConsumer()
        {
            string brokerUri = ConfigurationManager.AppSettings["RedPacketGrabActivityStoringMQ"];

            //创建连接工厂
            activeMQConnectionfactory = new ConnectionFactory(brokerUri);
            //通过工厂构建连接
            redPacketGrabActivityMQConnection = activeMQConnectionfactory.CreateConnection();
            //这个是连接的客户端名称标识
            redPacketGrabActivityMQConnection.ClientId = "MQ.StoringRedPacketActivity.ConnectionId";
            //启动连接，监听的话要主动启动连接
            redPacketGrabActivityMQConnection.Start();
            //通过连接创建一个会话
            redPacketGrabActivityMQsession = redPacketGrabActivityMQConnection.CreateSession();
            //通过会话创建一个消费者，这里就是Queue这种会话类型的监听参数设置
            redPacketGrabActivityConsumer = redPacketGrabActivityMQsession.CreateConsumer(new ActiveMQQueue("MQ.StoringRedPacketActivity"));
            //注册监听事件
            redPacketGrabActivityConsumer.Listener += new MessageListener(StoreRedPacketGrabActivity); 
        }

        private static void StoreRedPacketGrabActivity(IMessage message)
        {
            Task redPacketGrabActivityAddTask = Task.Factory.StartNew(() =>
            {
                RedPacketGrabActivity redPacketGrabActivity = (RedPacketGrabActivity)((ActiveMQObjectMessage)message).Body;

                using (IRepositoryContext repositoryContext = ServiceLocator.Instance.GetService<IRepositoryContext>())
                {
                    IRedPacketGrabActivityRepository repository = (IRedPacketGrabActivityRepository)
                                                                  repositoryContext.GetRepository<RedPacketGrabActivity, Guid>();

                    repository.Add(redPacketGrabActivity);
                    
                    try
                    {
                        repositoryContext.Commit();
                        LoggerContext.CurrentLogger.Info("保存了一个红包活动: Id:" + redPacketGrabActivity.Id + 
                                                         "金额:" + redPacketGrabActivity.TotalAmount + 
                                                         "个数:" + redPacketGrabActivity.RedPacketCount + 
                                                         "开始日期时间:" + redPacketGrabActivity.StartDateTime.ToString());
                    }
                    catch(Exception ex)
                    {
                        LoggerContext.CurrentLogger.Error("保存发布的红包活动错误: ", ex);
                    }
                }
           });

           redPacketGrabActivityAddTask.Wait();
        }
    }
}