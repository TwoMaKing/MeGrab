using Eagle.Domain.Application;
using MeGrab.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MeGrab.ServiceContracts
{
    /// <summary>
    /// 派发红包
    /// </summary>
    [ServiceContract]
    public interface IRedPacketDispatchService : IApplicationServiceContract
    {
        /// <summary>
        /// 派发红包, 将小红包放入到未领取红包的消息队列. 同时用异步的方式把派发的大红包信息存到数据库.
        /// 同时也用异步的方式通过Socket API/SignalR/Html5 web socket 等通信技术让群里的其他成员都能看到红包并领取.
        /// </summary>
        [OperationContract()]
        void Dispatch(DispatchRequest dispatchRequest);

        [OperationContract()]
        DispatchResponse GetDispatchedRedPacketGrabActivity();
    }

}
