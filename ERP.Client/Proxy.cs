﻿using ERP.Contracts.Contract;
using ERP.Contracts.Domain;
using ERP.Client.Mapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using ERP.Client.Model;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using ERP.Client.ViewModel;

// This channel can no longer be used to send messages as the output session was auto-closed due to a server-initiated shutdown. Either disable auto-close by setting the DispatchRuntime.AutomaticInputSessionShutdown to false, or consider modifying the shutdown protocol with the remote server.

namespace ERP.Client
{
    public static class Proxy
    {
        private static INRService _proxy;
        private static DuplexChannelFactory<INRService> _channel;
        public static NRServiceCallback ServiceCallback { get; set; }
        private static bool _connectionClosed = false;

        public static Guid DeviceId { get; set; }
        public static DeviceModel Device { get; set; }

        /// <summary>
        /// Configures NetTcpBinding for NRService
        /// </summary>
        /// <returns></returns>
        private static NetTcpBinding ConfigureNetTcpBinding()
        {
            var size = int.MaxValue;

            var binding = new NetTcpBinding()
            {
                CloseTimeout = TimeSpan.MaxValue,
                OpenTimeout = TimeSpan.MaxValue,
                ReceiveTimeout = TimeSpan.MaxValue,
                SendTimeout = TimeSpan.MaxValue,
                TransferMode = TransferMode.Buffered,
                MaxReceivedMessageSize = size
            };
            binding.ReaderQuotas.MaxDepth = 32;
            binding.ReaderQuotas.MaxStringContentLength = size;
            binding.ReaderQuotas.MaxArrayLength = size;
            binding.ReaderQuotas.MaxBytesPerRead = size;
            binding.ReaderQuotas.MaxNameTableCharCount = size;

            binding.Security.Mode = SecurityMode.None;
            return binding;
        }

        public static bool IsConnected
        {
            get
            {
                if (_proxy != null && _channel != null && _channel.State == CommunicationState.Opened && !_connectionClosed)
                    return true;

                return false;
            }
        }

        public static bool IsDeviceIdValid
        {
            get { return DeviceId != null && DeviceId != Guid.Empty; }
        }

        private static List<TDest> MapList<TDest, TSource>(List<TSource> src) => AutoMapperConfiguration.Mapper.Map<List<TSource>, List<TDest>>(src);

        public static Task InitializeAsync()
        {
            return Task.Factory.StartNew(() =>
            {
#if DEBUG
                var uri = "net.tcp://localhost:6565/NRService";
#else
                //var uri = "net.tcp://167.86.73.240:6565/NRService";
                var uri = "net.tcp://localhost:6565/NRService";
#endif
                ServiceCallback = new NRServiceCallback();
                var callback = new InstanceContext(ServiceCallback);
                var binding = ConfigureNetTcpBinding();
                _channel = new DuplexChannelFactory<INRService>(callback, binding);
                var endPoint = new EndpointAddress(uri);
                _proxy = _channel.CreateChannel(endPoint);
                _channel.Opened += Channel_Opened;
                _channel.Closed += Channel_Closed;
            });
        }

        private static void Channel_Closed(object sender, EventArgs e)
        {
            _connectionClosed = true;
        }

        private static void Channel_Opened(object sender, EventArgs e)
        {
            _connectionClosed = false;
        }

        public static bool Connect(Guid deviceId, string hostname, string username)
        {
            if (IsConnected)
            {
                _proxy.Connect(deviceId, hostname, username);
                return true;
            }

            return false;
        }

        public async static Task<List<DeviceModel>> GetAllDevices()
        {
            if (IsConnected && IsDeviceIdValid)
            {
                var list = await _proxy.GetAllDevicesAsync();
                var result = MapList<DeviceModel, DeviceDTO>(list);
                return await Task.FromResult(result);
            }

            return await Task.FromResult<List<DeviceModel>>(null);
        }

