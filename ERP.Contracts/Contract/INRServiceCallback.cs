using ERP.Contracts.Domain;
using ERP.Contracts.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace ERP.Contracts.Contract
{
    public interface INRServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void Authorized(DeviceDTO device);

        [OperationContract(IsOneWay = true)]
        void AuthorisationFailed(AuthorisationType authorisationType);

        [OperationContract(IsOneWay = true)]
        void ServiceError(ServiceExceptionDTO ex);

        [OperationContract(IsOneWay = true)]
        void ServiceMessage(string message);

        [OperationContract(IsOneWay = true)]
        void EmployeeUpdated(EmployeeDTO employee);
    }
}
