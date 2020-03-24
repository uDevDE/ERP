using ERP.Client.Mapper;
using ERP.Client.Model;
using ERP.Contracts.Contract;
using ERP.Contracts.Domain;
using ERP.Contracts.Domain.Core.Enums;
using System;
using System.ServiceModel;

namespace ERP.Client
{
    public class NRServiceCallback : INRServiceCallback
    {
        public delegate void AuthorizedHandler(DeviceModel device);
        public event AuthorizedHandler AuthorizedEvent;

        public delegate void AuthorizedFailedHandler(AuthorisationType authorisationType);
        public event AuthorizedFailedHandler AuthorizedFailedEvent;

        public delegate void ServiceMessageHandler(string message);
        public event ServiceMessageHandler ServiceMessageEvent;

        public delegate void EmployeeUpdatedHandler(EmployeeModel employee);
        public event EmployeeUpdatedHandler EmployeeUpdatedEvent;

        public void Authorized(DeviceDTO device)
        {
            var model = AutoMapperConfiguration.Mapper.Map<DeviceModel>(device);
            AuthorizedEvent?.Invoke(model);
        }

        public void AuthorisationFailed(AuthorisationType authorisationType)
        {
            AuthorizedFailedEvent?.Invoke(authorisationType);
        }

        public void ServiceError(ServiceExceptionDTO ex)
        {
            throw new NotImplementedException();
        }

        public void ServiceMessage(string message)
        {
            ServiceMessageEvent?.Invoke(message);
        }

        public void EmployeeUpdated(EmployeeDTO employee)
        {
            var model = AutoMapperConfiguration.Mapper.Map<EmployeeModel>(employee);
            EmployeeUpdatedEvent?.Invoke(model);
        }
    }
}