        public async static Task<List<EmployeeModel>> GetAllEmployees()
        {
            if (IsConnected && IsDeviceIdValid)
            {
                var list = await _proxy.GetAllEmployeesAsync();
                var result = MapList<EmployeeModel, EmployeeDTO>(list);
                return await Task.FromResult(result);
            }

            return await Task.FromResult<List<EmployeeModel>>(null);
        }

        public async static Task<List<DivisionInfoModel>> GetAllDivisionInfos()
        {
            if (IsConnected && IsDeviceIdValid)
            {
                var list = await _proxy.GetDivisionInfosAsync();
                var result = MapList<DivisionInfoModel, DivisionInfoDTO>(list);
                return await Task.FromResult(result);
            }

            return await Task.FromResult<List<DivisionInfoModel>>(null);
        }

        public async static Task<List<DivisionModel>> GetAllDivisions()
        {
            if (IsConnected && IsDeviceIdValid)
            {
                var list = await _proxy.GetDivisionsAsync();
                var result = MapList<DivisionModel, DivisionDTO>(list);
                return await Task.FromResult(result);
            }

            return await Task.FromResult<List<DivisionModel>>(null);
        }

        public async static Task<List<EmployeeModel>> GetAllEmployeesByDevice()
        {
            if (IsConnected && IsDeviceIdValid)
            {
                var list = await _proxy.GetEmployeesByDeviceAsync(DeviceId);
                var result = MapList<EmployeeModel, EmployeeDTO>(list);
                return await Task.FromResult(result);
            }

            return await Task.FromResult<List<EmployeeModel>>(null);
        }

        public static Task<int> UpsertEmployee(EmployeeModel employeeModel)
        {
            try
            {
                if (IsConnected && IsDeviceIdValid)
                {
                    var employee = AutoMapperConfiguration.Mapper.Map<EmployeeDTO>(employeeModel);
                    return _proxy.UpsertEmployeeAsync(employee);
                }

                return Task.FromResult(0);
            }
            catch (FaultException ex)
            {
                _ = ex;
                return Task.FromResult(0);
            }

        }

        public static Task<int> UpsertDivisionInfo(DivisionInfoModel divisionInfo)
        {
            try
            {
                if (IsConnected && IsDeviceIdValid)
                {
                    var result = AutoMapperConfiguration.Mapper.Map<DivisionInfoDTO>(divisionInfo);
                    return _proxy.UpsertDivisionInfoAsync(result);
                }

                return Task.FromResult(0);
            }
            catch (FaultException ex)
            {
                _ = ex;
                return Task.FromResult(0);
            }
        }

        public static Task<int> UpsertDivision(DivisionModel division)
        {
            try
            {
                if (IsConnected && IsDeviceIdValid)
                {
                    var result = AutoMapperConfiguration.Mapper.Map<DivisionDTO>(division);
                    _ = result;
                    return _proxy.UpsertDivisionAsync(result);
                }

                return Task.FromResult(0);
            }
            catch (FaultException ex)
            {
                _ = ex;
                return Task.FromResult(0);
            }
        }

        public async static Task<EmployeeModel> SwitchEmployee(int employeeId, string password, bool keepConnected)
        {
            if (IsConnected && IsDeviceIdValid)
            {
                var employee = await _proxy.SwitchEmployeeAsync(DeviceId, employeeId, password, keepConnected);
                var result = AutoMapperConfiguration.Mapper.Map<EmployeeModel>(employee);
                return await Task.FromResult(result);
            }

            return null;
        }

        public static Task<bool> ChangeEmployeeForegroundColor(int employeeId, string color)
        {
            if (IsConnected && IsDeviceIdValid)
            {
                return _proxy.ChangeEmployeeForegroundColor(employeeId, color);
            }

            return Task.FromResult(false);
        }

        public static Task<bool> ChangeEmployePassword(int employeeId, string password, string newPassword)
        {
            if (IsConnected && IsDeviceIdValid)
            {
                return _proxy.ChangeEmployeePassword(employeeId, password, newPassword);
            }

            return Task.FromResult(false);
        }

