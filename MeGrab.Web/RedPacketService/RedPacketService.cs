using Eagle.Core;
using MeGrab.ServiceContracts;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MeGrab.Web
{

    [HubName("RedPacketService")]
    public class RedPacketService : Hub
    {
        private static List<string> connectedUsers = new List<string>();
        private static readonly object lockObj = new object();

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public void Connect()
        {
            string connectionId = this.Context.ConnectionId;

            lock (lockObj)
            {
                if (!connectedUsers.Contains(connectionId))
                {
                    connectedUsers.Add(connectionId);
                }
            }
        }

        public void DisplayNewRedPacketActivity(DateTime displayedLast)
        {
            using (IRedPacketGrabActivityQueryService redPacketQueryService = ServiceLocator.Instance.GetService<IRedPacketGrabActivityQueryService>())
            {

            }

            this.Clients.Clients(connectedUsers).redPacketActivityDispatched();
        }
    }

    //[HubName("RedPacketServer")]
    //public class RedPacketService : Hub, IRedPacketService
    //{
    //    private static List<string> connectedUsers = new List<string>();

    //    private static Dictionary<string, List<LargeRedPacketDataObject>> redPacketWidgetsByChatGroup =
    //        new Dictionary<string, List<LargeRedPacketDataObject>>();

    //    private readonly static object lockObj = new object();

    //    public override Task OnConnected()
    //    {
    //        return base.OnConnected();
    //    }

    //    public void Connect()
    //    {
    //        string connectionId = this.Context.ConnectionId;

    //        lock (lockObj)
    //        {
    //            if (!connectedUsers.Contains(connectionId))
    //            {
    //                connectedUsers.Add(connectionId);
    //            }
    //        }
    //    }

    //    public void Dispatch(DispatchRequest dispatchRequest)
    //    {
    //        ChatGroupDataObject chatGroupDTO = dispatchRequest.ChatGroup;

    //        LargeRedPacketDataObject redPacketWidgetDTO = dispatchRequest.RedPacketWidget;

    //        RedPacketWidget redPackageWidgetModel = new RedPacketWidget();
    //        redPackageWidgetModel.Id = redPacketWidgetDTO.Id;
    //        redPackageWidgetModel.RedPacketCount = redPacketWidgetDTO.RedPacketCount;
    //        redPackageWidgetModel.TotalAmount = redPacketWidgetDTO.TotalAmount;
    //        redPackageWidgetModel.Mode = redPacketWidgetDTO.Mode;
    //        redPackageWidgetModel.Message = redPacketWidgetDTO.Message;
    //        redPackageWidgetModel.DispatchDateTime = redPacketWidgetDTO.DispatchDateTime;
    //        redPackageWidgetModel.Publisher = new User();
    //        redPackageWidgetModel.Publisher.Id = redPacketWidgetDTO.Publisher.Id;
    //        redPackageWidgetModel.Publisher.Name = redPacketWidgetDTO.Publisher.Name;

    //        //领域模型业务逻辑，通过红包个数，总金额和派发方式来生成红包. 这个过程可在前端执行以减少服务器的计算压力
    //        //或者在调用服务之前执行
    //        redPackageWidgetModel.GenerateRedPackets();

    //        // 您可以在此添加代码把 RedPacketWidget 的聚合保存到数据库中用 Repository/UnitOfWork 模式. 
    //        // Using(IRepositoryContext ctx = ServiceLocator.Instance.GetService<IRepositoryContext>())
    //        // {
    //        //      ctx.Add(redPackageWidget);
    //        //      ctx.Commit();
    //        // }
    //        // 当多用户高并发的时候可使用消息队列 i.e. MQBus 进行处理.
    //        // Using(IMQBus bus = ServiceLocator.Instance.GetService<IMQBus>())
    //        // {
    //        //      bus.Publish(redPackageWidget);
    //        //      ctx.Commit();
    //        // }

    //        LargeRedPacketDataObject clientRedPacketWidget = new LargeRedPacketDataObject();
    //        clientRedPacketWidget.Id = redPackageWidgetModel.Id;
    //        clientRedPacketWidget.Message = redPackageWidgetModel.Message;
    //        clientRedPacketWidget.RedPacketCount = redPackageWidgetModel.RedPacketCount;
    //        clientRedPacketWidget.TotalAmount = redPackageWidgetModel.TotalAmount;
    //        redPackageWidgetModel.DispatchDateTime = redPacketWidgetDTO.DispatchDateTime;
    //        clientRedPacketWidget.Publisher = new UserDataObject();
    //        clientRedPacketWidget.Publisher.Id = redPackageWidgetModel.Publisher.Id;
    //        clientRedPacketWidget.Publisher.Name = redPackageWidgetModel.Publisher.Name;

    //        List<RedPacketDataObject> clientRedPacketList = new List<RedPacketDataObject>();
    //        foreach(var r in redPackageWidgetModel.RedPackets )
    //        { 
    //            clientRedPacketList.Add(new RedPacketDataObject()
    //                                {
    //                                    Id = r.Id,
    //                                    Amount = r.Amount
    //                                });
    //        }

    //        clientRedPacketWidget.RedPackets = clientRedPacketList;

    //        // 把当前聊天群和聊天群的红包存到红包缓存中去
    //        lock (lockObj)
    //        {
    //            if (!redPacketWidgetsByChatGroup.ContainsKey(chatGroupDTO.Id))
    //            {
    //                List<LargeRedPacketDataObject> redPacketWidgetDTOList = new List<LargeRedPacketDataObject>();
    //                redPacketWidgetDTOList.Add(redPacketWidgetDTO);
    //                redPacketWidgetsByChatGroup.Add(chatGroupDTO.Id, redPacketWidgetDTOList);
    //            }
    //            else
    //            {
    //                var redPacketWidgetDTOList = redPacketWidgetsByChatGroup[chatGroupDTO.Id];
    //                if (redPacketWidgetDTOList == null)
    //                {
    //                    redPacketWidgetDTOList = new List<LargeRedPacketDataObject>();
    //                }
    //                redPacketWidgetDTOList.Add(redPacketWidgetDTO);
    //            }
    //        }

    //        // 派发红包给所有在聊天室里面的客户端
    //        this.Clients.Clients(connectedUsers).redPacketDispatched(clientRedPacketWidget);
    //    }

    //    /// <summary>
    //    /// 领取红包，然后返回 红包物件 的数据传输对象.
    //    /// </summary>
    //    /// <param name="user"></param>
    //    /// <returns></returns>
    //    public RedPacketSnatchResponse Receive(RedPacketSnatchRequest receiveRequest)
    //    {
    //        RedPacketSnatchResponse receiveResponse = new RedPacketSnatchResponse();

    //        string connectionId = this.Context.ConnectionId;
    //        string userId = receiveRequest.Snatcher.Id;
    //        string groupId = receiveRequest.ChatGroup.Id.ToString();

    //        if (redPacketWidgetsByChatGroup.ContainsKey(groupId))
    //        {
    //            List<LargeRedPacketDataObject> redPacketWidgetDTOList = redPacketWidgetsByChatGroup[groupId];

    //            LargeRedPacketDataObject redPacketWidgetDTO =
    //                redPacketWidgetDTOList.SingleOrDefault(r => r.Id.Equals(receiveRequest.LargeRedPacketId));

    //            if (redPacketWidgetDTO != null)
    //            {
    //                IEnumerable<RedPacketDataObject> remainingRedPacketDTOList = redPacketWidgetDTO.RedPackets.Where(r => !r.Received);

    //                if (remainingRedPacketDTOList != null &&
    //                    remainingRedPacketDTOList.Count() > 0)
    //                {
    //                    Random random = new Random();
    //                    int redPacketIndex = random.Next(remainingRedPacketDTOList.Count());
    //                    RedPacketDataObject redPacket = remainingRedPacketDTOList.ElementAt(redPacketIndex);
    //                    redPacket.ReceiverId = userId;
    //                    redPacket.ReceivedDate = DateTime.Now; //可以用Utc
    //                    receiveResponse.ReceivedAmount = redPacket.Amount;

    //                    //可通过User聚合对应的Repository的GetAccountByUserId方法来获取其对应的账户聚合对象,这里简单处理
    //                    Account accountByUser = GetAccountByUserId(userId);
    //                    accountByUser.TransferIn(redPacket.Amount);
    //                }

    //                // 将红包Widget状态转发给给所有在聊天室里面的客户端
    //                receiveResponse.LargeRedPacket = redPacketWidgetDTO;
    //                this.Clients.Clients(connectedUsers).redPacketReceived(receiveResponse);
    //            }
    //        }

    //        return receiveResponse;
    //    }

    //    private Account GetAccountByUserId(string userId)
    //    {
    //        Account account = new Account();
    //        account.Id = Guid.NewGuid().ToString();
    //        account.Amount = 1000;
    //        account.User = new User();
    //        account.User.Id = userId;

    //        return account;
    //    }
    //}
}