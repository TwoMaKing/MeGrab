using Eagle.Domain.Application;
using MeGrab.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace MeGrab.ServiceContracts
{
    [ServiceContract()]
    public interface IMeGrabUserService : IApplicationServiceContract
    {
        [OperationContract()]
        MeGrabUserDataObject GetRegisteredUserById(int id);

        [OperationContract()]
        MeGrabUserDataObject GetRegisteredUserByName(string name);
    }
}