        public static bool UpdateEmployee(EmployeeModel employee)
        {
            if (IsConnected && IsDeviceIdValid)
            {
                var employeeDTO = AutoMapperConfiguration.Mapper.Map<EmployeeDTO>(employee);
                _proxy.UpdateEmployee(employeeDTO);
            }

            return false;
        }

        public static Task<bool> DeleteDivision(int divisionId)
        {
            if (IsConnected && IsDeviceIdValid)
            {
                return _proxy.DeleteDivisionAsync(divisionId);
            }

            return Task.FromResult(false);
        }

        public async static Task<Dictionary<string, FolderModel>> GetAllProjects()
        {
            if (IsConnected && IsDeviceIdValid)
            {
                var folders = await _proxy.GetProjectsAsync();
                _ = folders;
                var result = AutoMapperConfiguration.Mapper.Map<Dictionary<string, FolderDTO>, Dictionary<string, FolderModel>>(folders);
                return await Task.FromResult(result);
            }

            return await Task.FromResult<Dictionary<string, FolderModel>>(null);
        }

        public static Task<PdfFile> GetPdfFile(string filePath)
        {
            if (IsConnected && IsDeviceIdValid)
            {
                return _proxy.GetPdfFile(filePath);
            }

            return Task.FromResult<PdfFile>(null);
        }

        public async static Task<List<PlantOrderModel>> GetPlantOrders(int plantOrderNumber)
        {
            if (IsConnected && IsDeviceIdValid)
            {
                var list = await _proxy.GetPlantOrders(plantOrderNumber);
                var result = AutoMapperConfiguration.Mapper.Map<List<PlantOrderDTO>, List<PlantOrderModel>>(list);
                return await Task.FromResult(result);
            }

            return await Task.FromResult<List<PlantOrderModel>>(null);
        }

        public async static Task<List<MaterialRequirementModel>> GetMaterialRequirements(string[] materialRequirements)
        {
            if (IsConnected && IsDeviceIdValid)
            {
                var list = await _proxy.GetMaterialRequirements(materialRequirements);
                var result = AutoMapperConfiguration.Mapper.Map<List<MaterialRequirementDTO>, List<MaterialRequirementModel>>(list);
                return await Task.FromResult(result);
            }

            return await Task.FromResult<List<MaterialRequirementModel>>(null);
        }

        public async static Task<List<PlantOrderProcessModel>> GetPlantOrderProcesses(int plantOrderId)
        {
            if (IsConnected && IsDeviceIdValid)
            {
                var list = await _proxy.GetPlantOrderProcesses(plantOrderId);
                var result = AutoMapperConfiguration.Mapper.Map<List<PlantOrderProcessDTO>, List<PlantOrderProcessModel>>(list);
                result.Reverse();
                return await Task.FromResult(result);
            }

            return await Task.FromResult<List<PlantOrderProcessModel>>(null);
        }

        public async static Task<List<ProcessTemplateModel>> GetProcessTemplates()
        {
            if (IsConnected && IsDeviceIdValid)
            {
                var list = await _proxy.GetProcessTemplates();
                var result = AutoMapperConfiguration.Mapper.Map<List<ProcessTemplateDTO>, List<ProcessTemplateModel>>(list);
                return await Task.FromResult(result);
            }

            return await Task.FromResult<List<ProcessTemplateModel>>(null);
        }

        public async static Task<List<ProjectNumberModel>> GetProjectNumbersAsync()
        {
            if (IsConnected && IsDeviceIdValid)
            {
                var list = await _proxy.GetProjectNumbersAsync();
                var result = AutoMapperConfiguration.Mapper.Map<List<ProjectNumberDTO>, List<ProjectNumberModel>>(list);
                return await Task.FromResult(result);
            }

            return await Task.FromResult<List<ProjectNumberModel>>(null);
        }

