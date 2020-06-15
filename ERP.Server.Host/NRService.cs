using ERP.Contracts.Contract;
using ERP.Contracts.Domain;
using ERP.Contracts.Domain.Core.Enums;
using ERP.Server.Entities.Entity;
using ERP.Server.Entities.Context;
using ERP.Server.Host.Mapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using ERP.Server.Host.Core;
using Newtonsoft.Json;

namespace ERP.Server.Host
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class NRService : INRService
    {
        private INRServiceCallback _callback = null;
        private ObservableCollection<DeviceDTO> _users;
        private Dictionary<Guid, INRServiceCallback> _clients;

        private static string _projectBasePath;
        private static List<string> _projectPaths;
        private static string _clientRootPath;

        public NRService()
        {
            _users = new ObservableCollection<DeviceDTO>();
            _clients = new Dictionary<Guid, INRServiceCallback>();
        }

        public static void Configure()
        {
            _projectBasePath = Helpers.NRServiceHelper.GetProjectBasePath();
            _projectPaths = Helpers.NRServiceHelper.GetProjectPaths();
            _clientRootPath = Helpers.NRServiceHelper.GetProjectRootPath();
        }

        private static void PrintException(Exception ex)
        {
            if (ex is DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }

            if (ex.HResult != -2146233087)
            {
                Console.WriteLine($"[ERROR] - {ex.Message}");
            }

            var innerEx = ex.InnerException;
            while (innerEx != null && innerEx.HResult != -2146233087)
            {
                Console.WriteLine($"[ERROR] - {innerEx.Message}");
            }
        }

        public async void Connect(Guid deviceId, string hostname, string username)
        {
            try
            {
                _callback = OperationContext.Current.GetCallbackChannel<INRServiceCallback>();
                if (_callback != null)
                {
                    var deviceCtx = new EmployeeContext();
                    var device = deviceCtx.Devices.Where(x => x.DeviceId == deviceId).Include(d => d.Division).Include(e => e.Employee).FirstOrDefault();
                    if (device == null)
                    {
                        device = new Device()
                        {
                            Hostname = hostname,
                            DeviceId = deviceId,
                            IpAddress = "127.0.0.1",
                            Status = true,
                            Username = username
                        };

                        deviceCtx.Devices.Add(device);
                        var result = await deviceCtx.SaveChangesAsync();

                        if (result > 0)
                        {
                            _callback.AuthorisationFailed(AuthorisationType.AuthorizeWaitingForRelease);
                        }
                    }
                    else
                    {
                        if (device.IsBlocked)
                        {
                            _callback.AuthorisationFailed(AuthorisationType.AuthorizeBlocked);
                            return;
                        }

                        var deviceDTO = AutoMapperConfiguration.Mapper.Map<DeviceDTO>(device);
                        if (deviceDTO != null)
                        {
                            _callback.Authorized(deviceDTO);
                        }

                        _clients.Add(deviceId, _callback);
                        _users.Add(deviceDTO);
                        //_clients?.ToList().ForEach(c => c.Value.UserConnected(_users));
                        Console.WriteLine($"DeviceId: {deviceId.ToString()} - Hostname: {hostname} - Username: {username}");
                    }
                }
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("Connect"), "Connect");
            }
        }

        public Task<List<DeviceDTO>> GetAllDevicesAsync()
        {
            try
            {
                var deviceCtx = new EmployeeContext();
                var devices = deviceCtx.Devices.ToList();
                var result = AutoMapperConfiguration.Mapper.Map<List<Device>, List<DeviceDTO>>(devices);
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("GetAllDevices"), "GetAllDevicesAsync");
            }
        }

        public Task<List<EmployeeDTO>> GetAllEmployeesAsync()
        {
            try
            {
                var employeeCtx = new EmployeeContext();
                var employees = employeeCtx.Employees.ToList();
                var result = AutoMapperConfiguration.Mapper.Map<List<Employee>, List<EmployeeDTO>>(employees);
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("GetAllEmployees"), "GetAllEmployeesAsync");
            }
        }

        public Task<List<DivisionInfoDTO>> GetDivisionInfosAsync()
        {
            try
            {
                var context = new EmployeeContext();
                var divisionInfos = context.DivisionInfos.ToList();
                var result = AutoMapperConfiguration.Mapper.Map<List<DivisionInfo>, List<DivisionInfoDTO>>(divisionInfos);
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("GetDivisionInfosAsync"), "GetDivisionInfosAsync");
            }
        }

        public Task<List<DivisionDTO>> GetDivisionsAsync()
        {
            try
            {
                var context = new EmployeeContext();
                var divisions = context.Divisions.Include(x => x.DivisionType).ToList();
                foreach (var item in divisions)
                {
                    Console.WriteLine($"DivisionId: {item.DivisionId} - DivisionInfoId: {item.DivisionType.DivisionInfoId}");
                }
                var result = AutoMapperConfiguration.Mapper.Map<List<Division>, List<DivisionDTO>>(divisions);
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("GetDivisionsAsync"), "GetDivisionsAsync");
            }
        }

        public Task<List<EmployeeDTO>> GetEmployeesByDeviceAsync(Guid deviceId)
        {
            try
            {
                var callback = OperationContext.Current.GetCallbackChannel<INRServiceCallback>();
                if (callback != null)
                {
                    var employeeCtx = new EmployeeContext();
                    var employees = employeeCtx.Employees.Where(x => x.DeviceId == deviceId)?.ToList();
                    var result = AutoMapperConfiguration.Mapper.Map<List<Employee>, List<EmployeeDTO>>(employees);
                    return Task.FromResult(result);
                }
                return Task.FromResult(new List<EmployeeDTO>());
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("GetEmployeesByDevice"), "GetEmployeesByDeviceAsync");
            }
        }

        public async Task<int> UpsertEmployeeAsync(EmployeeDTO employee)
        {
            try
            {
                var context = new EmployeeContext();
                var entity = AutoMapperConfiguration.Mapper.Map<Employee>(employee);
                var result = context.Employees.Find(employee.EmployeeId);
                if (result != null)
                {
                    result.Firstname = entity.Firstname;
                    result.Lastname = entity.Lastname;
                    result.Description = entity.Description;
                    result.Alias = entity.Alias;
                    result.Password = Helpers.NRServiceHelper.Sha256(entity.Password);
                    result.DeviceId = entity.DeviceId;
                    result.Device = entity.Device;
                    result.Color = entity.Color;
                    context.Entry(result).State = EntityState.Modified;
                    context.Entry(result.Device).State = EntityState.Modified;

                    return await context.SaveChangesAsync();
                }
                else
                {
                    context.Employees.Attach(entity);
                    entity.Password = Helpers.NRServiceHelper.Sha256(entity.Password);
                    entity.IsAdministrator = false;
                    context.Employees.Add(entity);

                    var employeeId = await context.SaveChangesAsync();
                    if (employeeId > 0)
                    {
                        return await Task.FromResult(entity.EmployeeId);
                    }
                    else
                    {
                        return await Task.FromResult(0);
                    }
                }
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("UpsertEmployeeAsync"), "UpsertEmployeeAsync");
            }
        }

        public async Task<int> UpsertDivisionInfoAsync(DivisionInfoDTO divisionInfo)
        {
            try
            {
                var context = new EmployeeContext();
                var entity = AutoMapperConfiguration.Mapper.Map<DivisionInfo>(divisionInfo);
                var result = context.DivisionInfos.Find(divisionInfo.DivisionInfoId);
                if (result != null)
                {
                    result.Name = entity.Name;
                    result.Description = entity.Description;
                    result.DivisionType = entity.DivisionType;
                    context.Entry(result).State = EntityState.Modified;
                }
                else
                {
                    context.DivisionInfos.Add(entity);
                }

                var divisionInfoId = await context.SaveChangesAsync();
                if (divisionInfoId > 0)
                {
                    return await Task.FromResult(entity.DivisionInfoId);
                }
                else
                {
                    return await Task.FromResult(0);
                }
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("UpsertDivisionInfo"), "UpsertDivisionInfo");
            }
        }

        public async Task<int> UpsertDivisionAsync(DivisionDTO division)
        {
            try
            {
                var context = new EmployeeContext();
                var entity = AutoMapperConfiguration.Mapper.Map<Division>(division);
                var result = context.Divisions.Find(division.DivisionId);
                if (result != null)
                {
                    result.Description = entity.Description;
                    result.DivisionInfoId = entity.DivisionInfoId;
                    result.DivisionType = entity.DivisionType;
                    result.Name = entity.Name;
                    context.Entry(result).State = EntityState.Modified;
                    context.Entry(result.DivisionType).State = EntityState.Modified;
                    return await context.SaveChangesAsync();
                }
                else
                {
                    context.Divisions.Attach(entity);
                    context.Divisions.Add(entity);

                    var divisionId = await context.SaveChangesAsync();
                    if (divisionId > 0)
                    {
                        return await Task.FromResult(entity.DivisionId);
                    }
                    else
                    {
                        return await Task.FromResult(0);
                    }
                }
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("UpsertDivisionAsync"), "UpsertDivisionAsync");
            }
        }

        public async Task<EmployeeDTO> SwitchEmployeeAsync(Guid deviceId, int employeeId, string password, bool keepConnected)
        {
            try
            {
                var context = new EmployeeContext();
                var device = context.Devices.Find(deviceId);
                if (device != null)
                {
                    var employee = context.Employees.Single(x => x.EmployeeId == employeeId);

                    if (employee?.Password == password)
                    {
                        var employeeDTO = AutoMapperConfiguration.Mapper.Map<EmployeeDTO>(employee);
                        employee.KeepConnected = keepConnected;
                        employee.LastLogin = DateTime.Now;
                        employee.IsLoggedIn = true;
                        device.Employee = employee;
                        var result = await context.SaveChangesAsync();
                        if (result > 0)
                        {
                            return await Task.FromResult(employeeDTO);
                        }
                    }                    
                }
                return await Task.FromResult<EmployeeDTO>(null);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("SwitchEmployeeAsync"), "SwitchEmployeeAsync");
            }
        }

        public async Task<bool> ChangeEmployeeForegroundColor(int employeeId, string color)
        {
            try
            {
                var context = new EmployeeContext();
                var employee = context.Employees.Find(employeeId);
                if (employee != null)
                {
                    employee.Color = color;
                    var result = await context.SaveChangesAsync();
                    if (result > 0)
                    {
                        return await Task.FromResult(true);
                    }
                }

                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("ChangeEmployeeForegroundColor"), "ChangeEmployeeForegroundColor");
            }
        }

        public async Task<bool> ChangeEmployeePassword(int employeeId, string password , string newPassword)
        {
            try
            {
                var context = new EmployeeContext();
                var employee = context.Employees.Find(employeeId);
                if (employee != null)
                {
                    if (employee.Password == Helpers.NRServiceHelper.Sha256(password))
                    {
                        employee.Password = Helpers.NRServiceHelper.Sha256(newPassword);
                        var result = await context.SaveChangesAsync();
                        if (result > 0)
                        {
                            return await Task.FromResult(true);
                        }
                    }
                }

                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("ChangeEmployeeForegroundColor"), "ChangeEmployeePassword");
            }
        }

        public void UpdateEmployee(EmployeeDTO employee) => _clients.ToList().ForEach(x => x.Value.EmployeeUpdated(employee));


        public async Task<bool> DeleteDivisionAsync(int divisionId)
        {
            try
            {
                using (var context = new EmployeeContext())
                {
                    var division = context.Divisions.Include(i => i.DivisionType).SingleOrDefault(x => x.DivisionId == divisionId);
                    Console.WriteLine($"Name: {division.DivisionType.Name}");
                    context.Entry(division).State = EntityState.Deleted;
                    return await context.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("DeleteDivision"), "DeleteDivision");
            }
        }

        public async Task<Dictionary<string, FolderDTO>> GetProjectsAsync()
        {
            try
            {
                var rootFolders = new Dictionary<string, FolderDTO>();

                foreach (var directory in _projectPaths)
                {
                    var directoryInfo = new DirectoryInfo(directory);
                    var fileSeacher = new FileSearcher(directory, "*.pdf", SearchOption.AllDirectories);
                    var folder = await fileSeacher.SearchAsync();
                    rootFolders.Add(directoryInfo.Name, folder);
                }

                _ = rootFolders;
                return await Task.FromResult(rootFolders);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("DeleteDivision"), "DeleteDivision");
            }
        }

        public Task<PdfFile> GetPdfFile(string filePath)
        {
            try
            {
                var filename = Path.Combine(_projectBasePath, filePath).Replace("%20", " ");
                Console.WriteLine(filename);
                if (File.Exists(filename))
                {
                    return Task.Run(() =>
                    {
                        var fileInfo = new FileInfo(filename);
                        var pdfFile = new PdfFile() { Filename = fileInfo.Name, FileSize = fileInfo.Length };
                        pdfFile.Buffer = File.ReadAllBytes(filename);

                        return pdfFile;
                    });
                }

                return Task.FromResult<PdfFile>(null);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("GetPdfFile"), "GetPdfFile");
            }
        }

        public Task<List<PlantOrderDTO>> GetPlantOrders(int plantOrderNumber)
        {
            try
            {
                var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plantorders.json");
                if (!File.Exists(filename))
                {
                    return Task.FromResult<List<PlantOrderDTO>>(null);
                }

                var json = File.ReadAllText(filename);
                var result = JsonConvert.DeserializeObject<List<PlantOrderDTO>>(json);
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("GetPlantOrders"), "GetPlantOrders");
            }
        }

        public Task<List<MaterialRequirementDTO>> GetMaterialRequirements(string[] materialRequirements)
        {
            try
            {
                var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "materialrequirements.json");
                if (!File.Exists(filename))
                {
                    return Task.FromResult<List<MaterialRequirementDTO>>(null);
                }

                var json = File.ReadAllText(filename);
                var result = JsonConvert.DeserializeObject<List<MaterialRequirementDTO>>(json);
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("GetMaterialRequirements"), "GetMaterialRequirements");
            }
        }

        public Task<List<PlantOrderProcessDTO>> GetPlantOrderProcesses(int plantOrderId)
        {
            try
            {
                var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plantorderprocesses.json");
                if (!File.Exists(filename))
                {
                    return Task.FromResult<List<PlantOrderProcessDTO>>(null);
                }

                var json = File.ReadAllText(filename);
                var result = JsonConvert.DeserializeObject<List<PlantOrderProcessDTO>>(json);
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("GetPlantOrderProcesses"), "GetPlantOrderProcesses");
            }
        }

        public Task<List<ElementDTO>> GetElements()
        {
            try
            {
                var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "positions.json");
                if (!File.Exists(filename))
                {
                    return Task.FromResult<List<ElementDTO>>(null);
                }

                var json = File.ReadAllText(filename);
                var result = JsonConvert.DeserializeObject<List<ElementDTO>>(json);
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("GetElements"), "GetElements");
            }
        }

        public Task<List<ProfileDTO>> GetElementProfilesAsync(int plantOrderId, int divisionId)
        {
            try
            {
                var context = new ElementContext();
                var list = context.Profiles.Where(x => x.PlantOrderId == plantOrderId && x.DivisionId == divisionId).ToList();
                var profiles = AutoMapperConfiguration.Mapper.Map<List<Profile>, List<ProfileDTO>>(list);
                return Task.FromResult(profiles);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("GetProfiles"), "GetProfiles");
            }
        }

        public Task<List<ProcessTemplateDTO>> GetProcessTemplates()
        {
            try
            {
                var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "processtemplates.json");
                if (!File.Exists(filename))
                {
                    return Task.FromResult<List<ProcessTemplateDTO>>(null);
                }

                var json = File.ReadAllText(filename);
                var result = JsonConvert.DeserializeObject<List<ProcessTemplateDTO>>(json);
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("GetProcessTemplates"), "GetProcessTemplates");
            }
        }

        public Task<List<ProjectNumberDTO>> GetProjectNumbersAsync()
        {
            try
            {
                var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "projectnumbers.json");
                if (!File.Exists(filename))
                {
                    return Task.FromResult<List<ProjectNumberDTO>>(null);
                }

                var json = File.ReadAllText(filename);
                var result = JsonConvert.DeserializeObject<List<ProjectNumberDTO>>(json);
                var list = result.Where(x => int.TryParse(x.Number, out int val) && val > 21065 && val < 50000).ToList();
                return Task.FromResult(list);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("GetProjectNumbersAsync"), "GetProjectNumbersAsync");
            }
        }

        public Task<string> GetRemoteRootPath()
        {
            try
            {
                return Task.FromResult(_clientRootPath);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("GetRemoteRootPath"), "GetRemoteRootPath");
            }
        }

        public async Task<bool> Logout(Guid deviceId, int employeeId)
        {
            try
            {
                var context = new EmployeeContext();
                var device = context.Devices.Find(deviceId);
                if (device != null)
                {
                    var employee = context.Employees.Single(x => x.EmployeeId == employeeId);

                    if (employee != null)
                    {
                        employee.IsLoggedIn = false;
                        employee.KeepConnected = false;
                        device.Employee = employee;
                        var result = await context.SaveChangesAsync();
                        if (result > 0)
                        {
                            return await Task.FromResult(true);
                        }
                    }
                }
                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("Logout"), "Logout");
            }
        }

        public async Task<int> CreateProfileAsync(ProfileDTO profile, int employeeId)
        {
            try
            {
                var context = new ElementContext();
                var entity = AutoMapperConfiguration.Mapper.Map<Profile>(profile);
                if (entity  != null)
                {
                    if (entity.Amount > 0)
                    {
                        //var ei = new ElementInfo() { Amount = entity.Amount, EmployeeId = employeeId, Time = DateTime.Now };
                        //var elementInfo = context.ElementInfos.Add(ei);
                        entity.ElementInfos?.Add(new ElementInfo() { Amount = entity.Amount, EmployeeId = employeeId, Time = DateTime.Now });
                    }

                    context.Profiles.Add(entity);
                    var result = await context.SaveChangesAsync();
                    if (result > 0)
                    {
                        Console.WriteLine($"ProfileID: {entity.ProfileId}");
                        return await Task.FromResult(entity.ProfileId);
                    }
                }

                return await Task.FromResult(0);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("CreateProfile"), "CreateProfile");
            }
        }

        public Task<List<ProfileDTO>> GetProfilesAsync(int plantOrderId)
        {
            try
            {
                var context = new ElementContext();
                var profiles = context.Profiles.Where(x => x.PlantOrderId == plantOrderId)?.Include(e => e.ElementInfos).ToList();
                var result = AutoMapperConfiguration.Mapper.Map<List<Profile>, List<ProfileDTO>>(profiles);
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("GetProfilesAsync"), "GetProfilesAsync");
            }
        }

        /*
                var context = new EmployeeContext();
                var entity = AutoMapperConfiguration.Mapper.Map<Division>(division);
                var result = context.Divisions.Find(division.DivisionId);
                if (result != null)
                {
                    result.Description = entity.Description;
                    result.DivisionInfoId = entity.DivisionInfoId;
                    result.DivisionType = entity.DivisionType;
                    result.Name = entity.Name;
                    context.Entry(result).State = EntityState.Modified;
                    context.Entry(result.DivisionType).State = EntityState.Modified;
                    return await context.SaveChangesAsync();
                }
                else
                {
                    context.Divisions.Attach(entity);
                    context.Divisions.Add(entity);

                    var divisionId = await context.SaveChangesAsync();
                    if (divisionId > 0)
                    {
                        return await Task.FromResult(entity.DivisionId);
                    }
                    else
                    {
                        return await Task.FromResult(0);
                    }
                }
         */

        public async Task<bool> UpdateProfileAsync(int employeeId, ProfileDTO profile)
        {
            try
            {
                var context = new ElementContext();
                var entity = AutoMapperConfiguration.Mapper.Map<Profile>(profile);
                var result = context.Profiles.Find(profile.ProfileId);
                if (result != null)
                {
                    if (result.Amount != entity.Amount)
                    {
                        result.ElementInfos.Add(new ElementInfo { Amount = entity.Amount, Time = DateTime.Now, EmployeeId = employeeId });
                    }

                    result.Amount = entity.Amount;
                    result.Contraction = entity.Contraction;
                    result.Count = entity.Count;
                    result.Description = entity.Description;
                    result.Length = entity.Length;
                    result.ProfileNumber = entity.ProfileNumber;
                    result.Surface = entity.Surface;
                    context.Entry(result).State = EntityState.Modified;
                    if (await context.SaveChangesAsync() > 0)
                    {
                        return await Task.FromResult(true);
                    }
                }

                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("UpdateProfileAsync"), "UpdateProfileAsync");
            }
        }

        public async Task<bool> DeleteProfileAsync(int profileId)
        {
            try
            {
                using (var context = new ElementContext())
                {
                    var profile = context.Profiles.SingleOrDefault(x => x.ProfileId == profileId);
                    Console.WriteLine($"Name: {profile.ProfileNumber}");
                    context.Entry(profile).State = EntityState.Deleted;
                    return await context.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("DeleteProfileAsync"), "DeleteProfileAsync");
            }
        }

        public async Task<int> UpsertFilterAsync(ElementFilterDTO filter)
        {
            try
            {
                var context = new FilterContext();
                var entity = AutoMapperConfiguration.Mapper.Map<ElementFilter>(filter);
                var result = context.Filters.Find(filter.FilterId);
                if (result != null)
                {
                    result.Action = entity.Action;
                    result.EmployeeId = entity.EmployeeId;
                    result.Filter = entity.Filter;
                    result.PlantOrderId = entity.PlantOrderId;
                    result.PropertyName = entity.PropertyName;
                    result.UsedCounter = entity.UsedCounter;
                    context.Entry(result).State = EntityState.Modified;
                    return await context.SaveChangesAsync();
                }
                else
                {
                    context.Filters.Attach(entity);
                    context.Filters.Add(entity);

                    var divisionId = await context.SaveChangesAsync();
                    if (divisionId > 0)
                    {
                        return await Task.FromResult(entity.FilterId);
                    }
                    else
                    {
                        return await Task.FromResult(0);
                    }
                }
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("CreateFilterAsync"), "CreateFilterAsync");
            }
        }

        public Task<List<ElementFilterDTO>> GetFiltersAsync(int plantOrderId, int employeeId)
        {
            try
            {
                var context = new FilterContext();
                var entities = context.Filters.Where(x => x.PlantOrderId == plantOrderId && x.EmployeeId == employeeId).ToList();
                var result = AutoMapperConfiguration.Mapper.Map<List<ElementFilter>, List<ElementFilterDTO>>(entities);
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                throw new FaultException(ex.Message, new FaultCode("GetFiltersAsync"), "GetFiltersAsync");
            }
        }

    }
}
