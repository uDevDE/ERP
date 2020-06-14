using ERP.Contracts.Domain;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Contracts.Contract
{
    [ServiceContract(CallbackContract = typeof(INRServiceCallback), SessionMode = SessionMode.Required)]
    public interface INRService
    {
        [OperationContract(IsOneWay = true)]
        void Connect(Guid deviceId, string hostname, string username);

        [OperationContract(IsOneWay = false)]
        Task<List<DeviceDTO>> GetAllDevicesAsync();

        [OperationContract(IsOneWay = false)]
        Task<List<EmployeeDTO>> GetAllEmployeesAsync();

        [OperationContract(IsOneWay = false)]
        Task<int> UpsertEmployeeAsync(EmployeeDTO employee);

        [OperationContract(IsOneWay = false)]
        Task<List<EmployeeDTO>> GetEmployeesByDeviceAsync(Guid deviceId);

        [OperationContract(IsOneWay = false)]
        Task<List<DivisionInfoDTO>> GetDivisionInfosAsync();

        [OperationContract(IsOneWay = false)]
        Task<List<DivisionDTO>> GetDivisionsAsync();

        [OperationContract(IsOneWay = false)]
        Task<EmployeeDTO> SwitchEmployeeAsync(Guid deviceId, int employeeId, string password, bool keepConnected);

        [OperationContract(IsOneWay = false)]
        Task<bool> ChangeEmployeeForegroundColor(int employeeId, string color);

        [OperationContract(IsOneWay = false)]
        Task<bool> ChangeEmployeePassword(int employeeId, string password, string newPassword);

        [OperationContract(IsOneWay = true)]
        void UpdateEmployee(EmployeeDTO employee);

        [OperationContract(IsOneWay = false)]
        Task<int> UpsertDivisionInfoAsync(DivisionInfoDTO divisionInfo);

        [OperationContract(IsOneWay = false)]
        Task<int> UpsertDivisionAsync(DivisionDTO division);

        [OperationContract(IsOneWay = false)]
        Task<bool> DeleteDivisionAsync(int divisionId);

        [OperationContract(IsOneWay = false)]
        Task<Dictionary<string, FolderDTO>> GetProjectsAsync();

        [OperationContract(IsOneWay = false)]
        Task<PdfFile> GetPdfFile(string filePath);

        [OperationContract(IsOneWay = false)]
        Task<List<PlantOrderDTO>> GetPlantOrders(int plantOrderNumber);

        [OperationContract(IsOneWay = false)]
        Task<List<MaterialRequirementDTO>> GetMaterialRequirements(string[] materialRequirements);

        [OperationContract(IsOneWay = false)]
        Task<List<PlantOrderProcessDTO>> GetPlantOrderProcesses(int plantOrderId);

        [OperationContract(IsOneWay = false)]
        Task<List<ElementDTO>> GetElements();

        [OperationContract(IsOneWay = false)]
        Task<List<ProcessTemplateDTO>> GetProcessTemplates();

        [OperationContract(IsOneWay = false)]
        Task<List<ProjectNumberDTO>> GetProjectNumbersAsync();

        [OperationContract(IsOneWay = false)]
        Task<string> GetRemoteRootPath();

        [OperationContract(IsOneWay = false)]
        Task<bool> Logout(Guid deviceId, int employeeId);

        [OperationContract(IsOneWay = false)]
        Task<int> CreateProfileAsync(ProfileDTO profile, int employeeId);

        [OperationContract(IsOneWay = false)]
        Task<List<ProfileDTO>> GetProfilesAsync(int plantOrderId);

        [OperationContract(IsOneWay = false)]
        Task<bool> UpdateProfileAsync(ProfileDTO profile);

        [OperationContract(IsOneWay = false)]
        Task<bool> DeleteProfileAsync(int profileId);

        [OperationContract(IsOneWay = false)]
        Task<int> UpsertFilterAsync(ElementFilterDTO filter);

        [OperationContract(IsOneWay = false)]
        Task<List<ElementFilterDTO>> GetFiltersAsync(int plantOrderId, int employeeId);

    }
}