        public async static Task<List<ElementModel>> GetElements()
        {
            if (IsConnected && IsDeviceIdValid)
            {
                var list = await _proxy.GetElements();
                var result = AutoMapperConfiguration.Mapper.Map<List<ElementDTO>, List<ElementModel>>(list);
                return await Task.FromResult(result);
            }

            return await Task.FromResult<List<ElementModel>>(null);
        }

        public static Task<string> GetRemoteRootPath()
        {
            if (IsConnected && IsDeviceIdValid)
            {
                return _proxy.GetRemoteRootPath();
            }

            return Task.FromResult<string>(null);
        }

        public static Task<bool> Logout(int employeeId)
        {
            if (IsConnected && IsDeviceIdValid)
            {
                return _proxy.Logout(DeviceId, employeeId);
            }

            return Task.FromResult(false);
        }

        public static Task<int> CreateProfile(ProfileDTO profile)
        {
            if (IsConnected && IsDeviceIdValid && LocalClient.Employee != null && LocalClient.Division != null)
            {
                profile.DivisionId = LocalClient.Division.DivisionId;
                return _proxy.CreateProfileAsync(profile, LocalClient.Employee.EmployeeId);
            }

            return Task.FromResult(0);
        }

        public async static Task<List<ElementModel>> GetProfiles(int plantOrderId)
        {
            if (IsConnected && IsDeviceIdValid)
            {
                var profiles = await _proxy.GetProfilesAsync(plantOrderId);
                List<ElementModel> elements = new List<ElementModel>();
                foreach (var profile in profiles)
                {
                    elements.Add(AutoMapperConfiguration.Map(profile));
                }

                return await Task.FromResult(elements);
            }

            return await Task.FromResult<List<ElementModel>>(null);
        }

        public async static Task<List<ElementModel>> GetElementProfiles(int plantOrderId, int divisionId)
        {
            if (IsConnected && IsDeviceIdValid)
            {
                var profiles = await _proxy.GetElementProfilesAsync(plantOrderId, divisionId);
                if (profiles != null)
                {
                    var elements = new List<ElementModel>();
                    foreach (var profile in profiles)
                    {
                        elements.Add(AutoMapperConfiguration.Map(profile));
                    }

                    return await Task.FromResult(elements);
                }
            }

            return await Task.FromResult<List<ElementModel>>(null);
        }

        public async static Task<bool> UpdateProfileAsync(ProfileDTO profile)
        {
            if (IsConnected && IsDeviceIdValid && LocalClient.Employee != null)
            {
                return await _proxy.UpdateProfileAsync(LocalClient.Employee.EmployeeId, profile);
            }

            return await Task.FromResult(false);
        }

        public static Task<bool> DeleteProfileAsync(int profileId)
        {
            if (IsConnected && IsDeviceIdValid)
            {
                return _proxy.DeleteProfileAsync(profileId);
            }

            return Task.FromResult(false);
        }

        public async static Task<int> UpsertElementFilter(ElementFilterModel filter)
        {
            if (IsConnected && IsDeviceIdValid)
            {
                var filterDTO = AutoMapperConfiguration.Mapper.Map<ElementFilterDTO>(filter);
                return await _proxy.UpsertFilterAsync(filterDTO);
            }

            return await Task.FromResult(0);
        }

        public async static Task<List<ElementFilterModel>> GetFiltersAsync(int plantOrderId, int employeeId)
        {
            if (IsConnected && IsDeviceIdValid)
            {
                var list = await _proxy.GetFiltersAsync(plantOrderId, employeeId);
                var filters = AutoMapperConfiguration.Mapper.Map<List<ElementFilterDTO>, List<ElementFilterModel>>(list);
                return await Task.FromResult(filters);
            }

            return await Task.FromResult<List<ElementFilterModel>>(null);
        }

        public static Task<double> UpdateProfileAmount(int profileId, double amount)
        {
            if (IsConnected && IsDeviceIdValid && LocalClient.Employee != null && profileId > 0)
            {
                return _proxy.UpdateProfileAmountAsync(LocalClient.Employee.EmployeeId, profileId, amount);
            }

            return Task.FromResult<double>(-1);
        }

    }
}
